<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HttpErrorPage.aspx.cs"
    Inherits="SRFROWCA.ErrorPages.HttpErrorPage" %>

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
        <asp:Panel ID="InnerErrorPanel" runat="server" Visible="false">
            <asp:Label ID="innerMessage" runat="server" Font-Bold="true" Font-Size="Large" /><br />
            <pre>
        <asp:Label ID="innerTrace" runat="server" />
      </pre>
        </asp:Panel>
        Error Message:<br />
        <asp:Label ID="exMessage" runat="server" Font-Bold="true" Font-Size="Large" />
        <pre>
      <asp:Label ID="exTrace" runat="server" Visible="false" />
    </pre>
        <br />
        Return to the <a href='~/Default.aspx'>Default Page</a>
    </div>
    </form>
</body>
</html>
