using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;

public static class Exceptions
{
    #region Private Members

    private const string ERROR_REDIRECTOR_PAGE = "Redirector.aspx?";
    private const string ERROR_EMAIL = "Email=";
    private const string ERROR_RETURN_URL = "&ReturnURL=";
    private const string ERROR_DESCRIPTION = "&ErrorDescription=";
    private const string ERROR_LOG_MESSAGE_SEPARATOR = "------------------------------------------------------------------";
    private const string ERROR_GENERIC_EXCEPTION = "Some error occured. ";
    private const string ERROR_MESSAGE_STACK_DATE = "Date and Time: ";
    private const string ERROR_MESSAGE_TITLE = "Message: ";
    private const string ERROR_MESSAGE_STACK_TRACE = "Stack Trace: ";

    private const string ERROR_MESSAGE_STACK_TARGET = "Target Site: ";
    public static string Email;

    #endregion

    public static string ReturnURL;

    #region Properties

    private static string ErrorLogFile
    {
        get
        {
            return HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["ErrorLogFile"]);
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Method to handle error handling routine
    /// </summary>
    /// <param name="ex">Pass original exception instance</param>
    /// <remarks></remarks>
    /// 
    public static void CatchException(bool isRedirectToErrorPage, Exception ex)
    {
        if (ThreadAbortCheck(ex))
        {
            return;
        }
        else
        {
            StringBuilder errorDescription = new StringBuilder();

            errorDescription.AppendLine();
            errorDescription.AppendLine(ERROR_MESSAGE_STACK_DATE + DateTime.Now);
            errorDescription.AppendLine();
            errorDescription.AppendLine(ERROR_MESSAGE_TITLE + ex.Message.ToString());
            errorDescription.AppendLine();
            errorDescription.AppendLine(ERROR_MESSAGE_STACK_TRACE + ex.StackTrace.ToString());
            errorDescription.AppendLine();
            errorDescription.AppendLine(ERROR_MESSAGE_STACK_TARGET + ex.TargetSite.ToString());
            errorDescription.AppendLine();
            errorDescription.AppendLine(ERROR_LOG_MESSAGE_SEPARATOR);

            SaveErrorToFile(errorDescription.ToString());

            if ((isRedirectToErrorPage))
            {
                HttpContext.Current.Response.Redirect(ERROR_REDIRECTOR_PAGE + ERROR_EMAIL + Email + ERROR_RETURN_URL + ReturnURL, true);
            }
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Method to check whether exception caused due to thread abortion
    /// </summary>
    /// <param name="ex">To check type of exception</param>
    /// <returns></returns>
    /// <remarks></remarks>
    /// 
    private static bool ThreadAbortCheck(Exception ex)
    {
        if (ex is System.Threading.ThreadAbortException)
        {
            System.Threading.Thread.ResetAbort();
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Method to save error in log file
    /// </summary>
    /// <param name="newContents">Error contents to save in log file</param>
    /// <remarks></remarks>
    /// 
    private static void SaveErrorToFile(string exceptionDetails)
    {
        string fullPath = ErrorLogFile;

        try
        {
            // If file not exists, create empty file on specified path.
            if (!(File.Exists(fullPath)))
            {
                CreateEmptyFile(fullPath);
            }

            // Append exception details in file at given path.
            using (StreamWriter writer = File.AppendText(fullPath))
            {
                writer.Write(exceptionDetails);
            }
        }
        catch
        {
            //Please handle exception whatever way you want to do here.
            // Return error
            // Show message on some control like label
            // show messagebox.
        }
    }

    // Create empty path.
    // You can directly call this code or can use Using satement, but it looks elegent.
    private static void CreateEmptyFile(string filename)
    {
        File.Create(filename).Dispose();
    }

    #endregion
}
