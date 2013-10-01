using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Web.UI;
using System.Data;
using SingleReporting.Utilities;



public class clsCommon
{


    public const int iPageSize = 10;
    public const int iDetailPageSize = 1000;
    public const int iMaxlengthName = 120;
    public const int iMaxAbbreviation = 50;
    public const int iSmallPageSize = 5;

    public const string sTaxNameAlready = "Tax/Fee name already exists";
    public const string sExpenseNameAlready = "Expense name already exists";
    public const string sLaborNameAlready = "Labor name already exists";
    public const string sBenefitNameAlready = "Benefit name already exists";
    public const string sErrorRoleAlreadyExist = "Role already exist";
    public const string sAssignedMessage = "Lead assign to member successfully";
    public const string sExceptionMessage = "Error processing information";
    public const string sRecordUpdated = "Record updated successfully";
    public const string sRecordAdded = "Record added successfully";
    public const string sRecordDeleted = "Record deleted successfully";
    public const string sSelectRole = "Select atleast one role";
    public const string sAccountCreated = "Account created successfully";
    public const string sAlreadyInUse = "Login name is already in use";
    public const string sSelectContractorType = "Select contractor type";
    public const string sErrorLoadingData = "Error loading data";
    public const string sConfirmRequest = "Your request has been sent successfully";
    public const string sMaxLength500 = "Text length can not be more than 500 characters";
    public const string sFormRequiredInformation = "Please Provide Required Information";
    public const string sInvalidZip = "Invalid Zip Code";
    public const string sInvalidCredentials = "Invalid Credentials Provided";
    public const string sPasswordSentMessage = "Your Password has been sent to your email address";
    public const string sInvalidUser = "Invalid User";
    public const string sUserTypeRequired = "User Type Required";
    public const string sMemberShipRequired = "Membership Level Required";
    public const string sContractorTypeRequired = "Contractor Type Required";
    public const string sErrorTerritoryAlreadyExist = "Territory with same name already exists";
    public const string sRoleNameRequired = "Role Name Required";
    public const string sParentTabCannotBeParent = "Parent tab cannot be a child tab of some other tab";
    public const string sRecordNotFound = "No record found";
    public const string sSelectLead = "Lead information required";
    public const string sDateMissing = "Date required";
    public const string sSelectAtleast = "Select at least one record";
    public const string sSelectAtleastRole = "Select at least one role";
    public const string sSelectAtleastCompany = "Select company name";
    public const string sSelectUserCompStatus = "Select user company status";
    public const string sNoRecord = "No record(s) found";

    /// <summary>
    /// This splits Email Address by Semicolon(;) and return us a List of Email address
    /// </summary>
    /// <param name="sAddress">a string of Addresses which should be split by Semicolon</param>
    /// <returns></returns>
    public static List<string> SplitEmailAddressBySemicolon(string sAddress)
    {
        List<string> lstAddress = new List<string>();

        try
        {
            if (string.IsNullOrEmpty(sAddress) == false)
            {
                foreach (string add in sAddress.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    lstAddress.Add(add);
                }
            }
        }
        catch { }

        return lstAddress;
    }
    /// <summary>
    /// This splits Email Address by Semicolon(;)
    /// </summary>
    /// <param name="sAddress">string of addresses which should be split</param>
    /// <param name="mailMsg">MailMessage class object to which addresses should be added</param>
    /// <param name="sType">Type of addresses</param>
    public static void SplitEmailAddressBySemicolon(string sAddress, MailMessage mailMsg, string sType)
    {
        try
        {
            if (string.IsNullOrEmpty(sAddress) == false)
            {
                switch (sType.ToLower())
                {
                    case "to":
                        foreach (string add in sAddress.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            mailMsg.To.Add(new MailAddress(add));
                        }
                        break;
                    case "cc":
                        foreach (string add in sAddress.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            mailMsg.CC.Add(new MailAddress(add));
                        }
                        break;
                    case "bcc":
                        foreach (string add in sAddress.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            mailMsg.Bcc.Add(new MailAddress(add));
                        }
                        break;
                }
            }
        }
        catch { }
    }
    /// <summary>
    /// This removes White spaces from the string
    /// </summary>
    /// <param name="sData">The string from which white space should be removed</param>
    /// <returns></returns>
    public static string RemoveWhiteSpace(string sData)
    {
        if (sData.IndexOf(" ") > 0)
        {
            string[] sArr = sData.Split(new char[] { ' ' });
            string sText = string.Empty;
            foreach (string s in sArr)
            {
                sText = sText + s;
            }

            return sText;
        }
        else
            return sData;
    }
    /// <summary>
    /// This removes any Special characters from supplied string
    /// </summary>
    /// <param name="input">The string from which special characters should be removed</param>
    /// <returns></returns>
    public static string removeSpecialChar(string input)
    {
        return System.Text.RegularExpressions.Regex.Replace(input, @"[^\w\.-]", "");
    }


