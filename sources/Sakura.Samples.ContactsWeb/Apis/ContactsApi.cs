namespace Sakura.Samples.ContactsWeb.Apis
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Threading.Tasks;

    using Sakura.Extensions.NHibernate;
    using Sakura.Samples.Contacts.Database.Entities;

    [ServiceContract]
    public class ContactsApi
    {
        [WebGet]
        public IEnumerable<ContactDto> Contacts(IUnitOfWork unitOfWork)
        {
            var contacts = unitOfWork.QueryOver<Contact>().Take(100).List();

            return contacts.Select(contact => new ContactDto() { Name = contact.Name });
        }

        [WebInvoke(UriTemplate = "add")]
        public Task<HttpResponseMessage> Add(HttpRequestMessage<ContactDto> request, IUnitOfWork unitOfWork)
        {
            return request.Content.ReadAsAsync().ContinueWith(
                result =>
                    {
                        var contactDto = result.Result;

                        unitOfWork.Save(new Contact() { Name = contactDto.Name });

                        return new HttpResponseMessage(HttpStatusCode.Created);
                    });
        }
    }
}