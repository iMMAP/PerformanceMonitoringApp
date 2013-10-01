using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Caching;

/// <summary>
/// Summary description for LanguageFilter
/// </summary>
public class LanguageFilter
{
    const string MASKEDCHAR = "*";
    const int CACHEDURATION = 60; // in minutes
    string BAD_WORDS_RULES = "";
    string BAD_TAGS_RULES = "";
    string RESTRICTED_USERNAME_RULES = "";
    string RESTRICTED_TAGS = "";
    string RESTRICTED_USER_SUBSTRINGS = "";
    public LanguageFilter()
    {
        Init();
    }

    private void Init()
    {
        BAD_WORDS_RULES = GetBadWordsRules();
        BAD_TAGS_RULES = string.Empty;
        RESTRICTED_USERNAME_RULES = GetRestrictedUserNameRules();
        RESTRICTED_TAGS =GetRestrictedTags();
        RESTRICTED_USER_SUBSTRINGS = GetRestrictedUserNameSubstrings();
    }
    /// <summary>
    /// Replace bad word with ****
    /// </summary>
    /// <param name="input">string to be filtered</param>
    /// <returns></returns>
    public string BadLanguageFilter(string input)
    {
        StringBuilder output = new StringBuilder(input.Length);

        Regex r = new Regex(BAD_WORDS_RULES, RegexOptions.IgnoreCase | RegexOptions.Multiline);

        MatchCollection matches = r.Matches(input);
        int curPos = 0;
        foreach (Match match in matches)
        {
            output.Append(input.Substring(curPos, match.Index - curPos));
            output.Append(maskedword(match.Value));
            curPos = match.Index + match.Value.Length;
        }
        output.Append(input.Substring(curPos));
        return output.ToString();
    }
    public bool hasRestrictedwords(string input) {
        
        Regex r = new Regex(BAD_WORDS_RULES, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        MatchCollection matches = r.Matches(input);
        if (matches.Count>0)
        {
            return true;
        }
        r = new Regex(BAD_TAGS_RULES, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        matches = r.Matches(input);
        if (matches.Count > 0)
        {
            return true;
        }
         r = new Regex(RESTRICTED_USERNAME_RULES, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        matches = r.Matches(input);
        if (matches.Count > 0)
        {
            return true;
        }
         r = new Regex(RESTRICTED_TAGS, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        matches = r.Matches(input);
        if (matches.Count > 0)
        {
            return true;
        }
         r = new Regex(RESTRICTED_USER_SUBSTRINGS, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        matches = r.Matches(input);
        if (matches.Count > 0)
        {
            return true;
        }

        return false;


    }

    /// <summary>
    /// Replace bad word with ****
    /// </summary>
    /// <param name="input">string to be filtered</param>
    /// <returns></returns>
    public string RestrictedUserNameFilter(string input)
    {
        StringBuilder output = new StringBuilder(input.Length);

        Regex r = new Regex(RESTRICTED_USERNAME_RULES, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);

        MatchCollection matches = r.Matches(input);
        int curPos = 0;
        foreach (Match match in matches)
        {
            output.Append(input.Substring(curPos, match.Index - curPos));
            output.Append(maskedword(match.Value));
            curPos = match.Index + match.Value.Length;
        }
        output.Append(input.Substring(curPos));
        return output.ToString();
    }

    public string RestrictedTagsFilter(string input)
    {
        StringBuilder output = new StringBuilder(input.Length);

        Regex r = new Regex(RESTRICTED_TAGS, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);

        MatchCollection matches = r.Matches(input);
        int curPos = 0;
        foreach (Match match in matches)
        {
            output.Append(input.Substring(curPos, match.Index - curPos));
            output.Append(maskedword(match.Value));
            curPos = match.Index + match.Value.Length;
        }
        output.Append(input.Substring(curPos));
        return output.ToString();
    }
    public string RestrictedUserSubstringFilter(string input)
    {
        StringBuilder output = new StringBuilder(input.Length);

        Regex r = new Regex(RESTRICTED_USER_SUBSTRINGS, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);

        MatchCollection matches = r.Matches(input);
        int curPos = 0;
        foreach (Match match in matches)
        {
            output.Append(input.Substring(curPos, match.Index - curPos));
            output.Append(maskedword(match.Value));
            curPos = match.Index + match.Value.Length;
        }
        output.Append(input.Substring(curPos));
        return output.ToString();
    }

    /// <summary>
    /// Filtered the restricted html tags
    /// </summary>
    /// <param name="input">string to be filtered</param>
    /// <returns></returns>
    public string HTMLTagFilter(string input)
    {
        StringBuilder output = new StringBuilder(input.Length);

        Regex r = new Regex(RESTRICTED_TAGS, RegexOptions.IgnoreCase | RegexOptions.Multiline);

        MatchCollection matches = r.Matches(input);
        int curPos = 0;
        foreach (Match match in matches)
        {
            output.Append(input.Substring(curPos, match.Index - curPos));
            output.Append(maskedTag(match.Value));
            curPos = match.Index + match.Value.Length;
        }
        output.Append(input.Substring(curPos));
        return output.ToString();
    }

    private string maskedTag(string input)
    {
        return input.Replace("<", "&lt;");
    }
    private string maskedword(string input)
    {
        string r = Regex.Replace(input, "[a-zA-Z]", MASKEDCHAR, RegexOptions.IgnoreCase);

        StringBuilder output = new StringBuilder(r);
        int firstChar = output.ToString().IndexOf(MASKEDCHAR);
        if (firstChar >= 0)
        {
            output.Insert(firstChar, input.Trim().Substring(0, 1));
            output.Remove(firstChar + 1, 1);
        }
        return output.ToString();
    }

    private string GetBadWordsRules()
    {
        string cacheName = "LanguageFilter-BadWords";
        string badwords = (string) HttpRuntime.Cache.Get(cacheName);

        if (string.IsNullOrEmpty(badwords))
        {
            StringBuilder sb = new StringBuilder();
            IDataReader reader = null;
            using (DbManager db = DbManager.GetDbManager())
            {
                try
                {
                    reader = db.GetDataReader("up_filter_getBadWords");
                    while (reader.Read())
                    {
                        string s = reader["vchWord"].ToString();

                        // start and end with spaces
                        sb.Append("\\s+");
                        sb.Append(s);
                        sb.Append("\\s+");
                        sb.Append("|");

                        // single word
                        sb.Append("^");
                        sb.Append(s);
                        sb.Append("$|");

                        // bol
                        //sb.Append("^\\s*");
                        //sb.Append(s);
                        //sb.Append("|");

                        //// eol
                        //sb.Append("\\s+");
                        //sb.Append(s);
                        //sb.Append("\\s+$");
                        //sb.Append("|");
                    }
                    badwords = sb.ToString().TrimEnd('|');
                    HttpRuntime.Cache.Insert(cacheName, badwords, null, DateTime.Now.AddMinutes(CACHEDURATION), Cache.NoSlidingExpiration);
                }
                catch (Exception ex) { }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
        }
        return badwords;
    }


    private string GetRestrictedUserNameRules()
    {
        string cacheName = "LanguageFilter-RestrictedUserNames";
        string badwords = (string)HttpRuntime.Cache.Get(cacheName);

        if (string.IsNullOrEmpty(badwords))
        {
            StringBuilder sb = new StringBuilder();
            string[] badUserNames = (string.IsNullOrEmpty(ConfigurationManager.AppSettings["RestrictedUsernames"]) ? null : ConfigurationManager.AppSettings["RestrictedUsernames"].Split(','));
            if (badUserNames != null && badUserNames.Length > 0)
            {
                foreach (string s in badUserNames)
                {
                    // start and end with spaces
                    sb.Append("\\s+");
                    sb.Append(s);
                    sb.Append("\\s+");
                    sb.Append("|");

                    // single word
                    sb.Append("^");
                    sb.Append(s);
                    sb.Append("$|");

                    // bol
                    //sb.Append("^\\s*");
                    //sb.Append(s);
                    //sb.Append("|");

                    //// eol
                    //sb.Append("\\s+");
                    //sb.Append(s);
                    //sb.Append("\\s*$");
                    //sb.Append("|");
                }
                badwords = sb.ToString().TrimEnd('|');
                HttpRuntime.Cache.Insert(cacheName, badwords, null, DateTime.Now.AddMinutes(CACHEDURATION), Cache.NoSlidingExpiration);
            }
        }
        return badwords;
    }


    private string GetRestrictedUserNameSubstrings()
    {
        string cacheName = "LanguageFilter-RestrictedUserNameSubstrings";
        string badwords = (string)HttpRuntime.Cache.Get(cacheName);

        if (string.IsNullOrEmpty(badwords))
        {
            StringBuilder sb = new StringBuilder();
            string[] badUserNames = (string.IsNullOrEmpty(ConfigurationManager.AppSettings["RestrictedUsernameSubstrings"]) ? null : ConfigurationManager.AppSettings["RestrictedUsernameSubstrings"].Split(','));
            if (badUserNames != null && badUserNames.Length > 0)
            {
                foreach (string s in badUserNames)
                {
                    // start and end with spaces
                    sb.Append("\\s+");
                    sb.Append(s);
                    sb.Append("\\s+");
                    sb.Append("|");

                    // single word
                    sb.Append("^");
                    sb.Append(s);
                    sb.Append("$|");

                    //// bol
                    //sb.Append("^\\s*");
                    //sb.Append(s);
                    //sb.Append("|");

                    //// eol
                    //sb.Append("\\s+");
                    //sb.Append(s);
                    //sb.Append("\\s*$");
                    //sb.Append("|");
                }
                badwords = sb.ToString().TrimEnd('|');
                HttpRuntime.Cache.Insert(cacheName, badwords, null, DateTime.Now.AddMinutes(CACHEDURATION), Cache.NoSlidingExpiration);
            }
        }
        return badwords;
    }

    private string GetRestrictedTags()
    {
        string cacheName = "LanguageFilter-RestrictedTags";
        string badwords = (string)HttpRuntime.Cache.Get(cacheName);

        if (string.IsNullOrEmpty(badwords))
        {
            StringBuilder sb = new StringBuilder();
            string[] badUserNames = (string.IsNullOrEmpty(ConfigurationManager.AppSettings["RestrictedTags"]) ? null : ConfigurationManager.AppSettings["RestrictedTags"].Split(','));
            if (badUserNames != null && badUserNames.Length > 0)
            {
                foreach (string s in badUserNames)
                {
                    // start and end with spaces
                    sb.Append("\\s+");
                    sb.Append(s);
                    sb.Append("\\s+");
                    sb.Append("|");

                    // single word
                    sb.Append("^");
                    sb.Append(s);
                    sb.Append("$|");

                    //// bol
                    //sb.Append("^\\s*");
                    //sb.Append(s);
                    //sb.Append("|");

                    //// eol
                    //sb.Append("\\s+");
                    //sb.Append(s);
                    //sb.Append("\\s*$");
                    //sb.Append("|");
                }
                badwords = sb.ToString().TrimEnd('|');
                HttpRuntime.Cache.Insert(cacheName, badwords, null, DateTime.Now.AddMinutes(CACHEDURATION), Cache.NoSlidingExpiration);
            }
        }
        return badwords;
    }
    private string GetBadTagsRules()
    {
        string cacheName = "LanguageFilter-BadTags";
        string badtags = (string) HttpRuntime.Cache.Get(cacheName);

        if (string.IsNullOrEmpty(badtags))
        {
            StringBuilder sb = new StringBuilder();
            IDataReader reader = null;
            using (DbManager db = DbManager.GetDbManager())
            {
                try
                {
                    reader = db.GetDataReader("up_filter_getBadTags");
                    while (reader.Read())
                    {
                        string s = reader["vchTag"].ToString();

                        sb.Append("<");
                        sb.Append(s);
                        sb.Append("|");
                    }
                    badtags = sb.ToString().TrimEnd('|');
                    HttpRuntime.Cache.Insert(cacheName, badtags, null, DateTime.Now.AddMinutes(CACHEDURATION), Cache.NoSlidingExpiration);
                }
                catch (Exception ex) { }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
        }
        return badtags;
    }
}
