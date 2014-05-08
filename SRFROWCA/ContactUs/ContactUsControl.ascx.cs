using System;
using System.Net.Mail;
using System.ServiceModel.Channels;
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
                string message = "";
                if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
                {
                    message = RC.SelectedSiteLanguageId == 2 ? "Merci de saisir votre email!" : "Please enter your email!";
                    ShowMessage(message, "error-message");
                    return;
                }

                SendMessage();
                message = RC.SelectedSiteLanguageId == 2 ? "Votre message a été envoyé" : "Your message has been sent!";
                ShowMessage(message, "info-message");
            }
            catch (Exception ex)
            {
                string message = "";
                if (ex.Message.Equals("The specified string is not in the form required for an e-mail address."))
                {
                    message = RC.SelectedSiteLanguageId == 2 ? "Merci de donner une adresse email valide!" : "Please provide valid E-Mail address!";
                    ShowMessage(message, "error-message");
                }
                else
                {
                    message = RC.SelectedSiteLanguageId == 2 ? "Une erreur est survenue, merci de réessayer" : "Some an error occoured, please try again!";
                    ShowMessage(message, "error-message");
                }
            }
        }

        private void SendMessage()
        {
            try
            {
                using (MailMessage mailMsg = new MailMessage())
                {
                    mailMsg.From = new MailAddress(txtEmail.Text.Trim());
                    mailMsg.To.Add(new MailAddress("orsocharowca@gmail.com"));
                    mailMsg.Subject = "ORS Contact US!";
                    mailMsg.IsBodyHtml = true;
                    mailMsg.Body = txtMessage.Text.Trim();
                    Mail.SendMail(mailMsg);
                    ShowMessage(@"You message has been sent!", "");
                }
            }
            catch
            {
                ShowMessage("Problem contacting ORS help desk, please check your email or contact us directly using ors@ocharowca.info", "");
            }
            //Mail.SendMail(txtEmail.Text.Trim(), "orsocharowca@gmail.com ", "3W Activities: " + txtSubject.Text.Trim(), txtMessage.Text.Trim());
        }

        private void ShowMessage(string msg, string cssClass)
        {
            lblMessage.Visible = true;
            lblMessage.CssClass = cssClass;
            lblMessage.Text = msg;
        }
    }
}