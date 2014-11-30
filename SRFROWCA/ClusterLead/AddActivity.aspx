<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AddActivity.aspx.cs" Inherits="SRFROWCA.ClusterLead.AddActivity" Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Add Activity</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <div id="divMessage" runat="server" class="error2">
        </div>
        <div class="alert2 alert-block alert-info">
            <h6>
                <asp:Localize ID="localActivityInfo" runat="server" Text="Please provide the activities that cluster partners will undertake in 2015. You can specify up to 25 Activities that partners will be able to associate with their projects during project upload on OPS." meta:resourcekey="localActivityInfoResource1"></asp:Localize>
            </h6>
        </div>

        <div class="widget-box no-border">
            <div class="widget-body">
                <div class="widget-main">
                    <div class="row">
                        <table>
                            <tr>
                                <td>Country:</td>
                                <td>
                                    <asp:DropDownList ID="ddlCountry" Width="300px" runat="server" meta:resourcekey="ddlCountryResource1">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ErrorMessage="Required"
                                        CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCountry" meta:resourcekey="rfvCountryResource1"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Cluster:</td>
                                <td>
                                    <asp:DropDownList ID="ddlCluster" Width="300px" runat="server" meta:resourcekey="ddlClusterResource1">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvCluster" runat="server" ErrorMessage="Required"
                                        CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCluster" meta:resourcekey="rfvClusterResource1"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Objective:</td>
                                <td>
                                    <asp:DropDownList ID="ddlObjective" Width="300px" runat="server" meta:resourcekey="ddlObjectiveResource1">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
                                        CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlObjective" meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Activity (English):</td>
                                <td>
                                    <asp:TextBox ID="txtActivityEng" runat="server" Width="500px" Height="90px" TextMode="MultiLine" meta:resourcekey="txtActivityEngResource1"></asp:TextBox>
                                    <asp:CustomValidator runat="server" ID="cvValidate" ClientValidationFunction="validateActivity" meta:resourcekey="cvValidateResource1"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Activity (French):</td>
                                <td>
                                    <asp:TextBox ID="txtActivityFr" runat="server" Width="500px" Height="90px" TextMode="MultiLine" meta:resourcekey="txtActivityFrResource1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" CssClass="width-10 btn btn-sm btn-primary" />
                                    <asp:Button ID="btnBackToSRPList" runat="server" Text="Back" OnClick="btnBackToSRPList_Click"
                                        CssClass="width-10 btn btn-sm btn-primary" CausesValidation="False" meta:resourcekey="btnBackToSRPListResource1" /></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        function validateActivity(sender, args) {
            var txtEng = $("[id$=txtActivityEng]").val();
            var txtFr = $("[id$=txtActivityFr]").val();

            if (txtEng.trim() == '' && txtFr.trim() == '') {

                alert("Please add Activity atleast in one Language!")
                args.IsValid = false;
                return false;
            }
            else {
                args.IsValid = true;
            }
        }

    </script>
</asp:Content>
