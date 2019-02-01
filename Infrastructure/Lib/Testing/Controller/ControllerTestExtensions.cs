namespace Lucid.Infrastructure.Lib.Testing.Controller
{
    public static class ControllerTestExtensions
    {
        public static string PrefixLocalhost(this string action)
        {
            return $"http://localhost{action}";
        }
    }
}
