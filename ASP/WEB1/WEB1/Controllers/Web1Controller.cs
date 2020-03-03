using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEB1.Controllers
{
    public class Web1Controller : Controller
    {
        // GET: Web1
        public ActionResult Index()
        {
            return View();
        }

        public string ChaoMung(string ten, int tuoi = 1)
        {
            return HttpUtility.HtmlEncode("Xin chào " + ten + ". Tuổi của bạn là : " + tuoi);
        }

        public ActionResult Hello(string name, int numTimes = 1)
        {
            ViewBag.Message = "Hello " + name;
            ViewBag.NumTimes = numTimes;
            return View();
        }
    }
}