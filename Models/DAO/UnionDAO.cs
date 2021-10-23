using System;
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
        public List<UnionBook> GetListAll()
        {
            return db.UnionBooks.Where(m => m.User1.PositionID == 2).OrderByDescending(m => m.ID).ToList();
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
            listResult = listResult.Where(m => status == 0 || m.Status == status || m.isEmail == status);
            listResult = listResult.OrderByDescending(m => m.NumID);
            return new UnionBookByConditionModel
            {
                List = listResult.Skip(start).Take(length).ToList(),
                TotalRecords = listResult.Count()
            };

        }

        public List<UnionBook> GetUnionBookByCondition(int classId, string unionId, int status, int facutyId, int semester)
        {
            int numID = -1;
            int.TryParse(string.IsNullOrEmpty(unionId) ? "" : unionId.Substring(3), out numID);
            var listResult = db.UnionBooks.Where(m => string.IsNullOrEmpty(unionId) ||
            (m.User1.StudentCode.StartsWith(unionId.Substring(0, 2)) && m.NumID == numID));
            listResult = listResult.Where(m => semester == 0 || m.User1.Class.Name.StartsWith(semester.ToString()));
            listResult = listResult.Where(m => facutyId == 0 || m.User1.Class.FacutyID == facutyId);
            listResult = listResult.Where(m => classId == 0 || m.User1.ClassID == classId);
            listResult = listResult.Where(m => status == 0 || m.Status == status || m.isEmail == status);
            listResult = listResult.OrderByDescending(m => m.NumID);
            return listResult.ToList();
        }
        /// <summary>
        /// Insert a list union to database
        /// </summary>
        /// <param name="listData"></param>
        /// <returns>List error data</returns>
        public static List<UnionBook> InsertList(List<UnionBook> listData)
        {
            try
            {
                using (DBContext context = new DBContext())
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.UnionBooks.AddRange(listData);
                    context.ChangeTracker.DetectChanges();
                    context.SaveChanges();
                }
            }
            catch
            {
                // Do no thing
            }
            return listData.Where(e => e.ID == 0).ToList();
        }
        public void UpdateIsEmail(int id, int isEmail)
        {
            try
            {
                var union = db.UnionBooks.Where(m => m.ID == id).FirstOrDefault();
                union.isEmail = isEmail;
                db.SaveChanges();
            }
            catch
            {

            }
        }
        public int Update(UnionBook book)
        {
            var item = db.UnionBooks.Where(m => m.ID == book.ID).FirstOrDefault();
            if (item == null) return -1;
            item.ReturnDate = book.ReturnDate;
            item.NumID = book.NumID;
            db.SaveChanges();
            return 1;
        }
       
    }
}
