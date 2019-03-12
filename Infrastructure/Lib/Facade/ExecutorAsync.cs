using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Lucid.Infrastructure.Lib.Facade
{
    public class ExecutorAsync : IExecutorAsync
    {
        protected IDictionary<Type, Func<object, Task>> _handlers = new Dictionary<Type, Func<object, Task>>();

        public virtual ExecutorAsync UsingHandlersFromAssemblyWithType<T>()
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

                    if (genericType == typeof(IHandleQueryAsync<,>))
                        HandleMethod(type, intrface, "Find");
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
                    return (Task)executeMethod.Invoke(target, new object[] { cmd });
                }
                catch (TargetInvocationException tie)
                {
                    ExceptionDispatchInfo.Capture(tie.InnerException).Throw();
                    return null;
                }
            });
        }

        async Task<object> IExecutorAsync.ExecuteAsync(IExecutionContext context)
        {
            var iExecutable = (context.Executable as IExecutableAsync);

            if (iExecutable != null)
                return iExecutable.ExecuteAsync();

            var executable = context.Executable;
            var executableType = executable.GetType();

            if (_handlers.ContainsKey(executableType))
            {
                var handler = _handlers[executableType];
                var task = handler(executable);
                await task;
                var resultProperty = task.GetType().GetProperty("Result");
                return resultProperty.GetValue(task);
            }

            throw new Exception($"Could not execute:{executable}\nEither it dit not implement IExecutable, or no handler implementing IHandle_xxx was registered (using UsingHandlersFromAssemblyWithType())");
        }
    }
}
