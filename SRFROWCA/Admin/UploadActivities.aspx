<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
	CodeBehind="UploadActivities.aspx.cs" Inherits="SRFROWCA.Admin.UploadActivities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<table>
		<tr>
			<td>
				<asp:Label ID="lblMessage" runat="server" CssClass="error-message" Visible="false"
					ViewStateMode="Disabled"></asp:Label>
			</td>
		</tr>
		<tr>
			<td>
                <br />
                <br />
				<b>Please select excel file and click on import. It might take a while to complete.
					Excel Sheet Name should be 'Formatted Data' and Column order and name should be
					(without hyphen) 'RowNumber' 'Cluster' 'sobjectives' 'objectives' 'Indicators' 'Activities'
					'Data' 'Unit' </b>
                    <br />
			</td>
		</tr>
		<tr>
			<td>
				<asp:DropDownList ID="ddlEmergency" runat="server" Width="300px">
				</asp:DropDownList>
				<asp:RequiredFieldValidator ID="rfvEmergency" runat="server" ErrorMessage="Select Emergency"
					InitialValue="0" Text="Required" ForeColor="Red" ControlToValidate="ddlEmergency"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td>
				<asp:FileUpload ID="fuExcel" runat="server" />
				<asp:Button ID="btnImport" runat="server" Text="Import" OnClick="btnImport_Click" />
			</td>
		</tr>
	</table>
</asp:Content>
