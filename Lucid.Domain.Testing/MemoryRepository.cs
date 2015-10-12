﻿using System;
using System.Collections.Generic;
using System.Linq;
using Lucid.Domain.Persistence;
using Lucid.Domain.Persistence.Queries;

namespace Lucid.Domain.Testing
{
    public class MemoryRepository<TId> : IRepository<TId>
    {
        private ConsistencyInspector    _consistencyInspector;
        private IList<IEntity<TId>>     _entities = new List<IEntity<TId>>();

        private int lastId = 101;

        public MemoryRepository(ConsistencyInspector consistencyInspector)
        {
            _consistencyInspector = consistencyInspector;
        }

        public T Save<T>(T entity) where T : IEntity<TId>
        {
            if (entity == null)
                throw new Exception("Entity to be saved should not be null");

            _consistencyInspector.BeforeSave(entity);
            var idProperty = entity.GetType().GetProperty("Id");
            idProperty.SetValue(entity, lastId++);
            _entities.Add(entity);
            return entity;
        }

        public Query<T, TId> Query<T>() where T : class, IEntity<TId>
        {
            return new Query<T, TId>(this);
        }

        public IList<T> Satisfy<T>(Query<T, TId> query) where T : class, IEntity<TId>
        {
            var result = _entities.Where(e => typeof(T).IsAssignableFrom(e.GetType())).Cast<T>();

            foreach (var restriction in query.Restrictions)
                result = result.Where(e => restriction.Satisfies(e));

            return result.ToList();
        }

        public void ShouldContain(IEntity<TId> entity)
        {
            if (entity == null)
                throw new Exception("Entity to be verified should not be null");

            if (entity.Id == null || entity.Id.Equals(default(TId)))
                throw new Exception("Entity to be verified has an unsaved Id value: " + entity.Id);

            if (!_entities.Contains(entity))
                throw new Exception(string.Format("Could not find Entity with Id {0} in Repository", entity.Id));
        }

        public void ShouldContain<T>(TId id)
        {
            var entity = _entities.Where(e => e.Id.Equals(id)).SingleOrDefault();

            if (entity == null)
                throw new Exception("could not find entity with id " + id + " and type " + typeof(T));
        }
    }
}
