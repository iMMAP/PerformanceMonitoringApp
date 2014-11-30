<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EditActivity.aspx.cs" Inherits="SRFROWCA.ClusterLead.EditActivity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Edit Activity</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div id="divMessage" runat="server" class="error2">
    </div>
    <div class="page-content">
        <div class="alert2 alert-block alert-info">
            <h6>
                <asp:Localize ID="localActivityInfo" runat="server" Text="Please provide the activities that cluster partners will undertake in 2015. You can specify up to 25 Activities that partners will be able to associate with their projects during project upload on OPS." meta:resourcekey="localActivityInfoResource1"></asp:Localize>
            </h6>
        </div>
        <div class="widget-box no-border">
            <div class="widget-body">
                <div class="widget-main">
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
                                <td>Activity:</td>
                                <td>
                                    <asp:TextBox ID="txtActivityEng" runat="server" width="400px" Height="100px" TextMode="MultiLine"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ErrorMessage="Required"
                                        CssClass="error2" Text="Required" ControlToValidate="txtActivityEng"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" class="width-10 btn btn-sm btn-primary" />

                                    <asp:Button ID="btnBackToSRPList" runat="server" Text="Back" OnClick="btnBackToSRPList_Click"
                                        CssClass="width-10 btn btn-sm btn-primary" CausesValidation="false" />
                                </td>
                            </tr>
                        </table>
                    </div>
            </div>
        </div>


    </div>
</asp:Content>
