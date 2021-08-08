using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.EntityModel;

namespace Models.DAO
{
    public class UserDAO
    {
        private DBContext db = new DBContext();
        public int Insert(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            return user.UserID;
        }
        public User GetUserByUsername(string username)
        {
            return db.Users.Where(m =>(m.StudentCode == username || m.Email == username) && m.Status == 1).FirstOrDefault();
        }
        public User GetUserByID(int id)
        {
            return db.Users.Where(m=>m.UserID == id && m.Status ==1).FirstOrDefault();
        }

    }
}
