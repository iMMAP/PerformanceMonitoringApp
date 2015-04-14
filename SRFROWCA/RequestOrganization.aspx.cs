using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA
{
    public partial class RequestOrganization : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            int result = 0;

            try
            {
                result = DBContext.Add("uspSaveOrgRequest", new object[] { txtOrganizationName.Text, 
                                                                           txtAcronym.Text, 
                                                                           txtType.Text, 
                                                                           "", 
                                                                           txtContactName.Text, 
                                                                           txtPhone.Text, 
                                                                           txtEmail.Text,
                                                                           DBNull.Value});

            }
            catch (Exception ex)
            {
                txtMessage.Text = "Error: " + ex.Message;
            }

            if (result > 0)
            {
                string mailBody = "An ORS Organization Request has been posted:";
                mailBody += "<br>" + "Organization Name: " + txtOrganizationName.Text;
                mailBody += "<br>" + "Organization Acronym: " + txtAcronym.Text;
                mailBody += "<br>" + "Organization Type: " + txtType.Text;
                mailBody += "<br>" + "Contact Name: " + txtContactName.Text;
                mailBody += "<br>" + "Phone: " + txtPhone.Text;
                mailBody += "<br>" + "Email: " + txtEmail.Text;
                mailBody += "<br>" + "Database ID: " + result.ToString();

                try
                {
                    using (MailMessage mailMsg = new MailMessage())
                    {
                        mailMsg.From = new MailAddress("orsocharowca@gmail.com");
                        mailMsg.To.Add(new MailAddress("orsocharowca@gmail.com"));
                        mailMsg.Subject = "ORS New Organization Request";
                        mailMsg.IsBodyHtml = true;
                        mailMsg.Body = mailBody;
                        Mail.SendMail(mailMsg);

                        txtMessage.Text = "Success:Your Organization entry has been forwarded. It will be entered into our record after proper scrutiny. Thanks<br>";
                        btnRegister.Enabled = false;
                        btnRegister.Text = "Thanks!";
                    }
                }
                catch
                {
                    txtMessage.Text = "Error: Failed to send email.";
                }
            }
            else
            {
                txtMessage.Text = "Error: Failed to insert record. Please try again later.";
            }
        }
    }
}