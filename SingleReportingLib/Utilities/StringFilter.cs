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
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Caching;

/// <summary>
/// Summary description for StringFilter
/// </summary>
public class StringFilter
{
    protected string[] _allowedTags;
    protected string[] _allowedAttr;
    protected string[] _restrictedTags;
    protected string[] _restrictedAttr;
    protected string[] _restrictedWords;

    protected struct TokenNode
    {
        public int nType;
        public string strToken;
    };

    protected struct TagNode
    {
        public int nElementType;
        public bool bDirty;     // Determines whether a tag is dirty
        public List<TokenNode> listToken;
    };

    protected enum TokenType
    {
        SF_STR = 0  // 0- String
        ,
        SF_DQ     // 1- Double Quote
      ,
        SF_SQ     // 2- Single Quote
      ,
        SF_SLSH   // 3- Slash (Currently not used since a '/' on its own is useless, hence '/' == SF_STR)
      ,
        SF_END    // 4- End Tag (/>)
      ,
        SF_LT     // 5- Less Than
      ,
        SF_GT     // 6- Greater Than
      ,
        SF_STRT   // 7- Start Tag (</)
      ,
        SF_EQ     // 8- Equals
      ,
        SF_SPC    // 9- Space
      , SF_ETC    // 10- Others (Currently not used primarily because...see SF_SLSH)
    };

    // A tag element is basically what a tag consist
    protected enum TagElement
    {
        TAG_STR = 0   // 0- String
        ,
        TAG_NAME    // 1- Tagname
      ,
        TAG_ELMT    // 2- Element
      ,
        TAG_VALU    // 3- Value
      ,
        TAG_OPEN    // 4- An open tag <a>
      ,
        TAG_UNOPEN  // 5- An unfinished open tag <a
      ,
        TAG_CLOSE   // 6- A close tag </a>
      , TAG_UNCLOSE // 7- An unfinished close tag </a
    };

    // A tag node type is what have is stored inside the Taglist
    // If it gets too hairy, and more explanation is needed ask Gilbert
    protected enum TagNodeType
    {
        TAG_STRING = 0  // 0- A string, an unfinished tag is not considered a string and is treated below
        ,
        TAG_OPEN      // 1- An open tag <a>
      ,
        TAG_UNOPEN    // 2- An unfinished open tag <a
      ,
        TAG_CLOSE     // 3- A close tag </a>
      , TAG_UNCLOSE   // 4- An unfinished close tag </a
    }

    private List<TokenNode> _listTokens = new List<TokenNode>();
    private List<TagNode> _listTags = new List<TagNode>();

    public StringFilter()
    {
        //
        // TODO: Add constructor logic here
        //
        Initialize();
    }

    /// <summary>
    /// Reads strBuffer and censors all restricted words, if a restricted tag or attribute
    /// is found then everything up to that point will be removed.
    /// For instance "This is a <script> script</script>" will be filtered to "This is a"
    /// </summary>
    /// <param name="strBuffer"></param>
    /// <param name="bCensor">Set to true to censor dirty words</param>
    /// <returns></returns>
    public StringBuilder FilterString(string strBuffer, bool bCensor)
    {
        string strDirty = "";

        Tokenize(strBuffer, out strDirty, bCensor, false);
        return ExportTokens();
    }

    /// <summary>
    /// Similar to FilterString instead it returns the dirty toen when the commment is dirty, 
    /// and empty string if its clean.
    /// A comment with a restricted word is not considered dirty.
    /// </summary>
    /// <param name="strBuffer">The dirty buffer to be cleaned</param>
    /// <param name="bCensor">Set to true to censor dirty words</param>
    /// <param name="strFiltered">Out param of the cleaned comment</param>
    /// <returns>See summary</returns>
    public string BlackFilter(string strBuffer, bool bCensor, out string strFiltered)
    {
        string strDirty = "";

        Tokenize(strBuffer, out strDirty, bCensor, false);

        strFiltered = ExportTokens().ToString();

        return strDirty;
    }

