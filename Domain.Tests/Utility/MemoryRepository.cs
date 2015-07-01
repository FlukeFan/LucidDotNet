using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Lucid.Domain.Utility;
using Lucid.Domain.Utility.Queries;

namespace Lucid.Domain.Tests.Utility
{
    public class MemoryRepository : IRepository
    {
        private static readonly Type            _entityType = typeof(Entity);
        private static readonly PropertyInfo    _idProperty = _entityType.GetProperty("Id");

        private IList<Entity> entities = new List<Entity>();

        private int lastId = 101;

        public T Save<T>(T entity) where T : Entity
        {
            entity.Should().NotBeNull("null entity supplied");
            _idProperty.SetValue(entity, lastId++);
            entities.Add(entity);
            return entity;
        }

        public Query<T> Query<T>() where T : Entity
        {
            return new Query<T>(this);
        }

        public IList<T> Satisfy<T>(Query<T> query)
        {
            var result = entities.Where(e => typeof(T).IsAssignableFrom(e.GetType())).Cast<T>();
            return result.ToList();
        }

        public void ShouldContain(Entity entity)
        {
            entity.Should().NotBeNull("entity was null");
            entity.Id.Should().NotBe(0, "entity Id was 0");
            entities.Should().Contain(entity);
        }
    }
}
