using FluentAssertions;
using Lucid.Infrastructure.Lib.MvcApp.RazorFolders;
using NUnit.Framework;

namespace Lucid.Infrastructure.Lib.MvcApp.Tests.RazorFolders
{
    [TestFixture]
    public class ViewPathTests
    {
        [Test]
        public void RelativeViewPath()
        {
            var viewPath = this.RelativeViewPath("View.cshtml");

            viewPath.Should().Be("/Lucid.Infrastructure.Lib.MvcApp.Tests/RazorFolders/View.cshtml");
        }

        [Test]
        public void AbsoluteViewPath()
        {
            var viewPath = this.AbsoluteViewPath("/Folder/View.cshtml");

            viewPath.Should().Be("/Lucid.Infrastructure.Lib.MvcApp.Tests/Folder/View.cshtml");
        }
    }
}
