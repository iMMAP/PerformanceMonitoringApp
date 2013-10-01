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
            catch
            {
                ShowMessage("Some error occoured, please try again!", "error-message");
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