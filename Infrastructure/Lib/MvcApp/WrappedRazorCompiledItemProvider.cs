using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Razor.Hosting;

namespace Lucid.Infrastructure.Lib.MvcApp
{
    public class WrappedRazorCompiledItemProvider : ApplicationPart, IRazorCompiledItemProvider
    {
        private IRazorCompiledItemProvider  _original;
        private IList<RazorCompiledItem>    _modifiedCompiledItems;

        public WrappedRazorCompiledItemProvider(IRazorCompiledItemProvider original)
        {
            _original = original;
            _modifiedCompiledItems = Wrap(original.CompiledItems);
        }

        public IEnumerable<RazorCompiledItem> CompiledItems => _modifiedCompiledItems;

        public override string Name => (_original as ApplicationPart).Name;

        private IList<RazorCompiledItem> Wrap(IEnumerable<RazorCompiledItem> originalItems)
        {
            return originalItems
                .Select(i => new WrappedRazorCompiledItem(i))
                .Cast<RazorCompiledItem>()
                .ToList();
        }
    }
}
