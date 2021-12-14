using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.EntityModel;
namespace Models.DAO
{
    public class EvaluationDAO
    {
        private DBContext db = new DBContext();
        /// <summary>
        ///     get list requirment of user by template id
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns>list</returns>
        public TemplateForm GetTemplate(int templateId)
        {
            TemplateForm list = new TemplateForm();
            list = db.TemplateForms.Where(m => m.TemplateID == templateId).FirstOrDefault();
            return list;
        }

        /// <summary>
        ///     get list criteriaID by TemplateId 
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns>list</returns>
        
        public List<EvaluativeCriteria> GetListCriteriaIdByTemplateId(int templateId)
        {
            return db.EvaluativeCriterias.Where(m => m.EvaluativeMain.TemplateID == templateId).ToList();
        }

        public int InsertEvaluationForm(EvalutionForm form)
        {
            try
            {
                db.EvalutionForms.Add(form);
                db.SaveChanges();
                return form.FormId;
            }
            catch
            {
                return -1;
            }
        }
        public int InsertListFormDetail(List<DetailEvalution> list)
        {
            try
            {
                db.Configuration.AutoDetectChangesEnabled = false;
                db.DetailEvalutions.AddRange(list);
                db.ChangeTracker.DetectChanges();
                db.SaveChanges();
                return 1;
            }
            catch
            {
                return -1;
            }
        }
        public int GetSemesterActive()
        {
            return db.Semesters.Where(m => m.Status == 1).Select(m => m.SemesterID).FirstOrDefault();
        }
        public int? UpdateTotal(int formId)
        {
            var item = db.EvalutionForms.Where(m => m.FormId == formId).FirstOrDefault();
            item.Total = db.DetailEvalutions.Where(m => m.FormId == formId).Sum(m => m.Score);
            db.SaveChanges();
            return item.Total;
        }
        public List<EvalutionForm> GetEvaluationPersonal(int userId)
        {
            if(userId != 0)
            {
                return db.EvalutionForms.Where(m => m.Create_by == userId && m.Type == 1).ToList();
            }
            return db.EvalutionForms.Where(m => m.Semester.Status == 1 && m.Type == 1).ToList();
        }
        public List<DetailEvalution> GetDetailEvalutions(int formId)
        {
            return db.DetailEvalutions.Where(m => m.FormId == formId && m.Type == 1).ToList();
        }
       
        public int UpdateEvaluationForm(DetailEvalution detail)
        {
            DetailEvalution form = db.DetailEvalutions.Where(m => m.FormId == detail.FormId && m.CriteriaID == detail.CriteriaID).FirstOrDefault();
            try
            {
                form.Status = detail.Status;
                form.Comment = detail.Comment;
                db.SaveChanges();
                return 1;
            }
            catch
            {
                return -1;
            }
            
        }
        public int UpdateStatusForm(int formId,int status)
        {
            var form = db.EvalutionForms.Where(m => m.FormId == formId).FirstOrDefault();
            try
            {
                form.Status = status;
                db.SaveChanges();
                return 1;
            }
            catch
            {
                return -1;
            }
        }
    }
}
