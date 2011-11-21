namespace Sakura.Extensions.Api.Tests.Apis
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    [ServiceContract]
    public class PersonApi
    {
        [WebGet]
        public string Echo(string message)
        {
            return string.Format("Echo {0}", message);
        }
    }
}