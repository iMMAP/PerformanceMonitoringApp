<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EmergencyLocations.aspx.cs" Inherits="SRFROWCA.Admin.EmergencyLocations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        #MainContent_cblLocations label {
            margin-top: -5px;
        }
    </style>
    
    <div class="page-content">
        <table border="0" cellpadding="2" cellspacing="0" class="pstyle1" width="100%">
            <tr>
                <td class="signupheading2" colspan="3" style="padding-left: 20px;">
                    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" CssClass="error2" Visible="false"
                                ViewStateMode="Disabled"></asp:Label>
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
                                   <%-- <h6>
                                        
                                            Add/Remove Locations In Emergency
                                        
                                    </h6>--%>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row">

                                                    <div class="contentarea">
                                                        <div class="formdiv" style="margin-top: 20px;">
                                                            <table style="width: 50%; margin-left: 20px;">
                                                                <tr>
                                                                    <td>Emergency:
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlEmergencies" runat="server" OnSelectedIndexChanged="ddlEmergencies_SelectedIndexChanged"
                                                                            AutoPostBack="true">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rgvEmg" runat="server" ErrorMessage="Required" InitialValue="0"
                                                                            Text="Required" ControlToValidate="ddlEmergencies" CssClass="error2"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="vertical-align: top;">
                                                                        <br />
                                                                        Locations:
                                                                    </td>
                                                                    <td>
                                                                        <br />
                                                                        <asp:CheckBoxList ID="cblLocations" CssClass="cb" runat="server" RepeatColumns="3" CellPadding="5">
                                                                        </asp:CheckBoxList>
                                                                        <asp:CustomValidator runat="server" ID="cvmodulelist"
                                                                            ClientValidationFunction="ValidateModuleList"
                                                                            ErrorMessage="Please Select Atleast one Location" CssClass="error2"></asp:CustomValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="width-20 btn btn-sm btn-success" />
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
                        </div>



                    </div>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        function ValidateModuleList(source, args) {
            var checkedCount = $("input[type=checkbox]:checked").length;

            if (checkedCount > 0) {
                args.IsValid = true;
                return;
            }
            args.IsValid = false;
        }

    </script>
</asp:Content>
