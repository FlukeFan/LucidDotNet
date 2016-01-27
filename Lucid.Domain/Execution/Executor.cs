using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace Lucid.Domain.Execution
{
    public class Executor : IExecutor
    {
        protected IDictionary<Type, Func<object, object>> _handlers = new Dictionary<Type, Func<object, object>>();

        public virtual Executor UsingHandlersFromAssemblyWithType<T>()
        {
            var types = typeof(T).Assembly.GetTypes()
                .Where(t => !t.IsAbstract);

            foreach (var type in types)
            {
                foreach (var intrface in type.GetInterfaces())
                {
                    if (!intrface.IsGenericType)
                        continue;

                    var genericType = intrface.GetGenericTypeDefinition();

                    if (genericType == typeof(IHandleVoidCommand<>) || genericType == typeof(IHandleCommand<,>))
                        HandleMethod(type, intrface, "Execute");
                    else if (genericType == typeof(IHandleQuerySingle<,>))
                        HandleMethod(type, intrface, "Find");
                    else if (genericType == typeof(IHandleQueryList<,>))
                        HandleMethod(type, intrface, "List");
                }
            }

            return this;
        }

        protected void HandleMethod(Type handlerType, Type intrface, string methodName)
        {
            var commandType = intrface.GetGenericArguments()[0];
            var executeMethod = intrface.GetMethod(methodName);
            _handlers.Add(commandType, cmd =>
            {
                try
                {
                    var target = Activator.CreateInstance(handlerType);
                    return executeMethod.Invoke(target, new object[] { cmd });
                }
                catch (TargetInvocationException tie)
                {
                    ExceptionDispatchInfo.Capture(tie.InnerException).Throw();
                    return null;
                }
            });
        }

        object IExecutor.Execute(object executable)
        {
            var iExecutable = executable as IExecutable;

            if (iExecutable != null)
                return iExecutable.Execute();

            var executableType = executable.GetType();

            if (_handlers.ContainsKey(executableType))
            {
                var handler = _handlers[executableType];
                return handler(executable);
            }

            return UnhandledExecute(executable);
        }

        protected virtual object UnhandledExecute(object executable)
        {
            throw new Exception(string.Format("Could not execute:{0}\nEither it dit not implement IExecutable, or no handler was registered (using UsingHandlerFrommAssemblyWithType())", executable));
        }

    }
}
