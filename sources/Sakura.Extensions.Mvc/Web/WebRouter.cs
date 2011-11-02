namespace Sakura.Extensions.Mvc.Web
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class WebRouter : IWebRouter
    {
        private readonly RouteCollection routes;

        public WebRouter(RouteCollection routes)
        {
            this.routes = routes;
        }

        public Route MapRoute(string name, string url)
        {
            return this.routes.MapRoute(name, url);
        }

        public Route MapRoute(string name, string url, object defaults)
        {
            return this.routes.MapRoute(name, url, defaults);
        }

        public Route MapRoute(string name, string url, object defaults, object constraints)
        {
            return this.routes.MapRoute(name, url, defaults, constraints);
        }

        public Route MapRoute(string name, string url, object defaults, object constraints, string[] namespaces)
        {
            return this.routes.MapRoute(name, url, defaults, constraints, namespaces);
        }

        public Route MapRoute(string name, string url, string[] namespaces)
        {
            return this.routes.MapRoute(name, url, namespaces);
        }

        public void IgnoreRoute(string url)
        {
            this.routes.IgnoreRoute(url);
        }

        public void IgnoreRoute(string url, object constraints)
        {
            this.routes.IgnoreRoute(url, constraints);
        }
    }
}