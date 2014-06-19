using System;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace DataAccessLayer.DataAccess
{
    public class DBOperations
    {
        public static DataTable GetData(String conString, String procName)
        {
            DataTable dt = new DataTable("Table");
            Database database = DataAccessConnection.GetDBConnection(conString);
            Database.ClearParameterCache();

            using (DbCommand dbCommand = database.GetStoredProcCommand(procName))
            {
                DataSet dataSet = new DataSet();
                dataSet = database.ExecuteDataSet(dbCommand);
                dt = dataSet.Tables[0] != null ? dataSet.Tables[0] : new DataTable();
            }
            return dt;
        }

        public static DataTable GetData(String conString, String procName, params Object[] parameters)
        {
            DataTable dt = new DataTable("Table");
            Database database = DataAccessConnection.GetDBConnection(conString);
            Database.ClearParameterCache();
            using (DbCommand dbCommand = database.GetStoredProcCommand(procName, parameters))
            {
                DataSet dataSet = database.ExecuteDataSet(dbCommand);
                dataSet = new DataSet();
                dataSet = database.ExecuteDataSet(dbCommand);
                if (dataSet.Tables.Count > 0)
                    dt = dataSet.Tables[0] != null ? dataSet.Tables[0] : new DataTable();
            }

            return dt;
        }

        //projectCode, projectTitle, appealFund, projectAppealAgencyId, appealId, dtAppealPhases, dtAppealSectors, DBNull.Value
        public static int Save(String loginEnvironment, String storedProcedureName, params Object[] parameters)
        {
            int recordId = 0;
            Database database = DataAccessConnection.GetDBConnection(loginEnvironment);
            using (DbConnection dbConnection = database.CreateConnection())
            {
                dbConnection.Open();
                Database.ClearParameterCache();
                using (DbCommand dBCommand = database.GetStoredProcCommand(storedProcedureName, parameters))
                {
                    database.ExecuteNonQuery(dBCommand);
                    if (database.GetParameterValue(dBCommand, "@UID") != DBNull.Value)
                    {
                        recordId = Convert.ToInt32(database.GetParameterValue(dBCommand, "@UID"));
                    }
                }
            }

            return recordId;
        }

        public static bool ExecuteSPROC(string loginEnvironment, string sprocName, List<object[]> listObjParams, out string errorMessage, string type)
        {
            Database database = DataAccessConnection.GetDBConnection(loginEnvironment);
            errorMessage = string.Empty;

            try
            {
                using (DbConnection dbConnection = database.CreateConnection())
                {
                    dbConnection.Open();
                    Database.ClearParameterCache();

                    if (type.Equals("Project"))
                    {
                        database.ExecuteNonQuery(CommandType.Text, "DELETE FROM FTSFunding");
                        database.ExecuteNonQuery(CommandType.Text, "DELETE FROM Projects");
                    }

                    foreach (object[] parameters in listObjParams)
                    {
                        using (DbCommand dbCommand = database.GetStoredProcCommand(sprocName, parameters))
                        {
                            database.ExecuteNonQuery(dbCommand);
                            //dtData = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            

            return true;
        }
    }
}