using MyFirstWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;

namespace MyFirstWebsite.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        // GET: Auth
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            var model = new LoginModel
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)//checks if the input fields have correct format
            {
                return View(model);//Returns the view with the input values so that the user doesn't have to retype again
            }
            //Checks whether the input is the same as those literals. Note: Never ever do this! This is just to demo the validation while we're not yet doing any database interaction
            if (model.Email == "admin@admin.com" && model.Password == "123456")
            {
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, "Alexey"),
                    new Claim(ClaimTypes.Email, "alexey@email.com"),
                    new Claim(ClaimTypes.Country, "Belarus")

                }, "ApplicationCookie");
                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                authManager.SignIn(identity);

                var redir = Redirect(GetRedirectUrl(model.ReturnUrl));
                return redir;
                //return Redirect(Url.Action("Index", "Home"));
            }
            ModelState.AddModelError("", "Invalid email or password");
            return View(model);
        }

        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                 var urlact = Url.Action("Index", "Home");
                return urlact;
            }
            return returnUrl;
        }

    }
    
}