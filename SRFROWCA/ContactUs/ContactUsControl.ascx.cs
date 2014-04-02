using System;
using SRFROWCA.Common;

namespace SRFROWCA.ContactUs
{
    public partial class ContactUsControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void txtSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
                {
                    ShowMessage("Please enter your email!", "error-message");
                    return;
                }

                SendMessage();
                ShowMessage("Your message has been sent!", "info-message");
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("The specified string is not in the form required for an e-mail address."))
                {
                    ShowMessage("Please provide valid E-Mail address!", "error-message");
                }
                else
                {
                    ShowMessage("Some an error occoured, please try again!", "error-message");
                }
            }
        }

        private void SendMessage()
        {
            Mail.SendMail(txtEmail.Text.Trim(), "3wopactivities@gmail.com", "3W Activities: " + txtSubject.Text.Trim(), txtMessage.Text.Trim());
        }

        private void ShowMessage(string msg, string cssClass)
        {
            lblMessage.Visible = true;
            lblMessage.CssClass = cssClass;
            lblMessage.Text = msg;
        }
    }
}