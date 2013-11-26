using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace SRFROWCA.Common
{
    public static class Mail
    {
        public static void SendMail(string from, string to, string subject, string body)
        {
            var client = new SmtpClient("smtp.googlemail.com", 587)
            {
                Credentials = new NetworkCredential("3wopactivities@googlemail.com", "sahel1ocha"),
                EnableSsl = true
            };
            
            client.Send(from, to, subject, body);
        }
    }
}