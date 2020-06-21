using CoursesApp.Data;
using CoursesApp.Models;
using CoursesApp.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CoursesApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<MyIdentityUser> userManager;
        private readonly TraineeService traineeService;
        public AccountController()
        {
            var db = new CoursesIdentityContext();

            var userStore = new UserStore<MyIdentityUser>(db);
            userManager = new UserManager<MyIdentityUser>(userStore);

            traineeService = new TraineeService();

        }

        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl = "")
        {
            return View(new LoginViewModel
            {
                ReturnUrl = ReturnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel loginData)
        {
            if (ModelState.IsValid)
            {
                var existsUser = await userManager.FindAsync(loginData.Email, loginData.Password);

                if (existsUser != null)
                {
                    await SignIn(existsUser);

                    // Business
                    if (!string.IsNullOrEmpty(loginData.ReturnUrl))
                    {
                        return Redirect(loginData.ReturnUrl);
                    }

                    var userRoles = userManager.GetRoles(existsUser.Id);
                    var role = userRoles.FirstOrDefault();
                    if (role == "Admin")
                    {
                        return RedirectToAction("Index", "Default", new { area = "Admin" });
                    }

                    return RedirectToAction("Index", "Default");
                }

                loginData.Message = "Email or Password is incorrect!";
            }
            return View(loginData);
        }

        private async Task SignIn(MyIdentityUser myIdentityUser)
        {
            var identity = await userManager.CreateIdentityAsync(myIdentityUser, DefaultAuthenticationTypes.ApplicationCookie);

            var owinContext = Request.GetOwinContext();
            var authManager = owinContext.Authentication;
            authManager.SignIn(identity);
        }


        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel userInfo)
        {
            if (ModelState.IsValid)
            {
                var identityUser = new MyIdentityUser
                {
                    Email = userInfo.Email,
                    UserName = userInfo.Email
                };
                var creationResult = await userManager.CreateAsync(identityUser, userInfo.Password);

                // User Created
                if (creationResult.Succeeded)
                {
                    var userId = identityUser.Id;
                    creationResult = userManager.AddToRole(userId, "Trainee");

                    // Role Assigned
                    if (creationResult.Succeeded)
                    {
                        // Save to Trainee Table
                        var savedTrainee = traineeService.Create(new Trainee
                        {
                            Email = userInfo.Email,
                            Name = userInfo.Name,
                            Is_Active = true,
                            Creation_Date = DateTime.Now
                        });

                        if (savedTrainee == null)
                        {
                            userInfo.Message = "An Error while creating your account!";
                            return View(userInfo);
                        }
                        return RedirectToAction("Index", "Default");
                    }
                }

                var message = creationResult.Errors.FirstOrDefault();

                userInfo.Message = message;
                return View(userInfo);
            }

            return View(userInfo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            var owinContext = Request.GetOwinContext();
            var authManager = owinContext.Authentication;
            authManager.SignOut("ApplicationCookie");
            Session.Abandon();

            return RedirectToAction("Index", "Default");
        }
    }
}