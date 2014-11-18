<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="AddCountryIndicator.aspx.cs" Inherits="SRFROWCA.ClusterLead.AddCountryIndicator" %>

<asp:Content ID="cntHeadCountryIndicator" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../assets/orsjs/jquery.numeric.min.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(function () {
            $(".numeric1").numeric();
        });
        </script>
    </asp:Content>
    <asp:Content ID="cntMainCountryIndicator" ContentPlaceHolderID="MainContent" runat="server">
        <div class="breadcrumbs" id="breadcrumbs">
            <script type="text/javascript">
                try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
    <ul class="breadcrumb">
        <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
        <li class="active">Add Output Indicator</li>
    </ul>
    <!-- .breadcrumb -->
    </div>
   
    <div id="divMessage" runat="server" class="error2">
    </div>
    <div class="page-content">

        <div class="row">
            <div class="col-xs-12 col-sm-12">
                <div class="widget-box no-border">
                    <div class="widget-body">
                        <div class="widget-main" style="padding-top: 0px;">
                            <div class="row">
                                <h6 class="header blue bolder smaller">Add Indicators:</h6>
                                <div class="col-sm-5">
                                    <div class="widget-box no-border">
                                        <div class="widget-body">
                                            <div class="widget-main no-padding-bottom no-padding-top">
                                                <div>
                                                    <asp:Label runat="server" ID="lblCluster" Text="Cluster:"></asp:Label>
                                                    <div>
                                                        <asp:DropDownList ID="ddlCluster" OnSelectedIndexChanged="ddlCluster_SelectedIndexChanged" runat="server" CssClass="width-70" AppendDataBoundItems="true">
                                                            <asp:ListItem Selected="True" Text="--- Select Cluster --" Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvddlCluster" runat="server" ErrorMessage="Required"
                                                            CssClass="error2" InitialValue="-1" Text="Required" ControlToValidate="ddlCluster">
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-5" style="<%=displayNone%>">
                                    <div class="widget-box no-border">
                                        <div class="widget-body">
                                            <div class="widget-main no-padding-bottom no-padding-top">
                                                <div>
                                                    <asp:Label runat="server" ID="lblCountry" Text="Country:"></asp:Label>
                                                    <div>
                                                        <asp:DropDownList ID="ddlCountry" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" runat="server" CssClass="width-70" AppendDataBoundItems="true">
                                                            <asp:ListItem Selected="True" Text="--- Select Country --" Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvddlCountry" runat="server" ErrorMessage="Required"
                                                            CssClass="error2" InitialValue="-1" Text="Required" ControlToValidate="ddlCountry">
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>

                            <asp:Panel ID="pnlIndicaotrs" runat="server">
                            </asp:Panel>

                            <div class="pull-right">
                                <button id="btnRemoveIndicatorControl" runat="server" onserverclick="btnAddIndicatorControl_ServerClick"
                                    causesvalidation="false" class="btn spinner-down btn-xs btn-danger" type="button" title="Remove Indicator"
                                    visible="false">
                                    <i class="icon-minus smaller-75"></i>
                                </button>
                                <button id="btnAddIndicatorControl" runat="server" onserverclick="btnAddIndicatorControl_ServerClick"
                                    causesvalidation="false" class="btn spinner-up btn-xs btn-success" type="button" title="Add Another Indicator">
                                    <i class="icon-plus smaller-75"></i>
                                </button>
                            </div>


                        </div>

                    </div>
                </div>
            </div>
            <button runat="server" id="btnSave" onserverclick="btnSave_ServerClick" class="width-10 btn btn-sm btn-primary"
                title="Save">
                <i class="icon-ok bigger-110"></i>Save
           
            </button>
        </div>
        <asp:CustomValidator ID="cvActivityEng" runat="server" ClientValidationFunction="validateIndicator"
            CssClass="error2">
        </asp:CustomValidator>
    </div>
    <script>
        function validateIndicator() {

            var counter = 0;
            $(".dvIndicator").each(function (index) {
                var txtEng = $(this).find("[id$=txtInd1Eng]").val();
                var txtFr = $(this).find("[id$=txtInd1Fr]").val();

                if (txtEng.trim() == '' && txtFr.trim() == '') {

                    alert("Please add Indicator " + (parseInt(index) + 1) + " atleast in one Language!")
                    return false;
                }
            });

        }
    </script>
</asp:Content>
