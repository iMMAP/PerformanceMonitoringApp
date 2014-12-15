<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AddActivityAndIndicators.aspx.cs" Inherits="SRFROWCA.ClusterLead.AddActivityAndIndicators" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Add Activity & Indicators</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div id="divMessage" runat="server" class="error2">
    </div>
    <div class="page-content">
        <div style="display: block; width: 100%; margin-bottom: 20px;">
            <div id="dvcluster" runat="server" style="float: left; width: 50%;">

                <label>
                    Cluster:</label>
                <div>
                    <asp:DropDownList ID="ddlCluster" runat="server" CssClass="width-90">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvCluster" runat="server" ErrorMessage="Required" Display="Dynamic"
                        CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCluster"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div id="dvCountry" runat="server" style="float: left; width: 50%;">

                <label>
                    Country:</label>
                <div>
                    <asp:DropDownList ID="ddlCountry" runat="server" CssClass="width-90" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ErrorMessage="Required" Display="Dynamic"
                        CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCountry"></asp:RequiredFieldValidator>
                </div>
            </div>

        </div>

        <div style="display: block; width: 100%;">

            <div style="float: left; width: 100%;">

                <label>
                    Objective:</label>

                <div>
                    <asp:DropDownList ID="ddlObjective" runat="server" CssClass="width-45">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
                        CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlObjective"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div style="display: block;">
            <h6 class="header blue bolder smaller">Add Activity</h6>
            <div class="col-xs-6 col-sm-6">
                <div class="widget-box no-border">
                    <div class="widget-body">

                        <label>
                            Activity (English):</label>
                        <div>
                            <asp:TextBox ID="txtActivityEng" runat="server" CssClass="width-90" TextMode="MultiLine"></asp:TextBox>
                            <asp:CustomValidator ID="cvActivityEng" runat="server" ClientValidationFunction="validateActivity"
                                CssClass="error2"></asp:CustomValidator>
                        </div>

                    </div>
                </div>
            </div>
            <div class="col-xs-6 col-sm-6">
                <div class="widget-box no-border">
                    <div class="widget-body">

                        <label>
                            Activity (French):</label>
                        <div>
                            <asp:TextBox ID="txtActivityFr" runat="server" CssClass="width-90" TextMode="MultiLine"></asp:TextBox>

                        </div>

                    </div>
                </div>
            </div>

        </div>



        <asp:Panel ID="pnlAdditionalIndicaotrs" runat="server">
        </asp:Panel>

        <div class="pull-right">
            <button id="btnRemoveIndicatorControl" runat="server" onserverclick="btnAddIndiatorControl_Click" causesvalidation="false"
                class="btn spinner-down btn-xs btn-danger" type="button" visible="false">
                <i class="icon-minus smaller-75"></i>
            </button>
            <button id="btnAddIndicatorControl" runat="server" onserverclick="btnAddIndiatorControl_Click" causesvalidation="false"
                class="btn spinner-up btn-xs btn-success" type="button">
                <i class="icon-plus smaller-75"></i>
            </button>
        </div>
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" CssClass="width-10 btn btn-sm btn-primary" />
        <asp:Button ID="btnBackToSRPList" runat="server" Text="Back" OnClick="btnBackToSRPList_Click"
            CssClass="width-10 btn btn-sm btn-primary" CausesValidation="false" />

    </div>
    <script>
        function validateActivity(sender, args) {
            var txtEng = $("[id$=txtActivityEng]").val();
            var txtFr = $("[id$=txtActivityFr]").val();

            if (txtEng.trim() == '' && txtFr.trim() == '') {

                alert("Please add Activity atleast in one Language!")
                return false;
            }
            else {
                validateUnit();
            }
        }

        function validateUnit() {
            var counter = 0;
            $(".dvIndicator").each(function (index) {
                var txtEng = $(this).find("[id$=txtInd1Eng]").val();
                var txtFr = $(this).find("[id$=txtInd1Fr]").val();
                var unitId = $(this).find("[id$=ddlUnit]").val();
                
                if (txtEng.trim() !== '' || txtFr.trim() !== '') {
                    if (unitId == "0") {
                        alert("Please select Unit!");
                        args.IsValid = false;
                        return false;
                    }
                }            
            });
        }

        //function validateIndicator() {

        //    var counter = 0;
        //    $(".dvIndicator").each(function (index) {
        //        var txtEng = $(this).find("[id$=txtInd1Eng]").val();
        //        var txtFr = $(this).find("[id$=txtInd1Fr]").val();

        //        if (txtEng.trim() == '' && txtFr.trim() == '') {

        //            alert("Please add Indicator " + (parseInt(index)+1) + " atleast in one Language!")
        //            return false;
        //        }
        //    });

        //}
    </script>
</asp:Content>
