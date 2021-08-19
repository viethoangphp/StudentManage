﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
            return db.Faculties.ToList();
        }
        public Faculty GetFacultyByName(string facultyName)
        {
            var result = db.Faculties.Where(m => m.Name.Trim().Equals(facultyName.Trim())).FirstOrDefault();
            if (result != null)
                return result;
            return null;
        }
        public int InsertClass(Class model)
        {
            db.Classes.Add(model);
            db.SaveChanges();
            return model.ClassID;
        }
        public Class GetClassByClassName(string className)
        {
            //Regex.Replace(name.Trim(), @"\s+", " ")
            var result = db.Classes.Where(m => m.Name.Equals(className.Trim())).FirstOrDefault();
            if (result != null)
                return result;
            return null;
        }
    }
}
