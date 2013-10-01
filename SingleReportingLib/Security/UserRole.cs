using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SingleReporting.Security
{
    class UserRole : UserInfo
    {
     
        private List<Role> roleofuser;


        public void getRoleInfoUser()
        {
            DataTable result;
            using (DbManager db = DbManager.GetDbManager())
            {
                SqlParameter[] parameter = new SqlParameter[1];

                parameter[0] = db.MakeInParam("@intUserId", SqlDbType.Int, 0,base.UserId );

                result = db.GetDataSet("up_getUserRole_Info", parameter).Tables[0];
                //db.RunProc("up_getUserRole_Info", parameter);
            }

        }
    
    }
}
