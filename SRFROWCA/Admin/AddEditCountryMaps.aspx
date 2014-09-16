<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddEditCountryMaps.aspx.cs" Inherits="SRFROWCA.Admin.AddEditCountryMaps" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">
                <asp:Localize ID="localBreadCrumbHome" runat="server" Text="Home" meta:resourcekey="localBreadCrumbHomeResource1"></asp:Localize></a>
            </li>
            <li class="active">
                Add/Edit Maps

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
        <table style="width: 100%;">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h6></h6>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row">
                                                    <table border="0" style="width: 50%; margin: 10px 10px 10px 20px">
                                                         <tr>
                                                            <td>
                                                                <label>
                                                                    Map Title:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtMapTitle" runat="server" CssClass="width-80" MaxLength="500"></asp:TextBox>
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvTitle" ControlToValidate="txtMapTitle" ErrorMessage="Required" Display="Dynamic"
                                                                    ValidationGroup ="save" CssClass="error2"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                           <tr>
                                                            <td>
                                                                <label>
                                                                    Country:</label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlCountry" runat="server" AppendDataBoundItems="true" CssClass="width-80">
                                                                    <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                 <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddlCountry" ErrorMessage="Required" Display="Dynamic"
                                                                    ValidationGroup ="save" CssClass="error2" InitialValue="-1"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td>
                                                                <label>
                                                                    Map Month:</label>
                                                            </td>
                                                            <td>
                                                               <asp:DropDownList ID="ddlMapType" runat="server" AppendDataBoundItems="true" CssClass="width-80">
                                                                    <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                 <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="ddlMapType" ErrorMessage="Required" Display="Dynamic"
                                                                    ValidationGroup ="save" CssClass="error2" InitialValue="-1"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                   Upload Map:</label>
                                                            </td>
                                                            <td>
                                                               <asp:FileUpload ID="fuMap" runat="server" class="btn btn-grey" />
                                                                <%--<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="fuMap" ErrorMessage="Required" Display="Dynamic"
                                                                    ValidationGroup ="save" CssClass="error2"></asp:RequiredFieldValidator>
                                                                <asp:RegularExpressionValidator ID="uplValidator" runat="server" ControlToValidate="fuMap" ErrorMessage="Only PDF format is allowed" 
                                                                     ValidationGroup ="save" ValidationExpression="(.+\.([Pp][Dd][Ff]))" CssClass="error2"></asp:RegularExpressionValidator>--%>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="trFileName" visible="false">
                                                            <td>
                                                                
                                                            </td>
                                                            <td>
                                                               <asp:Literal runat="server" ID="ltrFileName"></asp:Literal>
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td>
                                                                <label>
                                                                    Is Public:</label>
                                                            </td>
                                                            <td>
                                                              <asp:CheckBox runat="server" ID="chkIsPublic" />
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td>
                                                                <label>
                                                                    Is Active:</label>
                                                            </td>
                                                            <td>
                                                              <asp:CheckBox runat="server" ID="chkActive" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td style="padding-top:20px;">
                                                                <asp:Button ID="btnSubmit" runat="server" Text="Save Map" ValidationGroup ="save" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                                                                <asp:Button ID="btnBackToSRPList" runat="server" Text="Cancel" style="margin-left:10px;"
                                                                     CssClass="width-10 btn" CausesValidation="false" OnClick="btnBackToSRPList_Click" />
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
    </div>



</asp:Content>
