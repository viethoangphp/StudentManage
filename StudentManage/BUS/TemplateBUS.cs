using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentManage.Models;
using Models.DAO;
using Models.EntityModel;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Reflection;

namespace StudentManage.BUS
{
    public class TemplateBUS
    {
        public static TemplateModel GetTemplateById(int templateId)
        {
            TemplateModel template = new TemplateModel();
            var model = new EvaluationDAO().GetTemplate(templateId);
            //===============================================
            template.TemplateID = model.TemplateID;
            template.TemplateName = model.Name;
            template.ListMain = new List<EvaluationMainModel>();
            foreach (var item in model.EvaluativeMains)
            {
                EvaluationMainModel evaluationMain = new EvaluationMainModel();
                evaluationMain.MainID = item.MainID;
                evaluationMain.Title = item.Content;
                evaluationMain.ListRequriement = new List<PesonalEvalationModel>();
                foreach (var subItem in item.EvaluativeCriterias)
                {
                    PesonalEvalationModel evaluation = new PesonalEvalationModel();
                    evaluation.CriteriaID = subItem.CriteriaID;
                    evaluation.Content = subItem.CriteriaContent;
                    evaluation.Requirement = subItem.CriteriaRequirement;
                    evaluation.MaxScore = (int)subItem.Score;
                    evaluationMain.ListRequriement.Add(evaluation);
                }
                template.ListMain.Add(evaluationMain);
            }
           
            return template;
        }
        public static int InsertEvaluationForm(FormModel form)
        {
            EvalutionForm evalutionForm = new EvalutionForm();
            evalutionForm.Create_At = form.Create_At;
            evalutionForm.Create_by = form.Create_By;
            evalutionForm.SemesterID = GetSemesterActive();
            evalutionForm.Total = form.Total;
            evalutionForm.Status = form.Status; ;
            return new EvaluationDAO().InsertEvaluationForm(evalutionForm);
        }
     