    public static int GetQueryString(string strValue)
    {
        try
        {
            int iValue = 0;
            iValue = clsCommon.ParseInt(HttpContext.Current.Request[strValue].ToString());

            return iValue;
        }
        catch
        { }

        return 0;
    }
    /// <summary>
    /// This gives us QueryString variable value
    /// </summary>
    /// <param name="strValue">The name of a variable</param>
    /// <returns></returns>
    public static string GetQueryStringValue(string strValue)
    {
        try
        {
            if (HttpContext.Current.Request.QueryString[strValue] != null)
                return HttpContext.Current.Request.QueryString[strValue];
        }
        catch
        { }

        return string.Empty;
    }
    /// <summary>
    /// This gives us Decrypted QueryString variable value
    /// </summary>
    /// <param name="strValue">The name of a variable</param>
    /// <returns></returns>
    public static string GetDecryptQueryStringValue(string strValue)
    {
        try
        {
            if (HttpContext.Current.Request.QueryString[strValue] != null)
                return Encryption.Decrypt(HttpContext.Current.Request.QueryString[strValue]);
        }
        catch
        { }

        return string.Empty;
    }

    public static string GetQueryStringVal(object obj)
    {
        try
        {
            if (obj != null)
                return obj.ToString();
        }
        catch
        { }

        return string.Empty;
    }

    public static int ParseInt(object obj)
    {
        try
        {
            int iValue = 0;
            if (obj != null)
                int.TryParse(obj.ToString(), out iValue);

            return iValue;
        }
        catch
        { }

        return 0;
    }

    public static int ParseInt(string sVal)
    {
        try
        {
            //string str = Encryption.Decrypt("72cOpmWwY s=");
            int iValue = 0;
            if (string.IsNullOrEmpty(sVal) == false)
                int.TryParse(sVal, out iValue);

            return iValue;
        }
        catch
        { }

        return 0;
    }

    public static bool ParseBool(object obj)
    {
        try
        {
            bool bValue = false;
            if (obj != null)
                bool.TryParse(obj.ToString(), out bValue);

            return bValue;
        }
        catch
        { }

        return false;
    }

    public static string ParseDateTimeAsString(object obj)
    {
        try
        {
            if (obj != null)
                return obj.ToString();
        }
        catch
        { }

        return "";
    }

    public static DateTime ParseDateTime(string sDateTime)
    {
        try
        {
            DateTime dt = DateTime.Now;
            if (string.IsNullOrEmpty(sDateTime) == false)
                DateTime.TryParse(sDateTime, out dt);

            return dt;
        }
        catch
        { }

        return DateTime.Now;
    }

    public static string ParseString(object obj)
    {
        try
        {
            if (obj != null)
            {
                return obj.ToString();
            }
        }
        catch
        { }

        return string.Empty;
    }

    public static decimal ParseDecimal(object obj)
    {
        try
        {
            decimal iValue = 0;
            if (obj != null)
                decimal.TryParse(obj.ToString(), out iValue);

            return iValue;
        }
        catch
        { }

        return 0;
    }

