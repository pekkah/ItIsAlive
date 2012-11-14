namespace ItIsAlive.Samples.ContactsWeb.Controllers
{
    using System.Web.Mvc;
    using System.Web.Security;
    using Database.Entities;
    using Filters;
    using Models;
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
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                bool changePasswordSucceeded = false;

                var identityName = Request.RequestContext.HttpContext.User.Identity.Name;

                var user = unitOfWork.QueryOver<User>().Where(u => u.Name == identityName).SingleOrDefault();

                if (user != null)
                {
                    user.Password = model.ConfirmPassword;
                    changePasswordSucceeded = true;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }

                ModelState.AddModelError(
                    string.Empty, "The current password is incorrect or the new password is invalid.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User {Name = model.UserName, Password = model.Password, Email = model.Email};

                user.AddContact("Somebody");

                unitOfWork.Save(user);

                return RedirectToAction("Index", "Home");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private bool ValidateUser(string userName, string password)
        {
            var user = unitOfWork.QueryOver<User>().Where(u => u.Name == userName).SingleOrDefault();

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