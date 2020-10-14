using Restaurant.Models;
using Restaurant.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net;

namespace ResProject.Controllers
{
    public class ItemController : Controller
    {
        // GET: Item
        MVC_RestaurantEntities db = new MVC_RestaurantEntities();
        [Authorize]
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            try
            {
                ViewBag.NameSortParam = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.PriceSortParam = sortOrder == "Price" ? "Price_desc" : "Price";
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }
                ViewBag.CurrentFilter = searchString;
                var Item = from i in db.Items select i;
                if (!String.IsNullOrEmpty(searchString))
                {
                    Item = Item.Where(c => c.Item_Name.ToUpper().Contains(searchString.ToUpper()));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        Item = Item.OrderByDescending(c => c.Item_Name);
                        break;
                    case "Price_desc":
                        Item = Item.OrderByDescending(c => c.Unit_Price);
                        break;
                    case "Address":
                        Item = Item.OrderBy(c => c.Unit_Price);
                        break;
                    default:
                        Item = Item.OrderBy(c => c.Item_Name);
                        break;
                }
                ViewBag.CurrentSort = sortOrder;
                int pageSize = 3;
                int pageNumber = (page ?? 1);
                return View(Item.ToPagedList(pageNumber, pageSize));

            }
            catch (Exception ex)
            {
                
            }
            return View();
        }
        public ActionResult AllItem(string sortOrder, string searchString, string currentFilter, int? page)
        {
            try
            {
                ViewBag.NameSortParam = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.PriceSortParam = sortOrder == "Price" ? "Price_desc" : "Price";
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }
                ViewBag.CurrentFilter = searchString;
                var Item = from i in db.Items select i;
                if (!String.IsNullOrEmpty(searchString))
                {
                    Item = Item.Where(c => c.Item_Name.ToUpper().Contains(searchString.ToUpper()));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        Item = Item.OrderByDescending(c => c.Item_Name);
                        break;
                    case "Price_desc":
                        Item = Item.OrderByDescending(c => c.Unit_Price);
                        break;
                    case "Address":
                        Item = Item.OrderBy(c => c.Unit_Price);
                        break;
                    default:
                        Item = Item.OrderBy(c => c.Item_Name);
                        break;
                }
                ViewBag.CurrentSort = sortOrder;
                int pageSize = 3;
                int pageNumber = (page ?? 1);
                return View(Item.ToPagedList(pageNumber, pageSize));

            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
        public PartialViewResult CreatePrc()
        {
            return PartialView();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(ItemVm vm)
        {
            Item item = new Item();
            if (ModelState.IsValid)
            {
                if (vm.ImgFile != null)
                {
                    string ext = Path.GetExtension(vm.ImgFile.FileName).ToLower();
                    if(ext==".jpg"|| ext == ".jpeg" || ext == ".png")
                    {
                        string fileName = Path.GetFileNameWithoutExtension(vm.ImgFile.FileName)+"_"+Guid.NewGuid().ToString()+ext;
                        vm.ImgFile.SaveAs(Path.Combine(Server.MapPath("~/Photo"), fileName));
                        item.Picture = "~/Photo/" + fileName;
                        item.Item_Name = vm.Item_Name;
                        item.Unit_Price = vm.Unit_Price;
                        item.ID = vm.Id;
                    }
                }
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            var tbl = db.Items.Find(id);
            ItemVm vm = new ItemVm 
            { 
                Id = tbl.ID, 
                Item_Name = tbl.Item_Name, 
                Unit_Price = tbl.Unit_Price, 
                Picture = tbl.Picture 
            };
            return View(vm);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(ItemVm vm)
        {
            Item item = new Item();
            if (ModelState.IsValid)
            {
                
                    if (vm.ImgFile != null)
                    {
                        string ext = Path.GetExtension(vm.ImgFile.FileName).ToLower();
                        if (ext == ".jpg" || ext == ".jpeg" || ext == ".png")
                        {
                            string fileName = Path.GetFileNameWithoutExtension(vm.ImgFile.FileName) + "_" + Guid.NewGuid().ToString() + ext;
                            vm.ImgFile.SaveAs(Path.Combine(Server.MapPath("~/Photo"), fileName));
                            //vm.Picture = "~/Photo/" + fileName;
                            item.Picture= "~/Photo/" + fileName;
                        }
                    }
                    else
                    {
                        item.Picture = vm.Picture;
                    }
                    item.ID = vm.Id;
                    item.Item_Name = vm.Item_Name;
                    item.Unit_Price = vm.Unit_Price;
                    //item.Picture = vm.Picture;
                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                
            }
            return View(item);
        }
        [Authorize(Roles = "Super Admin")]
        public ActionResult Delete(int? id)
        {
            var i = db.Items.Find(id);
            db.Items.Remove(i);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}