    public static bool ParseDBNullBool(object obj)
    {
        try
        {
            bool bValue = false;
            if (obj != DBNull.Value)
                bool.TryParse(obj.ToString(), out bValue);

            return bValue;
        }
        catch
        { }

        return false;
    }

    public static DateTime ParseDBNullDateTime(object obj)
    {
        try
        {
            if (obj != DBNull.Value)
                return DateTime.Parse(obj.ToString());
        }
        catch
        { }

        return DateTime.Now;
    }

    public static string ParseDBNullString(object obj)
    {
        try
        {
            if (obj != DBNull.Value)
                return obj.ToString();
        }
        catch
        { }

        return string.Empty;
    }

    public static int ParseDBNullInt(object obj)
    {
        try
        {
            int iValue = 0;
            if (obj != DBNull.Value)
                int.TryParse(obj.ToString(), out iValue);

            return iValue;
        }
        catch
        { }

        return 0;
    }

    public static double ParseDBNullDouble(object obj)
    {
        try
        {
            double iValue = 0;
            if (obj != DBNull.Value)
                double.TryParse(obj.ToString(), out iValue);

            return iValue;
        }
        catch
        { }

        return 0;
    }

    public static decimal ParseDBNullDecimal(object obj)
    {
        try
        {
            decimal iValue = 0;
            if (obj != DBNull.Value)
                decimal.TryParse(obj.ToString(), out iValue);

            return iValue;
        }
        catch
        { }

        return 0;
    }



    public static string GetRequestFriendlyPageName(HttpRequest request)
    {
        try
        {
            if (request != null)
            {
                string[] sArr = request.CurrentExecutionFilePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string sPage in sArr)
                {
                    if (sPage.IndexOf(".aspx") > 0)
                        return sPage;
                }
            }
        }
        catch { }

        return string.Empty;
    }


    /// <summary>
    /// This will round up the value in left direction
    /// </summary>
    /// <param name="dVal">23343.343</param>
    /// <param name="dRange">1 = 10, 2= 100, 3= 1000 etc.</param>
    /// <returns></returns>
    public static string RoundToLeftDirection(decimal dVal, decimal dRange)
    {
        try
        {
            return Math.Round((dVal / dRange), 0).ToString() + "000";
        }
        catch { }
        return "0";
    }

    public static string GetCountString(List<string> lstValues)
    {
        decimal dVal = 0;

        try
        {
            if (lstValues != null && lstValues.Count > 0)
            {
                foreach (string sVal in lstValues)
                {
                    if (string.IsNullOrEmpty(sVal) == false)
                        dVal += ParseDecimal(sVal);
                }

                return ParseString(dVal);
            }
        }
        catch { }

        return "0";
    }

    /// <summary>
    /// Clear page controls
    /// </summary>
    /// <param name="Page"></param>
    public static void ClearPageControls(Control Page)
    {

        foreach (Control ctrl in Page.Controls)
        {
            if (ctrl is HiddenField)
            {
                ((HiddenField)(ctrl)).Value = "";

            }
            if (ctrl is DropDownList)
            {
                DropDownList lst = (DropDownList)ctrl;
                if (lst.Items.Count > 0)
                    ((DropDownList)(ctrl)).SelectedIndex = 0;
            }
            else if (ctrl is TextBox)
            {
                ((TextBox)(ctrl)).Text = "";
            }
            else if (ctrl is CheckBox)
            {
                ((CheckBox)(ctrl)).Checked = false;
            }
            else if (ctrl is CheckBoxList)
            {
                CheckBoxList Cblist = ((CheckBoxList)(ctrl));
                for (int i = 0; i < Cblist.Items.Count; i++)
                {
                    Cblist.Items[i].Selected = false;

                }
            }
            else if (ctrl is RadioButton)
            {
                ((RadioButton)(ctrl)).Checked = false;
            }
            else
            {
                if (ctrl.Controls.Count > 0)
                {
                    ClearPageControls(ctrl);
                }

            }

        }

    }
}
