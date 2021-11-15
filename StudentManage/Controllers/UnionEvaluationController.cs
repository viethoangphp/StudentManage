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
                    var preSemes = modelBUS.GetPresentSemesterByUserId(userSession.userID);
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
                // Sau thời gian chấm cá nhân
                //if(isInTime == 2)
                //{
                //    // Tạo Form khi chưa tạo
                //    var preSemes = modelBUS.GetPresentSemesterByUserId(userSession.userID);
                //    EvalutionFormModel form = new EvalutionFormModel()
                //    {
                //        semesterId = preSemes.semesterId,
                //        status = 1,
                //        createAt = DateTime.Now,
                //        createBy = userSession.userID,
                //    };
                //    // Insert Form
                //    formId = modelBUS.InsertEvaluationForm(form);
                //    user = userSession;
                //}    
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
            }
            #endregion

            #region Render hiển thị Input và Button chấm
            // Render Input và Button chấm
            if ((turn == 0 || turn == 1) && isInTime == 1)
            {
                model.IsInTime = 1;  
            }
            if((turn == 0 || turn == 1 || turn == 2) && isInTime == 2 && userSession.groupID == 3)
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
            #endregion

            //=====================================================================
            model.Total = listTotal;
            model.ListMain = listMain;
            model.ListCriteria = listCriteria;
            model.formId =  (int)formId;
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

            #region Insert điểm và trả thông báo + trả về list Error
            var detailForm = modelBUS.GetDetailEvalutionsByFormId(evaluationFormId);
            if(ModelState.IsValid)
            {
                // TH Tạo Form nhưng chưa chấm - Chấm điểm
                if (detailForm.Count == 0)
                {
                    // Insert Detail
                    modelBUS.InsertListDetailEvaluation(listmodel, user.userID, evaluationFormId);
                    TempData["MESSAGE"] = "Chấm điểm thành công!";
                    return RedirectToAction("Union");
                }
                // TH Tạo Form và đã chấm - Cập nhật điểm
                else
                {
                    // Insert Detail
                    modelBUS.InsertListDetailEvaluation(listmodel, user.userID, evaluationFormId);
                    TempData["MESSAGE"] = "Cập nhật điểm thành công!";
                }
                return RedirectToAction("Union");
            }
            TempData["ERROR"] = "Điểm không hợp lệ!";
            TempData["listError"] = listError;// ListError từ Validation
            #endregion

            //===========================================================================
            return RedirectToAction("EvaluationForm", new { formId = evaluationFormId});
        }
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
        // View bí thư chi đoàn 
        public ActionResult ClassEvaluation()
        {
            var user = new UserBUS().GetUserByID((int)Session["USER_ID"]);
            var nowSemester = modelBUS.GetPresentSemester();
            var test = modelBUS.GetClassFormByClassIdAndSemesterId(user.classID, nowSemester.semesterId);
            
            //===========================================================================
            return View(test);
        }    
        // View Bí thư đoàn khoa
        // View Bí thư đoàn trường
        
        
    }
}
