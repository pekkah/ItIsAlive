namespace Sakura.Extensions.Mvc.Web
{
    using System.Web.Routing;

    using Sakura.Framework.Dependencies;
    using Sakura.Framework.Dependencies.DefaultTypes;

    public interface IWebRouter : ISingleInstanceDependency
    {
        Route MapRoute(string name, string url);

        Route MapRoute(string name, string url, object defaults);

        Route MapRoute(string name, string url, object defaults, object constraints);

        Route MapRoute(string name, string url, object defaults, object constraints, string[] namespaces);

        Route MapRoute(string name, string url, string[] namespaces);

        void IgnoreRoute(string url);

        void IgnoreRoute(string url, object constraints);
    }
}