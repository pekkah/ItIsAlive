namespace Sakura.Samples.ContactsWeb.Api
{
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using Sakura.Extensions.NHibernate;
    using Sakura.Samples.Contacts.Database.Entities;

    [ServiceContract]
    public class ContactsApi
    {
        [WebGet]
        public IEnumerable<ContactDto> Contracts(IWorkContext workContext)
        {
            var contacts = workContext.QueryOver<Contact>().Take(100).List();

            return contacts.Select(contact => new ContactDto() { Name = contact.Name });
        }
    }
}