﻿using FluentAssertions;
using Lucid.Database.Tests;
using Lucid.Domain.Persistence;
using Lucid.Domain.Testing;
using Lucid.Domain.Tests.Persistence;
using NUnit.Framework;

namespace Lucid.Persistence.NHibernate.Tests
{
    public class NhRepositoryTests : IRepositoryTests
    {
        static NhRepositoryTests()
        {
            var environment = BuildEnvironment.Load();
            NhRepository<int>.Init(environment.LucidConnection, typeof(LucidEntity));
        }

        private NhRepository<int> _repository;

        protected override IRepository<int> New()
        {
            _repository = new NhRepository<int>().Open();
            return _repository;
        }

        public override void TearDown()
        {
            using (_repository) { }
            _repository = null;

            base.TearDown();
        }

        [Test]
        public void Flush_UpdatesUnderlyingTable()
        {
            var poly = new LucidPolyTypeBuilder()
                .With(p => p.String, "initial value")
                .Value();

            _repository.Save(poly);
            _repository.Flush();

            GetUnderlyingDbValue(poly.Id).Should().Be("initial value");

            Builder.Modify(poly).With(p => p.String, "modified value");

            GetUnderlyingDbValue(poly.Id).Should().Be("initial value", "DB has not been updated");

            _repository.Flush();

            GetUnderlyingDbValue(poly.Id).Should().Be("modified value", "in-memory changes have been flushed to the DB");
        }

        [Test]
        public void Clear_PurgesIdentityMap()
        {
            var poly = new LucidPolyTypeBuilder()
                .With(p => p.String, "initial value")
                .Value();

            _repository.Save(poly);
            _repository.Flush();

            GetUnderlyingDbValue(poly.Id).Should().Be("initial value");

            Builder.Modify(poly).With(p => p.String, "modified value");

            GetUnderlyingDbValue(poly.Id).Should().Be("initial value", "DB has not been updated");

            _repository.Clear();

            var loaded = _repository.Load<LucidPolyType>(poly.Id);

            loaded.String.Should().Be("initial value");
            loaded.Should().NotBeSameAs(poly);
        }

        private string GetUnderlyingDbValue(int id)
        {
            var sql = "SELECT [String] FROM [LucidPolyType] WHERE [Id] = :id";
            var query = _repository.Session.CreateSQLQuery(sql);
            query.SetInt32("id", id);
            return query.UniqueResult<string>();
        }

        private void SetUnderlyingDbValue(int id, string value)
        {
            var sql = "UPDATE [LucidPolyType] SET [String] = :val WHERE [Id] = :id";
            var query = _repository.Session.CreateSQLQuery(sql);
            query.SetAnsiString("val", value);
            query.SetInt32("id", id);
            query.ExecuteUpdate();
        }
    }
}