using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentManage.Models;
using Models.DAO;
using Models.EntityModel;

namespace StudentManage.BUS
{
    public class FacultyBUS
    {
        public List<FacultyModel> GetListFaculty()
        {
            var result = new FacultyDAO().GetListFaculty();
            List<FacultyModel> list = new List<FacultyModel>();
            foreach(var item in result)
            {
                FacultyModel model = new FacultyModel();
                model.facultyID = item.FacutyID;
                model.facultyName = item.Name;
                list.Add(model);
            }
            return list;

        }
        public FacultyModel GetFacultyByName(string name)
        {
            var result = new FacultyDAO().GetFacultyByName(name);
            if(result != null)
            {
                FacultyModel model = new FacultyModel() { facultyID = result.FacutyID, facultyName = result.Name };
                return model;
            }
            return null;
        }
        public int InsertClass(ClassModel model)
        {
            Class classItem = new Class()
            {
                Name = model.className,
                FacutyID = model.facultyID,
                Status = model.status
            };
            return new FacultyDAO().InsertClass(classItem);
        }
        public ClassModel GetClassModelByName(string name)
        {
            var result = new FacultyDAO().GetClassByClassName(name);
            if(result != null)
            {
                return new ClassModel()
                {
                    classID = result.ClassID,
                    className = result.Name,
                    facultyID = result.FacutyID,
                    status = (int)result.Status
                };
            }
            return null;
            
        }
    }
}