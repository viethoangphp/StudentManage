﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models.DAO;
using Models.EntityModel;
using StudentManage.Models;

namespace StudentManage.BUS
{
    public class EvaluationBUS
    {
        protected DetailEvaluationDAO dao = new DetailEvaluationDAO();

        /*
         * Lấy ra các Main và tiêu chí để render được Form chấm điểm
         */
        // lấy tất cả Main
        public List<EvaluativeMainModel> GetAllMainByTemplateId(int templateId)
        {
            var result = dao.GetAllMainByTemplateId(templateId);
            List<EvaluativeMainModel> listModel = new List<EvaluativeMainModel>();
            foreach (var item in result)
            {
                EvaluativeMainModel model = new EvaluativeMainModel()
                {
                    mainId = item.MainID,
                    content = item.Content,
                    status = (int)item.Status,
                    templateId = item.TemplateID
                };
                listModel.Add(model);
            }
            return listModel;
        }
        // Lấy tất cả các tiêu chi by TemplateName
        public List<EvaluativeCriteriaModel> GetAllCriteriaByTemplateId(int templateId)
        {
            var result = dao.GetAllCriteriaByTemplateId(templateId);
            List<EvaluativeCriteriaModel> listModel = new List<EvaluativeCriteriaModel>();
            foreach (var item in result)
            {
                EvaluativeCriteriaModel model = new EvaluativeCriteriaModel()
                {
                    criteriaId = item.CriteriaID,
                    criteriaContent = item.CriteriaContent,
                    criteriaRequirement = item.CriteriaRequirement,
                    score = item.Score,
                    mainId = item.MainID
                };
                listModel.Add(model);
            }
            return listModel;
        }
        //==============================================================================
        /*
         * Phần Chấm điểm đoàn viên
         */
        // Lấy tất cá các Form đã chấm (đã qua) của đoàn viên
        public List<EvalutionFormModel> GetPassedEvalutionFormsById(int userId)
        {
            var result = dao.GetPassedEvalutionFormsById(userId);
            List<EvalutionFormModel> listModel = new List<EvalutionFormModel>();
            foreach (var item in result)
            {
                EvalutionFormModel model = new EvalutionFormModel()
                {
                    formId = item.FormId,
                    createAt = item.Create_At,
                    createBy = item.Create_by,
                    total = item.Total,
                    semesterId = item.SemesterID
                };
                listModel.Add(model);
            }
            return listModel;
        }
        // Lấy tất các các DetailForm của Form đã chấm thuộc User
        public List<DetailEvalutionModel> GetDetailFormsById(int userId)
        {
            var result = dao.GetDetailFormsById(userId);
            List<DetailEvalutionModel> listModel = new List<DetailEvalutionModel>();
            foreach (var item in result)
            {
                DetailEvalutionModel model = new DetailEvalutionModel()
                {
                    formId = item.FormId,
                    userId = item.UserID,
                    critetiaId = item.CriteriaID,
                    score = item.Score,
                    note = item.Note,
                    level = item.Level,
                    imageProof = item.Image_proof,
                    evalutionForm =
                    {
                        formId = item.EvalutionForm.FormId,
                        createAt = item.EvalutionForm.Create_At,
                        createBy = item.EvalutionForm.Create_by,
                        total = item.EvalutionForm.Total,
                        status = item.EvalutionForm.Status,
                        semesterId = item.EvalutionForm.SemesterID
                    }

                };
                listModel.Add(model);
            }
            return listModel;
        }
        // Lấy tất cả các Chi tiết phiếu theo mã Phiếu
        public List<DetailEvalutionModel> GetDetailEvalutionsByFormId(int formId)
        {
            var result = dao.GetDetailEvalutionsByFormId(formId);
            var listModel = new List<DetailEvalutionModel>();    
            foreach(var item in result)
            {
                DetailEvalutionModel model = new DetailEvalutionModel()
                {
                    formId = item.FormId,
                    userId = item.UserID,
                    critetiaId = item.CriteriaID,
                    score = item.Score,
                    note = item.Note,
                    level = item.Level,
                    imageProof = item.Image_proof,
                    evalutionForm =
                    {
                        formId = item.EvalutionForm.FormId,
                        createAt = item.EvalutionForm.Create_At,
                        createBy = item.EvalutionForm.Create_by,
                        total = item.EvalutionForm.Total,
                        status = item.EvalutionForm.Status,
                        semesterId = item.EvalutionForm.SemesterID
                    }
                };
                listModel.Add(model);
            }
            return listModel;
        }
        // Lấy tất cả các học kì từ HK đầu tiên đến NAY
        public List<SemesterModel> GetSemesterById(int userId)
        {
            var result = dao.GetSemesterById(userId);
            List<SemesterModel> listSemesters = new List<SemesterModel>();
            foreach (var item in result)
            {
                var test = item.EvalutionForms.Where(x => x.Create_by == userId).FirstOrDefault();
                SemesterModel model = new SemesterModel();

                model.semesterId = item.SemesterID;
                model.name = item.Name;
                model.dayStart = item.Day_Start;
                model.dayEnd = item.Day_End;
                model.status = item.Status;
                model.year = item.Year;
                if (test != null)
                {
                    model.FormId = test.FormId;
                }
                listSemesters.Add(model);
            }
            return listSemesters;
        }
        // Lấy vòng chấm hiện tại
        public int GetTurnNow(int semsterId, int userid)
        {
            var preSemes = dao.GetPresentSemester();
            var detail = dao.GetDetailFormsById(userid);
            int? turn = detail.Where(x=>x.EvalutionForm.SemesterID==semsterId).Select(x=>x.Level).Distinct().Max();
            if(turn == null)
            {
                return 0;
            }    
            return (int)turn;
        }
        //lấy vòng chấm của một phiếu bất kì
        public int GetTurnByFormId(int formId)
        {
            var form = GetEvaluationFormById(formId);
            int? turn = dao.GetDetailEvalutionsByFormId(formId).Select(x=>x.Level).Distinct().Max();
            if(turn == null)
            {
                return 0;
            }
            return (int)turn;
        }
        // Lấy HK  hiện tại của User
        public SemesterModel GetPresentSemester(int userid)
        {
            var result = dao.GetPresentSemester();
            var hasForm = result.EvalutionForms.Where(x => x.Create_by == userid).FirstOrDefault();
            
            SemesterModel model = new SemesterModel();
            model.semesterId = result.SemesterID;
            model.dayEnd = result.Day_End;
            model.dayStart = result.Day_Start;
            model.status = result.Status;
            model.year = result.Year;
            model.name = result.Name;
            if(hasForm!=null)
            {
                model.FormId = hasForm.FormId;
            }
            return model;
        }
        // Lấy EvaluationForm có Formid 
        public EvalutionFormModel GetEvaluationFormById(int formId)
        {
            var result = dao.GetEvaluationFormById(formId);
            EvalutionFormModel model = new EvalutionFormModel()
            {
                formId = result.FormId,
                createAt = result.Create_At,
                createBy = result.Create_by,
                semesterId= result.SemesterID,
                status= result.Status,
                total = result.Total
            };
            return model;
        }
        // Insert EvaluationForm
        public int InsertEvaluationForm(EvalutionFormModel form)
        {
            EvalutionForm model = new EvalutionForm()
            {
                Total = form.total,
                Status = form.status,
                Create_by = form.createBy,
                SemesterID = form.semesterId,
                Create_At = form.createAt,
            };
            return dao.InsertEvaluationForm(model);
        }
        //Insert List of Detail Evaluation
        public int InsertListDetailEvaluation(List<EvaluationModel> listDetail, int userid)
        {
            var user = new UserDAO().GetUserByID(userid);
            int position = user.Position.PositionID;
            int positionTurn;
            int turn = GetTurnNow(GetPresentSemester(userid).semesterId,userid); 
            int dv = dao.FindPositionByName("Đoàn Viên");
            int btcd = dao.FindPositionByName("Bí Thư Chi Đoàn");
            int btdk = dao.FindPositionByName("Bí Thư Đoàn Khoa");
            // int btdt = dao.FindPositionByName("Bí Thư Đoàn Trường");
            if (position == dv ) positionTurn = 1;
            else
            {
                if(position == btcd)
                {
                    positionTurn = 2;
                    if(turn<positionTurn)
                    {
                        positionTurn = turn + 1;
                    }    
                }    
                    
                else
                {
                    if (position == btdk)
                    {
                        positionTurn = 3;
                        if (turn < positionTurn)
                        {
                            positionTurn = turn + 1;
                        }
                    }    
                        
                    else positionTurn = 4;
                }    
            }    
            var preSemes = GetPresentSemester(userid);
            foreach (EvaluationModel item in listDetail)
            {
                DetailEvalution model = new DetailEvalution()
                {
                    FormId = (int)preSemes.FormId,
                    UserID = userid,
                    CriteriaID = item.criteriaId,
                    Level = positionTurn,
                    Score = item.score,
                    Note = item.note,
                    Image_proof = item.imageProof,
                };
                dao.InsertDetailEvaluation(model);
            }
            return 1;
        }
        // lấy UserGroup có groupID
        public GroupUserModel GetGroupUserById(int groupId)
        {
            var result = dao.GetGroupUserById(groupId);
            if(result!=null)
            {
                GroupUserModel model = new GroupUserModel()
                {
                    groupId = result.GroupId,
                    name = result.Name,
                    status = (int)result.Status,
                    templateId = result.TemplateID,
                    timeId = result.TimeID
                };
                return model;
            }
            return null;
        }
        //Tính điểm Form theo học kỳ của User
        public List<SemesterModel> CalcScoreByUserId(int userId)
        {
            var detailforms = GetDetailFormsById(userId);
            var listSemesters = GetSemesterById(userId);

            //// Tính điểm
            foreach (var semester in listSemesters)
            {
                // Xét theo từng semester
                if (semester.FormId != null)
                {
                    // Đếm lượt chấm hiện tại
                    var turn = GetTurnNow(semester.semesterId, userId);
                    // Gán điểm 
                    switch (turn)
                    {
                        case 4:
                            semester.score4 = 0;
                            goto case 3;
                        case 3:
                            semester.score3 = 0;
                            goto case 2;
                        case 2:
                            semester.score2 = 0;
                            goto case 1;
                        case 1:
                            semester.score1 = 0;
                            break;
                    }
                    // Cộng điểm vào biến khi Chi tiết phiếu này có mã học kỳ bằng mã hk đang xét, theo level
                    foreach (var detail in detailforms)
                    {
                        // level1 Đoàn viên chấm
                        if (detail.evalutionForm.semesterId == semester.semesterId && detail.level == 1)
                        {
                            semester.score1 += detail.score;
                        }
                        // level2 BT chi đoàn chấm
                        if (detail.evalutionForm.semesterId == semester.semesterId && detail.level == 2)
                        {
                            semester.score2 += detail.score;
                        }
                        // level3 BT đoàn khoa chấm
                        if (detail.evalutionForm.semesterId == semester.semesterId && detail.level == 3)
                        {
                            semester.score3 += detail.score;
                        }
                        // level4 BT đoàn trường chấm
                        if (detail.evalutionForm.semesterId == semester.semesterId && detail.level == 4)
                        {
                            semester.score4 += detail.score;
                        }
                    }
                }
            }
            listSemesters[0].inProcess = true;
            return listSemesters;
        }    
    }
    
}