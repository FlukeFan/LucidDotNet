﻿using Demo.Domain.Utility;
using Lucid.Domain.Persistence;

namespace Demo.Domain.Tests.Utility
{
    public class MemoryRepositoryTests : IRepositoryTests
    {
        protected override IRepository<int> New()
        {
            var repository = new DemoMemoryRepository();
            Registry.Repository = repository;
            return repository;
        }
    }
}
