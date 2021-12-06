//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using Models.DAO;
//using Models.EntityModel;
//using StudentManage.Models;
//namespace StudentManage.BUS
//{
//    public class EvaluationBUS
//    {
//        protected DetailEvaluationDAO dao = new DetailEvaluationDAO();

//        /*
//         Lấy ra các Main và tiêu chí để render được Form chấm điểm
//         */
//        //lấy tất cả Main
//        public List<EvaluativeMainModel> getAllMainByTemplate(string templateName)
//        {
//            var result = dao.getAllMainByTemplate(templateName);
//            List<EvaluativeMainModel> listModel = new List<EvaluativeMainModel>();
//            foreach (var item in result)
//            {
//                EvaluativeMainModel model = new EvaluativeMainModel()
//                {
//                    mainId = item.MainID,
//                    content = item.Content,
//                    status = (int)item.Status,
//                    templateId = item.TemplateID
//                };
//                listModel.Add(model);
//            }
//            return listModel;
//        }
//        //Lấy tất cả các tiêu chi by TemplateName
//        public List<EvaluativeCriteriaModel> getAllCriteriaByTemplate(string templateName)
//        {
//            var result = dao.getAllCriteriaByTemplate(templateName);
//            List<EvaluativeCriteriaModel> listModel = new List<EvaluativeCriteriaModel>();
//            foreach (var item in result)
//            {
//                EvaluativeCriteriaModel model = new EvaluativeCriteriaModel()
//                {
//                    criteriaId = item.CriteriaID,
//                    criteriaContent = item.CriteriaContent,
//                    criteriaRequirement = item.CriteriaRequirement,
//                    score = item.Score,
//                    mainId = item.MainID
//                };
//                listModel.Add(model);
//            }
//            return listModel;
//        }

//        /*
//         *Phần Chấm điểm đoàn viên
//         */
//        //Lấy tất cá các Form đã chấm (đã qua) của đoàn viên
//        public List<EvalutionFormModel> getPassedEvalutionFormsById(int userId)
//        {
//            var result = dao.getPassedEvalutionFormsById(userId);
//            List<EvalutionFormModel> listModel = new List<EvalutionFormModel>();
//            foreach (var item in result)
//            {
//                EvalutionFormModel model = new EvalutionFormModel()
//                {
//                    formId = item.FormId,
//                    createAt = item.Create_At,
//                    createBy = item.Create_by,
//                    total = item.Total,
//                    semesterId = item.SemesterID
//                };
//                listModel.Add(model);
//            }
//            return listModel;
//        }
//        //Lấy tất các các DetailForm của Form đã chấm
//        public List<DetailEvalutionModel> getDetailFormsById(int userId)
//        {
//            var result = dao.getDetailFormsById(userId);
//            List<DetailEvalutionModel> listModel = new List<DetailEvalutionModel>();
//            foreach (var item in result)
//            {
//                DetailEvalutionModel model = new DetailEvalutionModel()
//                {
//                    formId = item.FormId,
//                    userId = item.UserID,
//                    critetiaId = item.CriteriaID,
//                    score = item.Score,
//                    note = item.Note,
//                    level = item.Level,
//                    imageProof = item.Image_proof,
//                    evalutionForm =
//                    {
//                        formId = item.EvalutionForm.FormId,
//                        createAt = item.EvalutionForm.Create_At,
//                        createBy = item.EvalutionForm.Create_by,
//                        total = item.EvalutionForm.Total,
//                        status = item.EvalutionForm.Status,
//                        semesterId = item.EvalutionForm.SemesterID
//                    }

//                };
//                listModel.Add(model);
//            }
//            return listModel;
//        }
//        //Lấy tất cả các học kì từ HK đầu tiên đến NAY
//        public List<SemesterModel> getSemesterById(int userId)
//        {
//            var result = dao.getSemesterById(userId);
//            List<SemesterModel> listSemesters = new List<SemesterModel>();
//            foreach (var item in result)
//            {
//                var test = item.EvalutionForms.Where(x => x.Create_by == userId).FirstOrDefault();
//                SemesterModel model = new SemesterModel();

