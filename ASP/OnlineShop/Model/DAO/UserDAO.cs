using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using PagedList;

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

        public IEnumerable<User> GetAllUser(string searchString, int page, int pageSize)
        {
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                return db.Users.Where(x => x.UserName.Contains(searchString) || x.Name.Contains(searchString))
                    .OrderBy(x => x.CreatedDate).ToPagedList(page, pageSize);
            }
            return db.Users.OrderBy(x => x.CreatedDate).ToPagedList(page, pageSize); 
        }

        public bool UpdateUser(User entity)
        {
            try
            {
                var user = db.Users.Find(entity.ID);
                user.Name = entity.Name;
                user.Password = entity.Password;
                user.Phone = entity.Phone;
                user.Email = entity.Email;
                user.ModifiedDate = entity.ModifiedDate;
                user.ModifiedBy = entity.ModifiedBy;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public User GetUserById(int id)
        {
            return db.Users.Find(id);
        }

        public bool IsExitstUser(User entity)
        {
            return db.Users.Contains(entity);
        }

        public bool DeleteUserById(int id)
        {
            try
            {
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
