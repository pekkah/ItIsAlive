namespace Bootstrapper.Samples.ContactsWeb.Controllers
{
    using System.Web.Mvc;
    using System.Web.Security;

    using Bootstrapper.Samples.ContactsWeb.Database.Entities;
    using Bootstrapper.Samples.ContactsWeb.Filters;
    using Bootstrapper.Samples.ContactsWeb.Models;

    using NHibernate;

    [MvcUnitOfWork]
    public class AccountController : Controller
    {
        private readonly ISession unitOfWork;

        public AccountController(ISession unitOfWork)
        {
            this.unitOfWork = unitOfWork;
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

                var identityName = this.Request.RequestContext.HttpContext.User.Identity.Name;

                var user = this.unitOfWork.QueryOver<User>().Where(u => u.Name == identityName).SingleOrDefault();

                if (user != null)
                {
                    user.Password = model.ConfirmPassword;
                    changePasswordSucceeded = true;
                }

                if (changePasswordSucceeded)
                {
                    return this.RedirectToAction("ChangePasswordSuccess");
                }

                this.ModelState.AddModelError(
                    string.Empty, "The current password is incorrect or the new password is invalid.");
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
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
            return this.View(model);
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
                var user = new User { Name = model.UserName, Password = model.Password, Email = model.Email };

                user.AddContact("Somebody");

                this.unitOfWork.Save(user);

                return this.RedirectToAction("Index", "Home");
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        private bool ValidateUser(string userName, string password)
        {
            var user = this.unitOfWork.QueryOver<User>().Where(u => u.Name == userName).SingleOrDefault();

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

            return true;
        }
    }
}