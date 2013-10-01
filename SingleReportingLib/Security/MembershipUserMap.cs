using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace OCHA.Security.Library
{
    class MembershipUserMap : MembershipUser
    {

        public MembershipUserMap(MemberInfo.Logins user)
            : base("CustomMembership", user.Login, user.LoginId, user.Password, "", "", true, false, user.DateCreated, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue)
		{
			
		}
    }
}
