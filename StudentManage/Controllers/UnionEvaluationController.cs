using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentManage.BUS;
using StudentManage.Models;
using Models.EntityModel;

namespace StudentManage.Controllers
{
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
        public ActionResult EvaluationForm()
        {
            EvaluationBUS modelBUS = new EvaluationBUS();
            var user = new UserBUS().GetUserByID((int)Session["USER_ID"]);
            var listMain = modelBUS.GetAllMainByTemplate("G4");
            var listCriteria = modelBUS.GetAllCriteriaByTemplate("G4");
            ViewBag.listMain = listMain;
            ViewBag.listCriteria = listCriteria;
            ViewBag.user = user;

            
            return View();
        }
        // View đoàn viên
        public ActionResult Union()
        {
            int userId = (int)Session["USER_ID"];
            var detailforms = modelBUS.GetDetailFormsById(userId);
            var listSemesters = modelBUS.GetSemesterById(userId);
            
            //// Tính điểm
            foreach (var semester in listSemesters)
            {
               
                // Xét theo từng semester
                if (semester.FormId != null)
                {
                    // Đếm lượt
                    var turn = modelBUS.GetTurnNow(semester.semesterId, userId);
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
            //===============================================
            ViewBag.detail = detailforms;
            ViewBag.semesters = listSemesters;
            return View();
        }
        [HttpPost]
        public ActionResult Evaluation(List<EvaluationModel> listmodel)
        {
           var user = new UserBUS().GetUserByID((int)Session["USER_ID"]);
            // Tạo mới form nếu chưa tạo
            var preSemes = modelBUS.GetPresentSemester(user.userID);
            if (preSemes.FormId == null)
            {
                EvalutionFormModel form = new EvalutionFormModel()
                {
                    semesterId = preSemes.semesterId,
                    status = 1,
                    createAt = DateTime.Now,
                    createBy = user.userID,
                };
                // Insert Form
                modelBUS.InsertEvaluationForm(form);
                // Insert Detail
                modelBUS.InsertListDetailEvaluation(listmodel, user.userID);
                TempData["message"] = 1;
            }
            // Trường hợp đã có Form
            else
            {
                modelBUS.InsertListDetailEvaluation(listmodel, user.userID);
                TempData["message"] = 2;
            }
            return RedirectToAction("EvaluationForm");
        }

        // View Bi thu chi doan
        // View Bi thu doan khoa
        // View Bi thu doan truong
    }
}
