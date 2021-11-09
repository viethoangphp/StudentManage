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
            //Model to View
            UnionFormModel model = new UnionFormModel();
            // Thông tin user
            var user = new UserBUS().GetUserByID((int)Session["USER_ID"]);
            // Render Form chấm điểm
            int templateId = modelBUS.GetGroupUserById(user.groupID).templateId;
            var listMain = modelBUS.GetAllMainByTemplateId(templateId);
            var listCriteria = modelBUS.GetAllCriteriaByTemplateId(templateId);
            // ====================================================================
            int? turn = 0;
            int isInTime = 2;
            // Gán điểm theo tiêu chí và lượt chấm
            int[] listTotal = {0,0,0,0};
            if (formId != null)
            {
                var details = modelBUS.GetDetailEvalutionsByFormId((int)formId);
                //Lấy Detail theo level để tìm ra lượt chấm hiện tại 
                turn = (int)details.Select(x => x.level).Distinct().Max();        
                turn = (turn==null) ? 0 : turn;
                model.Turn = turn;
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
                                    listTotal[3] += (int)detail.score; 
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
                                    listTotal[1] += (int)detail.score;
                                }
                                goto case 1;
                            case 1:
                                if (detail.critetiaId == critial.criteriaId && detail.level == 1)
                                {
                                    critial.score1 = detail.score;
                                    listTotal[0] += (int)detail.score;
                                }
                                break;
                        }       
                    }
                }
            }
            // Render Input và Button chấm
            if ((turn == 0 || turn == 1) && isInTime == 1)
            {
                model.IsInTime = 1;  
            }
            if((turn == 1 || turn == 2) && isInTime == 2 && user.groupID == 3)
            {
                model.IsInTime = 2;
            }
            if ((turn == 2 || turn == 3) && isInTime == 3 && user.groupID == 4)
            {
                model.IsInTime = 3;
            }
            if ((turn == 3 || turn == 4) && isInTime == 4 && user.groupID == 5)
            {
                model.IsInTime = 4;
            }
            //=====================================================================
            model.Total = listTotal;
            model.ListMain = listMain;
            model.ListCriteria = listCriteria;
            ViewBag.user = user;
            
            return View(model);
        }
        
        [HttpPost]
        public ActionResult Evaluation(List<EvaluationModel> listmodel)
        {
            // Thông tin user
            var user = new UserBUS().GetUserByID((int)Session["USER_ID"]);

            #region InputScoreValidation
            // Lấy điểm chuẩn các tiêu chí
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
            #endregion
            //==============================================================
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
        // View Bi thu chi doan
        public ActionResult ClassEvaluation()
        {
            var user = new UserBUS().GetUserByID((int)Session["USER_ID"]);
            var nowSemester = modelBUS.GetPresentSemester();

            var test = modelBUS.GetClassFormByClassIdAndSemesterId(user.classID, nowSemester.semesterId);
            
            return View(test);
        }    
        // View Bi thu doan khoa
        // View Bi thu doan truong
        
        
    }
}
