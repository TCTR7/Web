using Model.DAO;
using Model.EF;
using OnlineShop.Areas.Admin.Models;
using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                UserDAO userDAO = new UserDAO();
                var isValid = userDAO.Login(loginModel.UserNameOrEmail, Common.Encryptor.MD5Hash(loginModel.PassWord));
                switch (isValid)
                {
                    case UserDAO.LoginState.WRONG_PASSWORD:
                        ModelState.AddModelError("", "Mật khẩu không đúng");
                        break;
                    case UserDAO.LoginState.ACCOUNT_IS_LOCKED:
                        ModelState.AddModelError("", "Tài khoản đang bị khóa");
                        break;
                    case UserDAO.LoginState.DOES_NOT_EXITST_ACCOUNT:
                        ModelState.AddModelError("", "Tài khoản không tồn tại");
                        break;
                    case UserDAO.LoginState.SUSSCES:
                        User user = userDAO.GetUserByName(loginModel.UserNameOrEmail);
                        UserLogin currentUserLogin = new UserLogin()
                        {
                            UserID = user.ID,
                            UserName = user.Name
                        };
                        Session.Add(Common.CommonConstants.USER_LOGIN_SESSION, currentUserLogin);
                        return RedirectToAction("Index", "HomeAdmin");
                    default:
                        ModelState.AddModelError("", "Đăng nhập thất bại");
                        break;
                }
            }
            else
            {
                var error = ModelState.Values.SelectMany(v => v.Errors); // to debug
            }
            return View("Index");
        }
    }
}