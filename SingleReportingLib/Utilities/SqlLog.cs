using System;
using System.Threading;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;



//Author: Asad Aziz
//Date Creared: 07/04/2008
/// <summary>
/// Used to Log messages in SQL Server Database
/// </summary>
public class SqlLog : ILog
{
    //=======================================================================================		
    public void info(object objMessage, object objSource)
    {
        WriteLog(objMessage, objSource, Level.INFO);
    }
    //=======================================================================================			
    public void warn(object objMessage, object objSource)
    {
        WriteLog(objMessage, objSource, Level.WARN);
    }
    //=======================================================================================		
    public void debug(object objMessage, object objSource)
    {
        WriteLog(objMessage, objSource, Level.DEBUG);
    }
    //=======================================================================================		
    public void trace(object objMessage, object objSource)
    {
        WriteLog(objMessage, objSource, Level.TRACE);
    }
    //=======================================================================================		
    public void error(object objMessage, object objSource)
    {
        WriteLog(objMessage, objSource, Level.ERROR);
    }
    //=======================================================================================		
    public void fatal(object objMessage, object objSource)
    {
        WriteLog(objMessage, objSource, Level.FATAL);
    }
    //=======================================================================================		

    private string _application = "";
    public string Application
    {
        get { return _application; }
        set { _application = value; }
    }

    /// <summary>
    /// Write the ErrorMessage in EventLog Table on xdGlobal Database
    /// </summary>
    /// <param name="Message">Error Message</param>
    /// <param name="objSource">Source Component</param>
    /// <param name="level">Logging Level</param>
    private void WriteLog(object message, object source, Level level)
    {
        try
        {
            using (DbManager db = DbManager.GetDbManager())
            {
                SqlParameter[] parameters = new SqlParameter[]
                    {
                          db.MakeInParam("@vchMessage", SqlDbType.VarChar, -1, message.ToString())
                        , db.MakeInParam("@vchApplication", SqlDbType.VarChar, 500, Application)
                        , db.MakeInParam("@vchLevel", SqlDbType.VarChar, 50, level.ToString())
                        , db.MakeInParam("@vchDetail", SqlDbType.VarChar, -1, (source ?? "").ToString())
                    };
                db.RunProc("up_insertApplicationLog", parameters);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.WriteLine("Unable to write LOG in SQL" + ex.Message);
        }
    }


    public void InsertSqlLog(int userId, string source, Exception ex)
    {
        string errorMessage = ex.Message + " " + ex.StackTrace;
        string exceptionName = ex.ToString();

        using (DbManager db = DbManager.GetDbManager())
        {
            SqlParameter[] parameters = new SqlParameter[]
                    {
                          db.MakeInParam("@vchErrorSource", SqlDbType.VarChar, 500, source)
                        , db.MakeInParam("@vchErrorMessage", SqlDbType.VarChar, 500, errorMessage)
                        , db.MakeInParam("@tntErrorTypeId", SqlDbType.VarChar, 50, 1 )
                        , db.MakeInParam("@dtmDateCreated", SqlDbType.DateTime, 0, DateTime.Now)
                        , db.MakeInParam("@vchExceptionName", SqlDbType.VarChar, 500, exceptionName)
                        , db.MakeInParam("@intErrorUserId", SqlDbType.Int, 0, userId )
                    };
            db.RunProc("up_insertErrorLog", parameters);
        }
    }
    public void InsertSqlLog(int userId, string source, string exceptionName, string errorMessage)
    {
        //string errorMessage = ex.Message + " " + ex.StackTrace;
       // string exceptionName = ex.ToString();

        using (DbManager db = DbManager.GetDbManager())
        {
            SqlParameter[] parameters = new SqlParameter[]
                    {
                          db.MakeInParam("@vchErrorSource", SqlDbType.VarChar, 500, source)
                        , db.MakeInParam("@vchErrorMessage", SqlDbType.VarChar, 500, errorMessage)
                        , db.MakeInParam("@tntErrorTypeId", SqlDbType.VarChar, 50, 1 )
                        , db.MakeInParam("@dtmDateCreated", SqlDbType.DateTime, 0, DateTime.Now)
                        , db.MakeInParam("@vchExceptionName", SqlDbType.VarChar, 500, exceptionName)
                        , db.MakeInParam("@intErrorUserId", SqlDbType.Int, 0, userId )
                    };
            db.RunProc("up_insertErrorLog", parameters);
        }
    }
    public DataTable getSqllog(int pageId, int pageSize, out int rowCount)
    {
        DataSet ds = null;
        using (DbManager db = DbManager.GetDbManager())
        {
            try
            {
                SqlParameter[] prams = new SqlParameter[5];
                prams[0] = db.MakeInParam("@intPageId", SqlDbType.Int, 0, pageId);
                prams[1] = db.MakeInParam("@intPageSize", SqlDbType.Int, 0, pageSize);
                prams[2] = db.MakeInParam("@asc_desc", SqlDbType.VarChar, 0, "desc");
                prams[3] = db.MakeInParam("@orderBy", SqlDbType.VarChar, 0, "dtmDateCreated");
                prams[4] = db.MakeReturnParam(SqlDbType.Int, 0);
                ds = db.GetDataSet("up_select_ErrorLog", prams);
                rowCount = prams[4].Value == DBNull.Value ? 0 : Convert.ToInt32(prams[4].Value);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                else return null;
            }
            catch (Exception ex)
            {
                rowCount = 0;
                return null;
            }
            finally
            {
                db.Close();
            }
        }
    }
}
//=============================================================================================================

