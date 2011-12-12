namespace Sakura.Extensions.Mvc.UnitOfWork
{
    using System;
    using System.Web;

    public class UnitOfWorkModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += this.OnBeginRequest;
            context.EndRequest += this.OnEndRequest;
        }

        private void OnBeginRequest(object sender, EventArgs e)
        {

        }

        private void OnEndRequest(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}