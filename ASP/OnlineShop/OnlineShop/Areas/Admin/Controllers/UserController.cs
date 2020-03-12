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
        private int _currentPage;
        private int _pageSize;
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            UserDAO dao = new UserDAO();
            ViewBag.PageSize = _pageSize = pageSize;
            ViewBag.CurrentPage = _currentPage = page;
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
                    
                    return RedirectToAction("Create", "User");
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

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var dao = new UserDAO();
            var user = dao.GetUserById(id);
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                UserDAO userDAO = new UserDAO();
                if (!String.IsNullOrEmpty(user.Password))
                {
                    user.Password = Common.Encryptor.MD5Hash(user.Password);
                }
                var result = userDAO.UpdateUser(user);
                if (result)
                {
                    ModelState.AddModelError("", "Edit thành công");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Edit không thành công");
                }
            }
            else
            {
                ModelState.AddModelError("", "error");
            }
            return View("Index");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var result = new UserDAO().DeleteUserById(id);
            return RedirectToAction("Index");
        }
    }
}