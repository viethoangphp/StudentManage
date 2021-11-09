﻿using Models.BaseModel;
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
            return db.EvaluativeMains.Where(x=>x.TemplateID == temID).ToList();
        }
        // Get all criteria evaluation by template
        public List<EvaluativeCriteria> GetAllCriteriaByTemplateId(int templateId)
        {
            return db.EvaluativeCriterias.Where(x => x.EvaluativeMain.TemplateForm.TemplateID == templateId).ToList();
        }
        // Get all passed Form evalution
        public List<EvalutionForm> GetPassedEvalutionFormsById(int userid)
        {
            var result = db.DetailEvalutions.Where(x => x.UserID == userid).Select(x => x.EvalutionForm).Distinct().OrderByDescending(x => x.Create_At).ToList();
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
            return db.DetailEvalutions.Where(x => x.EvalutionForm.Create_by == userId).ToList();
        }
        // Get all detail By FormID
        public List<DetailEvalution> GetDetailEvalutionsByFormId(int formId)
        {
            return db.DetailEvalutions.Where(x => x.FormId == formId).OrderBy(x=>x.Level).ToList();
        }
        // Get list semester till now - include present semester
        public List<Semester> GetSemesterById(int userId)
        {
            var result = db.DetailEvalutions.Where(x => x.UserID == userId).Select(x => x.EvalutionForm).Distinct().OrderBy(x => x.Create_At).ToList();
            var firstform = result.FirstOrDefault();
            if(firstform != null)
            {
                var listSemeter = db.Semesters.Where(x => DateTime.Compare((DateTime)x.Day_Start, (DateTime)firstform.Semester.Day_Start) >= 0).OrderByDescending(x => x.Day_Start).ToList();
                return listSemeter;
            }
            return null;
        }
        // Get present semester
        public Semester GetPresentSemester()
        {
            DateTime now = DateTime.Now;
            return db.Semesters.Where(x =>x.Status == 1).FirstOrDefault();
        }
        // Get Evaluation Form by Form id 
        public EvalutionForm GetEvaluationFormById(int formId)
        {
            return db.EvalutionForms.Where(x => x.FormId == formId).FirstOrDefault();
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
            return db.Positions.Where(x=>x.Name==name && x.Status == 1).FirstOrDefault().PositionID;
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
    }
}
