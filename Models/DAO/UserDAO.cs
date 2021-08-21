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
            if(user.StudentCode != null)
            {
                if(db.Users.Where(m=>m.StudentCode == user.StudentCode).FirstOrDefault() ==null)
                {
                    try
                    {
                        db.Users.Add(user);
                        db.SaveChanges();
                        return user.UserID;
                    }
                    catch (Exception)
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                try
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    return user.UserID;
                }
                catch (Exception)
                {
                    return -1;
                }
            }    
           
        }
        public User GetUserByUsername(string username)
        {
            return db.Users.Where(m =>(m.StudentCode == username || m.Email == username) && m.Status == 1).FirstOrDefault();
        }
        public User GetUserByID(int id)
        {
            return db.Users.Where(m=>m.UserID == id && m.Status ==1).FirstOrDefault();
        }
        public List<User> GetListUserByClassName(string className)
        {
            return db.Users.Where(m=> m.Class.Name == className && m.Status == 1).ToList();
        }
        public bool ChangePassword(int userID ,string password,string passwordNew)
        {
            var user = db.Users.Find(userID);
            if(user != null)
            {
                if(user.Password.Trim() == password)
                {
                    user.Password = passwordNew;
                    db.SaveChanges();
                    return true;
                }    
            }
            return false;
        }
        public bool UpdateProfile(User model)
        {
            var user = db.Users.Find(model.UserID);
            if(user != null)
            {
                user.Birthday = model.Birthday;
                user.Gender = model.Gender;
                user.Phone = model.Phone;
                user.Email = model.Email;
                user.Address = model.Address;
                user.CityID = model.CityID;
                user.DistrictID = model.DistrictID;
                user.WardID = model.WardID;
                db.SaveChanges();
                return true;
            }
            return false;
        }
        public int GetClassIDByClassName(string className)
        {
            var result = db.Classes.Where(m => m.Name.Trim() == className && m.Status == 1).FirstOrDefault();
            if(result != null)
            {
                return result.ClassID;
            }
            return -1;
        }
        public List<Class> GetListClassByCondition(int facultyId, int year)
        {
            string year1 = year.ToString();
            if(year != 0 && facultyId !=0)
            {
                return db.Classes.Where(m => m.FacutyID == facultyId && m.Name.Substring(0,2).Equals(year1)).ToList();
            }
            else if(facultyId != 0)
            {
                return db.Classes.Where(m => m.FacutyID == facultyId).ToList();
            }
            else
            {
                return db.Classes.ToList();
            }
        }
        public List<User> GetListUser()
        {
            return db.Users.ToList();
        }
    }
}
