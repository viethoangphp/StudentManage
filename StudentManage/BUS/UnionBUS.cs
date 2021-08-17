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
        public int getTotalRecord()
        {
            return dao.getTotalRecord();
        }
        public List<UnionModel> GetListAll(int start , int length)
        {
            var list = dao.GetListAll(start,length);
            var listUnion = new List<UnionModel>();
            foreach(var item in list)
            {
                UnionModel union = new UnionModel();
                union.id = item.ID;
                union.unionID = item.User1.StudentCode.Substring(0,2)+"-"+ item.NumID.ToString().PadLeft(5,'0');
                union.fullname = item.User1.FullName;
                union.studentCode = item.User1.StudentCode;
                union.className = item.User1.Class.Name;
                union.facultyName = item.User1.Class.Faculty.Name;
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
                union.unionID = item.User1.StudentCode.Substring(0, 2) + "-" + item.NumID.ToString().PadLeft(5, '0');
                union.fullname = item.User1.FullName;
                union.studentCode = item.User1.StudentCode;
                union.className = item.User1.Class.Name;
                union.facultyName = item.User1.Class.Faculty.Name;
                union.create_At = item.Create_At;
                union.status = (int)item.Status;
                union.birthDay = String.Format("{0:dd/MM/yyyy}", item.User.Birthday);
                union.returnDate = item.ReturnDate;
                return union;
            }
            return null;
            
        }
        public bool ChangeStatus(int id)
        {
            return dao.ChangeStatus(id);
        }
        public List<UnionModel> GetUnionBookByCondition(int start, int lenght,int classId,string unionId,int status,int faculty,int semester)
        {
            var list = dao.GetUnionBookByCondition(start,lenght,classId,unionId,status,faculty,semester);
            var listUnion = new List<UnionModel>();
            foreach (var item in list)
            {
                UnionModel union = new UnionModel();
                union.id = item.ID;
                union.unionID = item.User1.StudentCode.Substring(0, 2) + "-" + item.NumID.ToString().PadLeft(5, '0');
                union.fullname = item.User1.FullName;
                union.studentCode = item.User1.StudentCode;
                union.className = item.User1.Class.Name;
                union.facultyName = item.User1.Class.Faculty.Name;
                union.create_At = item.Create_At;
                union.create_at = String.Format("{0:dd/MM/yyyy h:mm tt}", item.Create_At);
                union.status = (int)item.Status;
                listUnion.Add(union);
            }
            return listUnion;
        }
        
    }
}