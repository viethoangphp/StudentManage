﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.BaseModel;
using Models.EntityModel;
namespace Models.DAO
{
    public class UnionDAO
    {
        protected DBContext db = new DBContext();
        public int Insert(UnionBook book)
        {
            db.UnionBooks.Add(book);
            db.SaveChanges();
            return book.ID;
        }
        public int getTotalRecord()
        {
            return db.UnionBooks.Count();
        }
        public List<UnionBook> GetListAll(int start, int length)
        {
            return db.UnionBooks.Where(m => m.User1.PositionID == 2).OrderByDescending(m => m.ID).Skip(start).Take(length).ToList();
        }
        public UnionBook getUnionBookById(int id)
        {
            return db.UnionBooks.Find(id);
        }
        public bool ChangeStatus(int id)
        {
            var book = db.UnionBooks.Find(id);
            if (book != null)
            {
                if (book.Status == 1)
                {
                    book.Status = 2;
                    book.Update_At = DateTime.Now;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    book.Status = 1;
                    book.Update_At = DateTime.Now;
                    book.Create_At = DateTime.Now;
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        public UnionBookByConditionModel GetUnionBookByCondition(int start, int length, int classId, string unionId, int status, int facutyId, int semester)
        {
            int numID = -1;
            int.TryParse(string.IsNullOrEmpty(unionId) ? "" : unionId.Substring(3), out numID);
            var listResult = db.UnionBooks.Where(m => string.IsNullOrEmpty(unionId) ||
            (m.User1.StudentCode.StartsWith(unionId.Substring(0, 2)) && m.NumID == numID));
            listResult = listResult.Where(m => semester == 0 || m.User1.Class.Name.StartsWith(semester.ToString()));
            listResult = listResult.Where(m => facutyId == 0 || m.User1.Class.FacutyID == facutyId);
            listResult = listResult.Where(m => classId == 0 || m.User1.ClassID == classId);
            listResult = listResult.Where(m => status == 0 || m.Status == status);
            listResult = listResult.OrderByDescending(m => m.NumID);
            return new UnionBookByConditionModel
            {
                List = listResult.Skip(start).Take(length).ToList(),
                TotalRecords = listResult.Count()
            };


            //if (unionId !="")
            //{
            //    //19-00003
            //    string year = unionId.Substring(0, 2);
            //    int numID = Convert.ToInt32(unionId.Substring(3));
            //    return db.UnionBooks.Where(m =>m.User1.StudentCode.Substring(0,2).Equals(year) && m.NumID == numID).OrderByDescending(m=>m.NumID).Skip(statr).Take(length).ToList();
            //}
            //else
            //{
            //    if(classId !=0 && status !=0)
            //    {
            //        return db.UnionBooks.Where(m => m.User1.ClassID == classId && m.Status == status).OrderByDescending(m => m.NumID).Skip(statr).Take(length).ToList();
            //    }    
            //    else if(classId != 0)
            //    {
            //        return db.UnionBooks.Where(m => m.User1.ClassID == classId).OrderByDescending(m => m.NumID).Skip(statr).Take(length).ToList();
            //    }
            //    else if(facutyId != 0 && status != 0)
            //    {
            //        return db.UnionBooks.Where(m => m.Status == status && m.User1.Class.FacutyID == facutyId).OrderByDescending(m => m.NumID).Skip(statr).Take(length).ToList();
            //    }
            //    else if(facutyId !=0)
            //    {
            //        return db.UnionBooks.Where(m=>m.User1.Class.FacutyID == facutyId).OrderByDescending(m => m.NumID).Skip(statr).Take(length).ToList();
            //    }else if(semester != 0 && status != 0)
            //    {
            //        return db.UnionBooks.Where(m => m.User1.Class.Name.ToString().Substring(0,2).Equals(semester.ToString()) && m.Status == status).OrderByDescending(m => m.NumID).Skip(statr).Take(length).ToList();
            //    }else if(semester != 0)
            //    {
            //        return db.UnionBooks.Where(m => m.User1.Class.Name.ToString().Substring(0, 2).Equals(semester.ToString())).OrderByDescending(m => m.NumID).Skip(statr).Take(length).ToList();
            //    }
            //    else if(status != 0)
            //    {
            //        return db.UnionBooks.Where(m=>m.Status == status).OrderByDescending(m=>m.NumID).Skip(statr).Take(length).ToList();
            //    }
            //    else
            //    {
            //        return db.UnionBooks.OrderByDescending(m => m.NumID).Skip(statr).Take(length).ToList();
            //    }
            //}
        }
    }
}
