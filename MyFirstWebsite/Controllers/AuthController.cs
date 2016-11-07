using MyFirstWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using MyFirstWebsite.CustomLibraries; 

namespace MyFirstWebsite.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        // GET: Auth
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // GET: Home
        [HttpGet]
        public ActionResult Index()
        {
            MainDbContext db = new MainDbContext();
            return View(db.Lists.Where(x => x.Public == "YES").ToList());
        }

        [HttpPost]
        public ActionResult Login(Users model)
        {
            if (!ModelState.IsValid)//checks if the input fields have correct format
            {
                return View(model);//Returns the view with the input values so that the user doesn't have to retype again
            }
            using (var db = new MainDbContext())
            {
                var emailCheck = db.Users.FirstOrDefault(u => u.Email == model.Email);
                if (emailCheck != null)
                {
                    var getPassword = db.Users.Where(u => u.Email == model.Email).Select(u => u.Password);
                    var materializePassword = getPassword.ToList();
                    var password = materializePassword[0];
                    var decryptedPassword = CustomDecrypt.Decrypt(password);

                    //encrypt pass to check
                    var sentPassword = CustomEncrypt.Encrypt(model.Password);

                    //Checks whether the input is the same as those literals. Note: Never ever do this! This is just to demo the validation while we're not yet doing any database interaction
                    if (model.Email != null && sentPassword == password)
                    {
                        var getName = db.Users.Where(u => u.Email == model.Email).Select(u => u.Name);
                        var materializeName = getName.ToList();
                        var name = materializeName[0];

                        var getCountry = db.Users.Where(u => u.Email == model.Email).Select(u => u.Country);
                        var materializeCountry = getCountry.ToList();
                        var country = materializeCountry[0];

                        var getEmail = db.Users.Where(u => u.Email == model.Email).Select(u => u.Email);
                        var materializeEmail = getEmail.ToList();
                        var email = materializeEmail[0];

                        var identity = new ClaimsIdentity(new[]
                        {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Country, country)

                }, "ApplicationCookie");
                        var ctx = Request.GetOwinContext();
                        var authManager = ctx.Authentication;
                        authManager.SignIn(identity);

                        return RedirectToAction("Index", "Home");

                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please register yourself");
                    return View();
                }
                ModelState.AddModelError("", "Invalid email or password");
                return View(model);
            }
        }



        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(Users model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new MainDbContext())
                {
                    var queryUser = db.Users.FirstOrDefault(u => u.Email == model.Email);
                    if (queryUser == null)
                    {
                        var encryptedPassword = CustomEncrypt.Encrypt(model.Password);
                        var user = db.Users.Create();
                        user.Email = model.Email;
                        user.Password = encryptedPassword;
                        user.Country = model.Country;
                        user.Name = model.Name;

                        db.Users.Add(user);
                        db.SaveChanges();
                    }
                    else
                    {
                        return RedirectToAction("Registration");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "One or more fields have been");
            }
            return View();
        }
    }
    
}