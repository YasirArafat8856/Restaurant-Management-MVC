using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Restaurant.Models;
using PagedList;
namespace ResProject.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CustomersController : Controller
    {
        private MVC_RestaurantEntities db = new MVC_RestaurantEntities();

        
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetCustomer()
        {
            List<Customer> Lst = db.Customers.ToList();
            var jsObj = Lst.Select(s => new { Id = s.ID, Name = s.Cus_Name, Address = s.Cus_Address,Contact_NO=s.Cus_Contact_NO, Email = s.Cus_Email });
            return Json(new { data = jsObj }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Save(Customer customer)
        {
            db.Customers.Add(customer);
            if (db.SaveChanges() > 0)
            {
                return Json(new { result = customer });
            }
            else
            {
                return Json(new { result = "Failed" });
            }
        }
        [Authorize(Roles = "Admin")]
        public JsonResult Delete(int id)
        {
            db.Customers.Remove(db.Customers.Find(id));
            if (db.SaveChanges() > 0)
            {
                return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = "Failed" });
            }
        }
        public JsonResult GetByID(int id)
        {
            var customer = db.Customers.Where(s=>s.ID==id).Select(s=> new {
                Cus_Name=s.Cus_Name,
                Cus_Address=s.Cus_Address,
                Cus_Contact_NO=  s.Cus_Contact_NO,
                Cus_Email=  s.Cus_Email,
                ID= s.ID
            });
            return Json(customer, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(Customer customer)
        {
            db.Entry(customer).State = System.Data.Entity.EntityState.Modified;
            if (db.SaveChanges() > 0)
            {
                return Json(new { result = customer });
            }
            else
            {
                return Json(new { result = "Failed" });
            }
            return Json(new { result = "Invalid Object" });
        }
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Customer customer = db.Customers.Find(id);
        //    if (customer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(customer);
        //}


        //public ActionResult Create()
        //{
        //    return View();
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,Cus_Name,Cus_Address,Cus_Contact_NO,Cus_Email")] Customer customer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Customers.Add(customer);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(customer);
        //}


        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Customer customer = db.Customers.Find(id);
        //    if (customer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(customer);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,Cus_Name,Cus_Address,Cus_Contact_NO,Cus_Email")] Customer customer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(customer).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(customer);
        //}


        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Customer customer = db.Customers.Find(id);
        //    if (customer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(customer);
        //}


        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Customer customer = db.Customers.Find(id);
        //    db.Customers.Remove(customer);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
