namespace Sakura.Framework.Samples.ContactsWeb.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
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