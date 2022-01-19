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
        // Lấy thông tin GroupId cho User với UserID
        /* Input => UserId
         * return 1 - Nếu là Group Đoàn viên
         * return 2 - Nếu là Group Bí thư chi đoàn
         * return 3 - Nếu là Group Bí thư đoàn khoa
         * return 4 - Nếu là Group Bí thư đoàn trường  
         */
        public int GetGroupInfoByUserId(int userId)
        {
            var user = new UserDAO().GetUserByID(userId);
            int groupId = user.GroupId;
            int dv = dao.FindGroupIdByName("ĐOÀN VIÊN");
            int btcd = dao.FindGroupIdByName("Bí Thư Chi Đoàn");
            int btdk = dao.FindGroupIdByName("Bí Thư Đoàn Khoa");
            int btdt = dao.FindGroupIdByName("Bí Thư Đoàn Trường");
            if(groupId == dv)
            {
                return 1;
            }    
            else if(groupId == btcd)
            {
                return 2;
            }    
            else if(groupId == btdk)
            {
                return 3;
            }    
            else if(groupId == btdt)
            {
                return 4;
            }
            else
            {
                return 0;
            }
        }
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
        // Lấy tất cả học kỳ 
        public List<SemesterModel> GetAllSemesters()
        {
            var result = dao.GetAllSemesters();
            List<SemesterModel> list = new List<SemesterModel>();
            if(result != null)
            {
                foreach (var semester in result)
                {
                    var model = new SemesterModel();
                    model.semesterId = semester.SemesterID;
                    model.name = semester.Name;
                    model.year = semester.Year;
                    model.dayStart = semester.Day_Start;
                    model.dayEnd = semester.Day_End;
                    model.status = semester.Status;
                    model.displayName = "Đánh giá rèn luyện học kỳ " + semester.Name + " năm học " + semester.Year;
                    list.Add(model);
                    
                }
                return list;
            }
            return null;
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
        public SemesterModel GetPresentSemesterDetailByUserId(int userid)
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
        public int InsertListDetailEvaluation(List<EvaluationModelM> listDetail, int userid, int evaluationFormId)
        {
            var user = new UserDAO().GetUserByID(userid);
            // Tìm Position
            int position = user.Position.PositionID;
            int dv = dao.FindPositionByName("ĐOÀN VIÊN");
            int btcd = dao.FindPositionByName("Bí Thư Chi Đoàn");
            int btdk = dao.FindPositionByName("Bí Thư Đoàn Khoa");
            int btdt = dao.FindPositionByName("Bí Thư Đoàn Trường");

            int positionTurn;
            EvalutionFormModel evaluationForm = GetEvaluationFormById(evaluationFormId);
            int turn = GetTurnNow(GetPresentSemesterDetailByUserId(userid).semesterId, (int)evaluationForm.createBy);

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
            var preSemes = GetPresentSemesterDetailByUserId(userid);

            // Insert Detail Evaluation
            foreach (EvaluationModelM item in listDetail)
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
                    Type = 2
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

            //if (DateTime.Compare(DateTime.Now, (DateTime)timeEvaluation.Date_Start) > 0 &&
            //   DateTime.Compare((DateTime)timeEvaluation.Date_End, DateTime.Now) > 0)
            if(timeEvaluation.Status == 1)
            {
                return 0;
            }
            return 1;

        }
        /* InIsTime() => trả về (int) thời gian chấm đang trong giai đoạn nào?
            * return 1 - Trong thời gian chấm điểm cá nhân
            * return 2 - Trong thời gian BTCĐ chấm
            * return 3 - Trong thời gian BTĐK chấm
            * return 4 - Trong thời gian BTĐT chấm
            * return 0 - ngoài các thời gian chấm kể trên
            */
        public int IsInTime()
        {
            int dv =   dao.FindGroupIdByName("ĐOÀN VIÊN");
            int btcd = dao.FindGroupIdByName("Bí Thư Chi Đoàn");
            int btdk = dao.FindGroupIdByName("Bí Thư Đoàn Khoa");
            int btdt = dao.FindGroupIdByName("Bí Thư Đoàn Trường");
            if (IsInTimeEvaluationByGroupId(dv) == 0)
            {
                return 1;
            }
            else if (IsInTimeEvaluationByGroupId(btcd) == 0)
            {
                return 2;
            }
            else if (IsInTimeEvaluationByGroupId(btdk) == 0)
            {
                return 3;
            }
            else if (IsInTimeEvaluationByGroupId(btdt) == 0)
            {
                return 4;
            }
            else
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
                // Trường hợp có Detail => Có Phiếu và đã chấm
                if (detailforms.Count != 0)
                {
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
                // Trường hợp không có Detail => Có Phiếu nhưng chưa chấm => trả về null
                return null;

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
                int inTime = IsInTime();
                // Kiểm tra thời gian chấm có đang trong hk hiện tại
                if(inTime == 1)
                {
                    listSemesters[0].Available = true;
                }    
                
                return listSemesters;
            }
            else
            {
                if (listEvaluationForms.Count != 0)
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


        // Lấy tất cả các phiểu của lớp trong Học kỳ
        public List<PersonalFormModel> GetClassFormByClassIdAndSemesterId(int classId, int semesterId)
        {
            //Lấy tất cả các thành viên trong lớp
            var listClass = new UserBUS().GetListUserByClass(classId);
            var listForms = new List<EvalutionFormModel>();

            //Lấy Template Id của phiếu => Lấy các tiêu chí của phiếu
            var templateId = dao.FindTemplateIdByName("ĐOÀN VIÊN");
            var listCriteria = GetAllCriteriaByTemplateId(templateId);

            List<PersonalFormModel> list = new List<PersonalFormModel>();
            // Lấy tất cả các Form của SV 1 lớp, ở 1 Học kỳ
            foreach (var person in listClass)
            {
                // Lấy Evaluation Form trong học kỳ của từng người
                EvalutionFormModel model = GetPassedEvalutionFormsById(person.userID).Where(x => x.semesterId == semesterId && x.createBy == person.userID).FirstOrDefault();
                if (model != null) // Nếu có Form thì Add
                {
                    var detailSemester = CalcSingleSemesterScore((int)model.createBy, semesterId);
                    if(detailSemester == null)
                    {
                        int totalCriteria = GetAllCriteriaByTemplateId(4).Count;// Phiếu chấm điểm đoàn viên (4)
                        if(IsInTime()==1)
                        {
                            //Tạo phiếu điểm khi sinh viên chưa chấm => Mặc định Điểm các tiếu chí bằng 0
                            foreach (var criteria in listCriteria)
                            {
                                // Tạo detail Phiếu có điểm = 0
                                DetailEvalution detailForm_Intime = new DetailEvalution()
                                {
                                    FormId = model.formId,
                                    UserID = (int)model.createBy,
                                    CriteriaID = criteria.criteriaId,
                                    Level = 1,
                                    Type = 2,
                                };
                                dao.InsertDetailEvaluation(detailForm_Intime);
                            }
                        }
                    }
                    listForms.Add(model);
                }
                else // Không có thì tiến hành Thêm Evalution Form Nếu trong thời gian chấm 2 
                {
                    var test = GetPresentSemester().semesterId;
                    if (semesterId == GetPresentSemester().semesterId && IsInTime()==2)
                    {
                        EvalutionFormModel form = new EvalutionFormModel()
                        {
                            semesterId = semesterId,
                            status = 1,
                            createAt = DateTime.Now,
                            createBy = person.userID,
                        };
                        // Insert Form
                        form.formId = InsertEvaluationForm(form);
                        listForms.Add(form);
                    }
                    else
                    {
                        // Xứ lý trường hợp thêm các Form thử - Form chưa tạo => trong thời gian
                        #region Form_NoReal detail
                        PersonalFormModel model_addition = new PersonalFormModel();
                        model_addition.StudentCode = person.studentCode;
                        model_addition.FullName = person.fullname;
                        model_addition.BirthDate = String.Format("{0:dd/MM/yyyy}", person.birthDay);
                        model_addition.formId = null;
                        model_addition.Score1 = null;
                        model_addition.Score2 = null;
                        model_addition.Score3 = null;
                        model_addition.Score4 = null;
                        // Xếp loại
                        if (model_addition.Score4 > 90)
                        {
                            model_addition.Ranking = "Xuất Sắc";
                        }
                        else if (model_addition.Score4 > 70)
                        {
                            model_addition.Ranking = "Khá";
                        }
                        else if (model_addition.Score4 >= 50)
                        {
                            model_addition.Ranking = "Trung Bình";
                        }
                        else if (model_addition.Score4 != null)
                        {
                            model_addition.Ranking = "Yếu";
                        };
                        // Tình trạng
                        if (model_addition.Score1 == null)
                            model_addition.Situation = "Chờ Chấm";
                        else if (model_addition.Score2 == null)
                            model_addition.Situation = "Chờ Lớp";
                        else if (model_addition.Score3 == null)
                            model_addition.Situation = "Chờ Khoa";
                        else if (model_addition.Score4 == null)
                            model_addition.Situation = "Chờ Duyệt";
                        else model_addition.Situation = "Hoàn Thành";

                        //Status Đã chấm/ Chưa chấm 
                        // 1: Đã chấm - BTCĐ
                        // 0: chưa chấm - BTCĐ
                        model_addition.Status = 0;
                        #endregion
                        list.Add(model_addition);

                    }

                }
            }
            // Lấy Chi tiết điểm trong từng Evaluation Form
            foreach (var form in listForms)
            {
                var person = new UserBUS().GetUserByID((int)form.createBy);
                // Tính toán điểm của Form theo học kỳ đang xét
                var detailSemester = CalcSingleSemesterScore((int)form.createBy, semesterId);
                if (detailSemester == null || detailSemester.score1 == null)
                {
                    if (IsInTime() == 2)
                    {
                        int totalCriteria = GetAllCriteriaByTemplateId(4).Count;// Phiếu chấm điểm đoàn viên (4)

                        //Tạo phiếu điểm khi sinh viên chưa chấm => Mặc định Điểm các tiếu chí bằng 0
                        foreach (var criteria in listCriteria)
                        {
                            DetailEvalution detailForm_Passed = new DetailEvalution()
                            {
                                FormId = form.formId,
                                UserID = (int)form.createBy,
                                CriteriaID = criteria.criteriaId,
                                Level = 1,
                                Score = 0,
                                Type = 2,
                            };
                            dao.InsertDetailEvaluation(detailForm_Passed);
                        }
                    }
                    // Ngược lại không làm gì
                }
                #region Xếp loại và trạng thái theo điểm
                PersonalFormModel model = new PersonalFormModel();
                model.StudentCode = person.studentCode;
                model.FullName = person.fullname;
                model.BirthDate = String.Format("{0:dd/MM/yyyy}", person.birthDay);
                //Form id
                model.formId = form.formId;

                if (detailSemester != null)
                {
                    //Note
                    model.Note = detailSemester.Note;
                    // Điểm
                    model.Score1 = detailSemester.score1;
                    model.Score2 = detailSemester.score2;
                    model.Score3 = detailSemester.score3;
                    model.Score4 = detailSemester.score4;
                }
                else
                {
                    model.Score1 = 0;
                }
            
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
                    model.Situation = "Chờ Duyệt";
                else model.Situation = "Hoàn Thành";

                //Status Đã chấm/ Chưa chấm 
                // 1: Đã chấm - BTCĐ
                // 0: chưa chấm - BTCĐ
                model.Status = (model.Score2 == null) ? 0 : 1;




                #endregion
              
                list.Add(model);

                #region this region for testing purpose
                //// Testing Paging
                //PersonalFormModel model2 = new PersonalFormModel();
                //model2.StudentCode = "1811063044";
                //model2.FullName = "Nguyễn Quang Minh";
                //model2.BirthDate = "11/02/2000";
                ////Form id
                //model2.formId = 5;


                ////Note
                //model2.Note = "DM hello";
                //// Điểm
                //model2.Score1 = 20;
                //model2.Score2 = 20;
                //model2.Score3 = 30;
                //model2.Score4 = 30;

                //model2.Ranking = "Xuất Sắc";

                //model.Status = 1;

                //for (int i = 0; i < 20; i++)
                //{
                //    list.Add(model2);
                //}
                #endregion

            }
            return list;
        }
        // Update Evaluation Form Note
        public bool UpdateEvaluationFormNote(int formId, string updateNote)
        {
            return dao.UpdateEvaluationFormNote(formId, updateNote);
        }

        public List<FacultyEvaluationModel> GetListClassByFaculty(int id)
        {

            List<FacultyEvaluationModel> list = new List<FacultyEvaluationModel>();
            // Học kỳ hiện tại
            var presentSemes = GetPresentSemester();
            // Get List User
            var listUser = new UserDAO().GetListUser();
            // Get list Class
            var listClass = new FacultyBUS().GetListClassByFaculty(id);
            var faculty = new FacultyBUS().GetFacultyByID(id);
            foreach (var item in listClass)
            {
                FacultyEvaluationModel model = new FacultyEvaluationModel();
                model.ClassName = item.className;
                model.ClassId = item.classID;
                model.Total = listUser.Where(x=>x.ClassID==item.classID).Count();
                var listPresonalForms = GetClassFormByClassIdAndSemesterId(item.classID, presentSemes.semesterId);
                int classDone = 0, facultyDone = 0, schoolDone = 0;
                foreach (var form in listPresonalForms)
                {
                    if (form.Score2 != null)
                    {
                        classDone++;
                    }
                    if (form.Score3 != null)
                    {
                        facultyDone++;
                    }
                    if(form.Score4 !=null)
                    {
                        schoolDone++;
                    }    
                };
                model.ClassDone = classDone;
                model.FacultyDone = facultyDone;
                model.ClassNotDone = model.Total - classDone;
                model.FacultyNotDone = model.Total - facultyDone;
                model.SchoolDone = schoolDone;
                model.ClassSituation = (classDone == model.Total) ? 1 : 0; 
                model.FacultySituation = (facultyDone == model.Total) ? 1 : 0; 
                list.Add(model);
            }
            return list;
        }
        public List<SchoolEvaluationModel> GetListFacultyEvalution()
        {
            // Lấy danh sách khoa
            var listFaculty = new FacultyBUS().GetListFaculty();
            var list = new List<SchoolEvaluationModel>();
            foreach(var item in listFaculty)
            {
                var model = new SchoolEvaluationModel();
                var listClassModel = GetListClassByFaculty(item.facultyID);

                model.FacultyId = item.facultyID;
                model.FacultyName = item.facultyName;
                model.Phone = item.phone;
                int FacultyDone = 0, SchoolDone = 0, Total = 0;
                foreach(var classEva in listClassModel)
                {
                    FacultyDone += (int)classEva.FacultyDone;
                    SchoolDone += (int)classEva.SchoolDone;
                    Total += (int)classEva.Total;
                }
                model.FacultyDone = FacultyDone;
                model.SchoolDone = SchoolDone;
                model.FacultyNotDone = Total - FacultyDone;
                model.SchoolNotDone = Total - SchoolDone;
                model.FacultySituation = (FacultyDone == Total) ? 1 : 0;
                model.SchoolSituation = (SchoolDone == Total) ? 1 : 0;
                list.Add(model);
            }
            return list;
        }
    }

}

