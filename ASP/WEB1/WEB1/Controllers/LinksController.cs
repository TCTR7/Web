using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WEB1.Models;
using System.Linq.Dynamic;
using System.Reflection;

namespace WEB1.Controllers
{
    public class LinksController : Controller
    {
        private WEB1Entities1 db = new WEB1Entities1();

        public class ButtonClickAttribute : ActionNameSelectorAttribute
        {
            public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
            {
                if (actionName.Equals(methodInfo.Name, StringComparison.InvariantCultureIgnoreCase)) return true;
                var request = controllerContext.HttpContext.Request;
                return request[methodInfo.Name] != null;
            }
        }

        // GET: Links
        public ActionResult Index(string searchString, string sortOrder, string sortProperty, int categoryID = 0)
        {
            ViewBag.SearchValue = searchString;
            if (sortOrder == ("asc")) ViewBag.NameSortParm = "desc";
            if (sortOrder == ("desc")) ViewBag.NameSortParm = "";
            if (sortOrder == ("")) ViewBag.NameSortParm = "asc";
            var categories = from c in db.Categories select c;
            ViewBag.categoryID = new SelectList(categories, "CategoryID", "CategoryName");

            var links = from l in db.Links
                        join c in db.Categories on l.CategoryID equals c.CategoryID
                        select new { l.LinkID, l.LinkName, l.LinkURL, l.LinkDescription, l.CategoryID, c.CategoryName };

            OrderData(sortOrder, sortProperty);

            if (String.IsNullOrEmpty(sortProperty))
            {
                sortProperty = "LinkID";
            }

            if (categoryID != 0)
            {
                links = links.Where(x => x.CategoryID == categoryID);
            }

            if (sortOrder == "desc")
            {
                links = links.OrderBy(sortProperty + " desc");
            }
            else if (sortOrder == "asc")
            {
                links = links.OrderBy(sortProperty);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                links = links.Where(x => x.LinkName.Contains(searchString));
            }

            List<Link> listLink = new List<Link>();
            foreach (var item in links)
            {
                Link link = new Link()
                {
                    LinkID = item.LinkID,
                    LinkName = item.LinkName,
                    LinkURL = item.LinkURL,
                    LinkDescription = item.LinkDescription,
                    CategoryID = item.CategoryID,
                    CategoryName = item.CategoryName
                };
                listLink.Add(link);
            }

            return View(listLink);
        }

        private void OrderData(string sortOrder, string sortProperty)
        {
            var properties = typeof(Link).GetProperties();

            List<Tuple<string, bool, int>> listTupple = new List<Tuple<string, bool, int>>();
            foreach (var item in properties)
            {
                int order = 0;
                bool isVirtual = item.GetAccessors()[0].IsVirtual;
                if (item.Name.Equals("LinkID")) order = 1;
                if (item.Name.Equals("LinkName")) order = 2;
                if (item.Name.Equals("CategoryID")) order = 3;
                if (item.Name.Equals("LinkURL")) order = 4;
                if (item.Name.Equals("LinkDescription")) order = 5;
                if (item.Name.Equals("CategoryName")) continue;
                Tuple<string, bool, int> tuple = new Tuple<string, bool, int>(item.Name, isVirtual, order);
                listTupple.Add(tuple);
            }

            listTupple = listTupple.OrderBy(x => x.Item3).ToList();

            foreach (var item in listTupple)
            {
                if (!item.Item2)
                {
                    if (sortOrder == "desc" && sortProperty == item.Item1)
                    {
                        ViewBag.Headings += "<th><a href='?sortProperty=" + item.Item1 + "&sortOrder=" + ViewBag.NameSortParm + "&searchString=" + ViewBag.SearchValue + "'>" + item.Item1 + "<i class='fa fa-fw fa-sort-desc'/></a></th>";
                    }
                    else if (sortOrder == "asc" && sortProperty == item.Item1)
                    {
                        ViewBag.Headings += "<th><a href='?sortProperty=" + item.Item1 + "&sortOrder=" + ViewBag.NameSortParm + "&searchString=" + ViewBag.SearchValue + "'>" + item.Item1 + "<i class='fa fa-fw fa-sort-asc'/></a></th>";
                    }
                    else
                    {
                        ViewBag.Headings += "<th><a href='?sortProperty=" + item.Item1 + "&sortOrder=" + ViewBag.NameSortParm + "&searchString=" + ViewBag.SearchValue + "'>" + item.Item1 + "<i class='fa fa-fw fa-sort'/></a></th>";
                    }
                }
                else
                {
                    ViewBag.Headings += "<th>" + item.Item1 + "</th>";
                }
            }
        }
        [HttpPost, ButtonClick]
        public ActionResult Reset()
        {
            ViewBag.SearchValue = "";
            Index("", "", "");
            return View();
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
