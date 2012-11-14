namespace Bootstrapper.Samples.ContactsWeb.Apis
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Bootstrapper.Samples.ContactsWeb.Database.Entities;

    using NHibernate;

    public class ContactsController : ApiController
    {
        private readonly ISession unitOfWork;

        public ContactsController(ISession unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<ContactDto> GetAllContacts()
        {
            var contacts = this.unitOfWork.QueryOver<Contact>().Take(100).List();

            return contacts.Select(
                contact => new ContactDto
                               {
                                   Name = contact.Name
                               });
        }

        public HttpResponseMessage Post(ContactDto contactDto)
        {
            this.unitOfWork.Save(new Contact { Name = contactDto.Name });

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}