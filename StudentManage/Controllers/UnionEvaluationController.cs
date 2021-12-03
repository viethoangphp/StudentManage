using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentManage.BUS;
using StudentManage.Models;
using Models.EntityModel;
using StudentManage.Library;
namespace StudentManage.Controllers
{
    [UserAuthorze]
    public class UnionEvaluationController : Controller
    {
        protected EvaluationBUS modelBUS = new EvaluationBUS();
        // GET: UnionEvaluation
        public ActionResult Index()
        {
            // Phân chia trang theo user id
            return View();
        }
        // Form chấm điểm đoàn viên
        public ActionResult EvaluationForm(int? formId)
        {
            EvaluationBUS modelBUS = new EvaluationBUS();
            
            // Render Form chấm điểm
            //int templateId = modelBUS.GetGroupUserById(user.groupID).templateId;
            var listMain = modelBUS.GetAllMainByTemplateId(4);
            var listCriteria = modelBUS.GetAllCriteriaByTemplateId(4);

            // Thông tin user Session
            var userSession = new UserBUS().GetUserByID((int)Session["USER_ID"]);
            // Thông tin User được chấm
            var user = new UserModel();

            #region Xử lý Render các tiêu chí + điểm đánh giá
            //Model to View
            UnionFormModel model = new UnionFormModel();
            int? turn = 0;
            /* InIsTime() => trả về (int) thời gian chấm đang trong giai đoạn nào?
             * return 1 - Trong thời gian chấm điểm cá nhân
             * return 2 - Trong thời gian BTCĐ chấm
             * return 3 - Trong thời gian BTĐK chấm
             * return 4 - Trong thời gian BTĐT chấm
             * return 0 - ngoài các thời gian chấm kể trên
             */
            int isInTime = modelBUS.IsInTime();
            // Gán điểm theo tiêu chí và lượt chấm
            int[] listTotal = {0,0,0,0};
            if (formId == null)
            {
                // Trong thời gian chấm cá nhân
                if(isInTime == 1)
                {
                    // Tạo Form khi chưa tạo
                    var preSemes = modelBUS.GetPresentSemesterDetailByUserId(userSession.userID);
                    EvalutionFormModel form = new EvalutionFormModel()
                    {
                        semesterId = preSemes.semesterId,
                        status = 1,
                        createAt = DateTime.Now,
                        createBy = userSession.userID,
                    };
                    // Insert Form
                    formId = modelBUS.InsertEvaluationForm(form);
                    user = userSession;
                }   
                //Bạn thấy rối đúng không :)) Tôi cũng vậy !! I dont know what I am coding now
            }
            else
            {
                var details = modelBUS.GetDetailEvalutionsByFormId((int)formId);
                var form = modelBUS.GetEvaluationFormById((int)formId);
                user = new UserBUS().GetUserByID((int)form.createBy);
                // Xử lý trường hợp Tạo Form nhưng chưa chấm
                if(details.Count != 0)
                {
                    //Lấy Detail theo level để tìm ra lượt chấm hiện tại 
                    turn = (int)details.Select(x => x.level).Distinct().Max();
                    turn = (turn == null) ? 0 : turn;
                    model.Turn = turn;
                    foreach (var critial in listCriteria)
                    {
                        foreach (var detail in details)
                        {
                            switch (turn)
                            {
                                case 4:
                                    if (detail.critetiaId == critial.criteriaId && detail.level == 4)
                                    {
                                        critial.score4 = detail.score;
                                        if (detail.score != null)
                                        {
                                            listTotal[3] += (int)detail.score;
                                        }
                                    }
                                    goto case 3;
                                case 3:
                                    if (detail.critetiaId == critial.criteriaId && detail.level == 3)
                                    {
                                        critial.score3 = detail.score;
                                        listTotal[2] += (int)detail.score;
                                    }
                                    goto case 2;
                                case 2:
                                    if (detail.critetiaId == critial.criteriaId && detail.level == 2)
                                    {
                                        critial.score2 = detail.score;
                                        if (detail.score != null)
                                        {
                                            listTotal[1] += (int)detail.score;
                                        } 
                                    }
                                    goto case 1;
                                case 1:
                                    if (detail.critetiaId == critial.criteriaId && detail.level == 1)
                                    {
                                        critial.score1 = detail.score;
                                        if(detail.score != null)
                                        {
                                            listTotal[0] += (int)detail.score;
                                        }    
                                        
                                    }
                                    break;
                            }
                        }
                    }
                }     
            }
            #endregion

            #region Render hiển thị Input và Button chấm
            // Render Input và Button chấm
            //if(model.formId == modelBUS.GetPresentSemester().semesterId)// Xét học kỳ hiện tại/ không xét những học kỳ cũ
            //{
                if ((turn == 0 || turn == 1) && isInTime == 1)
                {
                    model.IsInTime = 1;
                }
                if ((turn == 0 || turn == 1 || turn == 2) && isInTime == 2 && userSession.groupID == 3)
                {
                    model.IsInTime = 2;
                }
                if ((turn == 2 || turn == 3) && isInTime == 3 && userSession.groupID == 4)
                {
                    model.IsInTime = 3;
                }
                if ((turn == 3 || turn == 4) && isInTime == 4 && userSession.groupID == 5)
                {
                    model.IsInTime = 4;
                }
            //}
            #endregion

            //=====================================================================
            model.Total = listTotal;
            model.ListMain = listMain;
            model.ListCriteria = listCriteria;
            model.formId =  (int)formId;
            // Check User Group Profile
            model.Assessor = modelBUS.GetGroupInfoByUserId(userSession.userID);
            model.Assessee = modelBUS.GetGroupInfoByUserId(user.userID);
            ViewBag.user = user;

            return View(model);
        }
        
