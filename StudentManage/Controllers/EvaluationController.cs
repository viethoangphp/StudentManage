using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentManage.Library;
using StudentManage.BUS;
using StudentManage.Models;
using System.IO;
namespace StudentManage.Controllers
{
    [UserAuthorze]
    public class EvaluationController : Controller
    {
        #region Xem điểm cá nhân 
        public ActionResult Index()
        {
            var userId = (int)Session["USER_ID"];
            var model = TemplateBUS.GetPersonalScores(userId);
            return View(model);
        }
        #endregion

        #region Thực hiện đánh giá cá nhân
        public ActionResult Detail()
        {
            var userId = (int)Session["USER_ID"];
            // Get TemplateId to User Login 
            var templateId = new UserBUS().GetUserByID(userId).templateId;
            // Get Template By TemplateId 
            var model = TemplateBUS.GetTemplateById(templateId);
            return View(model);
        }
       
        [HttpPost]
        public JsonResult Detail(FormCollection formCollection)
        {
            
            var userId = (int)Session["USER_ID"];
            // Get TemplateId to User Login 
            var templateId = new UserBUS().GetUserByID(userId).templateId;
            var listCriteriaId = TemplateBUS.GetListCriteria(templateId);
            // create formModel 
            FormModel formModel = new FormModel();
            var request = Request.Files[0].FileName;
            formModel.Create_At = DateTime.Now;
            formModel.Create_By = userId;
            formModel.Status = 0;
            formModel.Total = 0;
            var formId = TemplateBUS.InsertEvaluationForm(formModel);
            if(formId != -1)
            {
                List<DetailFormModel> detailFormModels = new List<DetailFormModel>();
                int index = 0;
                foreach (var item in listCriteriaId)
                {
                    DetailFormModel detailFormModel = new DetailFormModel();
                    detailFormModel.CriteriaId = item.CriteriaID;
                    detailFormModel.FormId = formId;
                    detailFormModel.UserId = userId;
                    int score = Int32.Parse(formCollection["score[" + item.CriteriaID + "]"]);
                    if (score > item.MaxScore)
                    {
                        score = 0;
                    }
                    detailFormModel.Score = score;
                    detailFormModel.Note = formCollection["proof[" + item.CriteriaID + "]"];
                    // Lưu ảnh minh chứng 
                    if(Request.Files[index].FileName != null && Request.Files[index].FileName != "")
                    {
                        HttpPostedFileBase fileTemp = Request.Files[index];
                        detailFormModel.Image = fileTemp.FileName.Trim();
                        fileTemp.SaveAs(Server.MapPath("~/Assets/img/Images_Proof/") + fileTemp.FileName);
                    }
                    else
                    {
                        detailFormModel.Image = null;
                    }
                    detailFormModel.Level = 1;
                    detailFormModels.Add(detailFormModel);
                    index++;
                }
                if(TemplateBUS.InsertListEvaluationDetail(detailFormModels) != -1)
                {
                    TemplateBUS.UpdateTotalScore(formId);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Danh sách điểm dánh giá
        public ActionResult List()
        {
            var model = TemplateBUS.GetPersonalScores();
            return View(model);
        }
        #endregion
        #region Xem Chi tiết Phiếu Đánh Giá
        public ActionResult View(int id)
        {
            var model = TemplateBUS.GetTemplateFormDetail(id);
            //Id for export excel button
            ViewData["ID"] = id;
            return View(model);
        }
        public ActionResult ViewAdmin(int id)
        {
            var model = TemplateBUS.GetTemplateFormDetail(id);
            return View(model);
        }
        #endregion
        
        #region Xuất file excel
        public ActionResult ExportExcel(int id)
        {
            MemoryStream stream = TemplateBUS.ExportExcel(id);
            string fileName = TemplateBUS.GetTemplateFormDetail(id).FullName;
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", fileName + ".xlsx");
		}
		#endregion
		
        #region Duyệt phiếu điểm cá nhân
        [HttpPost]
        public ActionResult ConfirmEvalution(int id)
        {
            List<PesonalEvalationModel> list = new List<PesonalEvalationModel>();
            var templateId = TemplateBUS.GetTemplateFormDetail(id).TemplateID;
            var listCriteriaId = TemplateBUS.GetListCriteria(templateId);
            foreach (var item in listCriteriaId)
            {
                PesonalEvalationModel detailFormModel = new PesonalEvalationModel();
                detailFormModel.CriteriaID = item.CriteriaID;
                detailFormModel.FormID = id;
                detailFormModel.Status = Int32.Parse(Request.Form["Status[" + item.CriteriaID + "]"]);
                detailFormModel.Comment = Request.Form["Comment[" + item.CriteriaID + "]"];
                list.Add(detailFormModel);
            }

            var result = TemplateBUS.UpdateEvaluationForm(list);
            var status = TemplateBUS.UpdateStatusForm(id, 1);
            return RedirectToAction("List");
        }
        #endregion
    }
}