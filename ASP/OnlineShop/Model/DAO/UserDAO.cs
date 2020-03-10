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
        private static UserDAO _intance;
        public static UserDAO Instance
        {
            get { if (_intance == null) _intance = new UserDAO(); return _intance; }
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

        public bool Login(string userName, string password)
        {
            var result = db.Users.Count(x => x.UserName == userName && x.Password == password);
            return result > 0;
        }
    }
}
