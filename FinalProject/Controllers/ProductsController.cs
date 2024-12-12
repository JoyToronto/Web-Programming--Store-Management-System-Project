using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class ProductsController : BaseController
    {
        private StoreContext db = new StoreContext();

        // GET: Products
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,Name,Description,Price,Stock")] Product product, HttpPostedFileBase productImage)
        {
            if (ModelState.IsValid)
            {
                if (productImage != null && productImage.ContentLength > 0)
                {
                    try
                    {
                        // Generate the upload directory if it doesn't exist
                        string uploadPath = Server.MapPath("~/Uploads");
                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }

                        // Save the uploaded file to the server
                        string fileName = Path.GetFileName(productImage.FileName);
                        string filePath = Path.Combine(uploadPath, fileName);
                        productImage.SaveAs(filePath);

                        // Save the relative file path in the database
                        product.ImagePath = "/Uploads/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "File upload failed: " + ex.Message);
                        return View(product);
                    }
                }

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,Name,Description,Price,Stock,ImagePath")] Product product, HttpPostedFileBase productImage)
        {
            if (ModelState.IsValid)
            {
                if (productImage != null && productImage.ContentLength > 0)
                {
                    try
                    {
                        // Generate the upload directory if it doesn't exist
                        string uploadPath = Server.MapPath("~/Uploads");
                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }

                        // Save the uploaded file to the server
                        string fileName = Path.GetFileName(productImage.FileName);
                        string filePath = Path.Combine(uploadPath, fileName);
                        productImage.SaveAs(filePath);

                        // Update the relative file path in the database
                        product.ImagePath = "/Uploads/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "File upload failed: " + ex.Message);
                        return View(product);
                    }
                }

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);

            // Optionally, delete the file from the server
            if (!string.IsNullOrEmpty(product.ImagePath))
            {
                string fullPath = Server.MapPath(product.ImagePath);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
