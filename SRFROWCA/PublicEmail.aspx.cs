using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA
{
    public partial class PublicEmail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnMail_Click(object sender, EventArgs e)
        {
            DataTable dtEmails = DBContext.GetData("UsersEmailAddress");
            string emails = string.Empty;
            emails = "orsocharowca@gmail.com";
            emails += ",kashif.nadeem@hotmail.com";
            using (MailMessage mailMsg = new MailMessage())
            {
                int j = 0;
                for (int ik = 0; i < dtEmails.Rows.Count; i++)
                {
                    string email = dtEmails.Rows[i]["Email"].ToString();
                    email = email.Trim().TrimEnd(',');
                    email = email.TrimStart(',');
                    try
                    {
                        MailAddress ma = new MailAddress(email);
                        j++;
                        emails += "," + email;
                    }
                    catch
                    {

                    }
                }

                mailMsgs.From = new MailAddress("ors@ocharowca.info", "Sahel - ORS");
                mailMsg.To.Add("ors@ocharowca.info");
                mailMsg.Bcc.Add(emails);
                mailMsg.Subject = "ORS: Partager l'information pour renforcer la coordination. Merci pour vos efforts/Sharing information to strenghten coordination. Thank you for your efforts";
                string body = @"    Chers collègues,<br/>
                                    J’aimerais transmettre les salutations de l’équipe du système de rapportage en ligne ORS.<br/>
                                    Vous recevez ce message car vous êtes le point focal cluster/secteur au sein de votre organisation.<br/>
                                    Tout d'abord, nous tenons à exprimer notre sincère gratitude pour votre collaboration quant au rapportage sur les réalisations de votre cluster pour l’année 2014.<br/>
                                    En 2015, nous avons amélioré le système en incorporant de nouvelles fonctions telles que le ‘Qui Fait, Quoi, Ou’ (3W). Nous avons également lié les projets aux partenaires d’implémentation et développé la visualisation de par les tableaux de bord (dashboard). Ces fonctionnalités vous permettront d’effectuer plus d’analyses sur la réponse et de diminuer les doublons (ex. dans le cas du 3W et du suivi de la performance). <br/>
                                    Comme vous le savez, le SRP 2015 a été lancé en février, et nous sommes dans la période où il est impératif de rapporter sur les indicateurs de résultat de 2015 (janvier, février et mars).<br/>
                                    Pour accéder au site et commencer le rapportage de l’indicateur de résultat de votre cluster, <a href='http://ors.ocharowca.info/'>cliquez ici</a><br/>
                                    Nous vous remercions et, pour tout renseignement complémentaire, n’hésitez pas à contacter le Helpdesk ORS par e-mail (ors@ocharowca.info ) ou par Skype (orshelpdesk).<br/>
                                    Cordialement,<br/>
                                    Sahel ORS<br/><br/>
                                    ****
                                    <br/><br/>
                                    Dear Colleague,<br/>
                                    Greetings from the Sahel Online Reporting System (ORS).<br/>
                                    You are receiving this message as the named cluster/sector focal point in your country.<br/>
                                    First, we would like to extend our sincere appreciation to your continued collaboration with regard to reporting on the achievements of your cluster for 2014.<br/>
                                    In 2015, we have enhanced the tool to incorporate some new features like the ‘Who What Where’ (3W), Key figures and linking of projects to implementing partners. We will also enhance the visualization by providing dashboards. These features will provide you with more analysis about the response and also minimize duplication of efforts i.e. in the case of the 3W and performance monitoring.<br/>
                                    In the meantime, the SRP 2015 was launched in February, and it is now time for you to report on the Output Indicators for 2015 (January, February and March).<br/>
                                    To proceed to ORS and start reporting on your cluster output indicator <a href='http://ors.ocharowca.info/'>click here</a><br/>
                                    Please do not hesitate to contact the ORS Helpdesk via e-mail ors@ocharowca.info  or Skype ID Orshelpdesk in case of any questions or queries.<br/>
                                    Regards,<br/> 
                                    Sahel ORS";
                mailMsg.Body = body;
                mailMsg.IsBodyHtml = true;
                
                //Mail.SendMail(mailMsg);
            }
        }
    }
}