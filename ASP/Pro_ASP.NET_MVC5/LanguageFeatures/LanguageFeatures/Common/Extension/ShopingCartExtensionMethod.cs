using LanguageFeatures.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace LanguageFeatures.Common.Extension
{
    public static class ShopingCartExtensionMethod 
    {
        public static decimal TotalPrice(this ShoppingCart cartParam)
        {
            decimal total = 0;
            foreach (var item in cartParam.Products)
            {
                total += item.Price;
            }
            return total;
        }

        public static decimal TotalPrice(this IEnumerable<Product> productEnum)
        {
            decimal total = 0;
            foreach (Product item in productEnum)
            {
                total += item.Price;
            }
            return total;
        }

        public static IEnumerable<Product> FilterByCategory(this IEnumerable<Product> productEnum, string categoryParam)
        {
            foreach (Product product in productEnum)
            {
                if (product.Category == categoryParam)
                {
                    yield return product;
                }
            }
        }

        public static IEnumerable<Product> Filter(this IEnumerable<Product> productEnum, Func<Product, bool> selectorParam)
        {
            foreach (Product item in productEnum)
            {
                if (selectorParam(item))
                {
                    yield return item;
                }
            }
        }

        public static Task<long?> GetPageLength()
        {
            HttpClient client = new HttpClient();
            var httpTask = client.GetAsync("http://apress.com");
            return httpTask.ContinueWith((Task<HttpResponseMessage> antecedent) => {
                return antecedent.Result.Content.Headers.ContentLength;
            });
        }
    }
}