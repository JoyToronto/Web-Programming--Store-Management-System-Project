using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.Controllers
{
    public class BaseController : Controller
    {
        private StoreContext db = new StoreContext();
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Fetch categories from the database
            var categories = db.Categories.ToList();

            // Pass categories to ViewBag (accessible in _Layout.cshtml)
            ViewBag.Categories = categories;

            base.OnActionExecuting(filterContext);
        }
    }
}