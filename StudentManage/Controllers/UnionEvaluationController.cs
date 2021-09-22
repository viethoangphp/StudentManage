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
            // Thông tin user
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
                int? turn = (int)details.Select(x => x.level).Distinct().Max();        
                turn = (turn==null) ? 0 : turn;
                ViewBag.turn = turn;
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

            // Trường hợp đoàn viên chưa trải qua học kì nào
            if (listSemesters == null)
            {
                listSemesters = new List<SemesterModel>();
                SemesterModel nowSemester = new SemesterModel();
                nowSemester = modelBUS.GetPresentSemester();
                listSemesters.Add(nowSemester);
            } 
            //===============================================
            ViewBag.semesters = listSemesters;
            return View();
        }
        
        [HttpPost]
        public ActionResult Evaluation(List<EvaluationModel> listmodel)
        {
            // Thông tin user
            var user = new UserBUS().GetUserByID((int)Session["USER_ID"]);
            // Render Form chấm điểm
            int templateId = modelBUS.GetGroupUserById(user.groupID).templateId;
            var listMain = modelBUS.GetAllMainByTemplateId(templateId);
            var listCriteria = modelBUS.GetAllCriteriaByTemplateId(templateId);

            var listError = new List<EvaluationModel>();
            for (int i = 0, length = listCriteria.Count; i < length; i++)
            {
                int evaScore = listmodel[i].score;
                if(evaScore < 0 || evaScore>listCriteria[1].score)
                {
                    ModelState.AddModelError("listmodel[" + i + "].score", "");
                    var model = new EvaluationModel()
                    {
                        criteriaId = listmodel[i].criteriaId,
                        score = evaScore
                    };
                    listError.Add(model);
                }    
            }
            var preSemes = modelBUS.GetPresentSemesterByUserId(user.userID);
            if(ModelState.IsValid)
            {
                // Tạo mới form nếu chưa tạo
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
                    int formId = modelBUS.InsertEvaluationForm(form);
                    // Insert Detail
                    modelBUS.InsertListDetailEvaluation(listmodel, user.userID);
                    TempData["MESSAGE"] = "Chấm điểm thành công!";
                    return RedirectToAction("Union");
                }
                // Trường hợp đã có Form
                else
                {
                    modelBUS.InsertListDetailEvaluation(listmodel, user.userID);
                    TempData["MESSAGE"] = "Cập nhật điểm thành công!";
                }
                return RedirectToAction("Union");
            }
            TempData["ERROR"] = "Điểm không hợp lệ!";
            TempData["listError"] = listError;
            return RedirectToAction("EvaluationForm", new { formId = preSemes.FormId});
        }
        // View Bi thu chi doan
        public ActionResult ClassEvaluation()
        {
            var user = new UserBUS().GetUserByID((int)Session["USER_ID"]);

            var listClass = new UserBUS().GetListUserByClass(user.classID);

            var nowSemester = modelBUS.GetPresentSemester();
            
            return View();
        }    
        // View Bi thu doan khoa
        // View Bi thu doan truong
        
    }
}
