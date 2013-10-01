using System;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace DataAccessLayer.DataAccess
{
    class DataAccessConnection
    {
        private Database database;
        private static DataAccessConnection dataAccessConnection;

        private DataAccessConnection(String Database)
        {
            database = DatabaseFactory.CreateDatabase(Database);
        }

        public static Database GetDBConnection(String strLoginEnvironment)
        {
            switch (strLoginEnvironment.ToUpper())
            {
                case "LIVE":
                    dataAccessConnection = new DataAccessConnection("live_dbName");

                    return dataAccessConnection.database;
                case "TESTING":
                    dataAccessConnection = new DataAccessConnection("test_dbName");

                    return dataAccessConnection.database;
                case "TRAINING":
                    dataAccessConnection = new DataAccessConnection("training_dbName");

                    return dataAccessConnection.database;
                default:
                    return DatabaseFactory.CreateDatabase("test_dbName");
            }
        }
    }
}
