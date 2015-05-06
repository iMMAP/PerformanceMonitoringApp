using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace SRFROWCA.Common
{
    public static class EmailNotifications
    {
        internal static void SendEmailRequestOrg(DataTable dt, bool isAdded)
        {
            if (dt.Rows.Count > 0)
            {
                string name = dt.Rows[0]["OrganizationContact"].ToString();
                string email = dt.Rows[0]["OrganizationEmail"].ToString();
                string orgName = dt.Rows[0]["OrganizationName"].ToString();

                try
                {
                    string emails = string.Empty;
                    emails = "orsocharowca@gmail.com";
                    //emails = ",kashif.nadeem@live.com";
                    bool isValid = false;
                    using (MailMessage mailMsg = new MailMessage())
                    {
                        try
                        {
                            MailAddress ma = new MailAddress(email);
                            emails += "," + email;
                            isValid = true;
                        }
                        catch { }
                        if (isValid)
                        {
                            mailMsg.From = new MailAddress("ors@ocharowca.info", "Sahel - ORS");
                            mailMsg.To.Add("ors@ocharowca.info");
                            mailMsg.Bcc.Add(emails);
                            mailMsg.Subject = "ORS - Organization Add Request.";
                            mailMsg.IsBodyHtml = true;
                            string body = "";
                            body = isAdded ? string.Format(@"Dear {0},<br/>Your requested organization '{1}' has been added in Sahel - ORS.<br/>Please do not hesitate to contact the ORS Helpdesk via e-mail ors@ocharowca.info or Skype ID Orshelpdesk in case of any questions or queries<br/>Regards,", name, orgName) :
                                string.Format(@"Dear {0},<br/>Your request to add organization '{1}' in Sahel - ORS is rejected by admin of the site. This organizaiton already exists in ORS.<br/>Please do not hesitate to contact the ORS Helpdesk via e-mail ors@ocharowca.info or Skype ID Orshelpdesk in case of any questions or queries<br/>Regards,", name, orgName);
                            mailMsg.Body = body;
                            Mail.SendMail(mailMsg);
                        }
                    }
                }
                catch
                {

                }
            }
        }
    }
}