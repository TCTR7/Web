using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        // GET: Admin/User
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            UserDAO dao = new UserDAO();
            var model = dao.GetAllUser(page, pageSize);
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                UserDAO userDAO = new UserDAO();
                user.Password = Common.Encryptor.MD5Hash(user.Password);
                var result = userDAO.Insert(user);
                if (result > 0)
                {
                    
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm không thành công");
                }
            }
            else
            {
                ModelState.AddModelError("", "error");
            }
            return View("Index");
        }
    }
}