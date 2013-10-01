<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="AllData.aspx.cs" Inherits="SRFROWCA.Admin.Reports.AllData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#<%=gvReport.ClientID %>").prepend('<colgroup><col /><col /><col /><col /><col /><col /><col /><col /><col /></colgroup>');
            $("#<%=gvReport.ClientID %>").kiketable_colsizable()
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <td>
                <asp:Button ID="btnExportToExcel" runat="server" Text="Export To Excel" OnClick="btnExportToExcel_Click" />
            </td>
        </tr>
    </table>
    <div style="overflow-x: auto; width: 100%">
        <asp:GridView ID="gvReport" runat="server" Width="100%" CssClass="gvr" OnRowDataBound="gvReport_RowDataBound">
            <HeaderStyle BackColor="ButtonFace" />
        </asp:GridView>
    </div>
</asp:Content>
