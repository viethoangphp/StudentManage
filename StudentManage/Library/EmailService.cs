using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using StudentManage.BUS;
using StudentManage.Models;

namespace StudentManage.Library
{
    public class EmailService
    {
        private string username = ConfigBUS.GetConfigByKey("EmailUser");
        private string password = ConfigBUS.GetConfigByKey("EmailPass");
        private string title = ConfigBUS.GetConfigByKey("EmailTitle");
        private string subject = ConfigBUS.GetConfigByKey("EmailSubject");
        public void Send(string toEmail, string content)
        {
            MailAddress fromAddress = new MailAddress(username, title);
            MailAddress toAddress = new MailAddress(toEmail);
            //const string fromPassword = "nffegjqoaiucskcb";
            //string subject = "HUTECH Thông Báo Xác Nhận Thông Tin Nộp Sổ Đoàn Thành Công !";
            string body = content;

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, password)
            };
            using (MailMessage message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            })
            {
                smtp.Send(message);
            }
        }
        public bool IsValid(string emailaddress)
        {

            if (System.Text.RegularExpressions.Regex.IsMatch(emailaddress, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                return true;
            }
            return false;
        }

    }
}