        public static int GetSemesterActive()
        {
            return new EvaluationDAO().GetSemesterActive();
        }
        public static List<DetailEvalution> ParseDetailFormModel(List<DetailFormModel> listData)
        {
            List<DetailEvalution> result = new List<DetailEvalution>();
            foreach (var item in listData)
            {
                DetailEvalution model = new DetailEvalution();
                model.FormId = item.FormId;
                model.UserID = item.UserId;
                model.CriteriaID = item.CriteriaId;
                model.Score = item.Score;
                model.Level = item.Level;
                model.Image_proof = item.Image;
                model.Note = item.Note;
                result.Add(model);
            }
            return result;
        }
        public static int InsertListEvaluationDetail(List<DetailFormModel> list)
        {
            List<DetailEvalution> result = ParseDetailFormModel(list);
            return new EvaluationDAO().InsertListFormDetail(result);
        }
        public static List<EvaluationModel> GetListCriteria(int templateId)
        {
            List<EvaluationModel> result = new List<EvaluationModel>();
            var listData = new EvaluationDAO().GetListCriteriaIdByTemplateId(templateId);
            foreach (var item in listData)
            {
                EvaluationModel model = new EvaluationModel();
                model.CriteriaID = item.CriteriaID;
                model.Content = item.CriteriaContent;
                model.Requirement = item.CriteriaRequirement;
                model.MaxScore = (int)item.Score;
                result.Add(model);
            }
            return result;
        }
        public static int? UpdateTotalScore(int formId)
        {
            return new EvaluationDAO().UpdateTotal(formId);
        }
        public static List<PersonalScore> GetPersonalScores(int userId = 0)
        {
            List<PersonalScore> list = new List<PersonalScore>();
            var model = new EvaluationDAO().GetEvaluationPersonal(userId);
            foreach (var item in model)
            {
                PersonalScore personalScore = new PersonalScore();
                personalScore.FormId = item.FormId;
                personalScore.FullName = item.DetailEvalutions.Last().User.FullName;
                personalScore.Semester = item.Semester.Name;
                personalScore.Year = item.Semester.Year;
                personalScore.Score = Int32.Parse(item.Total.ToString());
                if (item.Total >= 90)
                {
                    personalScore.Rank = "A";
                }
                if (item.Total >= 75 && item.Total < 90)
                {
                    personalScore.Rank = "B";
                }
                if (item.Total >= 50 && item.Total < 75)
                {
                    personalScore.Rank = "C";
                }
                if (item.Total < 50)
                {
                    personalScore.Rank = "D";
                }
                personalScore.Status = (item.Status == 0) ? "Chờ Duyệt" : "Đã Duyệt";
                personalScore.Note = "";
                list.Add(personalScore);
            }
            return list;
        }
        public static TemplateModel GetTemplateFormDetail(int formId)
        {

            TemplateModel template = new TemplateModel();
            var listDetail = new EvaluationDAO().GetDetailEvalutions(formId);
            var templateId = listDetail.First().EvaluativeCriteria.EvaluativeMain.TemplateID;
            var model = new EvaluationDAO().GetTemplate(templateId);
            //===============================================
            template.TemplateID = model.TemplateID;
            template.TemplateName = model.Name;
            template.FullName = listDetail.First().User.FullName;
            template.Email = listDetail.First().User.Email;
            template.Faculty = listDetail.First().User.Class.Faculty.Name;
            template.Phone = listDetail.First().User.Phone;
            template.Position = listDetail.First().User.Position.Name;
            template.ListMain = new List<EvaluationMainModel>();
            foreach (var item in model.EvaluativeMains)
            {
                EvaluationMainModel evaluationMain = new EvaluationMainModel();
                evaluationMain.MainID = item.MainID;
                evaluationMain.Title = item.Content;
                evaluationMain.ListRequriement = new List<PesonalEvalationModel>();
                foreach (var subItem in listDetail.Where(m => m.EvaluativeCriteria.MainID == item.MainID))
                {
                    PesonalEvalationModel evaluation = new PesonalEvalationModel();
                    evaluation.CriteriaID = subItem.CriteriaID;
                    evaluation.Content = subItem.EvaluativeCriteria.CriteriaContent;
                    evaluation.Requirement = subItem.EvaluativeCriteria.CriteriaRequirement;
                    evaluation.MaxScore = (int)subItem.EvaluativeCriteria.Score;
                    evaluation.Score = (int)subItem.Score;
                    evaluation.Proof = subItem.Note;
                    evaluation.Image = (subItem.Image_proof != null) ? subItem.Image_proof : "";
                    evaluation.Note = subItem.Note;
                    evaluation.Status = subItem.Status;
                    template.FormID = subItem.FormId;
                    evaluation.Comment = subItem.Comment;
                    evaluationMain.ListRequriement.Add(evaluation);
                }
                template.TotalScore = (int)listDetail.First().EvalutionForm.Total;
                if (template.TotalScore >= 90)
                {
                    template.Rank = "A";
                }
                if (template.TotalScore >= 75 && template.TotalScore < 90)
                {
                    template.Rank = "B";
                }
                if (template.TotalScore >= 50 && template.TotalScore < 75)
                {
                    template.Rank = "C";
                }
                if (template.TotalScore < 50)
                {
                    template.Rank = "D";
                }
                template.ListMain.Add(evaluationMain);
            }
            return template;
        }
        public static MemoryStream ExportExcel(int id)
        {
            //---=== Init ===---
            //Get data from GetTemplateFormDetail
            TemplateModel data = GetTemplateFormDetail(id);
            //Create excel file
            ExcelPackage pkg = new ExcelPackage();
            ExcelWorksheet sheet = pkg.Workbook.Worksheets.Add("Báo cáo");
            //Prepare data in header
            string[] basicInfo = new string[] {
                "NHÓM: "+(data.TemplateID-2) +"          CHỨC DANH: "+ data.Position +"          ĐƠN VỊ: " + data.Faculty,
                "HỌ TÊN: "+ data.FullName +"          SĐT: "+data.Phone,
                "E-MAIL: "+data.Email,
                "Thời gian công tác: từ tháng ……./2020 đến tháng ……./2021"
            };
            //Latin number array
            string[] latinNumbers = new string[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };
            //Columns name array
            string[] listColName = new string[] { "STT", "NỘI DUNG ĐÁNH GIÁ", "YÊU CẦU", "ĐIỂM TỐI ĐA", "ĐIỂM TỰ ĐÁNH GIÁ", "MINH CHỨNG - GIẢI TRÌNH" };
            //---=== Configure ===---
            //Header & Infomation
            ExcelRange header = sheet.Cells[1, 1, 1, listColName.Length];
            header.Style.Font.SetFromFont(new Font("Times New Roman", 14)); // This must always first before format other
            header.Style.Font.Bold = true;
            //Add text
            header.RichText.Add("BÁO CÁO ĐÁNH GIÁ CÔNG TÁC CỦA CÁN BỘ ĐOÀN - HỘI\n");
            header.RichText.Add("NĂM HỌC 2020 - 2021\n"); //Hardcoded cause no data to set dynamically
            header.RichText.Add("(Mẫu dành cho "+data.TemplateName+")");
            //Format after add text to avoid lost format
            header.Merge = true;
            header.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
            header.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            header.Style.WrapText = true;
            //------------------------------------------------------------------------------------
            //Column format
            sheet.Column(1).Width = 5;
            sheet.Column(2).Width = 25;
            sheet.Column(3).Width = 30;
            sheet.Column(4).Width = 10;
            sheet.Column(5).Width = 10;
            sheet.Column(6).Width = 35;
            //Row formar
            sheet.Row(1).Height = 60;
            //------------------------------------------------------------------------------------
            int posRow = 2; //Start position of row
            //Print header
            foreach (string info in basicInfo)
            {
                ExcelRange row = sheet.Cells[posRow, 1, posRow, listColName.Length];
                row.Merge = true;
                row.Style.Font.SetFromFont(new Font("Times New Roman", 12));
                row.Style.Font.Bold = true;
                row.RichText.Add(info);
                posRow++;
                
            }
            // -----===== Column name =====-----
            int startRow = posRow; // Save start row for autofit column later
            int posCol = 1; //Start position of column
            //Print columns name
            //int titleRow = posRow; //Title row is current row after print out header information
            foreach (string name in listColName)
            {
                ExcelRange colTitle = sheet.Cells[posRow, posCol];
                colTitle.Style.Font.SetFromFont(new Font("Times New Roman", 12));
                colTitle.Style.Font.Bold = true;
                colTitle.RichText.Add(name);
                colTitle.Style.Fill.SetBackground(Color.FromArgb(146, 208, 80));
                colTitle.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                posCol++;
            }
            //Autofit row height (by wrap text)
            ExcelRange titleRange = sheet.Cells[posRow, 1, posRow, listColName.Length];
            titleRange.Style.WrapText = true;
            //Apply vertical alignment center
            titleRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            //Add sorting to column name (NOT NEED)
            //ExcelRange colTitleRow = sheet.Cells[titleRow, 1, titleRow, listColName.Length];
            //colTitleRow.AutoFilter = true;

            // -----===== Column data =====-----
            posRow++; // Set next row
            int num = 0; //Init num counter
            int groupNum = 0; //Init group counter (using Latin number)
            //Select name row
            string[] selected = new string[] { "Content", "Requirement", "MaxScore", "Score", "Proof" };
            foreach (EvaluationMainModel item in data.ListMain)
            {
                // -----===== Group Name =====-----
                //Set position and counter
                posCol = 1;
                //Init selection
                ExcelRange numCell = sheet.Cells[posRow, 1];
                ExcelRange groupName = sheet.Cells[posRow, 2, posRow, listColName.Length];
                //Format
                groupName.Merge = true;
                numCell.Style.Font.SetFromFont(new Font("Times New Roman", 12));
                groupName.Style.Font.SetFromFont(new Font("Times New Roman", 12));
                numCell.Style.Font.Bold = true;
                groupName.Style.Font.Bold = true;
                //Set text
                numCell.RichText.Add(latinNumbers[groupNum]);
                groupName.RichText.Add(item.Title);
                //Format color
                numCell.Style.Fill.SetBackground(Color.FromKnownColor(KnownColor.Yellow));
                groupName.Style.Fill.SetBackground(Color.FromKnownColor(KnownColor.Yellow));
                //Switch to next row
                posRow++;
                // -----===== Details =====-----
                foreach (PesonalEvalationModel requirement in item.ListRequriement)
                {
                    //First cell is number
                    num++;
                    ExcelRange nCell = sheet.Cells[posRow, posCol];
                    nCell.Style.Font.SetFromFont(new Font("Times New Roman", 12));
                    nCell.RichText.Add(num.ToString());
                    nCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //Next cell in row
                    posCol++;
                    for(int i = 0; i < selected.Length; i++)
                    {
                        foreach (PropertyInfo info in requirement.GetType().GetProperties())
                        {
                            //Only print required field
                            if (selected.ElementAt(i).Equals(info.Name))
                            {
                                ExcelRange cell = sheet.Cells[posRow, posCol];
                                //Format cell
                                cell.Style.Font.SetFromFont(new Font("Times New Roman", 12));
                                if (info.Name.Equals("Content")) { cell.Style.Font.Bold = true; }
                                //Check if text inside field is null
                                string text = "";
                                var obj = info.GetValue(requirement); //Get Object First
                                if (obj != null) { text = obj.ToString(); } //ToString only if it not null
                                if (!string.IsNullOrEmpty(text)) { cell.RichText.Add(text); }
                                //Wrap cell text
                                cell.Style.WrapText = true;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                if (info.Name.Equals("MaxScore") || info.Name.Equals("Score"))
                                {
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                }
                            }
                        }
                        posCol++; //Move col position to next col
                    }
                    //Move to next row and set column pos back to 1
                    posRow++;
                    posCol = 1;
                }
                //Next group
                groupNum++;
            }
            //Print result and end data
            ExcelRange totalScoreTextRow = sheet.Cells[posRow, posCol, posRow, 3];
            ExcelRange totalScoreMaxCell = sheet.Cells[posRow, 4];
            ExcelRange totalScoreCell = sheet.Cells[posRow, 5];
            ExcelRange rankDescription = sheet.Cells[posRow + 1, posCol, posRow + 1, 3];

            totalScoreTextRow.Style.Font.SetFromFont(new Font("Times New Roman", 12));
            totalScoreTextRow.Style.Font.Bold = true;
            totalScoreTextRow.Merge = true;
            totalScoreTextRow.RichText.Add("TỔNG ĐIỂM");
            totalScoreTextRow.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            totalScoreMaxCell.Style.Font.SetFromFont(new Font("Times New Roman", 12));
            totalScoreMaxCell.Style.Font.Bold = true;
            totalScoreMaxCell.RichText.Add("100");
            totalScoreMaxCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            totalScoreCell.Style.Font.SetFromFont(new Font("Times New Roman", 12));
            totalScoreCell.Style.Font.Bold = true;
            totalScoreCell.RichText.Add(data.TotalScore.ToString());
            totalScoreCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            rankDescription.Style.Font.SetFromFont(new Font("Times New Roman", 12));
            rankDescription.Style.Font.Bold = true;
            rankDescription.Merge = true;
            rankDescription.RichText.Add("Xếp loại: (A: >=90 đ, B: >=75 đ, C: >=50 đ, D: <50 đ)");
            rankDescription.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            //Format color
            ExcelRange footerSection = sheet.Cells[posRow, posCol, posRow + 1, 6];
            footerSection.Style.Fill.SetBackground(Color.FromArgb(146, 208, 80));

            //Signature
            ExcelRange signatureText = sheet.Cells[posRow + 3, 4, posRow + 3, 6];
            ExcelRange signatureDescription = sheet.Cells[posRow + 4, 4, posRow + 4, 6]; //Last posRow position is posRow + 4

            signatureText.Style.Font.SetFromFont(new Font("Times New Roman", 14));
            signatureText.Style.Font.Bold = true;
            signatureText.Merge = true;
            signatureText.RichText.Add("CÁN BỘ THỰC HIỆN");
            signatureText.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            signatureDescription.Style.Font.SetFromFont(new Font("Times New Roman", 14));
            signatureDescription.Style.Font.Italic = true;
            signatureDescription.Merge = true;
            signatureDescription.RichText.Add("(kí và ghi rõ họ tên)");
            signatureDescription.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            //Auto fit all row
            //ExcelRange fitRow = sheet.Cells[startRow, 1, posRow + 4, listColName.Length];
            //fitRow.AutoFitColumns();

            //Apply border to table
            ExcelRange table = sheet.Cells[startRow, 1, posRow + 1, listColName.Length];
            table.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            table.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            table.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            table.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            // -----===== Download File =====-----
            //Send back to client for download
            MemoryStream stream = new MemoryStream();
            pkg.SaveAs(stream);
            return stream;
		}
        public static int UpdateEvaluationForm(List<PesonalEvalationModel> list)
        {
            foreach(var item in list)
            {
                DetailEvalution detail = new DetailEvalution();
                detail.FormId = item.FormID;
                detail.CriteriaID = item.CriteriaID;
                detail.Status = item.Status;
                detail.Comment = item.Comment;
                var result = new EvaluationDAO().UpdateEvaluationForm(detail);
            }
            return 1;
        }
        public static int UpdateStatusForm(int formId , int status)
        {
            return new EvaluationDAO().UpdateStatusForm(formId, status);
        }
    }
}
