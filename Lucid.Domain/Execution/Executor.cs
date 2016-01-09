using System;
using System.Collections.Generic;

namespace Lucid.Domain.Execution
{
    public class Executor : IExecutor
    {
        protected IDictionary<Type, Func<object, object>> _handlers = new Dictionary<Type, Func<object, object>>();

        public Executor UsingHandlersFromAssemblyWithType<T>()
        {
            var types = typeof(T).Assembly.GetTypes();

            foreach (var type in types)
            {
                foreach (var intrface in type.GetInterfaces())
                {
                    if (!intrface.IsGenericType)
                        continue;

                    var genericType = intrface.GetGenericTypeDefinition();

                    if (genericType == typeof(IHandleVoidCommand<>) || genericType == typeof(IHandleCommand<,>))
                    {
                        var commandType = intrface.GetGenericArguments()[0];
                        var executeMethod = intrface.GetMethod("Execute");
                        _handlers.Add(commandType, cmd => executeMethod.Invoke(Activator.CreateInstance(type), new object[] { cmd }));
                    }
                }
            }

            return this;
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
