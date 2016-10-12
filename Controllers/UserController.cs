using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Users.Models;
using DapperApp.Factory;
using Microsoft.AspNetCore.Identity;

namespace Users.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserRepository userFactory;

        public UsersController()
        {
            //Instantiate a UserFactory object that is immutable (READONLY)
            //This is establish the initial DB connection for us.
            userFactory = new UserRepository();
        }
        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            if(TempData["errors"] != null)
            {
                ViewBag.errors = TempData["errors"]; 
            }
            return View("index");
        }

        [HttpPost]
        [Route("/register")]
        public IActionResult Register(User user)
        {   
            if(ModelState.IsValid)
            {
                 PasswordHasher<User> Hasher = new PasswordHasher<User>();
                 user.password = Hasher.HashPassword(user, user.password);
                 userFactory.Create(user);
                 return RedirectToAction("Users");
            }
            Console.WriteLine("REGISTER errors!!!!!!");
            List<string> errors = new List<string>();
            foreach(var error in ModelState.Values)
            {
                if(error.Errors.Count > 0)
                {
                    errors.Add(error.Errors[0].ErrorMessage);
                }
            }
            errors.Reverse();
            TempData["errors"] = errors;
            Console.WriteLine("Here are the REGISTER errors: " + errors);
            return RedirectToAction("Index"); 
        }
        
        [HttpGet]
        [Route("/users")] 
        public IActionResult Users() 
        {
            ViewBag.users = userFactory.FindAll();
            ViewBag.name = TempData["name"];
            return View("users");
        }
        [HttpPost]
        [Route("/login")]
        public IActionResult Login(User user) 
        {
            Console.WriteLine("Inputted email: " + user.email);
            User found_user = userFactory.FindByEmail(user);

            if(found_user != null && user.password != null)
            {
                Console.WriteLine("A user was found in the DB: " + found_user + ", and the password was entered in. Now verifying password");
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                if(0 != hasher.VerifyHashedPassword(found_user, found_user.password, user.password))
                {
                    TempData["name"] = found_user.first_name;  
                    Console.WriteLine("The passwords matched! " + found_user.first_name + " logged in successfully!");
                    return RedirectToAction("Users");
                } 
                Console.WriteLine("The user was found: " + found_user.email + ", but the passwords didn't match");
                return RedirectToAction("Index");
            }
            else
            {
                Console.WriteLine("User not found, redirecting to index");
                return RedirectToAction("Index");
            }
        }
    }
}


