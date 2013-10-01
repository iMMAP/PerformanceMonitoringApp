using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.Mail;
using System.Net.Mail;
using System.Configuration;

/// <summary>
//Created By Abaid on 06 Aug 
/// </summary>

/// 
namespace SingleReportingLib
{
    /// <summary>
    /// Get Details for Email
    /// </summary>
    public class Email
    {
        public string SmtpServer
        {
            get { return _strSmtpServer; }
            set { _strSmtpServer = value; }
        }
        public string EmailFrom
        {
            get { return _strEmailFrom; }
            set { _strEmailFrom = value; }
        }
        public string EmailTo
        {
            get { return _strEmailTo; }
            set { _strEmailTo = value; }
        }
        public string EmailCc
        {
            get { return _strEmailCc; }
            set { _strEmailCc = value; }
        }
        public string EmailSubject
        {
            get { return _strEmailSubject; }
            set { _strEmailSubject = value; }
        }
        public string EmailMessageBody
        {
            get { return _strEmailMessageBody; }
            set { _strEmailMessageBody = value; }
        }
        public string EmailBCC
        {
            get { return _strEmailBCC; }
            set { _strEmailBCC = value; }
        }

        private string _strSmtpServer = "";
        private string _strEmailFrom = "";
        private string _strEmailTo = "";
        private string _strEmailCc = "";
        private string _strEmailSubject = "";
        private string _strEmailMessageBody = "";
        private string _strEmailBCC = "";
        public Email()
        {
            //
            // TODO: Add constructor logic here
            //
        }
  
        /// <summary>
        /// 
        /// </summary>
        /// <param name="to">Recipent Email Address</param>
        /// <param name="subject">Subject of Email</param>
        /// <param name="body">Body of Email as your Message</param>
        public Email(string to, string subject, string body)
        {

            SendMail(to, subject, body);
        }

       
        
        
        /// <summary>
        /// Email Contructor with Arguments
        /// </summary>
        /// <param name="strEmailFrom">Email Address of Sender</param>
        /// <param name="strEmailTo">Email Address of Reciever</param>
        /// <param name="strEmailSubject">Subject of Email </param>
        /// <param name="strEmailMessageBody">Body of Email </param>
              
        public Email(string strEmailFrom, string strEmailTo, string strEmailSubject,
         string strEmailMessageBody)
        {
            _strEmailFrom = strEmailFrom;
            _strEmailTo = strEmailTo;
            _strEmailSubject = strEmailSubject;
            _strEmailMessageBody = strEmailMessageBody;
            _strSmtpServer = clsCommon.ParseString(System.Configuration.ConfigurationManager.AppSettings["smtpServer"]); //ConfigurationSettings.AppSettings.Get("smtpServer");//"mail.intelligentsiasoftware.com";
            SendEmail();
        }

        /// <summary>
        /// This Sends Email Message
        /// </summary>
        /// <returns>True for Successful send / Fasle for when Exception cought</returns>
        public bool SendEmail()
        {
            System.Web.Mail.MailMessage _objMail = new System.Web.Mail.MailMessage();
            if ((_strSmtpServer != "") && (_strSmtpServer != null))
            {
                try
                {

                    _objMail.From = EmailFrom;
                    _objMail.To = EmailTo.Trim();
                    _objMail.Cc = EmailCc;
                    _objMail.Subject = EmailSubject;
                    _objMail.Body = EmailMessageBody;
                    _objMail.BodyFormat = MailFormat.Html;
                    _objMail.Bcc = EmailBCC;
                    SmtpMail.SmtpServer = _strSmtpServer;
                    SmtpMail.Send(_objMail);
                    return true;
                }
                catch (Exception ex)
                {
                    new SqlLog().InsertSqlLog(0, "Email.cs", ex);
                    return false;
                }
                finally
                {

                    if (_objMail != null)
                        _objMail = null;
                }
                return true;
            }
            return false;
        }


        #region Send Mail Via gmail Account

        public static void SendMail(string to, string subject, string body)
        {

        string from = string.Empty;
        //from = "pakistansrf@gmail.com";
        from = System.Configuration.ConfigurationManager.AppSettings["FromEmail"].ToString();

        if (string.IsNullOrEmpty(from))
        {
            from = "pakistansrf@gmail.com";
        }
        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
        mail.To.Add(to);
        mail.From = new MailAddress(from, "SRF Team", System.Text.Encoding.UTF8);
        mail.Subject = subject;
        mail.SubjectEncoding = System.Text.Encoding.UTF8;
        mail.Body = body;
        mail.BodyEncoding = System.Text.Encoding.UTF8;
        mail.IsBodyHtml = true;
        mail.Priority = System.Net.Mail.MailPriority.High;

        SmtpClient client = new SmtpClient();
        //Add the Creddentials- use your own email id and password

        client.Credentials = new System.Net.NetworkCredential(from, "ndmaunocha");

        client.Port = 587; // Gmail works on this port
        client.Host = "smtp.gmail.com";
        client.EnableSsl = true; //Gmail works on Server Secured Layer
        try
        {
            client.Send(mail);
        }
        catch (Exception ex)
        {
            Exception ex2 = ex;
            string errorMessage = string.Empty;
            while (ex2 != null)
            {
               errorMessage += ex2.ToString();
               ex2 = ex2.InnerException;
            }
           
            //HttpContext.Current.Response.Write(errorMessage);
            //HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.ToString()+"?e=email&d=cantsendemail");
        }
    }
        
        #endregion





    }
}

