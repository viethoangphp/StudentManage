using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentManage.Models;
using Models.DAO;
using Models.EntityModel;
namespace StudentManage.BUS
{
    public class TemplateBUS
    {
        public static TemplateModel GetTemplateById(int templateId)
        {
            TemplateModel template = new TemplateModel();
            var model = new EvaluationDAO().GetTemplate(templateId);
            //===============================================
            template.TemplateID = model.TemplateID;
            template.TemplateName = model.Name;
            template.ListMain = new List<EvaluationMainModel>();
            foreach (var item in model.EvaluativeMains)
            {
                EvaluationMainModel evaluationMain = new EvaluationMainModel(); 
                evaluationMain.MainID = item.MainID;
                evaluationMain.Title = item.Content;
                evaluationMain.ListRequriement = new List<PesonalEvalationModel>();
                foreach (var subItem in item.EvaluativeCriterias)
                {
                    PesonalEvalationModel evaluation = new PesonalEvalationModel();
                    evaluation.CriteriaID = subItem.CriteriaID;
                    evaluation.Content = subItem.CriteriaContent;
                    evaluation.Requirement = subItem.CriteriaRequirement;
                    evaluation.MaxScore = (int)subItem.Score;
                    evaluationMain.ListRequriement.Add(evaluation);
                }
                template.ListMain.Add(evaluationMain);
            }
            return template;
        }
        public static int InsertEvaluationForm(FormModel form)
        {
            EvalutionForm evalutionForm = new EvalutionForm();
            evalutionForm.Create_At = form.Create_At;
            evalutionForm.Create_by = form.Create_By;
            evalutionForm.SemesterID = GetSemesterActive();
            evalutionForm.Total = form.Total;
            evalutionForm.Status = form.Status; ;
            return new EvaluationDAO().InsertEvaluationForm(evalutionForm);
        }
        public static int GetSemesterActive()
        {
            return new EvaluationDAO().GetSemesterActive();
        }
        public static List<DetailEvalution> ParseDetailFormModel(List<DetailFormModel> listData)
        {
            List<DetailEvalution> result = new List<DetailEvalution>();
            foreach(var item in listData)
            {
                DetailEvalution model = new DetailEvalution();
                model.FormId = item.FormId;
                model.UserID = item.UserId;
                model.CriteriaID = item.CriteriaId;
                model.Score = item.Score;
                model.Level = item.Level;
                model.Image_proof = item.Image;
                model.Note = item.Note;
                result.Add(model);
            }
            return result;
        }
        public static int InsertListEvaluationDetail(List<DetailFormModel> list)
        {
            List<DetailEvalution> result = ParseDetailFormModel(list);
            return new EvaluationDAO().InsertListFormDetail(result);
        }
        public static List<EvaluationModel> GetListCriteria(int templateId)
        {
            List<EvaluationModel> result = new List<EvaluationModel>();
            var listData = new EvaluationDAO().GetListCriteriaIdByTemplateId(templateId);
            foreach(var item in listData)
            {
                EvaluationModel model = new EvaluationModel();
                model.CriteriaID = item.CriteriaID;
                model.Content = item.CriteriaContent;
                model.Requirement = item.CriteriaRequirement;
                model.MaxScore = (int)item.Score;
                result.Add(model);
            }
            return result;
        }
        public static int? UpdateTotalScore(int formId)
        {
            return new EvaluationDAO().UpdateTotal(formId);
        }
        public static List<PersonalScore> GetPersonalScores(int userId = 0 )
        {
            List<PersonalScore> list = new List<PersonalScore>();
            var model = new EvaluationDAO().GetEvaluationPersonal(userId);
            foreach(var item in model)
            {
                PersonalScore personalScore = new PersonalScore();
                personalScore.FormId = item.FormId;
                personalScore.FullName = item.DetailEvalutions.Last().User.FullName;
                personalScore.Semester = item.Semester.Name;
                personalScore.Year = item.Semester.Year;
                personalScore.Score = Int32.Parse(item.Total.ToString());
                if(item.Total >= 90)
                {
                    personalScore.Rank = "A";
                }
                if (item.Total >= 75 && item.Total < 90)
                {
                    personalScore.Rank = "B";
                }
                if (item.Total >= 50 && item.Total < 75)
                {
                    personalScore.Rank = "C";
                }
                if (item.Total < 50)
                {
                    personalScore.Rank = "D";
                }
                personalScore.Status = (item.Status == 0) ? "Chờ Duyệt" : "Đã Duyệt";
                personalScore.Note = "";
                list.Add(personalScore);
            }
            return list;
        }
        public static TemplateModel GetTemplateFormDetail(int formId)
        {

            TemplateModel template = new TemplateModel();
            var listDetail = new EvaluationDAO().GetDetailEvalutions(formId);
            var templateId = listDetail.First().EvaluativeCriteria.EvaluativeMain.TemplateID;
            var model = new EvaluationDAO().GetTemplate(templateId);
            //===============================================
            template.TemplateID = model.TemplateID;
            template.TemplateName = model.Name;
            template.FullName = listDetail.First().User.FullName;
            template.Email = listDetail.First().User.Email;
            template.Faculty = listDetail.First().User.Class.Faculty.Name;
            template.Phone = listDetail.First().User.Phone;
            template.Position = listDetail.First().User.Position.Name;
            template.ListMain = new List<EvaluationMainModel>();
            foreach (var item in model.EvaluativeMains)
            {
                EvaluationMainModel evaluationMain = new EvaluationMainModel();
                evaluationMain.MainID = item.MainID;
                evaluationMain.Title = item.Content;
                evaluationMain.ListRequriement = new List<PesonalEvalationModel>();
                foreach (var subItem in listDetail.Where(m=>m.EvaluativeCriteria.MainID == item.MainID))
                {
                    PesonalEvalationModel evaluation = new PesonalEvalationModel();
                    evaluation.CriteriaID = subItem.CriteriaID;
                    evaluation.Content = subItem.EvaluativeCriteria.CriteriaContent;
                    evaluation.Requirement = subItem.EvaluativeCriteria.CriteriaRequirement;
                    evaluation.MaxScore = (int)subItem.EvaluativeCriteria.Score;
                    evaluation.Score = (int)subItem.Score;
                    evaluation.Proof = subItem.Note;
                    evaluation.Image = (subItem.Image_proof != null) ? subItem.Image_proof : "";
                    evaluation.Note = subItem.Note;
                    evaluation.Status = subItem.Status;
                    evaluation.Comment = subItem.Comment;
                    evaluationMain.ListRequriement.Add(evaluation);
                }
                template.TotalScore = (int)listDetail.First().EvalutionForm.Total;
                if (template.TotalScore >= 90)
                {
                    template.Rank = "A";
                }
                if (template.TotalScore >= 75 && template.TotalScore < 90)
                {
                    template.Rank = "B";
                }
                if (template.TotalScore >= 50 && template.TotalScore < 75)
                {
                    template.Rank = "C";
                }
                if (template.TotalScore < 50)
                {
                    template.Rank = "D";
                }
                template.ListMain.Add(evaluationMain);
            }
            return template;
        }
    }
}