        [HttpPost]
        public ActionResult Evaluation(List<EvaluationModel> listmodel, int evaluationFormId)
        {
            // Thông tin user Session
            var user = new UserBUS().GetUserByID((int)Session["USER_ID"]);

            #region InputScoreValidation
            // Lấy điểm chuẩn các tiêu chí
            int templateId = modelBUS.GetGroupUserById(user.groupID).templateId;
            var listMain = modelBUS.GetAllMainByTemplateId(templateId);
            var listCriteria = modelBUS.GetAllCriteriaByTemplateId(templateId);
            var listError = new List<EvaluationModel>();
            for (int i = 0, length = listCriteria.Count; i < length; i++)
            {
                int? evaScore = listmodel[i].score;
                if(evaScore < 0 || evaScore>listCriteria[1].score)
                {
                    ModelState.AddModelError("listmodel[" + i + "].score", "");
                    var model = new EvaluationModel()
                    {
                        criteriaId = listmodel[i].criteriaId,
                        score = evaScore,
                        Order = i
                    };
                    listError.Add(model);
                }    
            }
            #endregion

            #region Insert điểm và trả thông báo + trả về list Error
            // Lấy thông tin Detail Evaluation - Chi tiết phiếu
            var detailForm = modelBUS.GetDetailEvalutionsByFormId(evaluationFormId);
            // Lấy Evaluation Form - Phiếu
            var evaluationForm = modelBUS.GetEvaluationFormById(evaluationFormId);
            if (ModelState.IsValid)
            {
                // TH Tạo Form nhưng chưa chấm - Chấm điểm
                if (detailForm.Count == 0)
                {
                    // Insert Detail
                    modelBUS.InsertListDetailEvaluation(listmodel, user.userID, evaluationFormId);
                    TempData["MESSAGE"] = "Chấm điểm thành công!";
                }
                // TH Tạo Form và đã chấm - Cập nhật điểm
                else
                {
                    // Insert Detail
                    modelBUS.InsertListDetailEvaluation(listmodel, user.userID, evaluationFormId);
                    TempData["MESSAGE"] = "Cập nhật điểm thành công!";
                }
                if(evaluationForm.createBy == user.userID)
                {
                    return RedirectToAction("Union");
                }    
            }
            if(listError.Count != 0)
            {
                TempData["ERROR"] = "Điểm không hợp lệ!";
                TempData["listError"] = listError;// ListError từ Validation
            }
            #endregion

            //===========================================================================
            return RedirectToAction("EvaluationForm", new { formId = evaluationFormId });
        }

        #region View Đoàn Viên
        // View đoàn viên
        public ActionResult Union()
        {
            int userId = (int)Session["USER_ID"];
            // Tính điểm từng phiếu trong từng học kỳ
            var listSemesters = modelBUS.CalcScoreByUserId(userId);

            // Trường hợp đoàn viên chưa trải qua học kì nào
            // => Thêm học kỳ hiện tại để Đoàn viên tiến hành đánh giá
            if (listSemesters == null)
            {
                listSemesters = new List<SemesterModel>();
                SemesterModel nowSemester = new SemesterModel();
                nowSemester = modelBUS.GetPresentSemester();
                listSemesters.Add(nowSemester);
            }
            //=========================================================================
            ViewBag.semesters = listSemesters;
            return View();
        }
        #endregion

