namespace Sakura.Framework.Samples.ContactsWeb.Controllers
{
    using System.Web.Mvc;

    using Sakura.Framework.Dependencies;
    using Sakura.Framework.Dependencies.DefaultTypes;

    public class HomeController : Controller, ITransientDependency
    {
        public ActionResult About()
        {
            return this.View();
        }

        public ActionResult Index()
        {
            this.ViewBag.Message = "Welcome to ASP.NET MVC!";

            return this.View();
        }
    }
}