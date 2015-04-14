<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddKeyFigureSubCategoryCtrl.ascx.cs" Inherits="SRFROWCA.KeyFigures.AddKeyFigureSubCategoryCtrl" %>
<table>    
    <tr>
        <td>English:</td>
        <td><asp:TextBox ID="txtKeyFigEng"  TextMode="MultiLine" runat="server" Width="300px"></asp:TextBox></td>
        <td>French:</td>
        <td><asp:TextBox ID="txtKeyFigFr"  TextMode="MultiLine" runat="server" Width="300px"></asp:TextBox></td>
        <td><asp:CheckBox ID="cbIsPopulation" runat="server" Text="Is Population" /></td>
    </tr>
</table>
<hr />
