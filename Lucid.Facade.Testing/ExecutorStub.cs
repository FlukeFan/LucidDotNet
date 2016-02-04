using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lucid.Facade.Execution;

namespace Lucid.Facade.Testing
{
    public class ExecutorStub : IExecutor
    {
        protected IList<object>                                     _executed       = new List<object>();
        protected IDictionary<Type, Func<object, object, object>>   _setupResults   = new Dictionary<Type, Func<object, object, object>>();

        public object[] Executed()
        {
            return _executed.ToArray(); ;
        }

        public T Executed<T>(uint index)
        {
            var executedCmds = Executed<T>().ToArray();

            if (index >= executedCmds.Length)
                throw new Exception(string.Format("Could not find executed command of type {0} with index {1}.\nFound {2} executed commands of type {3}.",
                    typeof(T), index, executedCmds.Length, typeof(T)));

            return executedCmds[index];
        }

        public T[] Executed<T>()
        {
            return _executed.Where(e => typeof(T).IsAssignableFrom(e.GetType())).Cast<T>().ToArray();
        }

        object IExecutor.Execute(object executable)
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

            if (type.IsArray)
                return Array.CreateInstance(type.GetElementType(), 0);

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

        public ExecutorStub SetupCommand<TCmd, TReturn>(TCmd cmdType, TReturn result)
            where TCmd : ICommand<TReturn>
        {
            return SetupCommand<TCmd, TReturn>(cmdType, (exe, def) => result);
        }

        public ExecutorStub SetupCommand<TCmd, TReturn>(TCmd cmdType, Func<TCmd, TReturn, TReturn> setup)
            where TCmd : ICommand<TReturn>
        {
            return SetupObjectResult<TCmd>((exe, def) => setup((TCmd)exe, (TReturn)def));
        }

        public ExecutorStub SetupQueryList<TQuery, TListItem>(TQuery queryType, IList<TListItem> result)
            where TQuery : IQueryList<TListItem>
        {
            return SetupQueryList<TQuery, TListItem>(queryType, (exe, def) => result);
        }

        public ExecutorStub SetupQueryList<TQuery, TListItem>(TQuery queryType, Func<TQuery, IList<TListItem>, IList<TListItem>> setup)
            where TQuery : IQueryList<TListItem>
        {
            return SetupObjectResult<TQuery>((exe, def) => setup((TQuery)exe, (IList<TListItem>)def));
        }

        public ExecutorStub SetupQuerySingle<TQuery, TReturn>(TQuery queryType, TReturn result)
            where TQuery : IQuerySingle<TReturn>
        {
            return SetupQuerySingle<TQuery, TReturn>(queryType, (exe, def) => result);
        }

        public ExecutorStub SetupQuerySingle<TQuery, TReturn>(TQuery queryType, Func<TQuery, TReturn, TReturn> setup)
            where TQuery : IQuerySingle<TReturn>
        {
            return SetupObjectResult<TQuery>((exe, def) => setup((TQuery)exe, (TReturn)def));
        }
    }
}
