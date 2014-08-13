using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace SRFROWCA.Common
{
    public static class Mail
    {
        //public static void SendMail(string from, string to, string subject, string body)
        //{
        //    var client = new SmtpClient("smtp.googlemail.com", 587)
        //    {
        //        Credentials = new NetworkCredential("orsocharowca@gmail.com", "rOcha$w2Af"),
        //        EnableSsl = true
        //    };

        //    client.Send(from, to, subject, body);
        //}

        //internal static void SendMail(MailMessage mailMsg)
        //{
        //    var client = new SmtpClient("smtp.googlemail.com", 587)
        //    {
        //        Credentials = new NetworkCredential("orsocharowca@gmail.com", "rOcha$w2Af"),
        //        EnableSsl = true
        //    };

        //    client.Send(mailMsg);
        //}

        internal static void SendMail(MailMessage mailMsg)
        {
            var client = new SmtpClient("41.191.198.195", 25)
            {
                Credentials = new NetworkCredential("dakar@ochasomalia.org", "Ocha@123"),
                //EnableSsl = true
            };

            string appendSubject = Convert.ToString(ConfigurationManager.AppSettings["StagingEmailSubjectText"]);

            if (!string.IsNullOrEmpty(appendSubject))
                mailMsg.Subject = appendSubject + ": " + mailMsg.Subject;

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]))
                client.Send(mailMsg);
        }
    }
}