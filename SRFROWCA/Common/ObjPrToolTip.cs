using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace SRFROWCA.Common
{
    public static class ObjPrToolTip
    {
        public static void PrioritiesIconToolTip(GridViewRowEventArgs e, int index)
        {
            Image imghp = e.Row.FindControl("imgPriority") as Image;
            if (imghp != null)
            {
                string txtHP = e.Row.Cells[index].Text;
                if (txtHP == "1")
                {
                    imghp.ImageUrl = "~/assets/orsimages/icon/hp1.png";
                    imghp.ToolTip = RC.SelectedSiteLanguageId == 2 ?
                        "Répondre aux conséquences humanitaires dues aux catastrophes naturelles (inondations, etc.)" :
                        "Addressing the humanitarian impact Natural disasters (floods, etc.)";
                }
                else if (txtHP == "2")
                {
                    imghp.ImageUrl = "~/assets/orsimages/icon/hp2.png";
                    imghp.ToolTip = RC.SelectedSiteLanguageId == 2 ?
                        "Répondre aux conséquences humanitaires dues aux conflits (PDIs, refugies, protection, etc.)" :
                        "Addressing the humanitarian impact of Conflict (IDPs, refugees, protection, etc.)";
                }
                else if (txtHP == "3")
                {
                    imghp.ImageUrl = "~/assets/orsimages/icon/hp3.png";
                    imghp.ToolTip = RC.SelectedSiteLanguageId == 2 ?
                        "Répondre aux conséquences humanitaires dues aux épidémies (cholera, paludisme, etc.)" :
                        "Addressing the humanitarian impact of Epidemics (cholera, malaria, etc.)";
                }
                else if (txtHP == "4")
                {
                    imghp.ImageUrl = "~/assets/orsimages/icon/hp4.png";
                    imghp.ToolTip = RC.SelectedSiteLanguageId == 2 ?
                        "Répondre aux conséquences humanitaires dues à l’insécurité alimentaire" :
                        "Addressing the humanitarian impact of Food insecurity";
                }
                else if (txtHP == "5")
                {
                    imghp.ImageUrl = "~/assets/orsimages/icon/hp5.png";
                    imghp.ToolTip = RC.SelectedSiteLanguageId == 2 ?
                        "Répondre aux conséquences humanitaires dues à la malnutrition" :
                        "Addressing the humanitarian impact of Malnutrition";
                }
            }
        }

        public static void ObjectiveIconToolTip(GridViewRowEventArgs e, int index)
        {
            Image imgObj = e.Row.FindControl("imgObjective") as Image;
            if (imgObj != null)
            {
                string txt = e.Row.Cells[index].Text;

                if (txt.Contains("1"))
                {
                    imgObj.ImageUrl = "~/assets/orsimages/icon/so1.png";
                    imgObj.ToolTip = RC.SelectedSiteLanguageId == 2 ? "OBJECTIF STRATÉGIQUE N°1 : Recueillir les données sur les risques et les vulnérabilités, les analyser et intégrer les résultats dans la programmation humanitaire et de développement." :
                        "STRATEGIC OBJECTIVE 1: Track and analyse risk and vulnerability, integrating findings into humanitarian and evelopment programming.";
                }
                else if (txt.Contains("2"))
                {
                    imgObj.ImageUrl = "~/assets/orsimages/icon/so2.png";
                    imgObj.ToolTip = RC.SelectedSiteLanguageId == 2 ? "OBJECTIF STRATÉGIQUE N°2 : Soutenir les populations vulnérables à mieux faire face aux chocs en répondant aux signaux d’alerte de manière anticipée, réduisant la durée du relèvement post-crise et renforçant les capacités des acteurs nationaux." :
                        "STRATEGIC OBJECTIVE 2: Support vulnerable populations to better cope with shocks by responding earlier to warning signals, by reducing post-crisis recovery times and by building capacity of national actors.";
                }
                else if (txt.Contains("3"))
                {
                    imgObj.ImageUrl = "~/assets/orsimages/icon/so3.png";
                    imgObj.ToolTip = RC.SelectedSiteLanguageId == 2 ? "OBJECTIF STRATÉGIQUE N°3 : Fournir aux personnes en situation d’urgence une assistance coordonnée et intégrée, nécessaire à leur survie." :
                        "STRATEGIC OBJECTIVE 3: Deliver coordinated and integrated life-saving assistance to people affected by emergencies.";
                }
            }
        }

        public static void ObjectivesToolTip(ListControl ctl)
        {
            if (ctl.Items.Count > 2)
            {
                if (RC.SelectedSiteLanguageId == 2)
                {
                    ctl.Items[0].Attributes["title"] = "OBJECTIF STRATÉGIQUE N°1 : Recueillir les données sur les risques et les vulnérabilités, les analyser et intégrer les résultats dans la programmation humanitaire et de développement.";
                    ctl.Items[1].Attributes["title"] = "OBJECTIF STRATÉGIQUE N°2 : Soutenir les populations vulnérables à mieux faire face aux chocs en répondant aux signaux d’alerte de manière anticipée, réduisant la durée du relèvement post-crise et renforçant les capacités des acteurs nationaux.";
                    ctl.Items[2].Attributes["title"] = "OBJECTIF STRATÉGIQUE N°3 : Fournir aux personnes en situation d’urgence une assistance coordonnée et intégrée, nécessaire à leur survie.";
                }
                else
                {
                    ctl.Items[0].Attributes["title"] = "STRATEGIC OBJECTIVE 1: Track and analyse risk and vulnerability, integrating findings into humanitarian and development programming.";
                    ctl.Items[1].Attributes["title"] = "STRATEGIC OBJECTIVE 2: Support vulnerable populations to better cope with shocks by responding earlier to warning signals, by reducing post-crisis recovery times and by building capacity of national actors.";
                    ctl.Items[2].Attributes["title"] = "STRATEGIC OBJECTIVE 3: Deliver coordinated and integrated life-saving assistance to people affected by emergencies.";
                }
            }
        }

        public static void ObjectiveLableToolTip(GridViewRowEventArgs e, int index)
        {
            Label lblObj = e.Row.FindControl("lblObjective") as Label;
            if (lblObj != null)
            {
                string txt = e.Row.Cells[index].Text;

                if (txt == "1")
                {
                    lblObj.ToolTip = RC.SelectedSiteLanguageId == 2 ? "OBJECTIF STRATÉGIQUE N°1 : Recueillir les données sur les risques et les vulnérabilités, les analyser et intégrer les résultats dans la programmation humanitaire et de développement." :
                        "STRATEGIC OBJECTIVE 1: Track and analyse risk and vulnerability, integrating findings into humanitarian and evelopment programming.";
                }
                else if (txt == "3")
                {
                    lblObj.ToolTip = RC.SelectedSiteLanguageId == 2 ? "OBJECTIF STRATÉGIQUE N°2 : Soutenir les populations vulnérables à mieux faire face aux chocs en répondant aux signaux d’alerte de manière anticipée, réduisant la durée du relèvement post-crise et renforçant les capacités des acteurs nationaux." :
                        "STRATEGIC OBJECTIVE 2: Support vulnerable populations to better cope with shocks by responding earlier to warning signals, by reducing post-crisis recovery times and by building capacity of national actors.";
                }
                else if (txt == "3")
                 {
                     lblObj.ToolTip = RC.SelectedSiteLanguageId == 2 ? "OBJECTIF STRATÉGIQUE N°3 : Fournir aux personnes en situation d’urgence une assistance coordonnée et intégrée, nécessaire à leur survie." :
                        "STRATEGIC OBJECTIVE 3: Deliver coordinated and integrated life-saving assistance to people affected by emergencies.";
                }
            }
        }

        public static void PrioritiesToolTip(ListControl ctl)
        {
            if (ctl.Items.Count > 5)
            {
                if (RC.SelectedSiteLanguageId == 2)
                {
                    ctl.Items[0].Attributes["title"] = "Répondre aux conséquences humanitaires dues aux catastrophes naturelles (inondations, etc.)";
                    ctl.Items[1].Attributes["title"] = "Répondre aux conséquences humanitaires dues aux conflits (PDIs, refugies, protection, etc.)";
                    ctl.Items[2].Attributes["title"] = "Répondre aux conséquences humanitaires dues aux épidémies (cholera, paludisme, etc.)";
                    ctl.Items[3].Attributes["title"] = "Répondre aux conséquences humanitaires dues à l’insécurité alimentaire";
                    ctl.Items[4].Attributes["title"] = "Répondre aux conséquences humanitaires dues à la malnutrition";
                }
                else
                {
                    ctl.Items[0].Attributes["title"] = "Addressing the humanitarian impact Natural disasters (floods, etc.)";
                    ctl.Items[1].Attributes["title"] = "Addressing the humanitarian impact of Conflict (IDPs, refugees, protection, etc.)";
                    ctl.Items[2].Attributes["title"] = "Addressing the humanitarian impact of Epidemics (cholera, malaria, etc.)";
                    ctl.Items[3].Attributes["title"] = "Addressing the humanitarian impact of Food insecurity";
                    ctl.Items[4].Attributes["title"] = "Addressing the humanitarian impact of Malnutrition";
                }
            }
        }

        public static void RegionalIndicatorIcon(GridViewRowEventArgs e, int cellIndex)
        {
            Image imgRind = e.Row.FindControl("imgRind") as Image;
            if (imgRind != null)
            {
                string txt = e.Row.Cells[cellIndex].Text;
                if (txt.Equals("True"))
                {
                    imgRind.ImageUrl = "~/assets/orsimages/rind.png";
                    imgRind.ToolTip = "Regional Indicator";
                }
                else
                {
                    imgRind.Visible = false;
                }
            }
        }

        public static void CountryIndicatorIcon(GridViewRowEventArgs e, int cellIndex)
        {
            Image imgCind = e.Row.FindControl("imgCind") as Image;
            if (imgCind != null)
            {
                string txt = e.Row.Cells[cellIndex].Text;
                if (txt.Equals("True"))
                {
                    imgCind.ImageUrl = "~/assets/orsimages/cind.png";
                    imgCind.ToolTip = "Country Indicator";
                }
                else
                {
                    imgCind.Visible = false;
                }
            }
        }
    }
}