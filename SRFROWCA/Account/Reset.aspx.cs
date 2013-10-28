using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogic;
using SRFROWCA.Common;
using System.Configuration;
using System.Web.Security;

namespace SRFROWCA.Account
{
    public partial class Reset : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsPostBack) return;

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
                            ShowMessage(ROWCACommon.InfoMessage, "Your password has been changed!");
                        }
                    }
                }
                else
                {
                    Session["FromResetPageError"] = "Msg1";
                    Response.Redirect("~/Account/ForgotPassword.aspx");
                    //ShowMessage(ROWCACommon.ErrorMessage, "We're sorry, but this reset code has expired. Please request a new one.");
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ROWCACommon.ErrorMessage, ex.Message);
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
            string date = Convert.ToDateTime(dt.Rows[0]["LinkGeneratedDate"]).ToString("yyyy-MM-dd hh:mm:ss");

            string tempString = "";
            if (ConfigurationManager.AppSettings["tempresetstring"] != null)
            {
                tempString = ConfigurationManager.AppSettings["tempresetstring"].ToString();
            }

            string hash = ROWCACommon.GetHashString(guid + tempString + date);
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
    }
}