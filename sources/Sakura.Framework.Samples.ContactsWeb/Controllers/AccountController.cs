namespace Sakura.Framework.Samples.ContactsWeb.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Security;

    using NHibernate;

    using Sakura.Framework.Dependencies;
    using Sakura.Framework.Samples.ContactsWeb.Database;
    using Sakura.Framework.Samples.ContactsWeb.Models;

    public class AccountController : Controller, ITransientDependency
    {
        private readonly ISession session;

        public AccountController(ISession session)
        {
            this.session = session;
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (this.ModelState.IsValid)
            {
                bool changePasswordSucceeded = false;

                if (changePasswordSucceeded)
                {
                    return this.RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    this.ModelState.AddModelError(
                        string.Empty, "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult ChangePasswordSuccess()
        {
            return this.View();
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return this.RedirectToAction("Index", "Home");
        }

        public ActionResult LogOn()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (this.ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (this.Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return this.Redirect(returnUrl);
                    }
                    else
                    {
                        return this.RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (this.ModelState.IsValid)
            {
                using (var tx = this.session.BeginTransaction())
                {
                    var user = new User();
                    user.Name = model.UserName;
                    user.Password = model.Password; // todo (pekka): hashing
                    user.Email = model.Email;

                    this.session.Save(user);
                    tx.Commit();
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}