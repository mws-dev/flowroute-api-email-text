using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowrouteApi.DataModels;
using FlowrouteApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace FlowrouteApi.Controllers
{
    public class LoginController : Controller
    {
        private readonly DataContext _context;
        private readonly FlowrouteSettings _settings;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public LoginController(DataContext context, UserManager<User> u, SignInManager<User> s, IConfiguration configuration)
        {
            _settings = configuration.GetSection("Flowroute").Get<FlowrouteSettings>();
            _context = context;
            _userManager = u;
            _signInManager = s;
        }
        public IActionResult Index()
        {
            LoginModel login = new LoginModel()
            {
                ReturnUrl = Request.Query["ReturnUrl"]
            };
            if (TempData["LoginModel"] == null)
                TempData["LoginModel"] = JsonConvert.SerializeObject(login);

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel m)
        {
            if (!ModelState.IsValid)
            {
                TempData["LoginModel"] = JsonConvert.SerializeObject(m);
                TempData["LoginError"] = true;
                return View("Index");
            }

            var user = _context.Users.Where(x => x.UserName == m.UserName).FirstOrDefault();
            if (user == null)
            {
                TempData["LoginModel"] = JsonConvert.SerializeObject(m);
                TempData["LoginError"] = true;
                TempData["ErrorMessage"] = "Invalid credentials.";
                return View("Index");
            }

            var result = _signInManager.CheckPasswordSignInAsync(user, m.Password, false).Result;
            if (!result.Succeeded)
            {
                TempData["LoginModel"] = JsonConvert.SerializeObject(m);
                TempData["LoginError"] = true;
                TempData["ErrorMessage"] = "Invalid credentials.";
                return View("Index");
            }
            else
            {
                _signInManager.SignInAsync(user, false).Wait();
                
                if (Url.IsLocalUrl(m.ReturnUrl))
                    return Redirect(m.ReturnUrl);
                else
                    return Redirect("/");

                return Redirect(m.ReturnUrl);
            }
        }
    }
}
