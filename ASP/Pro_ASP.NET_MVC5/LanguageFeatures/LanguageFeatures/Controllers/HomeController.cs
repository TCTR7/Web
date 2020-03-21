using LanguageFeatures.Common.Extension;
using LanguageFeatures.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageFeatures.Controllers
{
    public class HomeController : Controller
    {
        public string Index()
        {
            return "Navigate to a URL to show an example";
        }

        public ActionResult AutoProperty()
        {
            Product product = new Product();
            product.Name = "Kayak";
            string productName = product.Name;
            return View("Result", (object)String.Format("Product Name: {0}", productName));
        }

        public ActionResult CreateProduct()
        {
            Product product = new Product()
            {
                ProductID = 100,
                Name = "TCT",
                Description = "A boat for one person",
                Price = 275M,
                Category = "Watersports"
            };
            return View("Result", (object)String.Format("Category: {0}", product.Category));
        }

        public ActionResult UseExtension()
        {
            ShoppingCart cart = new ShoppingCart()
            {
                Products = new List<Product>()
                {
                    new Product(){ Name = "A", Price = 275M},
                    new Product(){ Name = "B", Price = 480.9M},
                    new Product(){ Name = "C", Price = 100.25M},
                    new Product(){ Name = "D", Price = 555M},
                }
            };
            decimal total = cart.TotalPrice();
            return View("Result", (object)String.Format("Total: {0}", total));
        }

        public ActionResult UseExtensionEnurable()
        {
            IEnumerable<Product> products = new ShoppingCart()
            {
                Products = new List<Product>()
                {
                    new Product(){ Name = "A", Price = 275M},
                    new Product(){ Name = "B", Price = 480.9M},
                    new Product(){ Name = "C", Price = 100.25M},
                    new Product(){ Name = "D", Price = 555M},
                }
            };
            decimal total = products.TotalPrice();
            return View("Result", (object)String.Format("Total: {0}", total));
        }

        public ActionResult UseFilterExtensionMethod()
        {
            IEnumerable<Product> products = new ShoppingCart()
            {
                Products = new List<Product>()
                {
                    new Product {Name = "Kayak", Category = "Watersports", Price = 275M},
                    new Product {Name = "Lifejacket", Category = "Watersports", Price = 48.95M},
                    new Product {Name = "Soccer ball", Category = "Soccer", Price = 19.50M},
                    new Product {Name = "Corner flag", Category = "Soccer", Price = 34.95M}
                }
            };
            decimal total = products.FilterByCategory("Soccer").TotalPrice();
            return View("Result", (object)String.Format("Total: {0}", total));
        }

        public ActionResult UserFilterExtensionMethodWithDelegate()
        {
            IEnumerable<Product> products = new ShoppingCart()
            {
                Products = new List<Product>()
                {
                    new Product {Name = "Kayak", Category = "Watersports", Price = 275M},
                    new Product {Name = "Lifejacket", Category = "Watersports", Price = 48.95M},
                    new Product {Name = "Soccer ball", Category = "Soccer", Price = 19.50M},
                    new Product {Name = "Corner flag", Category = "Soccer", Price = 34.95M}
                }
            };
            Func<Product, bool> categoryFilter = pro => pro.Category == "Soccer";
            decimal total = products.Filter(categoryFilter).TotalPrice();
            return View("Result", (object)String.Format("Total: {0}", total));
        }
    }
}