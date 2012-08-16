﻿namespace Sakura.Samples.ContactsWeb.Controllers
{
    using System.Web.Mvc;

    using Sakura.Composition.Markers;
    using Sakura.Samples.Contacts;

    public class HomeController : Controller, ITransientDependency
    {
        public ActionResult About()
        {
            return this.View();
        }

        public ActionResult Index()
        {
            this.ViewBag.Message = "Welcome to ASP.NET MVC!";

            return this.View();
        }
    }
}