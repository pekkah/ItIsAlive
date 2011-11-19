namespace Sakura.Samples.ContactsWeb.Apis
{
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using Sakura.Extensions.NHibernate;
    using Sakura.Framework.Dependencies.DefaultTypes;
    using Sakura.Samples.Contacts.Database.Entities;

    public class ContactDto
    {
        public string Name
        {
            get;
            set;
        }
    }

    [ServiceContract]
    public class ContactsApi : ITransientDependency, IAsSelf
    {
        [WebGet]
        public IEnumerable<ContactDto> Contacts(IWorkContext workContext)
        {
            var contacts = workContext.QueryOver<Contact>().Take(100).List();

            return contacts.Select(contact => new ContactDto()
            {
                Name = contact.Name
            });
        }

        [WebGet(UriTemplate = "test")]
        public void Test()
        {
            
        }
    }
}