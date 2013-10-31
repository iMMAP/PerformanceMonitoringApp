using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using BusinessLogic;

namespace SRFROWCA.Common
{
    public sealed class ExceptionUtility
    {
        // All methods are static, so this can be private
        private ExceptionUtility()
        { }

        public static void LogException(Exception exc, string source, HttpServerUtility Server)
        {
            if (exc is InvalidOperationException)
            {
                // Pass the error on to the Generic Error page            
                Server.Transfer("~/ErrorPages/GenericErrorPage.aspx", true);

            }

            if (exc is System.Data.SqlClient.SqlException)
            {

                Server.Transfer("~/ErrorPages/GenericErrorPage.aspx", true);
            }
        }


        // Notify System Operators about an exception
        public static void NotifySystemOps(Exception exc, string source, System.Security.Principal.IPrincipal iPrincipal)
        {
            if (exc != null)
            {
                string userName = iPrincipal != null ? iPrincipal.Identity.Name : "";
                object[] items = GetParameters(exc, source, userName);
                string subject = string.Format(@"3W Exception, source: {0} at: {1} by: {2}", source, DateTime.Now.ToString("yyyy-MM-dd hh:mm"), userName);
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
                Mail.SendMail("3wopactivities@gmail.com", "3wopactivities@gmail.com", subject, mailBody);
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
            string excType = exc != null ? exc.GetType().ToString() : null;
            string exception = exc != null ? exc.Message : null;
            string excStackTrace = exc != null ? exc.StackTrace : null;

            string userAgent = null;
            string url = null;

            if (System.Web.HttpContext.Current != null)
            {
                userAgent = HttpContext.Current.Request.UserAgent;
                url = HttpContext.Current.Request.Url.ToString();
            }

            int? httpCode = null;
            if (exc is System.Web.HttpException)
            {
                httpCode = ((HttpException)exc).GetHttpCode();
            }

            return new object[] { userName, innerExcType, innerExcSource, innerExc,
                                    innerExcStackTrace, excType, exception, excStackTrace,
                                    source, httpCode, userAgent, url, DBNull.Value };
        }
    }
}