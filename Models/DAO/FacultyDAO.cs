using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Models.BaseModel;
using Models.EntityModel;
namespace Models.DAO
{
    public class FacultyDAO
    {
        DBContext db = new DBContext();
        public int Insert(Faculty faculty)
        {
            db.Faculties.Add(faculty);
            db.SaveChanges();
            return faculty.FacutyID;
        }
        public List<Faculty> GetListFaculty()
        {
            return db.Faculties.Where(m => m.Status == 1).ToList();
        }
        public Faculty GetFacultyByID(int id)
        {
            return db.Faculties.FirstOrDefault(m => m.FacutyID == id && m.Status == 1);
        }
        public Faculty GetFacultyByName(string facultyName)
        {
            var result = db.Faculties.Where(m => m.Name.Trim().Equals(facultyName.Trim())).FirstOrDefault();
            if (result != null)
                return result;
            return null;
        }
        public ListClassByConditionModel GetListClass(int start,int length)
        {
            List<Class> list = db.Classes.Where(m=>m.Status == 1).OrderBy(m => m.ClassID).Skip(start).Take(length).ToList();
            int count = db.Classes.Where(m => m.Status == 1).Count();
            return new ListClassByConditionModel()
            {
                List = list,
                TotalRecords = count
            };
        }
        public List<Class> GetListClassByFaculty(int id)
        {
            return db.Classes.Where(m => m.FacutyID == id && m.Status == 1).ToList();
        }
        public Class GetClassByID(int id)
        {
            return db.Classes.FirstOrDefault(m => m.ClassID == id && (m.Faculty.Status == 1 && m.Status == 1));
        }
        public int InsertClass(Class model)
        {
            try
            {
                model.Name = Regex.Replace(model.Name, "\\s+", "").Trim();
                Class dup = db.Classes.FirstOrDefault(m => m.Name.Contains(model.Name));
                if (dup != null) return -1;
                Class cls = GetClassByClassName(model.Name);
                if (cls != null) return -1;
                db.Classes.Add(model);
                db.SaveChanges();
                return model.ClassID;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public Class GetClassByClassName(string className)
        {
            //Regex.Replace(name.Trim(), @"\s+", " ")
            var result = db.Classes.Where(m => m.Name.Equals(className.Trim())).FirstOrDefault();
            if (result != null)
                return result;
            return null;
        }
        public int InsertFaculty(Faculty faculty)
        {
            try
            {
                faculty.Name = Regex.Replace(faculty.Name,"\\s+"," ").Trim();
                db.Faculties.Add(faculty);
                db.SaveChanges();
                return faculty.FacutyID;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public int UpdateFaculty(Faculty faculty)
        {
            try
            {
                Faculty fac = db.Faculties.FirstOrDefault(m => m.FacutyID == faculty.FacutyID);
                if (fac != null)
                {
                    fac.Name = faculty.Name;
                    fac.Phone = faculty.Phone;
                }
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public int DeleteFaculty(int id)
        {
            try
            {
                Faculty fac = db.Faculties.FirstOrDefault(m => m.FacutyID == id);
                if (fac != null)
                {
                    fac.Status = 0;
                    db.SaveChanges();
                    return 1;
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public int UpdateClass(Class cls)
        {
            try
            {
                cls.Name = Regex.Replace(cls.Name, "\\s+", "").Trim();
                Class dup = db.Classes.FirstOrDefault(m => m.Name.Contains(cls.Name));
                if (dup != null) return -1;
                Class c = db.Classes.FirstOrDefault(m => m.ClassID == cls.ClassID);
                if (c != null)
                {
                    c.Name = cls.Name;
                    c.FacutyID = cls.FacutyID;
                }
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public int DeleteClass(int id)
        {
            try
            {
                Class cls = db.Classes.FirstOrDefault(m => m.ClassID == id);
                if (cls != null)
                {
                    cls.Status = 0;
                    db.SaveChanges();
                    return 1;
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public ListClassByConditionModel GetListClassByCondition(int start, int length, int facultyID, string className)
        {
            var listResult = db.Classes.Where(m => string.IsNullOrEmpty(className) || m.Name == className);
            listResult = listResult.Where(m => facultyID == 0 || m.FacutyID == facultyID);
            listResult = listResult.Where(m => m.Status == 1);
            return new ListClassByConditionModel
            {
                List = listResult.OrderBy(m=>m.ClassID).Skip(start).Take(length).ToList(),
                TotalRecords = listResult.Count()
            };
        }
    }
}
