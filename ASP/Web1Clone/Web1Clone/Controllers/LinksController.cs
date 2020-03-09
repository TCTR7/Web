using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web1Clone.Models;
using PagedList;

namespace Web1Clone.Controllers
{
    public class LinksController : Controller
    {
        private WEB1Entities db = new WEB1Entities();

        // GET: Links
        public ActionResult Index(int? sizePage, int? page, string searchString, string sortProperty, string sortOrder)
        {
            sizePage = sizePage ?? 5;
            int pageNumber = page ?? 1;

            List<SelectListItem> items = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "5", Value = "5"},
                new SelectListItem(){Text = "10", Value = "10"},
                new SelectListItem(){Text = "25", Value = "25"},
                new SelectListItem(){Text = "50", Value = "50"}
            };

            foreach (var item in items)
            {
                if (item.Value == sizePage.ToString()) item.Selected = true;
            }

            ViewBag.DropDownSizePage = items;
            ViewBag.PageNumber = pageNumber;

            var links = from l in db.Links select l;
            if (!String.IsNullOrEmpty(searchString))
            {
                links = links.Where(x => x.LinkName.Contains(searchString));
            }

            return View(links.OrderBy(x => x.LinkID).ToPagedList(pageNumber, (int)sizePage));
        }

        // GET: Links/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Link link = db.Links.Find(id);
            if (link == null)
            {
                return HttpNotFound();
            }
            return View(link);
        }

        // GET: Links/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Links/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LinkID,LinkName,LinkURL,LinkDescription,CategoryID")] Link link)
        {
            if (ModelState.IsValid)
            {
                db.Links.Add(link);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(link);
        }

        // GET: Links/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Link link = db.Links.Find(id);
            if (link == null)
            {
                return HttpNotFound();
            }
            return View(link);
        }

        // POST: Links/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LinkID,LinkName,LinkURL,LinkDescription,CategoryID")] Link link)
        {
            if (ModelState.IsValid)
            {
                db.Entry(link).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(link);
        }

        // GET: Links/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Link link = db.Links.Find(id);
            if (link == null)
            {
                return HttpNotFound();
            }
            return View(link);
        }

        // POST: Links/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Link link = db.Links.Find(id);
            db.Links.Remove(link);
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
