namespace Sakura.Framework.Samples.ContactsWeb.Controllers
{
    using System.Web.Mvc;
    using System.Web.Security;

    using NHibernate;

    using Sakura.Framework.Dependencies;
    using Sakura.Framework.Dependencies.DefaultTypes;
    using Sakura.Framework.Samples.Contacts.Database.Entities;
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

                using (var tx = this.session.BeginTransaction())
                {
                    var identityName = this.Request.RequestContext.HttpContext.User.Identity.Name;

                    var user = this.session.QueryOver<User>().Where(u => u.Name == identityName).SingleOrDefault();

                    if (user != null)
                    {
                        user.Password = model.ConfirmPassword;
                        tx.Commit();
                        changePasswordSucceeded = true;
                    }
                }

                if (changePasswordSucceeded)
                {
                    return this.RedirectToAction("ChangePasswordSuccess");
                }

                this.ModelState.AddModelError(string.Empty, "The current password is incorrect or the new password is invalid.");
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
                if (this.ValidateUser(model.UserName, model.Password))
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

        private bool ValidateUser(string userName, string password)
        {
            using (var tx = this.session.BeginTransaction())
            {
                var user = this.session.QueryOver<User>().Where(u => u.Name == userName).SingleOrDefault();

                if (user == null)
                {
                    return false;
                }

                // todo password hashing
                var hashedPassword = password;

                if (user.Password != hashedPassword)
                {
                    return false;
                }

                tx.Commit();
                return true;
            }
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
                    var user = new User
                    {
                        Name = model.UserName,
                        Password = model.Password,
                        Email = model.Email
                    };

                    user.AddContact("Somebody");

                    this.session.Save(user);
                    tx.Commit();
                }

                return this.RedirectToAction("Index", "Home");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}