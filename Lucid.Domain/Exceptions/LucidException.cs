﻿using System;

namespace Lucid.Domain.Exceptions
{
    public class LucidException : ApplicationException
    {
        public ErrorContext Context { get; protected set; }

        public LucidException(ErrorContext context) : base(context.FormatMessage())
        {
            Context = context;
        }
    }
}
