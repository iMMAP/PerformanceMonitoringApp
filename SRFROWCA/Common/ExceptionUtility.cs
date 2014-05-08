using System;
using System.Net.Mail;
using System.Web;
using BusinessLogic;

namespace SRFROWCA.Common
{
    public static class ExceptionUtility
    {
        public static void LogException(Exception exc, string source, HttpServerUtility server)
        {
            if (exc is InvalidOperationException || exc is System.Data.SqlClient.SqlException)
            {
                // Pass the error on to the Generic Error page            
                server.Transfer("~/ErrorPages/GenericErrorPage.aspx", true);
            }
        }

        // Notify System Operators about an exception
        public static void NotifySystemOps(Exception exc, string source, System.Security.Principal.IPrincipal iPrincipal)
        {
            if (exc != null)
            {
                string userName = iPrincipal != null ? iPrincipal.Identity.Name : "";
                object[] items = GetParameters(exc, source, userName);
                string subject = string.Format(@"ORS Exception, source: {0} at: {1} by: {2}", source, DateTime.Now.ToString("yyyy-MM-dd hh:mm"), userName);
                string mailBody = string.Format(@"Inner Exception Type: {0}
                                            Inner Exception Source: {1} 
                                            Inner Exception: {2}
                                            Inner Exception Stack Trace: {3}
                                            Exception Type: {4}
                                            Exception: {5}
                                            Exception Trace: {6}", items[1],
                                                                     items[2],
                                                                     items[3],
                                                                     items[4],
                                                                     items[5],
                                                                     items[6],
                                                                     items[7]);
                //Mail.SendMail("orsocharowca@gmail.com ", "orsocharowca@gmail.com ", subject, mailBody);

                try
                {
                    using (MailMessage mailMsg = new MailMessage())
                    {
                        mailMsg.From = new MailAddress("orsocharowca@gmail.com");
                        mailMsg.To.Add(new MailAddress("orsocharowca@gmail.com"));
                        mailMsg.Subject = subject;
                        mailMsg.IsBodyHtml = true;
                        mailMsg.Body = mailBody;
                        Mail.SendMail(mailMsg);
                    }
                }
                catch
                {
                }
            }
        }

        internal static void LogException(Exception exc, string source, System.Security.Principal.IPrincipal iPrincipal)
        {            
            if (exc != null)
            {
                string userName = iPrincipal != null ? iPrincipal.Identity.Name : null;
                object[] parameters = GetParameters(exc, source, userName);
                WriteExceptionInDB(parameters);

                //if (source != "GlobalASAX")
                    //NotifySystemOps(exc, source, iPrincipal);
            }
        }

        private static void WriteExceptionInDB(object[] parameters)
        {
            DBContext.Add("InsertApplicationExceptions", parameters);
        }

        private static object[] GetParameters(Exception exc, string source, string userName)
        {
            string innerExcType = exc.InnerException != null ? exc.InnerException.GetType().ToString() : null;
            string innerExcSource = exc.InnerException != null ? exc.InnerException.Source : null;
            string innerExc = exc.InnerException != null ? exc.InnerException.Message : null;
            string innerExcStackTrace = exc.InnerException != null ? exc.InnerException.StackTrace : null;
            string excType = exc.GetType().ToString();
            string exception = exc.Message;
            string excStackTrace = exc.StackTrace;

            string userAgent = null;
            string url = null;

            if (HttpContext.Current != null)
            {
                userAgent = HttpContext.Current.Request.UserAgent;
                url = HttpContext.Current.Request.Url.ToString();
            }

            int? httpCode = null;
            if (exc is HttpException)
            {
                httpCode = ((HttpException)exc).GetHttpCode();
            }

            return new object[] { userName, innerExcType, innerExcSource, innerExc,
                                    innerExcStackTrace, excType, exception, excStackTrace,
                                    source, httpCode, userAgent, url, DBNull.Value };
        }
    }
}