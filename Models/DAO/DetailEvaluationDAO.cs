using Models.BaseModel;
using Models.EntityModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    
    public class DetailEvaluationDAO
    {
        private DBContext db = new DBContext();
        /*
         * Phiếu chấm điểm
         */
        //Get all main Criteria
        public List<EvaluativeMain> getAllMainByTemplate(string templateName)
        {
            var temID = db.TemplateForms.Where(x => x.Name == templateName).FirstOrDefault().TemplateID;
            return db.EvaluativeMains.Where(x=>x.TemplateID == temID).ToList();
        }
        //get all criteria evaluation by template
        public List<EvaluativeCriteria> getAllCriteriaByTemplate(string templateName)
        {
            return db.EvaluativeCriterias.Where(x => x.EvaluativeMain.TemplateForm.Name == templateName).ToList();
        }
        //get all passed Form evalution
        public List<EvalutionForm> getPassedEvalutionFormsById(int userid)
        {
            var result = db.DetailEvalutions.Where(x => x.UserID == userid).Select(x => x.EvalutionForm).Distinct().OrderByDescending(x => x.Create_At).ToList();
            return result;
        }
        /*
         * Chấm điểm
         */
        //get all detailForm by userId
        public List<DetailEvalution> getDetailFormsById(int userId)
        {
            return db.DetailEvalutions.Where(x => x.EvalutionForm.Create_by == userId).ToList();
        }
        //get list semester till now - include present semester
        public List<Semester> getSemesterById(int userId)
        {
            var result = db.DetailEvalutions.Where(x => x.UserID == userId).Select(x => x.EvalutionForm).Distinct().OrderBy(x => x.Create_At).ToList();
            var firstform = result.FirstOrDefault();
            var listSemeter = db.Semesters.Where(x=>DateTime.Compare((DateTime)x.Day_Start,(DateTime)firstform.Semester.Day_Start)>=0).OrderByDescending(x=>x.Day_Start).ToList();
            return listSemeter;
        }
        //get present semesterS
        public Semester getPresentSemester()
        {
            DateTime now = DateTime.Now;
            return db.Semesters.Where(x => DateTime.Compare((DateTime)x.Day_Start, (DateTime)now) <= 1 && DateTime.Compare((DateTime)x.Day_End, (DateTime)now) >= 1).FirstOrDefault();
        }
        //========================================================
        //Insert EvaluationForm
        public int InsertEvaluationForm(EvalutionForm form)
        {
            try
            {
                db.EvalutionForms.Add(form);
                db.SaveChanges();
                return form.FormId;
            }
            catch (Exception ex)
            {
                return 0;
            } 
        }
        //Insert or Update DetailEvaluation
        public int InsertDetailEvaluation(DetailEvalution detail)
        {
            try
            {
                db.DetailEvalutions.AddOrUpdate(detail);
                db.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        //Find Position Id by Name
        public int findPositionByName(string name)
        {
            return db.Positions.Where(x=>x.Name==name && x.Status == 1).FirstOrDefault().PositionID;
        }
    }
}
