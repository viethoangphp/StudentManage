using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public List<UnionBook> GetListAll()
        {
            return db.UnionBooks.OrderByDescending(m=>m.ID).ToList();
        }
        public UnionBook getUnionBookById(int id)
        {
            return db.UnionBooks.Find(id);
        }
        public bool ChangeStatus(int id)
        {
            var book = db.UnionBooks.Find(id);
            if(book != null)
            {
                if(book.Status == 1)
                {
                    book.Status = 2;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    book.Status = 1;
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }
    }
}
