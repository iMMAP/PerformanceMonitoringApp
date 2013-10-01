using System.Data;
using DataAccessLayer.DataAccess;
using DataAccessLayer;

namespace BusinessLogic
{
    public static class DBContext
    {
        // Get All Records from db without any parameter
        public static DataTable GetData(string spName)
        {
            return DBOperations.GetData(CommonProperties.ConnectionString, spName);
        }

        // Get data from db on passed parameters
        public static DataTable GetData(string spName, object[] parameters)
        {
            return DBOperations.GetData(CommonProperties.ConnectionString, spName, parameters);
        }

        // Add Datain db. (No diff in following all methods, these are just to give user sense that what s/he is doing.
        public static int Add(string procName, object[] values)
        {
            return DBOperations.Save(CommonProperties.ConnectionString, procName, values);
        }

        public static int Update(string procName, object[] values)
        {
            return DBOperations.Save(CommonProperties.ConnectionString, procName, values);
        }

        public static int Delete(string procName, object[] values)
        {
            return DBOperations.Save(CommonProperties.ConnectionString, procName, values);
        }
    }
}
