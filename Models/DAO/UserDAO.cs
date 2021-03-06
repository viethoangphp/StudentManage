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
                if(db.Users.Where(m=>m.StudentCode == user.StudentCode).FirstOrDefault()==null)
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
            var result = db.Classes.AsNoTracking().Where(m => m.Name.Trim() == className && m.Status == 1).FirstOrDefault();
            if(result != null)
            {
                return result.ClassID;
            }
            return -1;
        }
        public List<Class> GetListClassByCondition(int facultyId, int year)
        {
            string year1 = year.ToString();
            if (year != 0 && facultyId != 0)
            {
                return db.Classes.Where(m => m.FacutyID == facultyId && m.Name.Substring(0, 2).Equals(year1)).ToList();
            }
            else if (facultyId != 0 && year == 0)
            {
                return db.Classes.Where(m => m.FacutyID == facultyId).ToList();
            }
            else if (facultyId == 0 && year != 0)
            {

                return db.Classes.Where(m => m.Name.Substring(0, 2).Equals(year1)).ToList();
            }
            else
            {
                return db.Classes.ToList();
            }
        }
        public int Update(User user)
        {
            using(DBContext db = new DBContext())
            {
                var item = db.Users.Where(m => m.UserID == user.UserID && m.Status == 1).FirstOrDefault();
                if (item == null) return -1;
                if(db.Users.Where(m=>m.StudentCode == user.StudentCode && m.UserID != user.UserID).FirstOrDefault() == null)
                {
                    item.FullName = user.FullName;
                    item.StudentCode = user.StudentCode;
                    item.Phone = user.Phone;
                    item.Email = user.Email.Trim();
                    item.Gender = user.Gender;
                    item.Birthday = user.Birthday;
                    item.ClassID = user.ClassID;
                    item.JoinDate = user.JoinDate;
                    item.CityID = user.CityID;
                    item.DistrictID = user.DistrictID;
                    item.WardID = user.WardID;
                    item.Address = user.Address;
                    db.SaveChanges();
                    return 1;
                }
                return -1;
            }
           
        }
        //Get user list (for scoring)
        public List<User> GetListUser()
        {
            return db.Users.Where(m => m.Status == 1).ToList();
        }
        //Get user list by page
        public List<User> GetListUserByPage(int? pageNum)
        {
            int page = pageNum == null ? 1 : (int)pageNum;
            return db.Users.Where(m=>m.Status == 1 && m.GroupId != 2).OrderBy(m=>m.UserID).Skip((page - 1) * 12).Take(12).ToList();
        }
        //Get user list by page
        public int GetUserCount()
        {
            return db.Users.Where(m=>m.Status == 1 && m.GroupId != 2).Count();
        }
        //Get position list
        public List<Position> GetListPosition()
        {
            return db.Positions.ToList();
        }
        //Get group list
        public List<GroupUser> GetListGroup()
        {
            return db.GroupUsers.ToList();
        }
        //Update user infomation
        public int UpdateUser(User user)
        {
            try
            {
                User changeUser = db.Users.FirstOrDefault(m => m.UserID == user.UserID);
                changeUser.FullName = user.FullName;
                changeUser.Email = user.Email;
                changeUser.Phone = user.Phone;
                changeUser.Gender = user.Gender;
                changeUser.Birthday = user.Birthday;
                changeUser.CityID = user.CityID;
                changeUser.DistrictID = user.DistrictID;
                changeUser.WardID = user.WardID;
                changeUser.Address = user.Address;
                changeUser.JoinDate = user.JoinDate;
                changeUser.PositionID = user.PositionID;
                changeUser.GroupId = 9 - user.PositionID; //HARDCODED BASE ON POSITION ID
                //THIS DOESN'T RELATE BUT HAVE TO SET IT TO AVOID PROBLEM
                changeUser.StudentCode = changeUser.StudentCode == null ? "" : changeUser.StudentCode.Trim();
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        //Delete user
        public int DeleteUser(int id)
        {
            User user = db.Users.FirstOrDefault(m=>m.UserID == id);
            if (user == null) return 0;
            try
            {
                user.Status = 0;
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