        #region View BT Chi đoàn
        // View bí thư chi đoàn 
        public ActionResult ClassEvaluation(int classId)
        {
            return View(classId);
        }
        // Danh sách phiếu trong lớp - JSON
        public JsonResult GetClassEvaluation(int classId)
        {
            var result = modelBUS.GetClassFormByClassIdAndSemesterId(classId, modelBUS.GetPresentSemester().semesterId);
            return Json(new {data = result}, JsonRequestBehavior.AllowGet);
        }
        // Search và Filter Danh sách phiếu trong lớp
        public JsonResult SearchClassEvaluation(int classId = 0, int semesterId = 0, int status = -1, string situation = "", string searchText = "")
        {
            // Thông tin user Session
            var userSession = new UserBUS().GetUserByID((int)Session["USER_ID"]);
            if (classId == 0)
            {
                classId = userSession.classID;
            }
            if (semesterId == 0)
            {
                semesterId = modelBUS.GetPresentSemester().semesterId;
            }
            
            var result = modelBUS.GetClassFormByClassIdAndSemesterId(classId, semesterId).Where(x=> status == -1||x.Status == status);
            result = result.Where(x => String.Equals(x.Situation, situation) || String.IsNullOrEmpty(situation));
            result = result.Where(x => String.IsNullOrEmpty(searchText) || x.FullName.ToLower().Contains(searchText.ToLower()) || x.StudentCode.Contains(searchText));
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
        // Partial ListSemester
        public ActionResult ListSemester()
        {
            var model = modelBUS.GetAllSemesters().ToList();
            return PartialView(model);
        }
        

        // Thêm hay cập nhật Note cho phiếu chấm
        [HttpPost]
        public JsonResult AddOrUpdateEvaluationFormNote(int formId, string updateNote)
        {
            string text = String.IsNullOrEmpty(updateNote) ? null : updateNote;
            bool result = modelBUS.UpdateEvaluationFormNote(formId, text);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region View BT đoàn khoa
        // View Bí thư đoàn khoa
        public ActionResult FacultyEvaluation(int facultyId)
        {
            var faculty = new FacultyBUS().GetFacultyByID(facultyId);
            return View(faculty);
        }
        // Json Faculty Mặc định
        public JsonResult GetFacultyEvaluation(int facultyId)
        {
            List<FacultyEvaluationModel> list = modelBUS.GetListClassByFaculty(facultyId);
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }
        // Partial View DS lớp
        public ActionResult GetListClassByFacultyId(int facultyId)
        {
            var result = new FacultyBUS().GetListClassByFaculty(facultyId).ToList();
            return PartialView(result);
        }
        // Tìm kiếm 
        public JsonResult SearchFacultyEvaluation(int facultyId = 0, int classCondition = -1, int facultyCondition = -1, int classId = 0)
        {
            // Thông tin user Session
            var userSession = new UserBUS().GetUserByID((int)Session["USER_ID"]);
            if (facultyId == 0)
            {
                facultyId = userSession.facultyID;
            }
            var list = modelBUS.GetListClassByFaculty(facultyId).Where(x => classCondition == -1 || x.ClassSituation == classCondition);
            list = list.Where(x => facultyCondition == -1 || x.FacultySituation == facultyCondition);
            list = list.Where(x => classId == 0 || x.ClassId == classId);
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region View BT Đoàn trường
        // View Bí thư đoàn trường
        public ActionResult SchoolEvaluation()
        {
            return View();
        }
        public JsonResult GetListFacultyEvaluation()
        {
            var result = modelBUS.GetListFacultyEvalution();
            return Json(new { data = result}, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListFaculty()
        {
            var result = new FacultyBUS().GetListFaculty();
            return PartialView(result);
        }
        public JsonResult SearchSchoolEvaluation(int FacultyId = 0, int FacultySituation = -1, int SchoolSituation = -1)
        {
            // Thông tin user Session
            var userSession = new UserBUS().GetUserByID((int)Session["USER_ID"]); 
            var result = modelBUS.GetListFacultyEvalution().Where(x=>FacultyId == 0 ||x.FacultyId == FacultyId);
            result = result.Where(x => FacultySituation == -1 || x.FacultySituation == FacultySituation);
            result = result.Where(x => SchoolSituation == -1 || x.SchoolSituation == SchoolSituation);
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }    
        #endregion

    }
}