    /// <summary>
    /// Runs a basic blacklist filter similar to BlackFilter plus an additional WhiteFilter check.
    /// The white filter check queries the database to see whether it is part of the allowed keywords
    /// or not.  If it is not then it is removed using the definitioned aforementioned above. If it
    /// is, then it is allowed.
    /// </summary>
    /// <param name="strBuffer"></param>
    /// <param name="bCensor">Set to true to censor dirty words</param>
    /// <param name="strFiltered"></param>
    /// <returns></returns>
    public bool WhiteFilter(string strBuffer, bool bCensor, out string strFiltered)
    {
        bool bDirty = false;
        string strDirty = "";

        bDirty = Tokenize(strBuffer, out strDirty, bCensor, true);

        strFiltered = ExportTokens().ToString();

        return bDirty;
    }

    private void Initialize()
    {
        InitWordFilter();
        InitRestrictedTags();
        InitRestrictedAttr();
        InitAllowedTags();
        InitAllowedAttr();
    }

    private void InitWordFilter()
    {
        InitializeTokens("up_word_getFilter", "dbo.up_word_getFilter", "word", ref _restrictedWords);
    }

    private void InitRestrictedTags()
    {
        InitializeTokens("up_tag_getRestricted", "dbo.up_tag_getRestricted", "tag", ref _restrictedTags);
    }

    private void InitRestrictedAttr()
    {
        InitializeTokens("up_attribute_getRestricted", "dbo.up_attribute_getRestricted", "attribute", ref _restrictedAttr);
    }

    private void InitAllowedTags()
    {
        InitializeTokens("up_tag_getAllowed", "dbo.up_tag_getAllowed", "tag", ref _allowedTags);
    }

    private void InitAllowedAttr()
    {
        InitializeTokens("up_attribute_getAllowed", "dbo.up_attribute_getAllowed", "attribute", ref _allowedAttr);
    }

