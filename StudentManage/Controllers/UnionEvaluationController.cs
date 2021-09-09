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
            /*
            // Thông tin User
            var user = new UserBUS().GetUserByID((int)Session["USER_ID"]);

            // Render Form chấm điểm
            int templateId = modelBUS.GetGroupUserById(user.groupID).templateId;

            var listMain = modelBUS.GetAllMainByTemplateId(templateId);
            var listCriteria = modelBUS.GetAllCriteriaByTemplateId(templateId);

            // Lấy detail của Form nếu Đã có Form 
            if (formId != null)
            {   
                //Kiểm tra giả mạo link Form ID
                //if(modelBUS.GetEvaluationFormById((int)formId).createBy != user.userID)
                //{
                //    return RedirectToAction("Union", "UnionEvaluation");
                //}    
                var details = modelBUS.GetDetailEvalutionsByFormId((int)formId);
                ViewBag.listDetail = details;

                //  Kiểm tra Form có phải thuộc Hk đang xét=> Form đang chấm thì set lượt chấm hiện tại
                int turn;
                if (modelBUS.GetEvaluationFormById((int)formId).semesterId == modelBUS.GetPresentSemester(user.userID).semesterId)
                {
                    //Đếm lượt chấm hiện tại
                    turn = modelBUS.GetTurnNow(modelBUS.GetPresentSemester(user.userID).semesterId, user.userID);
                }
                else
                {
                    // Form đã chấm set lượt chấm 4
                    turn = 4;
                }
                ViewBag.turn = turn;
            }
            //  ===========================================================

            ViewBag.listMain = listMain;
            ViewBag.listCriteria = listCriteria;
            ViewBag.user = user;
            */
            var user = new UserBUS().GetUserByID((int)Session["USER_ID"]);
            // Render Form chấm điểm
            int templateId = modelBUS.GetGroupUserById(user.groupID).templateId;
            var listMain = modelBUS.GetAllMainByTemplateId(templateId);
            var listCriteria = modelBUS.GetAllCriteriaByTemplateId(templateId);

            int[] total = new int[4];
            // Gán điểm theo tiêu chí và lượt chấm
            if(formId!=null)
            {
                var details = modelBUS.GetDetailEvalutionsByFormId((int)formId);
                int? turn = (int)details.Select(x=>x.level).Distinct().Max();
                turn = (turn==null) ? 0 : turn;
                
                foreach (var critial in listCriteria)
                {
                    foreach (var detail in details)
                    {
                        switch(turn)
                        {
                            case 4:
                                if (detail.critetiaId == critial.criteriaId && detail.level == 4)
                                {
                                    critial.score4 = detail.score;
                                    total[3] += (int)detail.score; 
                                }
                                goto case 3;
                            case 3:
                                if (detail.critetiaId == critial.criteriaId && detail.level == 3)
                                {
                                    critial.score3 = detail.score;
                                    total[2] += (int)detail.score;
                                }
                                goto case 2;
                            case 2:
                                if (detail.critetiaId == critial.criteriaId && detail.level == 2)
                                {
                                    critial.score2 = detail.score;
                                    total[1] += (int)detail.score;
                                }
                                goto case 1;
                            case 1:
                                if (detail.critetiaId == critial.criteriaId && detail.level == 1)
                                {
                                    critial.score1 = detail.score;
                                    total[0] += (int)detail.score;
                                }
                                break;
                        }       
                    }
                }
            }    
            // ==========================================================
            ViewBag.user = user;
            ViewBag.listMain = listMain;
            ViewBag.listCriteria = listCriteria;
            ViewBag.total = total;
            return View();
        }
        // View đoàn viên
        public ActionResult Union()
        {
            int userId = (int)Session["USER_ID"];
            // Tính điểm từng phiếu trong từng học kỳ
            var listSemesters = modelBUS.CalcScoreByUserId(userId);
            //===============================================
            ViewBag.semesters = listSemesters;
            return View();
        }
        [HttpPost]
        public ActionResult Evaluation(List<EvaluationModel> listmodel)
        {
            //Kiểm tra xem Quyền chấm phiếu: Người chấm phiếu phải là Đoàn viên đang đăng nhập hoặc Các bí thư đoàn trường
            //
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
            }
            return RedirectToAction("EvaluationForm");
        }

        // View Bi thu chi doan
        // View Bi thu doan khoa
        // View Bi thu doan truong
    }
}
