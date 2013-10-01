﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using SingleReporting.Utilities;
using System.Web;
using System.Web.UI;

namespace OCHA.Security.Library
{
    class Membership : MembershipProvider
    {
        
        public override string ApplicationName
        {
            get

               ;

            set

                ;

        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            //throw new NotImplementedException();
            if (MemberInfo.Logins.CheckEmailExists(username))
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;

            }
            else
            {
                MemberInfo.Logins newUser = new MemberInfo.Logins();
                newUser.Login = username;
                newUser.Password = (password); // TODO encrypt

                MemberInfo.Logins.InsertLogin(newUser);
                MembershipUserMap usr = new MembershipUserMap(newUser);
                status = MembershipCreateStatus.Success;
                return usr;
            }
        }


        #region Not Implemented

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }
        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }
        #endregion



        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            MembershipUserMap user;
            MemberInfo.Logins User = MemberInfo.Logins.GetUserByUserName(username);
            user = new MembershipUserMap(User);
            return user; 
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            return null;
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

      

        public override void UpdateUser(MembershipUser user)
        {

        }

        public override bool ValidateUser(string username, string password)
        {   
              bool blnCheck = false;
              bool blnUserAccountStatus = false;



             blnUserAccountStatus = MemberInfo.CheckLock(username, Encryption.Encrypt(password));

             if (blnUserAccountStatus)
             {
                 HttpContext.Current.Response.Redirect("Login.aspx?userstat=inactive");
                 
             }
             else

             {
                 blnCheck = MemberInfo.AuthenticateMember(username, Encryption.Encrypt(password)) != null ? true : false;
             }
             
       
             return blnCheck;
             
            
            


       }

       
      

    }
}
