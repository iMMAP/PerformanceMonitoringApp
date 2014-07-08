<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ActivityListing.aspx.cs" Inherits="SRFROWCA.Admin.ActivityListing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
              
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Activities</li>
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
                                      
                                          <asp:Button ID="btnAddActivity" runat="server" Text="Add New Activity" CausesValidation="false"
                 CssClass="btn btn-yellow pull-right"  OnClick="btnAddActivity_Click" />
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
                                                                    Activity Name:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                <asp:TextBox ID="txtActivityName" runat="server" CssClass="width-80"></asp:TextBox>
                                                            </td>
                                                            <td class="width-20">
                                                                <label>
                                                                    Cluster:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                 <asp:DropDownList ID="ddlCluster" runat="server" AppendDataBoundItems="true"  CssClass="width-80">
                                                                    <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>
                                                                    
                                                                </asp:DropDownList>
                                                            </td>
                                                             
                                                            
                                                        </tr>
                                                        <tr>
                                                            <td class="width-20">
                                                                <label>
                                                                    Objective:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                 <asp:DropDownList ID="ddlObjective" runat="server" AppendDataBoundItems="true"  CssClass="width-80">
                                                                    <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>
                                                                    
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="width-20">
                                                                <label>
                                                                    Priority:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                 <asp:DropDownList ID="ddlPriority" runat="server" AppendDataBoundItems="true"  CssClass="width-80">
                                                                    <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>
                                                                    
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td colspan="2">
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
           <asp:GridView ID="gvActivity" runat="server" AutoGenerateColumns="false" AllowSorting="True" AllowPaging="true" PagerSettings-Mode="NumericFirstLast"
                OnRowCommand="gvActivity_RowCommand" Width="100%" OnRowDataBound="gvActivity_RowDataBound" PagerSettings-Position="Bottom" DataKeyNames="ClusterId,ClusterObjectiveId,ObjectivePriorityId,ActivityTypeId,HumanitarianPriority,SiteLanguageId,ActivityName"  
                CssClass="imagetable" OnSorting="gvActivity_Sorting" OnPageIndexChanging="gvActivity_PageIndexChanging" PageSize="30" OnRowDeleting="gvActivity_RowDeleting">
                <RowStyle CssClass="istrow" />
                <AlternatingRowStyle CssClass="altcolor" />
                <Columns>
                  <asp:BoundField DataField="ClusterName" HeaderText="Cluster" SortExpression="ClusterName" />
                    <asp:BoundField DataField="Objective" HeaderText="Objective" SortExpression="Objective"/>
                    <asp:BoundField DataField="HumanitarianPriority" HeaderText="Priority" SortExpression="HumanitarianPriority"/>
                    <asp:BoundField DataField="ActivityName" HeaderText="Activity" SortExpression="ActivityName"/>
                     <asp:BoundField DataField="ActivityType" HeaderText="Activity Type" SortExpression="ActivityType"/>
                    <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" Text="Edit" Width="80px" CausesValidation="false"
                                CommandName="EditActivity" CommandArgument='<%# Eval("PriorityActivityId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" Width="80px" CausesValidation="false"
                                CommandName="Delete" CommandArgument='<%# Eval("PriorityActivityId") %>' />
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
                                    Add/Edit Activity
                                </div>
                                <div class="contentarea">
                                    <div class="formdiv">
                                        <table border="0" style="margin: 0 auto;">
                                            <tr>
                                                <td>
                                                    Cluster:
                                                </td>
                                                <td class="frmControl">
                                                     <asp:DropDownList ID="ddlClusterNew" runat="server" AppendDataBoundItems="true" AutoPostBack="true"  Width="300px" OnSelectedIndexChanged="ddlClusterNew_SelectedIndexChanged">
                                                                    <asp:ListItem Text="Select" Value="-1" Selected="True"></asp:ListItem>
                                                         </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
                                                        Text="Required" ControlToValidate="ddlClusterNew" CssClass="error2" InitialValue="-1"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td>
                                                    Objective:
                                                </td>
                                                <td class="frmControl">
                                                     <asp:DropDownList ID="ddlObjectiveNew" runat="server" AutoPostBack="true" AppendDataBoundItems="true"  Width="300px" OnSelectedIndexChanged="ddlObjectiveNew_SelectedIndexChanged">
                                                                    <asp:ListItem Text="Select" Value="-1" Selected="True"></asp:ListItem>
                                                         </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required"
                                                        Text="Required" ControlToValidate="ddlObjectiveNew" CssClass="error2" InitialValue="-1"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                              <tr>
                                                <td>
                                                    Priority:
                                                </td>
                                                <td class="frmControl">
                                                     <asp:DropDownList ID="ddlPriorityNew" runat="server" AppendDataBoundItems="true"  Width="300px">
                                                                    <asp:ListItem Text="Select" Value="-1" Selected="True"></asp:ListItem>
                                                         </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Required"
                                                        Text="Required" ControlToValidate="ddlPriorityNew" CssClass="error2" InitialValue="-1"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trEnglish">
                                                <td>
                                                    Activity Name (English):
                                                </td>
                                                <td class="frmControl">
                                                    <asp:TextBox ID="txtActivityEng" runat="server" Width="300px" MaxLength="200"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="rfvEmgName" runat="server" ErrorMessage="Required"
                                                        Text="Required" ControlToValidate="txtActivityEng" CssClass="error2"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trFrench">
                                                <td>
                                                    Activity Name (French):
                                                </td>
                                                <td class="frmControl">
                                                    <asp:TextBox ID="txtActivityFr" runat="server" Width="300px" MaxLength="200"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="rfvEmgNameFr" runat="server" ErrorMessage="Required"
                                                        Text="Required" ControlToValidate="txtActivityFr" CssClass="error2"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Activity Type:
                                                </td>
                                                <td class="frmControl">
                                                    <asp:DropDownList ID="ddlActivityType" runat="server" Width="300px">
                                                        <asp:ListItem Text="Select" Value="-1" Selected="True"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="rgvEmgType" runat="server" ErrorMessage="Required"
                                                        InitialValue="-1" Text="Required" ControlToValidate="ddlActivityType" CssClass="error2"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td  align="left" class="frmControl">
                                                    <br />
                                                    <asp:HiddenField ID="hdnPriorityActivityId" runat="server" />
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
                            <asp:AsyncPostBackTrigger ControlID="ddlClusterNew" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ddlObjectiveNew" EventName="SelectedIndexChanged" />
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
