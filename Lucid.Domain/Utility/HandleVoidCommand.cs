﻿using System.Diagnostics;
using SimpleFacade;

namespace Lucid.Domain.Utility
{
    public abstract class HandleVoidCommand<TCmd> : IHandleVoidCommand<TCmd>
        where TCmd : ICommand
    {
        protected static ILucidRepository Repository {[DebuggerStepThrough] get { return Registry.Repository; } }

        public abstract void Execute(TCmd cmd);
    }
}
