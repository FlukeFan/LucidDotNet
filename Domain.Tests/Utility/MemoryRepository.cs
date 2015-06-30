using System;
using System.Reflection;
using Lucid.Domain.Utility;

namespace Lucid.Domain.Tests.Utility
{
    public class MemoryRepository : IRepository
    {
        private static readonly Type            entityType = typeof(Entity);
        private static readonly PropertyInfo    idProperty = entityType.GetProperty("Id");

        private int lastId = 101;

        public T Save<T>(T entity) where T : Entity
        {
            idProperty.SetValue(entity, lastId++);
            return entity;
        }
    }
}
