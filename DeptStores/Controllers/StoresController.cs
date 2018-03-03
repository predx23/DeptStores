using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeptStores.Models;

namespace DeptStores.Controllers
{
    public class StoresController : Controller
    {
        private DepartmentStoresEntities db = new DepartmentStoresEntities();

        // GET: Stores
        public ActionResult Index(string location, string searchString)
        {
            //Search by location
            var locationList = new List<string>();

            var locationQuery = from data in db.stores
                                orderby data.Location
                                select data.Location;

            locationList.AddRange(locationQuery.Distinct());  //Without the Distinct modifier, duplicate locations would be added.
            ViewBag.location = new SelectList(locationList);

            var Stores = from s in db.stores select s;

            //Search by Name
            if (!string.IsNullOrEmpty(searchString))
            {
                Stores = Stores.Where(z => z.Name.Contains(searchString));
            }

            //Search by Location... continued
            if (!string.IsNullOrEmpty(location))
            {
                Stores = Stores.Where(x => x.Location == location);
            }

            return View(Stores);
        }

        public ActionResult Like(int id)
        {
            store store = db.stores.Find(id);

            int? currentLikes = store.Likes;
            store.Likes = currentLikes + 1;

            if (ModelState.IsValid)
            {
                db.Entry(store).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Dislike(int id)
        {
            store store = db.stores.Find(id);

            int? currentDislikes = store.Dislikes;
            store.Dislikes = currentDislikes + 1;

            if (ModelState.IsValid)
            {
                db.Entry(store).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        // GET: Stores/Details/5
        public ActionResult Details(int? id)
        {            
            store store = db.stores.Find(id);
            return View(store);
        }


        // GET: Stores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Picture,Name,Location,Description,Likes,Dislikes,Visitors_Frequency,Profit_Yearly,Number_of_stores,Added_by,Created_Date,Modified_Date,Pic_Courtesy_of")] store store)
        {
            if (store.Picture == null)
            {
                store.Picture = "https://upload.wikimedia.org/wikipedia/commons/e/e3/Wako1000.jpg";
            }

            if (store.Pic_Courtesy_of == null)
            {
                store.Pic_Courtesy_of = "Source not known";
            }

            DateTime dateNow = DateTime.UtcNow;
            store.Created_Date = dateNow;
            store.Modified_Date = dateNow;

            int? currentLikes = 0;
            store.Likes = currentLikes;
            store.Dislikes = currentLikes;

            if (ModelState.IsValid)
            {
                db.stores.Add(store);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(store);
        }

        // GET: Stores/Edit/5
        public ActionResult Edit(int? id)
        {            
            store store = db.stores.Find(id);
            return View(store);
        }

        // POST: Stores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Picture,Name,Location,Description,Likes,Dislikes,Visitors_Frequency,Profit_Yearly,Number_of_stores,Added_by,Created_Date,Modified_Date,Pic_Courtesy_of")] store store)
        {
            if (store.Picture == null)
            {
                store.Picture = "https://upload.wikimedia.org/wikipedia/commons/e/e3/Wako1000.jpg";
            }

            if (store.Pic_Courtesy_of == null)
            {
                store.Pic_Courtesy_of = "Unknown Author";
            }

            DateTime dateNow = DateTime.UtcNow;
            store.Modified_Date = dateNow;

            if (ModelState.IsValid)
            {
                db.Entry(store).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(store);
        }

        // GET: Stores/Delete/5
        public ActionResult Delete(int? id)
        {            
            store store = db.stores.Find(id);
            return View(store);
        }

        // POST: Stores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            store store = db.stores.Find(id);
            db.stores.Remove(store);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
              
    }
}