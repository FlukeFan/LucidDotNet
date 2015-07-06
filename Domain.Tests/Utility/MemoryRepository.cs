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

        private Action<Entity>  _beforeSave;
        private IList<Entity>   _entities = new List<Entity>();

        private int lastId = 101;

        public MemoryRepository(Action<Entity> beforeSave)
        {
            _beforeSave = beforeSave;
        }

        public T Save<T>(T entity) where T : Entity
        {
            entity.Should().NotBeNull("null entity supplied");
            _beforeSave(entity);
            _idProperty.SetValue(entity, lastId++);
            _entities.Add(entity);
            return entity;
        }

        public Query<T> Query<T>() where T : Entity
        {
            return new Query<T>(this);
        }

        public IList<T> Satisfy<T>(Query<T> query) where T : class
        {
            var result = _entities.Where(e => typeof(T).IsAssignableFrom(e.GetType())).Cast<T>();

            foreach (var restriction in query.Restrictions)
                result = result.Where(e => restriction.Satisfies(e));

            return result.ToList();
        }

        public void ShouldContain(Entity entity)
        {
            entity.Should().NotBeNull("entity was null");
            entity.Id.Should().NotBe(0, "entity Id was 0");
            _entities.Should().Contain(entity);
        }

        public void ShouldContain<T>(int id)
        {
            var entity = _entities.Where(e => e.Id == id).SingleOrDefault();
            entity.Should().NotBeNull("could not find entity with id " + id + " and type " + typeof(T));
        }
    }
}