//                model.semesterId = item.SemesterID;
//                model.name = item.Name;
//                model.dayStart = item.Day_Start;
//                model.dayEnd = item.Day_End;
//                model.status = item.Status;
//                model.year = item.Year;
//                if (test != null)
//                {
//                    model.FormId = test.FormId;
//                }
//                listSemesters.Add(model);
//            }
//            return listSemesters;
//        }
//        //Lấy vòng chấm hiện tại
//        public int getTurnNow(int semsterId, int userid)
//        {
//            var preSemes = dao.getPresentSemester();
//            var detail = dao.getDetailFormsById(userid);
//            int? turn = detail.Where(x=>x.EvalutionForm.SemesterID==semsterId).Select(x=>x.Level).Distinct().Max();
//            if(turn == null)
//            {
//                return 0;
//            }    
//            return (int)turn;
//        }
//        //get present semester of User
//        public SemesterModel getPresentSemester(int userid)
//        {
//            var result = dao.getPresentSemester();
//            var hasForm = result.EvalutionForms.Where(x => x.Create_by == userid).FirstOrDefault();
            
//            SemesterModel model = new SemesterModel();
//            model.semesterId = result.SemesterID;
//            model.dayEnd = result.Day_End;
//            model.dayStart = result.Day_Start;
//            model.status = result.Status;
//            model.year = result.Year;
//            model.name = result.Name;
//            if(hasForm!=null)
//            {
//                model.FormId = hasForm.FormId;
//            }
//            return model;
//        }
//        //==========================================
//        //Insert EvaluationForm
//        public int InsertEvaluationForm(EvalutionFormModel form)
//        {
//            EvalutionForm model = new EvalutionForm()
//            {
//                Total = form.total,
//                Status = form.status,
//                Create_by = form.createBy,
//                SemesterID = form.semesterId,
//                Create_At = form.createAt,
//            };
//            return dao.InsertEvaluationForm(model);
//        }
//        //Insert List of Detail Evaluation
//        public int InsertListDetailEvaluation(List<EvaluationModel> listDetail, int userid)
//        {
//            var user = new UserDAO().GetUserByID(userid);
//            int position = user.Position.PositionID;
//            int positionTurn;
//            int turn = getTurnNow(getPresentSemester(userid).semesterId,userid); 
//            int dv = dao.findPositionByName("Đoàn Viên");
//            int btcd = dao.findPositionByName("Bí Thư Chi Đoàn");
//            int btdk = dao.findPositionByName("Bí Thư Đoàn Khoa");
//            int btdt = dao.findPositionByName("Bí Thư Đoàn Trường");
//            if (position == dv ) positionTurn = 1;
//            else
//            {
//                if(position == btcd)
//                {
//                    positionTurn = 2;
//                    if(turn<positionTurn)
//                    {
//                        positionTurn = turn + 1;
//                    }    
//                }    
                    
//                else
//                {
//                    if (position == btdk)
//                    {
//                        positionTurn = 3;
//                        if (turn < positionTurn)
//                        {
//                            positionTurn = turn + 1;
//                        }
//                    }    
                        
//                    else positionTurn = 4;
//                }    
//            }    
//            var preSemes = getPresentSemester(userid);
//            foreach (EvaluationModel item in listDetail)
//            {
//                DetailEvalution model = new DetailEvalution()
//                {
//                    FormId = (int)preSemes.FormId,
//                    UserID = userid,
//                    CriteriaID = item.criteriaId,
//                    Level = positionTurn,
//                    Score = item.score,
//                    Note = item.note,
//                    Image_proof = item.imageProof,
//                };
//                dao.InsertDetailEvaluation(model);
//            }
//            return 1;
//        }
//    }
//}