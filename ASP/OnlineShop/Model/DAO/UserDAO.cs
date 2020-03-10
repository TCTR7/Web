using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.DAO
{
    public class UserDAO
    {
        public enum LoginState
        {
            WRONG_PASSWORD = -2,
            ACCOUNT_IS_LOCKED,
            DOES_NOT_EXITST_ACCOUNT,
            SUSSCES
        }

        private OnlineShopDBContext db = null;
        public UserDAO()
        {
            db = new OnlineShopDBContext();
        }
        public long Insert(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            return user.ID;
        }

        public LoginState Login(string userName, string password)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == userName);
            if (result == null)
            {
                return LoginState.DOES_NOT_EXITST_ACCOUNT;
            }
            else
            {
                if (result.Status == false)
                {
                    return LoginState.ACCOUNT_IS_LOCKED;
                }
                else if (result.Password != password)
                {
                    return LoginState.WRONG_PASSWORD;
                }
            }
            return LoginState.SUSSCES;
        }

        public User GetUserByName(string userName)
        {
            return db.Users.SingleOrDefault(x => x.UserName == userName);
        }
    }
}
