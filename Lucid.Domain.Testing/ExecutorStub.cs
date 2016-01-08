using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lucid.Domain.Execution;

namespace Lucid.Domain.Testing
{
    public class ExecutorStub : IExecutor
    {
        protected IList<object>                                     _executed       = new List<object>();
        protected IDictionary<Type, Func<object, object, object>>   _setupResults   = new Dictionary<Type, Func<object, object, object>>();

        public IEnumerable<object> AllExecuted()
        {
            return _executed;
        }

        public IEnumerable<T> Executed<T>()
        {
            return _executed.Where(e => typeof(T).IsAssignableFrom(e.GetType())).Cast<T>();
        }

        public object Execute(object executable)
        {
            var executableType = executable.GetType();
            _executed.Add(executable);

            var defaultResponseType = FindResponseType(executable);
            var defaultResponse = CreateDefaultResponse(defaultResponseType);

            if (_setupResults.ContainsKey(executableType))
                return _setupResults[executableType](executable, defaultResponse);

            return defaultResponse;
        }

        protected virtual Type FindResponseType(object executable)
        {
            var executableType = executable.GetType();
            var interfaces = executable.GetType().GetInterfaces();

            var commandInterface = typeof(ICommand<>);
            var queryListInterface = typeof(IQueryList<>);
            var querySingleInterface = typeof(IQuerySingle<>);

            foreach (var intrface in interfaces)
            {
                if (!intrface.IsGenericType)
                    continue;

                var genericType = intrface.GetGenericTypeDefinition();

                if (genericType == commandInterface || genericType == querySingleInterface)
                    return intrface.GetGenericArguments()[0];

                if (genericType == queryListInterface)
                    return typeof(IList<>).MakeGenericType(intrface.GetGenericArguments()[0]);
            }

            return null;
        }

        protected virtual object CreateDefaultResponse(Type type)
        {
            if (type == null)
                return null;

            // if the return type has a default constructor, then create an object to return
            var constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[0], null);

            if (constructor != null)
                return constructor.Invoke(new object[0]);

            if (type.IsValueType)
                return Activator.CreateInstance(type);

            if (type.IsInterface)
            {
                var typeName = type.Name;

                if (typeName.StartsWith("IList") || typeName.StartsWith("IEnumerable"))
                    return CreateList(type);

                if (typeName.StartsWith("IDictionary"))
                    return CreateDictionary(type);
            }

            return null;
        }

        protected object CreateList(Type type)
        {
            return Activator.CreateInstance(typeof(List<>).MakeGenericType(type.GetGenericArguments()));
        }

        protected object CreateDictionary(Type type)
        {
            return Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(type.GetGenericArguments()));
        }

        public ExecutorStub SetupObjectResult<T>(object result)
        {
            return SetupObjectResult<T>((exe, def) => result);
        }

        public ExecutorStub SetupObjectResult<T>(Func<object, object, object> setup)
        {
            _setupResults.Add(typeof(T), setup);
            return this;
        }

        public ExecutorStub Setup<TCmd, TReturn>(TCmd cmdType, TReturn result)
            where TCmd : ICommand<TReturn>
        {
            return Setup<TCmd, TReturn>(cmdType, (exe, def) => result);
        }

        public ExecutorStub Setup<TCmd, TReturn>(TCmd cmdType, Func<TCmd, TReturn, TReturn> setup)
            where TCmd : ICommand<TReturn>
        {
            return SetupObjectResult<TCmd>((exe, def) => setup((TCmd)exe, (TReturn)def));
        }
    }
}
