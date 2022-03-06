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

            // Thông tin user Session
            var userSession = new UserBUS().GetUserByID((int)Session["USER_ID"]);
            // Model
            var model = modelBUS.RenderEvaluationForm(formId, userSession.userID);
            model.Assessor = modelBUS.GetGroupInfoByUserId(userSession.userID);
            ViewBag.user = new UserBUS().GetUserByID(model.Assessee);

            // Chặn trường hợp cố tình tạo phiếu 
            if (model.HasCreatedForm == true && formId == null)
            {
                return RedirectToAction("Union", "UnionEvaluation");
            }
            else
            {  
                return View(model);
            }    
        }
        [HttpPost]
        public ActionResult Evaluation(List<EvaluationModelM> listmodel, int evaluationFormId)
        {
            // Thông tin user Session
            var user = new UserBUS().GetUserByID((int)Session["USER_ID"]);

            #region InputScoreValidation
            // Lấy điểm chuẩn các tiêu chí
            int templateId = modelBUS.GetGroupUserById(user.groupID).templateId;
            var listMain = modelBUS.GetAllMainByTemplateId(templateId);
            var listCriteria = modelBUS.GetAllCriteriaByTemplateId(templateId);
            var listError = new List<EvaluationModelM>();
            for (int i = 0, length = listCriteria.Count; i < length; i++)
            {
                int? evaScore = listmodel[i].score;
                if (evaScore < 0 || evaScore > listCriteria[1].score || evaScore == null)
                {
                    ModelState.AddModelError("listmodel[" + i + "].score", "");
                    var model = new EvaluationModelM()
                    {
                        criteriaId = listmodel[i].criteriaId,
                        score = evaScore,
                        Order = i
                    };
                    listError.Add(model);
                }
            }

            #endregion
            // Xử lý minh chứng ảnh 
            foreach (var item in listmodel)
            {
                if (item.imageProof != null)
                {
                    var fileName = "ImageProof_" + evaluationFormId + "_" + item.criteriaId + System.IO.Path.GetExtension(item.imageProof.FileName);
                    HttpPostedFileBase fileTemp = item.imageProof;
                    item.ImageURL = fileName;
                    fileTemp.SaveAs(Server.MapPath("~/Assets/img/Images_Proof_02/") + fileName);
                }
            }

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
                if (evaluationForm.createBy == user.userID)
                {
                    return RedirectToAction("Union");
                }
            }
            if (listError.Count != 0)
            {
                TempData["ERROR"] = "Điểm không hợp lệ!";
                TempData["listError"] = listError;// ListError từ Validation
            }
            #endregion

            //===========================================================================
            return RedirectToAction("EvaluationForm", new { formId = evaluationFormId });
        }
        [HttpPost]
        public JsonResult deleteImageProof(int formId, int criteriaId)
        {
            return Json(modelBUS.deleteImageProof(formId, criteriaId), JsonRequestBehavior.AllowGet);
        }

        #region AC Đoàn Viên
        // View đoàn viên
        public ActionResult Union()
        {
            int userId = (int)Session["USER_ID"];
            // Tính điểm từng phiếu trong từng học kỳ
            var listSemesters = modelBUS.CalcSemesterScoreByUserId(userId);
            //=========================================================================
            ViewBag.semesters = listSemesters;
            return View();
        }
        #endregion

        #region AC BT Chi đoàn
        // View bí thư chi đoàn 
        public ActionResult ClassEvaluation(int classId)
        {
            return View(classId);
        }
        // Danh sách phiếu trong lớp - JSON
        public JsonResult GetClassEvaluation(int classId)
        {
            var result = modelBUS.GetClassFormByClassIdAndSemesterId(classId, modelBUS.GetPresentSemester().semesterId);
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
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

            var result = modelBUS.GetClassFormByClassIdAndSemesterId(classId, semesterId).Where(x => status == -1 || x.Status == status);
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

        #region AC BT Đoàn Khoa
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

        #region AC BT Đoàn trường
        // View Bí thư đoàn trường
        public ActionResult SchoolEvaluation()
        {
            return View();
        }
        public JsonResult GetListFacultyEvaluation()
        {
            var result = modelBUS.GetListFacultyEvalution();
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
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
            var result = modelBUS.GetListFacultyEvalution().Where(x => FacultyId == 0 || x.FacultyId == FacultyId);
            result = result.Where(x => FacultySituation == -1 || x.FacultySituation == FacultySituation);
            result = result.Where(x => SchoolSituation == -1 || x.SchoolSituation == SchoolSituation);
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}


