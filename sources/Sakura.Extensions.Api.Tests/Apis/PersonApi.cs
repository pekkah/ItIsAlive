namespace Sakura.Extensions.Api.Tests.Apis
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using Sakura.Framework.Dependencies;
    using Sakura.Framework.Dependencies.DefaultTypes;

    [ServiceContract]
    public class PersonApi: ITransientDependency, IAsSelf
    {
        [WebGet] 
        public string Echo(string message)
        {
            return string.Format("Echo {0}", message);
        }
    }
}