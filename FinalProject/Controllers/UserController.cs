using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using BCrypt.Net;
using System.Web.Mvc;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class UserController : BaseController
    {
        private StoreContext db = new StoreContext();

        // GET: User/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Check if the email is already registered
                if (db.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("", "Email is already registered.");
                    return View(user);
                }

                if (user.Role != "Admin")
                {
                    user.Role = "User"; // Regular user
                }

                // Add user to the database
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password); // Hash password
                db.Users.Add(user);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Registration successful! Please log in.";
                return RedirectToAction("Login");
            }

            return View(user);
        }

        // GET: User/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == email);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                // Store user information in session
                Session["UserID"] = user.UserID;
                Session["UserName"] = user.Username;

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View();
        }

        // GET: User/Logout
        public ActionResult Logout()
        {
            Session.Clear(); // Clear session
            return RedirectToAction("Login");
        }
    }
}
