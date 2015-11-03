using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace SRFROWCA.Admin
{
    public partial class TargetSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadCombos();
            LoadSettings();
        }

        private void LoadCombos()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);
            ddlCountry.Items.Insert(0, new ListItem("Select Country", "0"));

            UI.FillEmergnecyClusters(ddlCluster, RC.EmergencySahel2015);
            ddlCluster.Items.Insert(0, new ListItem("Select Cluster", "0"));
        }

        private void LoadSettings()
        {
            gvSettings.DataSource = GetFrameworkSettings();
            gvSettings.DataBind();
        }

        private DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Key");
            dt.Columns.Add("Country");
            dt.Columns.Add("Cluster");
            dt.Columns.Add("Target");
            dt.Columns.Add("Level");
            dt.Columns.Add("Category");
            dt.Columns.Add("Mandatory");
            return dt;
        }

        private DataTable GetFrameworkSettings()
        {
            DataTable dt = CreateDataTable();
            string path = GetFilePath();
            if (File.Exists(path))
            {
                XElement root = XElement.Load(path);
                IEnumerable<XElement> address =
                    from el in root.Elements("AdminTarget")
                    select el;
                foreach (XElement el in address)
                {
                    AddRowInDataTable(el, dt);
                }
            }

            return dt;
        }

        private void AddRowInDataTable(XElement el, DataTable dt)
        {
            DataRow row = dt.NewRow();

            string country = null;
            if (el.Attribute("CountryID") != null)
                country = el.Attribute("CountryID").Value;

            string cluster = null;
            if (el.Attribute("ClusterID") != null)
                cluster = el.Attribute("ClusterID").Value;
            if (country != null && cluster != null)
            {
                row["Key"] = country + cluster;
                row["Country"] = el.Attribute("Country") == null ? "" : el.Attribute("Country").Value;
                row["Cluster"] = el.Attribute("Cluster") == null ? "" : el.Attribute("Cluster").Value;
                row["Target"] = el.Attribute("IsTarget") == null ? "" : el.Attribute("IsTarget").Value;
                string level = "";
                if (el.Attribute("Level") != null)
                {
                    if (el.Attribute("Level").Value == "National")
                        level = "Country";
                    else if (el.Attribute("Level").Value == "Governorate")
                        level = "Admin1";
                    else if (el.Attribute("Level").Value == "District")
                        level = "Admin2";
                }

                row["Level"] = level;

                row["Category"] = el.Attribute("Category") == null ? "" : el.Attribute("Category").Value;
                row["Mandatory"] = el.Attribute("IsMandatory") == null ? "" : el.Attribute("IsMandatory").Value;
            }

            dt.Rows.Add(row);
        }

        protected void ddlSelected(object sender, EventArgs e)
        {
            int countryId = RC.GetSelectedIntVal(ddlCountry);
            int clusterId = RC.GetSelectedIntVal(ddlCluster);

            if (countryId > 0 && clusterId > 0)
            {
                string key = ddlCountry.SelectedValue + ddlCluster.SelectedValue;
                FillControls(key);
            }
        }

        private void FillControls(string key)
        {
            string path = GetFilePath();

            XElement root = XElement.Load(path);
            IEnumerable<XElement> address =
                from el in root.Elements("AdminTarget")
                where (string)el.Attribute("Key") == key
                select el;
            foreach (XElement el in address)
            {
                if (el.Attribute("IsTarget") != null)
                {
                    if (el.Attribute("IsTarget").Value == "Yes")
                    {
                        rbTargetYes.Checked = true;
                        rbTargetNo.Checked = false;
                    }
                    else
                    {
                        rbTargetNo.Checked = true;
                        rbTargetYes.Checked = false;
                    }
                }
                else
                {
                    rbTargetNo.Checked = true;
                    rbTargetYes.Checked = false;
                }

                if (el.Attribute("Level") != null)
                    ddlLevel.SelectedValue = el.Attribute("Level").Value;
                if (el.Attribute("Category") != null)
                    ddlType.SelectedValue = el.Attribute("Category").Value;

                if (el.Attribute("IsMandatory") != null)
                {
                    if (el.Attribute("IsMandatory").Value == "Yes")
                    {
                        rbMandatoryYes.Checked = true;
                        rbMandatoryNo.Checked = false;
                    }
                    else
                    {
                        rbMandatoryNo.Checked = true;
                        rbMandatoryYes.Checked = false;
                    }
                }
                else
                {
                    rbMandatoryNo.Checked = true;
                    rbMandatoryYes.Checked = false;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SetFrameworkSettings();
            LoadSettings();
            lblFrameworkSettings.Text = "Settings save successfully!";
        }

        private void SetFrameworkSettings()
        {
            DeleteElement();
            AddSettingsKey();
            LoadSettings();
        }

        private void AddSettingsKey()
        {
            string path = GetFilePath();
            XDocument doc = null;
            if (!File.Exists(path))
            {
                doc = new XDocument(new XDeclaration("1.0", "utf-8", ""));
                XElement root = new XElement("AdminTargets");
                doc.Add(root);
            }
            else
            {
                doc = XDocument.Load(path);
            }

            List<ListItem> countriesList = RC.GetListControlItems(ddlCountry);
            List<ListItem> clustersList = RC.GetListControlItems(ddlCluster);

            foreach (ListItem country in countriesList)
            {
                foreach (ListItem cluster in clustersList)
                {
                    string key = country.Value + cluster.Value;
                    string isTarget = rbTargetYes.Checked ? "Yes" : "No";
                    string isMandatory = rbMandatoryYes.Checked ? "Yes" : "No";

                    XElement el = new XElement("AdminTarget",
                                    new XAttribute("Key", key),
                                    new XAttribute("CountryID", country.Value),
                                    new XAttribute("Country", country.Text),
                                    new XAttribute("ClusterID", cluster.Value),
                                    new XAttribute("Cluster", cluster.Text),
                                    new XAttribute("IsTarget", isTarget),
                                    new XAttribute("Level", ddlLevel.SelectedValue),
                                    new XAttribute("Category", ddlType.SelectedValue),
                                    new XAttribute("IsMandatory", isMandatory)
                                    );
                    doc.Element("AdminTargets").Add(el);
                }
            }

            doc.Save(path);

        }

        private void CreateSettingsAttribute(XmlDocument doc, XmlElement elem, string name, string value)
        {
            XmlAttribute key;
            key = doc.CreateAttribute(name);
            key.Value = value;
            elem.SetAttributeNode(key);
        }

        private void DeleteElement()
        {
            string path = GetFilePath();
            XDocument doc = XDocument.Load(path);

            List<ListItem> countriesList = RC.GetListControlItems(ddlCountry);
            List<ListItem> clustersList = RC.GetListControlItems(ddlCluster);

            foreach (ListItem country in countriesList)
            {
                foreach (ListItem cluster in clustersList)
                {
                    string key = country.Value + cluster.Value;
                    doc.Descendants("AdminTarget")
                    .Where(e => (string)e.Attribute("Key") == key)
                    .Remove();
                }
            }

            doc.Save(path);
        }

        //private void DeleteSettingsKey(string configKey, XmlDocument doc)
        //{
        //    string path = GetFilePath();
        //    if (File.Exists(path))
        //    {
        //        XmlNode settingsNode = doc.DocumentElement;
        //        XDocument delKey = XDocument.Load(path);
        //        foreach (XmlNode node in settingsNode.ChildNodes)
        //        {
        //            if (node.Name.Equals(configKey))
        //            {
        //                delKey.Descendants(configKey).Remove();
        //                delKey.Save(path);
        //                doc.Load(path);
        //            }
        //        }
        //    }
        //}

        private string GetFilePath()
        {
            string path = HttpRuntime.AppDomainAppPath;
            return path.Substring(0, path.LastIndexOf(@"\") + 1) + @"Configurations\AdminTargetSettings.xml";
        }

    }
}