    private void InitializeTokens(string strCacheName, string strQuery, string strColName, ref string[] arrayToPopulate)
    {
        arrayToPopulate = (string[])HttpContext.Current.Cache.Get(strCacheName);

        if (arrayToPopulate == null)
        {
            using (SqlCommand cmd = new SqlCommand(strQuery, new SqlConnection(ConfigurationManager.ConnectionStrings["power"].ConnectionString)))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Connection.Open();
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    sqlAdapter.Fill(ds);

                    arrayToPopulate = new string[ds.Tables[0].Rows.Count];
                    int nCount = 0;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        arrayToPopulate[nCount] = dr[strColName].ToString();
                        nCount++;
                    }

                    // Add to cache
                    HttpContext.Current.Cache.Insert(strCacheName, arrayToPopulate, null, DateTime.Now.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["CacheDuration"])), Cache.NoSlidingExpiration);
                }
                finally
                {
                    if (cmd.Connection.State == ConnectionState.Open)
                        cmd.Connection.Close();
                }
            }
        }
    }

    private void ClearTokens()
    {
        _listTokens.Clear();
        _listTags.Clear();
    }

    protected bool Tokenize(string strBuffer, out string strDirtyWord, bool bCensor, bool bWhiteList)
    {
        int nState = 0;
        int nAction = 0;
        int nNextState = 0;
        int nType = 0;
        bool bPossibleTag = false;
        bool bProcessed = false;
        bool bDirty = false;
        string strToken = "";
        List<TokenNode> listRawTagTokens = new List<TokenNode>();

        ClearTokens();

        strDirtyWord = "";
        strBuffer += ' '; // Since the scanner has a look ahead buffer, an additional char is added.
        // The addition of this character will not be processed
        foreach (char cNext in strBuffer)
        {
            bProcessed = false;
            // Check and see if a < token is detected
            if (cNext == '<')
            {
                // If so, flag it so that the comment could be processed
                bPossibleTag = true;
            }

            do
            {
                nNextState = GetNextState(nState, cNext);
                nAction = GetAction(nState, cNext);
                nType = GetType(nState, cNext);
                nState = nNextState;

                if (nNextState != 0)
                {
                    strToken += cNext;
                    bProcessed = true;
                }

                if (nAction == 0)
                {
                    if (bCensor)
                    {
                        strToken = FilterWord(strToken).ToString();
                    }
                    TokenNode nodeToken = new TokenNode();
                    nodeToken.strToken = strToken;
                    nodeToken.nType = nType;

                    if (_listTokens == null)
                    {
                        _listTokens = new List<TokenNode>();
                    }
                    _listTokens.Add(nodeToken);
                    nState = 0;
                    strToken = "";
                }
            }
            while (!bProcessed);
        }

        if (bPossibleTag)
        {
            bDirty = ProcessTags(bWhiteList, out strDirtyWord);
        }

        return bDirty;
    }

    private bool ProcessTags(bool bWhiteList, out string strDirtyWord)
    {
        int nState = 0;
        int nAction = 0;
        int nNextState = 0;
        int nType = 0;
        bool bIllegal = false;
        bool bProcessed = false;
        bool bDirty = false;
        string strToken = "";

        strDirtyWord = "";

        List<TokenNode> listTokens = new List<TokenNode>();

        foreach (TokenNode nodeToken in _listTokens)
        {
            strToken = nodeToken.strToken;
            bProcessed = false;
            do
            {
                nNextState = GetNextTagState(nState, nodeToken.nType);
                nAction = GetTagAction(nState, nodeToken.nType);
                nType = GetTagType(nState, nodeToken.nType);

                if (nNextState != 0)
                {
                    bDirty = false;
                    switch (nType)
                    {
                        case 1: // Tag Name
                            bDirty = CheckTagBlackList(nodeToken.strToken);
                            // Check if it is in the white list if the tag is not restricted
                            // AND if the user wants it to be within the allowed tags
                            if (bWhiteList && !bDirty)
                            {
                                // Make sure that the tag is allowed
                                bDirty = !CheckTagWhiteList(nodeToken.strToken);
                            }
                            break;
                        case 2: // Tag Attribute
                            bDirty = CheckAttrBlackList(strToken);
                            // Check if it is in the white list if the tag is not restricted
                            // AND if the user wants it to be within the allowed tags
                            if (bWhiteList && !bDirty)
                            {
                                // Make sure that the tag is allowed
                                bDirty = !CheckAttrWhiteList(strToken);
                            }
                            break;
                    }

                    if (bDirty)
                    {
                        strDirtyWord = strToken;
                        bIllegal = true;
                        break;
                    }
                    nState = nNextState;

                    TokenNode nodeNew = new TokenNode();
                    nodeNew.strToken = strToken;
                    nodeNew.nType = nType;

                    listTokens.Add(nodeNew);
                    bProcessed = true;
                }

                if (nAction == 0)
                {
                    TagNode nodeTag = new TagNode();
                    nodeTag.listToken = listTokens;
                    nodeTag.nElementType = nType;
                    nodeTag.bDirty = bIllegal;

                    if (_listTags == null)
                    {
                        _listTags = new List<TagNode>();
                    }
                    _listTags.Add(nodeTag);

                    nState = 0;
                    listTokens = new List<TokenNode>();
                    bIllegal = false;
                }
            }
            while (!bProcessed && !bIllegal);

            if (bIllegal)
            {
                break;
            }
        }

        if (listTokens.Count > 0)
        {
            TagNode nodeTag = new TagNode();
            nodeTag.listToken = listTokens;
            nodeTag.nElementType = nType;
            nodeTag.bDirty = bIllegal;

            if (_listTags == null)
            {
                _listTags = new List<TagNode>();
            }
            _listTags.Add(nodeTag);

        }

        ExportTagList();
        return bIllegal;
    }

    private void ExportTagList()
    {
        string strDebug = "";
        _listTokens.Clear();

        foreach (TagNode nodeTag in _listTags)
        {
            if (!nodeTag.bDirty)
            {
                foreach (TokenNode nodeToken in nodeTag.listToken)
                {
                    strDebug = nodeToken.strToken;
                    TokenNode nodeNew = new TokenNode();
                    nodeNew = nodeToken;
                    _listTokens.Add(nodeNew);
                }
            }
            else
            {
                break;
            }
        }
    }

    private bool CheckAttrBlackList(string strText)
    {
        bool bInvalid = false;
        foreach (string strAttr in _restrictedAttr)
        {
            if (0 == strAttr.CompareTo(strText.ToLower()))
            {
                bInvalid = true;
                break;
            }
        }
        return bInvalid;
    }

    private bool CheckTagBlackList(string strText)
    {
        bool bInvalidTag = false;
        foreach (string strTag in _restrictedTags)
        {
            if (0 == strTag.CompareTo(strText.ToLower()))
            {
                bInvalidTag = true;
                break;
            }
        }
        return bInvalidTag;
    }

    private bool CheckAttrWhiteList(string strText)
    {
        bool bValid = false;
        foreach (string strAttr in _allowedAttr)
        {
            if (0 == strAttr.CompareTo(strText.ToLower()))
            {
                bValid = true;
                break;
            }
        }
        return bValid;
    }

    private bool CheckTagWhiteList(string strText)
    {
        bool bValid = false;
        foreach (string strTag in _allowedTags)
        {
            if (0 == strTag.CompareTo(strText.ToLower()))
            {
                bValid = true;
                break;
            }
        }
        return bValid;
    }

    private StringBuilder FilterWord(string strText)
    {
        bool bClean = true;
        StringBuilder sbClean = new StringBuilder();

        foreach (string strCurr in _restrictedWords)
        {
            if (0 == strCurr.CompareTo(strText.ToLower()))
            {
                bClean = false;
                break;
            }
        }

        if (!bClean)
        {
            foreach (char cIndex in strText)
            {
                sbClean.Append('*');
            }
        }
        else
        {
            sbClean.Append(strText);
        }

        return sbClean;
    }



    private StringBuilder ExportTokens()
    {
        StringBuilder sbTokens = new StringBuilder();

        if (_listTokens != null)
        {
            foreach (TokenNode node in _listTokens)
            {
                sbTokens.Append(node.strToken);
            }
        }

        return sbTokens;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cCurr"></param>
    /// <param name="cNext"></param>
    /// <returns>Returns 0 for an accept state, -1 on an error.</returns>
    protected int GetNextState(int nState, char cCurr)
    {
        int nCurr = GetCharPos(cCurr);

        int[,] anLookup = new int[,]
        //     L, D, ", ', /, <, >, =,  , Other  
            {{ 1, 2, 3, 4, 5, 6, 7, 8, 9,10 },   // S0
             { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   // S1
             { 0, 2, 0, 0, 0, 0, 0, 0, 0, 0 },   // S2
             { 0, 0, 3, 0, 0, 0, 0, 0, 0, 0 },   // S3
             { 0, 0, 0, 4, 0, 0, 0, 0, 0, 0 },   // S4
             { 0, 0, 0, 0, 5, 0,11, 0, 0, 0 },   // S5
             { 0, 0, 0, 0,12, 6, 0, 0, 0, 0 },   // S6
             { 0, 0, 0, 0, 0, 0, 7, 0, 0, 0 },   // S7
             { 0, 0, 0, 0, 0, 0, 0, 8, 0, 0 },   // S8
             { 0, 0, 0, 0, 0, 0, 0, 0, 9, 0 },   // S9
             { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   // S10
             { 0, 0, 0, 0, 0, 0,11, 0, 0, 0 },   // S11
             { 0, 0, 0, 0,12, 0, 0, 0, 0, 0 },   // S12
            };

        return anLookup[nState, nCurr];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nState"></param>
    /// <param name="cCurr"></param>
    /// <returns>1 is a Move Append and 0 is an Accept State</returns>
    protected int GetAction(int nState, char cCurr)
    {
        int nCurr = GetCharPos(cCurr);

        int[,] anLookup = new int[,]
        //     L, D, ", ', /, <, >, =,  , Other  
            {{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },   // S0
             { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   // S1
             { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },   // S2
             { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },   // S3
             { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },   // S4
             { 0, 0, 0, 0, 1, 0, 1, 0, 0, 0 },   // S5
             { 0, 0, 0, 0, 1, 1, 0, 0, 0, 0 },   // S6
             { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },   // S7
             { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },   // S8
             { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },   // S9
             { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },   // S10
             { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },   // S11
             { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },   // S12
            };

        return anLookup[nState, nCurr];
    }

    // Returns the accept state of the token type
    protected int GetType(int nState, char cCurr)
    {
        int nCurr = GetCharPos(cCurr);

        int[,] anLookup = new int[,]
        //     L, D, ", ', /, >, <, =,  , Other  
            {{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   // S0
             { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   // S1
             { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   // S2
             { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },   // S3
             { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },   // S4
             { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   // S5
             { 5, 5, 5, 5, 5, 5, 5, 5, 5, 5 },   // S6
             { 6, 6, 6, 6, 6, 6, 6, 6, 6, 6 },   // S7
             { 8, 8, 8, 8, 8, 8, 8, 8, 8, 8 },   // S8
             { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 },   // S9
             { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   // S10
             { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 },   // S11
             { 7, 7, 7, 7, 7, 7, 7, 7, 7, 7 },   // S12
            };

        return anLookup[nState, nCurr];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cCurr"></param>
    /// <param name="cNext"></param>
    /// <returns>Returns 0 for an accept state, -1 on an error.</returns>
    protected int GetNextTagState(int nState, int nTokenType)
    {
        // Legend:
        //  $ == String
        //  _ == space
        // States within 1 through 13 is constructing a start tag
        // States within 14 through 17 is constructing an end tag
        // States 18 are for everything else

        int[,] anLookup = new int[,]
        //     $, ", ', /,/>, <, >,</, =, _  
            {{18,18,18,18,18, 1,18,14,18,18 },   // S0
             { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   // S1
             { 0, 2, 0, 0, 4, 0, 5, 0, 0, 3 },   // S2
             { 6, 0, 0, 0, 0, 0, 0, 0, 0, 2 },   // S3
             { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   // S4
             { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   // S5
             { 0, 0, 0, 0, 0, 0, 0, 0, 8, 7 },   // S6
             { 0, 0, 0, 0, 0, 0, 0, 0, 8, 0 },   // S7
             { 0,11, 9, 0, 0, 0, 0, 0, 0,10 },   // S8
             {12,12, 2,12,12,12,12,12,12,12 },   // S9
             { 0,11, 9, 0, 0, 0, 0, 0, 0,10 },   // S10
             {13, 2,13,13,13,13,13,13,13,13 },   // S11
             {12,12, 2,12,12,12,12,12,12,12 },   // S12
             {13, 2,13,13,13,13,13,13,13,13 },   // S13
             {15, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   // S14
             { 0, 0, 0, 0, 0, 0,17, 0, 0,16 },   // S15
             { 0, 0, 0, 0, 0, 0,18, 0, 0,16 },   // S16
             { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   // S17
             { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   // S18
            };


        return anLookup[nState, nTokenType];
    }

    protected int GetTagAction(int nState, int nTokenType)
    {
        // Legend:
        //  $ == String
        //  _ == space
        // States within 1 through 13 is constructing a start tag
        // States within 14 through 17 is constructing an end tag
        // States 18 are for everything else
        int[,] anLookup = new int[,]
        //     $, ", ', /,/>, <, >,</, =, _  
            {{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },   // S0
             { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   // S1
             { 0, 1, 0, 0, 1, 0, 1, 0, 0, 1 },   // S2
             { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },   // S3
             { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   // S4
             { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   // S5
             { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 },   // S6
             { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },   // S7
             { 0, 1, 1, 0, 0, 0, 0, 0, 0, 1 },   // S8
             { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },   // S9
             { 0, 1, 1, 0, 0, 0, 0, 0, 0, 1 },   // S10
             { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },   // S11
             { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },   // S12
             { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },   // S13
             { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   // S14
             { 0, 0, 0, 0, 0, 0, 1, 0, 0, 1 },   // S15
             { 0, 0, 0, 0, 0, 0, 1, 0, 0, 1 },   // S16
             { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   // S17
             { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   // S18
            };

        return anLookup[nState, nTokenType];
    }

    // Returns the accept state of the tag type
    protected int GetTagType(int nState, int nTokenType)
    {
        // Legend:
        //  $ == String
        //  _ == space

        // 0- String
        // 1- Tagname
        // 2- Element
        // 3- Value
        // 4- An open tag <a>
        // 5- An unfinished open tag <a
        // 6- A close tag </a>
        // 7- An unfinished close tag </a

        int[,] anLookup = new int[,]
        //     $, ", ', /,/>, <, >,</, =, _  
            {{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   // S0
             { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   // S1
             { 0, 0, 0, 0, 5, 0, 4, 0, 0, 0 },   // S2
             { 2, 5, 5, 5, 5, 5, 5, 5, 5, 0 },   // S3
             { 4, 4, 4, 4, 4, 4, 4, 4, 4, 0 },   // S4
             { 4, 4, 4, 4, 4, 4, 4, 4, 4, 0 },   // S5
             { 2, 5, 5, 5, 5, 5, 5, 5, 5, 0 },   // S6
             { 5, 5, 5, 5, 5, 5, 5, 5, 5, 0 },   // S7
             { 5, 5, 5, 5, 5, 5, 5, 5, 5, 0 },   // S8
             { 5, 5, 5, 5, 5, 5, 5, 5, 5, 0 },   // S9
             { 5, 5, 5, 5, 5, 5, 5, 5, 5, 0 },   // S10
             { 5, 5, 5, 5, 5, 5, 5, 5, 5, 0 },   // S11
             { 5, 5, 3, 5, 5, 5, 5, 5, 5, 0 },   // S12
             { 5, 3, 5, 5, 5, 5, 5, 5, 5, 0 },   // S13
             { 1, 7, 7, 7, 7, 7, 7, 7, 7, 0 },   // S14
             { 7, 7, 7, 7, 7, 7, 7, 7, 7, 0 },   // S15
             { 6, 6, 6, 6, 6, 6, 6, 6, 6, 0 },   // S16
             { 6, 6, 6, 6, 6, 6, 6, 6, 6, 0 },   // S17
             { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   // S18
            };

        return anLookup[nState, nTokenType];
    }

    protected int GetCharPos(char cBuffer)
    {
        int nPos = 0;

        if (Utils.IsAlpha(cBuffer))
        {
            nPos = 0;
        }
        else if (Utils.IsNumeric(cBuffer))
        {
            nPos = 1;
        }
        else
        {
            switch (cBuffer)
            {
                case '"':
                    nPos = 2;
                    break;
                case '\'':
                    nPos = 3;
                    break;
                case '/':
                    nPos = 4;
                    break;
                case '<':
                    nPos = 5;
                    break;
                case '>':
                    nPos = 6;
                    break;
                case '=':
                    nPos = 7;
                    break;
                case ' ':
                    nPos = 8;
                    break;
                default:
                    nPos = 9;
                    break;
            }
        }
        return nPos;
    }
}
