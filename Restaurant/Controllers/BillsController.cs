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
    public class BillsController : Controller
    {
        private MVC_RestaurantEntities db = new MVC_RestaurantEntities();

        
        public ActionResult Index()
        {
            //var bills = db.Bills.Include(b => b.Food_Order);
            //return View(bills.ToList());
            return View();
        }
        public PartialViewResult GetCus()
        {
            var bills = db.Bills.Include(b => b.Food_Order);
            return PartialView(bills.ToList());
        }
        public PartialViewResult CreateCus()
        {
            ViewBag.Order_Id = new SelectList(db.Food_Order, "ID", "ID");
            
            return PartialView();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        // GET: Bills/Create
        public ActionResult Create()
        {
            ViewBag.Order_Id = new SelectList(db.Food_Order, "ID", "ID");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Order_Id,Total_Price,Payment_Method")] Bill bill)
        {
            if (ModelState.IsValid)
            {
                db.Bills.Add(bill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Order_Id = new SelectList(db.Food_Order, "ID", "ID", bill.Order_Id);
            return View(bill);
        }

        // GET: Bills/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            ViewBag.Order_Id = new SelectList(db.Food_Order, "ID", "ID", bill.Order_Id);
            return View(bill);
        }

        // POST: Bills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Order_Id,Total_Price,Payment_Method")] Bill bill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Order_Id = new SelectList(db.Food_Order, "ID", "ID", bill.Order_Id);
            return View(bill);
        }

        [Authorize(Roles = "Admin", Users = "y@y.com")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        [Authorize(Roles = "Admin", Users = "y@y.com")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bill bill = db.Bills.Find(id);
            db.Bills.Remove(bill);
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
