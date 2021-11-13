using System;
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
        protected UnionEvaluationDAO dao = new UnionEvaluationDAO();
        #region RenderForm
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
        #endregion
        //==============================================================================
        /*
         * Phần Chấm điểm đoàn viên
         */
        // Lấy Học kỳ bởi ID Học Kỳ
        public SemesterModel GetSemesterBySemesterId(int semesterId)
        {
            var result = dao.GetSemesterBySemesterId(semesterId);
            if (result != null)
            {
                SemesterModel model = new SemesterModel()
                {
                    semesterId = semesterId,
                    dayStart = result.Day_Start,
                    dayEnd = result.Day_End,
                    status = result.Status,
                    year = result.Year,
                    name = result.Name
                };
                return model;
            }
            return null;
        }
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
                    semesterId = item.SemesterID, 
                    note = item.Note
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

        // Lấy tất cả các học kì từ HK đầu tiên đến NAY của User đang xét
        public List<SemesterModel> GetSemesterById(int userId)
        {
            var result = dao.GetSemesterById(userId);
            if (result != null)
            {
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
            return null;
        }

        // Lấy vòng chấm hiện tại
        public int GetTurnNow(int semsterId, int userid)
        {
            var preSemes = dao.GetPresentSemester();
            var detail = dao.GetDetailFormsById(userid);
            int? turn = detail.Where(x => x.EvalutionForm.SemesterID == semsterId).Max(x => x?.Level);
            if (turn == null)
            {
                return 0;
            }
            return (int)turn;
        }
        //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        //lấy vòng chấm của một phiếu bất kì
        public int GetTurnByFormId(int formId)
        {
            var form = GetEvaluationFormById(formId);
            int? turn = dao.GetDetailEvalutionsByFormId(formId).Select(x => x.Level).Distinct().Max();
            if (turn == null)
            {
                return 0;
            }
            return (int)turn;
        }

        // Lấy chi tiết HK hiện tại của User
        public SemesterModel GetPresentSemesterByUserId(int userid)
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
            if (hasForm != null)
            {
                model.FormId = hasForm.FormId;
            }
            else
            {
                model.FormId = null;
            }
            return model;
        }

        // Lấy HK hiện tại của toàn trường
        public SemesterModel GetPresentSemester()
        {
            var result = dao.GetPresentSemester();
            if (result != null)
            {
                var model = new SemesterModel();
                model.semesterId = result.SemesterID;
                model.status = result.Status;
                model.year = result.Year;
                model.name = result.Name;
                model.dayEnd = result.Day_End;
                model.dayStart = result.Day_Start;
                return model;
            }
            return null;
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
                semesterId = result.SemesterID,
                status = result.Status,
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
        //Insert chi tiết phiếu
        public int InsertListDetailEvaluation(List<EvaluationModel> listDetail, int userid, int evaluationFormId)
        {
            var user = new UserDAO().GetUserByID(userid);
            // Tìm Position
            int position = user.Position.PositionID;
            int dv = dao.FindPositionByName("Đoàn Viên");
            int btcd = dao.FindPositionByName("Bí Thư Chi Đoàn");
            int btdk = dao.FindPositionByName("Bí Thư Đoàn Khoa");
            int btdt = dao.FindPositionByName("Bí Thư Đoàn Trường");
            //
            int positionTurn;
            int turn = GetTurnNow(GetPresentSemesterByUserId(userid).semesterId, userid);

            if (position == dv) positionTurn = 1;
            else
            {
                if (position == btcd)
                {
                    if (IsInTimeEvaluationByGroupId(2) == 0)
                    {
                        positionTurn = 1;
                    }
                    else
                    {
                        positionTurn = 2;
                    }
                }
                else
                {
                    if (position == btdk)
                    {
                        if (IsInTimeEvaluationByGroupId(2) == 0)
                        {
                            positionTurn = 1;
                        }
                        else
                        {
                            if (IsInTimeEvaluationByGroupId(3) == 0)
                            {
                                positionTurn = 2;
                            }
                            else
                            {
                                positionTurn = 3;
                            }
                        }
                    }
                    else positionTurn = 4;
                }
            }
            var preSemes = GetPresentSemesterByUserId(userid);
            foreach (EvaluationModel item in listDetail)
            {
                DetailEvalution model = new DetailEvalution()
                {
                    FormId = evaluationFormId,
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

        // Lấy UserGroup có groupID
        public GroupUserModel GetGroupUserById(int groupId)
        {
            var result = dao.GetGroupUserById(groupId);
            if (result != null)
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
        // Lấy thời gian đánh giá theo GroupUser Id 
        //  0 : Đang trong thời gian đánh giá
        //  1 : Đã qua thời gian đánh giá
        public int IsInTimeEvaluationByGroupId(int groupId)
        {
            var groupUser = GetGroupUserById(groupId);
            var timeEvaluation = dao.GetTimeEvaluationByTimeId(groupUser.timeId);
            if (DateTime.Compare(DateTime.Now, (DateTime)timeEvaluation.Date_Start) > 0 &&
               DateTime.Compare((DateTime)timeEvaluation.Date_End, DateTime.Now) > 0)
            {
                return 0;
            }
            return 1;

        }
        public int IsInTime()
        {
            if(IsInTimeEvaluationByGroupId(2) == 0)
            {
                return 1;
            }
            else if(IsInTimeEvaluationByGroupId(3) == 0)
            {
                return 2;
            }
            else if (IsInTimeEvaluationByGroupId(4) == 0)
            {
                return 3;
            }else if (IsInTimeEvaluationByGroupId(5) == 0)
            {
                return 4;
            }else
            {
                return 0;
            }    
        }    

        // Tính điểm Form 1 Học kỳ của User
        public SemesterModel CalcSingleSemesterScore(int userId, int semesterId)
        {
            var semester = GetSemesterBySemesterId(semesterId);
            var form = GetPassedEvalutionFormsById(userId).Where(x => x.semesterId == semesterId).FirstOrDefault();
            if (form != null)
            {
                var detailforms = GetDetailEvalutionsByFormId(form.formId);
                // Đếm lượt chấm hiện tại
                var turn = GetTurnNow(semesterId, userId);

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
                // Gán Note
                semester.Note = form.note;

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
                return semester;
            }
            return null;
        }
        // Tính điểm Form tất cả học kỳ của User
        public List<SemesterModel> CalcScoreByUserId(int userId)
        {
            var detailforms = GetDetailFormsById(userId);
            var listEvaluationForms = dao.GetAllEvaluationFormsByUserId(userId);
            

            // Trường hợp đã có Form đã chấm
            var listSemesters = GetSemesterById(userId);
            if (listSemesters != null)
            {
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
            else
            {
                if(listEvaluationForms.Count != 0)
                {
                    var presentForm = listEvaluationForms.OrderByDescending(x => x.Create_At).FirstOrDefault();
                    // Trường hợp Tạo Form nhưng chưa chấm
                    if (presentForm.SemesterID == GetPresentSemester().semesterId)
                    {
                        listSemesters = new List<SemesterModel>();
                        SemesterModel nowSemester = new SemesterModel();
                        nowSemester = GetPresentSemester();
                        nowSemester.FormId = presentForm.FormId;
                        listSemesters.Add(nowSemester);
                        return listSemesters;
                    }
                }    
            }
            return null;
        }


        // Lấy tất cả các phiểu của lớp
        public List<PersonalFormModel> GetClassFormByClassIdAndSemesterId(int classId, int semesterId)
        {
            var listClass = new UserBUS().GetListUserByClass(classId);
            var listForms = new List<EvalutionFormModel>();
            List<PersonalFormModel> list = new List<PersonalFormModel>();
            // Lấy tất cả các Form của SV 1 lớp, ở 1 hk
            foreach (var person in listClass)
            {
                EvalutionFormModel model = GetPassedEvalutionFormsById(person.userID).Where(x => x.semesterId == semesterId && x.createBy == person.userID).FirstOrDefault();
                if(model!=null)
                {
                    listForms.Add(model);
                }     
            }
            var test = listForms;
            foreach (var form in listForms)
            {
                var person = new UserDAO().GetUserByID((int)form.createBy);
                var detailSemester = CalcSingleSemesterScore((int)form.createBy, semesterId);
                if(detailSemester != null)
                {
                    PersonalFormModel model = new PersonalFormModel();
                    model.StudentCode = person.StudentCode;
                    model.FullName = person.FullName;
                    model.BirthDate = person.Birthday;
                    //Form id
                    model.formId = form.formId;
                    //Note
                    model.Note = detailSemester.Note;
                    // Điểm                
                    model.Score1 = detailSemester.score1;
                    model.Score2 = detailSemester.score2;
                    model.Score3 = detailSemester.score3;
                    model.Score4 = detailSemester.score4;
                    // Xếp loại
                    if (model.Score4 > 90)
                    {
                        model.Ranking = "Xuất Sắc";
                    }
                    else if (model.Score4 > 70)
                    {
                        model.Ranking = "Khá";
                    }
                    else if (model.Score4 >= 50)
                    {
                        model.Ranking = "Trung Bình";
                    }
                    else if (model.Score4 != null)
                    {
                        model.Ranking = "Yếu";
                    };
                    // Tình trạng
                    if (model.Score1 == null)
                        model.Situation = "Chờ Chấm";
                    else if (model.Score2 == null)
                        model.Situation = "Chờ Lớp";
                    else if (model.Score3 == null)
                        model.Situation = "Chờ Khoa";
                    else if (model.Score4 == null)
                        model.Situation = "Chờ Trường";
                    else model.Situation = "Hoàn Thành";
                    list.Add(model);
                }    
            }
            return list;
        }
    }

}