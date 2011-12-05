namespace Sakura.Samples.ContactsWeb.Controllers
{
    using System.Web.Mvc;

    [Authorize]
    public class ContactsController : Controller
    {
        public ActionResult Index()
        {
            // note (pekka) contact management is done using jQuery and ajax
            return this.View();
        }
    }
}