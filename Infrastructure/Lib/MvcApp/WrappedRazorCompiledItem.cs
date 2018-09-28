using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Hosting;

namespace Lucid.Infrastructure.Lib.MvcApp
{
    /// <summary> Wraps a RazorCompiledItem to prefix the identifier with the view's type's namespace </summary>
    public class WrappedRazorCompiledItem : RazorCompiledItem
    {
        private RazorCompiledItem   _item;
        private string              _identifier;

        public WrappedRazorCompiledItem(RazorCompiledItem item)
        {
            _item = item;
            _identifier = $"/{_item.Type.Namespace}{_item.Identifier}";
        }

        public override string Identifier => _identifier;

        public override string                  Kind        => _item.Kind;
        public override IReadOnlyList<object>   Metadata    => _item.Metadata;
        public override Type                    Type        => _item.Type;

        public override bool    Equals(object obj)  { return _identifier.Equals(obj); }
        public override int     GetHashCode()       { return _identifier.GetHashCode(); }
        public override string  ToString()          { return _identifier.ToString(); }
    }
}
