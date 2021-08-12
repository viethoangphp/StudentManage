using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models.DAO;
using Models.EntityModel;
using StudentManage.Models;
namespace StudentManage.BUS
{
    public class UnionBUS
    {
        UnionDAO dao = new UnionDAO();
        public int Insert(UnionModel model)
        {
            var book = new UnionBook();
            book.Create_At = DateTime.Now;
            book.Create_By = model.create_by;
            book.ReturnDate = model.returnDate;
            book.UserID = model.userID;
            book.Status = model.status;
            return dao.Insert(book);
        }
        public List<UnionModel> GetListAll()
        {
            var list = dao.GetListAll();
            var listUnion = new List<UnionModel>();
            foreach(var item in list)
            {
                UnionModel union = new UnionModel();
                union.id = item.ID;
                union.unionID = item.User.StudentCode.Substring(0,2)+"-"+item.CharID;
                union.fullname = item.User1.FullName;
                union.studentCode = item.User1.StudentCode;
                union.className = item.User.Class.Name;
                union.facultyName = item.User.Class.Faculty.Name;
                union.create_At = item.Create_At;
                union.create_at = String.Format("{0:dd/MM/yyyy h:mm tt}", item.Create_At);
                union.status = (int)item.Status;
                listUnion.Add(union);
            }
            return listUnion;
        }
        public UnionModel GetUnionById(int id)
        {
            var item = dao.getUnionBookById(id);
            if(item != null)
            {
                var union = new UnionModel();
                union.id = item.ID;
                union.unionID = item.User.StudentCode.Substring(0, 2) + "-" + item.CharID;
                union.fullname = item.User1.FullName;
                union.studentCode = item.User1.StudentCode;
                union.className = item.User.Class.Name;
                union.facultyName = item.User.Class.Faculty.Name;
                union.create_At = item.Create_At;
                union.status = (int)item.Status;
                return union;
            }
            return null;
            
        }
        public bool ChangeStatus(int id)
        {
            return dao.ChangeStatus(id);
        }
    }
}