<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EmergencyListing.aspx.cs" Inherits="SRFROWCA.Admin.EmergencyListing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">       
        
        $(function () {
            $('#txtSearch').on("keyup paste", function () {
                searchTable($(this).val());
            });
        });

        function searchTable(inputVal) {
            var table = $('#<%=gvEmergency.ClientID %>');
            table.find('tr').each(function (index, row) {
                var allCells = $(row).find('td');
                if (allCells.length > 0) {
                    var found = false;
                    allCells.each(function (index, td) {
                        var regExp = new RegExp(inputVal, 'i');
                        if (regExp.test($(td).text())) {
                            found = true;
                            return false;
                        }
                    });
                    if (found == true) {
                        $(row).show('fast');
                    }
                    else {
                        $(row).hide('fast');
                    }
                }
            });
        }          
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Emergencies</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
     <div class="page-content">
    <table border="0" cellpadding="2" cellspacing="0" class="pstyle1" width="100%">
        <tr>
            <td class="signupheading2" colspan="3">
                <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="divMsg">
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
          <table width="100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h6>
                                       <button runat="server" id="btnExportToExcel" onserverclick="btnExportExcel_Click" class="btn btn-yellow"  CausesValidation="false"
                                            title="Excel">
                                            <i class="icon-download"></i>Excel
                                        </button>
                                      
                                          <asp:Button ID="btnAddEmergency" runat="server" Text="Add New Emergency" CausesValidation="false"
                 CssClass="btn btn-yellow pull-right"  OnClick="btnAddEmergency_Click" />
                                    </h6>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row">
                                                    <table border="0" width="100%">
                                                        <tr>
                                                            <td class="width-20">
                                                                <label>
                                                                    Emergency Name:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                <asp:TextBox ID="txtOrganizationName" runat="server" CssClass="width-80"></asp:TextBox>
                                                            </td>
                                                            <td class="width-20">
                                                                <label>
                                                                    Disaster Type:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                 <asp:DropDownList ID="ddlDisasterType" runat="server" AppendDataBoundItems="true"  CssClass="width-80">
                                                                    <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>
                                                                    
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                              <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch2_Click" CssClass="btn btn-primary" CausesValidation="false" />
                                                            </td>
                                                        </tr>
                                                   
                                                        
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>



                    </div>
                </td>
            </tr>
        </table>
  
    <div class="tablegrid">
        <div style="overflow-x: auto; width: 100%">
            <asp:GridView ID="gvEmergency" runat="server" AutoGenerateColumns="false" AllowSorting="True"
                OnRowCommand="gvEmergency_RowCommand" Width="100%" OnRowDataBound="gvEmergency_RowDataBound" DataKeyNames="SiteLanguageId"
                CssClass="imagetable" OnSorting="gvEmergency_Sorting">
                <RowStyle CssClass="istrow" />
                <AlternatingRowStyle CssClass="altcolor" />
                <Columns>
                    <asp:TemplateField ItemStyle-CssClass="rownum" ItemStyle-Width="2%" HeaderText="#">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="EmergencyId" HeaderText="Emergency Id" SortExpression="EmergencyId" />                    
                
                    <asp:BoundField DataField="EmergencyName" HeaderText="Emergency Name" SortExpression="EmergencyName" />
                    <asp:BoundField DataField="EmergencyType" HeaderText="Disaster Type" SortExpression="EmergencyType" /> 
                        <asp:BoundField DataField="SiteLanguageId" HeaderText="Emergency Id" SortExpression="SiteLanguageId" Visible="false" />                   
                    <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" Text="Edit" Width="80px" CausesValidation="false"
                                CommandName="EditEmergency" CommandArgument='<%# Eval("EmergencyId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" Width="80px" CausesValidation="false"
                                CommandName="DeleteEmergency" CommandArgument='<%# Eval("EmergencyId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblEmergencyTypeId" runat="server" Text='<%# Eval("EmergencyTypeId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle BackColor="ButtonFace" />
            </asp:GridView>
        </div>
    </div>
    <table>
        <tr>
            <td>
                <asp:ModalPopupExtender ID="mpeAddOrg" BehaviorID="mpeAddOrg" runat="server" TargetControlID="btntest"
                    PopupControlID="pnlOrg" BackgroundCssClass="modalpopupbackground" CancelControlID="btnClose">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlOrg" runat="server" Width="650px">
                    <asp:UpdatePanel ID="uPanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="containerPopup">
                                <div class="popupheading">
                                    Add/Edit Emergency
                                </div>
                                <div class="contentarea">
                                    <div class="formdiv">
                                        <table border="0" style="margin: 0 auto;">
                                            <tr>
                                                <td>
                                                    Emergency Name (English):
                                                </td>
                                                <td class="frmControl">
                                                    <asp:TextBox ID="txtEmgNameEng" runat="server" Width="300px" MaxLength="200"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="rfvEmgName" runat="server" ErrorMessage="Emg Name (Eng)"
                                                        Text="Required" ControlToValidate="txtEmgNameEng" CssClass="error2"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Emergency Name (French):
                                                </td>
                                                <td class="frmControl">
                                                    <asp:TextBox ID="txtEmgNameFr" runat="server" Width="300px" MaxLength="200"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="rfvEmgNameFr" runat="server" ErrorMessage="Emg Name (Fr)"
                                                        Text="Required" ControlToValidate="txtEmgNameFr" CssClass="error2"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Emergency Type:
                                                </td>
                                                <td class="frmControl">
                                                    <asp:DropDownList ID="ddlEmgType" runat="server" Width="300px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="rgvEmgType" runat="server" ErrorMessage="Required"
                                                        InitialValue="0" Text="Required" ControlToValidate="ddlEmgType" CssClass="error2"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td  align="left" class="frmControl">
                                                    <br />
                                                    <asp:HiddenField ID="hfLocEmgId" runat="server" />
                                                    <asp:Button ID="btnAdd" runat="server" Text="Add/Update" OnClick="btnAdd_Click" CssClass="button_example" />
                                                    <asp:Button ID="btnClose" runat="server" Text="Close" CausesValidation="false" CssClass="button_example" />
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblMessage2" runat="server" CssClass="error-message" Visible="false"
                                                        ViewStateMode="Disabled"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="spacer" style="clear: both;">
                                        </div>
                                    </div>
                                </div>
                                <div class="graybarcontainer">
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnAdd" />
                            <asp:PostBackTrigger ControlID="btnClose" />
                        </Triggers>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <div style="display: none">
        <asp:Button ID="btntest" runat="server" Width="1px" />
    </div>
         </div>
</asp:Content>
