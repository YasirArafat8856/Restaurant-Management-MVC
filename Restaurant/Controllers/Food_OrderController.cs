using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Restaurant.Models;

namespace ResProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class Food_OrderController : Controller
    {
        private MVC_RestaurantEntities db = new MVC_RestaurantEntities();

        
        public ActionResult Index()
        {
            var food_Order = db.Food_Order.Include(f => f.Customer).Include(f => f.Item);
            return View(food_Order.ToList());
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food_Order food_Order = db.Food_Order.Find(id);
            if (food_Order == null)
            {
                return HttpNotFound();
            }
            return View(food_Order);
        }
        public PartialViewResult CreatePrc()
        {
            ViewBag.Cus_Id = new SelectList(db.Customers, "ID", "Cus_Name");
            ViewBag.Item_Id = new SelectList(db.Items, "ID", "Item_Name");
            return PartialView();
        }
        
        public ActionResult Create()
        {
            ViewBag.Cus_Id = new SelectList(db.Customers, "ID", "Cus_Name");
            ViewBag.Item_Id = new SelectList(db.Items, "ID", "Item_Name");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Cus_Id,Item_Id,OrderDate,Quantity,Total_Price")] Food_Order food_Order)
        {
            if (ModelState.IsValid)
            {
                db.Food_Order.Add(food_Order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Cus_Id = new SelectList(db.Customers, "ID", "Cus_Name", food_Order.Cus_Id);
            ViewBag.Item_Id = new SelectList(db.Items, "ID", "Item_Name", food_Order.Item_Id);
            return View(food_Order);
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food_Order food_Order = db.Food_Order.Find(id);
            if (food_Order == null)
            {
                return HttpNotFound();
            }
            ViewBag.Cus_Id = new SelectList(db.Customers, "ID", "Cus_Name", food_Order.Cus_Id);
            ViewBag.Item_Id = new SelectList(db.Items, "ID", "Item_Name", food_Order.Item_Id);
            return View(food_Order);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Cus_Id,Item_Id,OrderDate,Quantity,Total_Price")] Food_Order food_Order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(food_Order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Cus_Id = new SelectList(db.Customers, "ID", "Cus_Name", food_Order.Cus_Id);
            ViewBag.Item_Id = new SelectList(db.Items, "ID", "Item_Name", food_Order.Item_Id);
            return View(food_Order);
        }

        [Authorize(Roles = "Admin", Users = "y@y.com")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food_Order food_Order = db.Food_Order.Find(id);
            if (food_Order == null)
            {
                return HttpNotFound();
            }
            return View(food_Order);
        }

        [Authorize(Roles = "Admin", Users = "y@y.com")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Food_Order food_Order = db.Food_Order.Find(id);
            db.Food_Order.Remove(food_Order);
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
