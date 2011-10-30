namespace Fugu.Extensions.Api.Apis
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using Fugu.Extensions.Data.Model;
    using Fugu.Framework.Dependencies;

    using Microsoft.ApplicationServer.Http.Dispatcher;

    using NHibernate;
    using NHibernate.Linq;

    [ServiceContract]
    public class ProjectsApi : ITransientDependency
    {
        private readonly ISession session;

        public ProjectsApi(ISession session)
        {
            this.session = session;
        }

        [WebInvoke(UriTemplate = "create/{name}")]
        public HttpResponseMessage Create(HttpRequestMessage<string> nameRequest)
        {
            var name = nameRequest.Content.ReadAsString();

            if (string.IsNullOrWhiteSpace(name) || name.Length < 4)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                        { Content = new StringContent("Name must be at least four characters long") });
            }

            if (name.Contains(" "))
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                        { Content = new StringContent("Name must not contain whitespace characters") });
            }

            var project = new Project() { Name = name };

            using (var tx = this.session.BeginTransaction())
            {
                this.session.Save(project);
                tx.Commit();
            }

            var response = new HttpResponseMessage(HttpStatusCode.Created);

            var baseUri = nameRequest.RequestUri;
            var projectUri = string.Format("api/project/{0}", project.Name);

            response.Headers.Location = new Uri(baseUri, projectUri);

            return response;
        }

        [WebGet(UriTemplate = "")]
        public IQueryable<Project> Projects()
        {
            return this.session.Query<Project>();
        }
    }
}