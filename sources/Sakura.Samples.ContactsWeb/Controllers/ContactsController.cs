namespace Sakura.Samples.ContactsWeb.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using Sakura.Extensions.NHibernate;
    using Sakura.Samples.Contacts.Database.Entities;
    using Sakura.Samples.ContactsWeb.Models;

    [Authorize]
    public class ContactsController : Controller
    {
        public ActionResult Index(IWorkContext workContext)
        {
            // todo (pekka) get from model binder
            var identityName = this.Request.RequestContext.HttpContext.User.Identity.Name;

            var contacts = workContext
                .QueryOver<User>()
                .Where(u => u.Name == identityName)
                .JoinQueryOver(u => u.Contacts)
                .SingleOrDefault().Contacts;

            return this.View(contacts.Select(c => new ContactModel() { Name = c.Name, Id = c.Id }));
        }
    }
}