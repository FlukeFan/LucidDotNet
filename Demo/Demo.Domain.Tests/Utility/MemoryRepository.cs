using System;
using System.Collections.Generic;
using System.Linq;
using Demo.Domain.Utility;
using FluentAssertions;
using Lucid.Domain.Persistence;
using Lucid.Domain.Persistence.Queries;

namespace Lucid.Domain.Tests.Utility
{
    public class DemoMemoryRepository : MemoryRepository<int>, IDemoRepository
    {
        public DemoMemoryRepository(Action<DemoEntity> beforeSave) : base(e => beforeSave((DemoEntity)e)) { }
    }

    public class MemoryRepository<TId> : IRepository<TId>
    {
        private Action<IEntity<TId>>    _beforeSave;
        private IList<IEntity<TId>>     _entities = new List<IEntity<TId>>();

        private int lastId = 101;

        public MemoryRepository(Action<IEntity<TId>> beforeSave)
        {
            _beforeSave = beforeSave;
        }

        public T Save<T>(T entity) where T : IEntity<TId>
        {
            entity.Should().NotBeNull("null entity supplied");
            _beforeSave(entity);
            var idProperty = entity.GetType().GetProperty("Id");
            idProperty.SetValue(entity, lastId++);
            _entities.Add(entity);
            return entity;
        }

        public Query<T, TId> Query<T>() where T : IEntity<TId>
        {
            return new Query<T, TId>(this);
        }

        public IList<T> Satisfy<T>(Query<T, TId> query) where T : IEntity<TId>
        {
            var result = _entities.Where(e => typeof(T).IsAssignableFrom(e.GetType())).Cast<T>();

            foreach (var restriction in query.Restrictions)
                result = result.Where(e => restriction.Satisfies(e));

            return result.ToList();
        }

        public void ShouldContain(IEntity<TId> entity)
        {
            entity.Should().NotBeNull("entity was null");
            entity.Id.Should().NotBe(0, "entity Id was 0");
            _entities.Should().Contain(entity);
        }

        public void ShouldContain<T>(TId id)
        {
            var entity = _entities.Where(e => e.Id.Equals(id)).SingleOrDefault();
            entity.Should().NotBeNull("could not find entity with id " + id + " and type " + typeof(T));
        }
    }
}
