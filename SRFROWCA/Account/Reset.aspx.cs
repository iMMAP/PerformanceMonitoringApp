using System;
using System.Configuration;
using System.Data;
using System.Web.Security;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Account
{
    public partial class Reset : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsPostBack) return;

                this.Form.DefaultButton = this.btnSubmit.UniqueID;
                this.Form.DefaultFocus = this.txtPassword.UniqueID;

                if (Request.QueryString["key"] == null)
                {
                    Session["FromResetPageError"] = "Msg1";
                    Response.Redirect("~/Account/ForgotPassword.aspx");
                }

                Session["ResetKey"] = Request.QueryString["key"].ToString();
                DataTable dt = GetResetPasswordValues();
                if (dt.Rows.Count == 0)
                {
                    Session["FromResetPageError"] = "Msg2";
                    Response.Redirect("~/Account/ForgotPassword.aspx");
                }

                DateTime linkDate = (Convert.ToDateTime(dt.Rows[0]["LinkGeneratedDate"].ToString())).AddHours(24);
                if (DateTime.Now > linkDate)
                {
                    Session["FromResetPageError"] = "Msg1";
                    Response.Redirect("~/Account/ForgotPassword.aspx");
                }
            }
            catch
            {
                Session["FromResetPageError"] = "Msg1";
                Response.Redirect("~/Account/ForgotPassword.aspx");
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["ResetKey"] != null)
                {
                    DataTable dt = GetResetPasswordValues();
                    if (dt.Rows.Count > 0)
                    {
                        if (!IsHashMatched(dt))
                        {
                            Session["FromResetPageError"] = "Msg1";
                            Response.Redirect("~/Account/ForgotPassword.aspx");
                        }
                        else
                        {
                            string userName = dt.Rows[0]["UserName"].ToString();
                            string password = txtPassword.Text;
                            MembershipUser mu = Membership.GetUser(userName);
                            mu.ChangePassword(mu.ResetPassword(), password);
                            DeleteKeyFromDB();
                            Session["ResetKey"] = null;
                            ShowMessage(RC.InfoMessage, "Your password has been changed!");
                        }
                    }
                }
                else
                {
                    Session["FromResetPageError"] = "Msg1";
                    Response.Redirect("~/Account/ForgotPassword.aspx");
                    //ShowMessage(RC.ErrorMessage, "We're sorry, but this reset code has expired. Please request a new one.");
                }
            }
            catch (Exception ex)
            {
                ShowMessage(RC.ErrorMessage, ex.Message);
            }
        }

        private void DeleteKeyFromDB()
        {
            Guid guid = new Guid(Session["ResetKey"].ToString());
            DBContext.Delete("DeleteResetPasswordKey", new object[] { guid, DBNull.Value });
        }

        private bool IsHashMatched(DataTable dt)
        {
            string guid = dt.Rows[0]["LinkGUID"].ToString();
            string date = Convert.ToDateTime(dt.Rows[0]["LinkGeneratedDate"]).ToString("yyyy-MM-dd HH:mm:ss");

            string tempString = "I8$pUs9\\";
            if (ConfigurationManager.AppSettings["tempresetstring"] != null)
            {
                tempString += ConfigurationManager.AppSettings["tempresetstring"].ToString();
            }

            string hash = RC.GetHashString(guid + tempString + date);
            string savedHash = dt.Rows[0]["LinkHash"].ToString();
            return hash == savedHash;
        }

        private DataTable GetResetPasswordValues()
        {
            Guid guid = new Guid(Session["ResetKey"].ToString());
            return DBContext.GetData("GetResetPasswordValues", new object[] { guid });
        }

        private void ShowMessage(string css, string message)
        {
            lblMessage.Visible = true;
            lblMessage.CssClass = css;
            lblMessage.Text = message;
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "Reset", this.User);
        }
    }
}