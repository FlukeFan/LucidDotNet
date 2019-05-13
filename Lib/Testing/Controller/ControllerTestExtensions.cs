using Microsoft.AspNetCore.Mvc;
using MvcTesting.Http;

namespace Lucid.Lib.Testing.Controller
{
    public static class ControllerTestExtensions
    {
        public static string PrefixLocalhost(this string action)
        {
            return $"http://localhost{action}";
        }

        public static T ViewResultModel<T>(this Response response)
        {
            return (T)response.ActionResultOf<ViewResult>().ViewData.Model;
        }
    }
}
