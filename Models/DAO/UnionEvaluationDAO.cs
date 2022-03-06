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

    public class UnionEvaluationDAO
    {
        private DBContext db = new DBContext();
        /*
         * Form render
         */
        #region FormEvaluation
        // Get all main Criteria
        public List<EvaluativeMain> GetAllMainByTemplateId(int templateId)
        {
            var temID = db.TemplateForms.Where(x => x.TemplateID == templateId).FirstOrDefault().TemplateID;
            return db.EvaluativeMains.Where(x => x.TemplateID == temID).ToList();
        }
        // Get all criteria evaluation by template
        public List<EvaluativeCriteria> GetAllCriteriaByTemplateId(int templateId)
        {
            return db.EvaluativeCriterias.Where(x => x.EvaluativeMain.TemplateForm.TemplateID == templateId).ToList();
        }
        // Get all passed Form evalution by userid
        public List<EvalutionForm> GetPassedEvalutionFormsById(int userId)
        {
            var result = db.EvalutionForms.Where(x => x.Create_by == userId).ToList();
            return result;
        }
        #endregion
        /*
         * Chấm điểm
         */
        // Get Semester By Semester Id
        public Semester GetSemesterBySemesterId(int semesterId)
        {
            return db.Semesters.Where(x => x.SemesterID == semesterId).FirstOrDefault();
        }
        // Get all detailForm by userId
        public List<DetailEvalution> GetDetailFormsById(int userId)
        {
            return db.DetailEvalutions.Where(x => x.EvalutionForm.Create_by == userId && x.Type == 2).ToList();
        }
        // Get all detail By FormID
        public List<DetailEvalution> GetDetailEvalutionsByFormId(int formId)
        {
            return db.DetailEvalutions.Where(x => x.FormId == formId && x.Type == 2).OrderBy(x => x.Level).ToList();
        }
        // Get all passed semesters
        public List<Semester> GetAllSemesters()
        {
            return db.Semesters.OrderByDescending(x => x.Day_Start).ToList();
        }

        // Get list semesters till now - include present semester
        public List<Semester> GetSemesterById(int userId)
        {
            //var result = db.DetailEvalutions.Where(x => x.UserID == userId && x.Type == 2).Select(x => x.EvalutionForm).Distinct().OrderBy(x => x.Create_At).ToList();
            //var firstform = result.FirstOrDefault();
            //if (firstform != null)
            //{
            //    var listSemeter = db.Semesters.Where(x => DateTime.Compare((DateTime)x.Day_Start, (DateTime)firstform.Semester.Day_Start) >= 0).OrderByDescending(x => x.Day_Start).ToList();
            //    return listSemeter;
            //}
            //return null;
            var result = new List<Semester>();
            var listForms = db.EvalutionForms.Where(x => x.Create_by == userId).OrderByDescending(x => x.Create_At).ToList();
            foreach (var form in listForms)
            {
                var semester = listForms.Where(x => x.FormId == form.FormId).FirstOrDefault().Semester;
                result.Add(semester);
            }
            return result;
        }
        // Get present semester
        public Semester GetPresentSemester()
        {
            DateTime now = DateTime.Now;
            return db.Semesters.Where(x => x.Status == 1).FirstOrDefault();
        }
        // Get Evaluation Form by Form id 
        public EvalutionForm GetEvaluationFormById(int formId)
        {
            return db.EvalutionForms.Where(x => x.FormId == formId).FirstOrDefault();
        }
        // Get All Evaluation Form by UserID
        public List<EvalutionForm> GetAllEvaluationFormsByUserId(int userId)
        {
            return db.EvalutionForms.Where(x => x.Create_by == userId).ToList();
        }
        //========================================================
        // Insert EvaluationForm
        public int InsertEvaluationForm(EvalutionForm form)
        {
            try
            {
                db.EvalutionForms.Add(form);
                db.SaveChanges();
                return form.FormId;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        // Insert or Update DetailEvaluation
        public int InsertDetailEvaluation(DetailEvalution detail)
        {
            try
            {

                db.DetailEvalutions.AddOrUpdate(detail);
                if (detail.Image_proof == null)
                {
                    db.Entry(detail).Property(x => x.Image_proof).IsModified = false;
                }
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        // Find Position Id by Name
        public int FindPositionByName(string name)
        {
            return db.Positions.Where(x => x.Name == name && x.Status == 1).FirstOrDefault().PositionID;
        }
        // Find Group ID by Name
        public int FindGroupIdByName(string name)
        {
            return db.GroupUsers.Where(x => x.Name == name && x.Status == 1).FirstOrDefault().GroupId;
        }
        // Find Template Id by Name
        public int FindTemplateIdByName(string name)
        {
            return db.TemplateForms.Where(x => x.Name == name && x.Status == 1).FirstOrDefault().TemplateID;
        }
        // Get TimeEvaluation by Id
        public TimeEvalution GetTimeEvaluationByTimeId(int timeId)
        {
            return db.TimeEvalutions.Find(timeId);
        }
        // Get UserGroup By ID
        public GroupUser GetGroupUserById(int groupId)
        {
            return db.GroupUsers.FirstOrDefault(x => x.GroupId == groupId);
        }
        // Update Evaluation Form Note
        public bool UpdateEvaluationFormNote(int formId, string updateNote)
        {
            var evaluationForm = db.EvalutionForms.Where(x => x.FormId == formId).FirstOrDefault();
            if (evaluationForm != null)
            {
                try
                {
                    evaluationForm.Note = updateNote;
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
        // Get all TimeEvaluation
        public List<TimeEvalution> GetAllTimeEvaluation()
        {
            var result = db.TimeEvalutions.ToList();
            if (result != null)
            {
                return result;
            }
            return null;
        }
        // Delete Image Proof
        public int deleteImageProof(int formId, int critetiaId)
        {
            var details = GetDetailEvalutionsByFormId(formId);
            if (details.Any())
            {
                try
                {
                    var detail = details.Where(x => x.CriteriaID == critetiaId && x.Level == 1).FirstOrDefault();
                    detail.Image_proof = null;
                    db.SaveChanges();
                    return 1;
                }
                catch
                {
                    return 0;
                }
            }
            return -1;
        }
    }
}
