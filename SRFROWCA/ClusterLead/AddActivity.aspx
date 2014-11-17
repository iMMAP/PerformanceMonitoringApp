<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AddActivity.aspx.cs" Inherits="SRFROWCA.ClusterLead.AddActivity"  Culture="auto" UICulture="auto" meta:resourcekey="PageResource1"%>

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
    <div id="divMessage" runat="server" class="error2">
    </div>
    <div style="float:left;width:80%;margin:20px 10px;">

        <asp:Localize ID="localActivityInfo" runat="server" Text="Please provide the activities that cluster partners will undertake in 2015. You can specify up to 25 Activities that partners will be able to associate with their projects during project upload on OPS." meta:resourcekey="localActivityInfoResource1"></asp:Localize>
    </div>
     
    <div class="page-content">
        <div class="row">
            <div class="col-xs-12 col-sm-12">
                <div class="widget-box no-border">
                    <div class="widget-body">
                        <div class="widget-main">
                            <div>
                                <div class="row" id="dvCountry" runat="server">
                                    <h6 class="header blue bolder smaller">
                                        Select Country</h6>
                                    <div class="col-xs-6 col-sm-6">
                                        <div class="widget-box no-border">
                                            <div class="widget-body">
                                                <div class="widget-main no-padding-bottom no-padding-top">
                                                    <div>
                                                        <asp:DropDownList ID="ddlCountry" runat="server" CssClass="width-90" meta:resourcekey="ddlCountryResource1">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ErrorMessage="Required"
                                                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCountry" meta:resourcekey="rfvCountryResource1"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>                                   
                                </div>
                            </div>
                             <div>
                                <div class="row" id="dvcluster" runat="server">
                                    <h6 class="header blue bolder smaller">
                                        Select Cluster</h6>
                                    <div class="col-xs-6 col-sm-6">
                                        <div class="widget-box no-border">
                                            <div class="widget-body">
                                                <div class="widget-main no-padding-bottom no-padding-top">
                                                    <div>
                                                        <asp:DropDownList ID="ddlCluster" runat="server" CssClass="width-90" meta:resourcekey="ddlClusterResource1">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvCluster" runat="server" ErrorMessage="Required"
                                                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCluster" meta:resourcekey="rfvClusterResource1"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>                                   
                                </div>
                            </div>

                            <div>
                                <div class="row">
                                    <h6 class="header blue bolder smaller">
                                        Select Objective</h6>
                                    <div class="col-xs-6 col-sm-6">
                                        <div class="widget-box no-border">
                                            <div class="widget-body">
                                                <div class="widget-main no-padding-bottom no-padding-top">
                                                    <div>
                                                        <asp:DropDownList ID="ddlObjective" runat="server" CssClass="width-90" meta:resourcekey="ddlObjectiveResource1" >
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
                                                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlObjective" meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>                                   
                                </div>
                            </div>
                            <div class="row">
                                <h6 class="header blue bolder smaller">
                                    Add Activity</h6>
                                <div class="col-xs-6 col-sm-6">
                                    <div class="widget-box no-border">
                                        <div class="widget-body">
                                            <div class="widget-main no-padding-bottom no-padding-top">
                                                <label>
                                                    Activity (English):</label>
                                                <div>
                                                    <asp:TextBox ID="txtActivityEng" runat="server" CssClass="width-90" TextMode="MultiLine" meta:resourcekey="txtActivityEngResource1"></asp:TextBox>
                                                    <asp:CustomValidator runat="server" id="cvValidate"  ClientValidationFunction="validateActivity" meta:resourcekey="cvValidateResource1"></asp:CustomValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xs-6 col-sm-6">
                                    <div class="widget-box no-border">
                                        <div class="widget-body">
                                            <div class="widget-main no-padding-bottom no-padding-top">
                                                <label>
                                                    Activity (French):</label>
                                                <div>
                                                    <asp:TextBox ID="txtActivityFr" runat="server" CssClass="width-90" TextMode="MultiLine" meta:resourcekey="txtActivityFrResource1"></asp:TextBox>
                                                   
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                           
                        </div>
                      
                        <button runat="server" id="btnSave" onserverclick="btnSave_Click" class="width-10 btn btn-sm btn-primary"
                            title="Save">
                            <i class="icon-ok bigger-110"></i>Save
                        </button>
                        <asp:Button ID="btnBackToSRPList" runat="server" Text="Back" OnClick="btnBackToSRPList_Click"
                            CssClass="width-10 btn btn-sm btn-primary" CausesValidation="False" meta:resourcekey="btnBackToSRPListResource1" />
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
