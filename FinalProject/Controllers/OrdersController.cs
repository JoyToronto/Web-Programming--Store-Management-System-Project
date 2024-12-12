using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class OrdersController : BaseController
    {
        private StoreContext db = new StoreContext();

        // GET: Orders
        public ActionResult Index(string customerName, DateTime? startDate, DateTime? endDate, decimal? minAmount, decimal? maxAmount)
        {
            var orders = db.Orders.Include(o => o.Customer);

            if (!string.IsNullOrEmpty(customerName))
                orders = orders.Where(o => o.Customer.Name.Contains(customerName));

            if (startDate.HasValue)
                orders = orders.Where(o => o.OrderDate >= startDate.Value);

            if (endDate.HasValue)
                orders = orders.Where(o => o.OrderDate <= endDate.Value);

            if (minAmount.HasValue)
            {
                orders = orders.Where(o => o.TotalAmount >= minAmount.Value);
            }

            if (maxAmount.HasValue)
            {
                orders = orders.Where(o => o.TotalAmount <= maxAmount.Value);
            }

            ViewBag.CustomerName = customerName;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            ViewBag.MinAmount = minAmount;
            ViewBag.MaxAmount = maxAmount;

            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.Products = new SelectList(db.Products, "ProductID", "Name");
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "Name");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,CustomerID,OrderDate")] Order order, List<OrderProduct> orderProducts)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            foreach (var orderProduct in orderProducts)
            {
                orderProduct.OrderID = order.OrderID; // Link to the order
                db.OrderProducts.Add(orderProduct);
            }
            db.SaveChanges();
            return RedirectToAction("Index");

            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "Name", order.CustomerID);
            ViewBag.Products = new SelectList(db.Products, "ProductID", "Name");
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "Name", order.CustomerID);
            ViewBag.Products = new SelectList(db.Products, "ProductID", "Name");
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,CustomerID,OrderDate")] Order order, List<OrderProduct> orderProducts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                // Update existing OrderProducts
                foreach (var orderProduct in orderProducts)
                {
                    if (orderProduct.OrderProductID == 0) // New OrderProduct
                    {
                        orderProduct.OrderID = order.OrderID;
                        db.OrderProducts.Add(orderProduct);
                    }
                    else // Existing OrderProduct
                    {
                        db.Entry(orderProduct).State = EntityState.Modified;
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "Name", order.CustomerID);
            ViewBag.Products = new SelectList(db.Products, "ProductID", "Name");
            return View(order);
        }

        // GET: Orders/History
        public ActionResult History(string customerName, DateTime? startDate, DateTime? endDate, decimal? minAmount, decimal? maxAmount)
        {
            var orders = db.Orders.Include(o => o.Customer).Include(o => o.OrderProducts);

            // Filter by Customer Name
            if (!string.IsNullOrEmpty(customerName))
            {
                orders = orders.Where(o => o.Customer.Name.Contains(customerName));
            }

            // Filter by Order Date Range
            if (startDate.HasValue)
            {
                orders = orders.Where(o => o.OrderDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                orders = orders.Where(o => o.OrderDate <= endDate.Value);
            }

            // Filter by Total Amount Range
            if (minAmount.HasValue)
            {
                orders = orders.Where(o => o.TotalAmount >= minAmount.Value);
            }

            if (maxAmount.HasValue)
            {
                orders = orders.Where(o => o.TotalAmount <= maxAmount.Value);
            }

            ViewBag.CustomerName = customerName;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.MinAmount = minAmount?.ToString();
            ViewBag.MaxAmount = maxAmount?.ToString();

            return View(orders.ToList());
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
