using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Hosting;

namespace Lucid.Infrastructure.Lib.MvcApp.RazorFolders
{
    /// <summary> Wraps a RazorCompiledItem to prefix the identifier with the view's type's assembly </summary>
    public class WrappedRazorCompiledItem : RazorCompiledItem
    {
        private RazorCompiledItem   _item;
        private string              _identifier;

        public WrappedRazorCompiledItem(RazorCompiledItem item)
        {
            _item = item;
            var assembly = _item.Type.Assembly.GetName().Name;

            if (assembly.EndsWith(".Views"))
                assembly = assembly.Substring(0, assembly.Length - 6);

            _identifier = $"/{assembly}{_item.Identifier}";
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
