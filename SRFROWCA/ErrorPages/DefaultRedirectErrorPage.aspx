<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DefaultRedirectErrorPage.aspx.cs"
    Inherits="SRFROWCA.ErrorPages.DefaultRedirectErrorPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Error Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2>
            Error Page</h2>
        Standard error message suitable for all unhandled errors. The original exception
        object is not available.<br />
        <br />
        Return to the <a href='~/Default.aspx'>Default Page</a>
    </div>
    </form>
</body>
</html>
