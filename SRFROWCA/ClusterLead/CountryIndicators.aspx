<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="CountryIndicators.aspx.cs" Inherits="SRFROWCA.ClusterLead.CountryIndicators" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="cntHeadCountryIndicators" ContentPlaceHolderID="HeadContent" runat="server">

    
    <script type="text/javascript">
        function validate() {

            var objEng = document.getElementById('<%=txtIndicatorEng.ClientID%>');
            var objFr = document.getElementById('<%=txtIndicatorFr.ClientID%>');
            
            if (objEng.value == '' && objFr.value == '') {

                alert("Please enter atleast one Indicator!");
                return false;
            }

            //if (cmbEm.value < 0) {
            //    alert("Please select a Country!");
            //    return false;
            //}


        }
    </script>

</asp:Content>


<asp:Content ID="cntMainContentCountryIndicators" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">Home</a> </li>
            <li class="active">Country Indicators</li>
        </ul>

    </div>
    <div class="page-content">

        <table width="100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h6>

                                        <asp:Button ID="btnAddIndicator" runat="server" OnClick="btnAddIndicator_Click" Text="Add Indicator" CausesValidation="false"
                                            CssClass="btn btn-yellow pull-right" />
                                    </h6>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">


                                        <table border="0" style="width: 80%; margin: 0px 10px 0px 20px">

                                            <tr>
                                                <td>
                                                    <label>
                                                        Indicator:</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtIndicatorName" runat="server" Width="270"></asp:TextBox>
                                                </td>

                                                <td>
                                                    <asp:Label runat="server" ID="lblCluster" Text="Cluster:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlCluster_SelectedIndexChanged" AppendDataBoundItems="true" ID="ddlCluster" Width="270">
                                                        <asp:ListItem Selected="True" Text="--- Select Cluster ---" Value="-1"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>


                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <%--<asp:TextBox ID="txtObjectiveName" runat="server" Width="270"></asp:TextBox>--%>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblCountry" Text="Country:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" runat="server" AppendDataBoundItems="true" ID="ddlCountry" Width="270">
                                                        <asp:ListItem Selected="True" Text="--- Select Country ---" Value="-1"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>

                                            </tr>

                                            <tr>
                                                <td>&nbsp;</td>
                                                <td style="padding-top: 10px;">
                                                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" CssClass="btn btn-primary" CausesValidation="false" />
                                                </td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                            <%--<tr>
                                                    <td>&nbsp;</td>
                                                    <td style="padding-top: 10px;">
                                                        <asp:Label runat="server" ID="lblMessage" Text=""></asp:Label>
                                                    </td>
                                                    <td></td>
                                                    <td></td>
                                                </tr>--%>
                                        </table>

                                        <asp:Label runat="server" ID="lblMessage" Text=""></asp:Label>

                                    </div>
                                </div>
                            </div>
                        </div>



                    </div>
                </td>
            </tr>
        </table>

        <div class="table-responsive">
            <div style="overflow-x: auto; width: 100%">
                <asp:GridView ID="gvClusterIndicators" Width="100%" runat="server" AutoGenerateColumns="false" AllowSorting="True" DataKeyNames="SiteLanguageId"
                    OnRowDataBound="gvClusterIndicators_RowDataBound" OnSorting="gvClusterIndicators_Sorting" OnRowCommand="gvClusterIndicators_RowCommand" CssClass=" table-striped table-bordered table-hover">

                    <Columns>
                        <asp:TemplateField ItemStyle-Width="2%" HeaderText="#"> 
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField Visible="false" DataField="ClusterIndicatorId" HeaderText="ID" SortExpression="ClusterIndicatorId" />

                        <asp:BoundField ItemStyle-Width="10%" DataField="Country" HeaderText="Country" SortExpression="Country" />
                        <asp:BoundField ItemStyle-Width="10%" DataField="Cluster" HeaderText="Cluster" SortExpression="Cluster" />

                        <%--<asp:BoundField ItemStyle-Width="25%" Visible="false" DataField="Objective" HeaderText="Objective" SortExpression="Objective" />--%>
                        <asp:BoundField ItemStyle-Width="48%" DataField="Indicator" HeaderText="Indicator" SortExpression="Indicator" />
                        <asp:BoundField ItemStyle-Width="10%" DataField="Target" HeaderText="Target" SortExpression="Target" />
                        <asp:BoundField ItemStyle-Width="10%" DataField="Unit" HeaderText="Unit" SortExpression="Unit" />
                        <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>

                                <asp:LinkButton runat="server" ID="btnEdit" CausesValidation="false"
                                    CommandName="EditIndicator" CommandArgument='<%# Eval("ClusterIndicatorId") %>' Text="Edit">

                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" Width="80px" CausesValidation="false"
                                    CommandName="DeleteIndicator" CommandArgument='<%# Eval("ClusterIndicatorId") %>'>

                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCountryID" runat="server" Text='<%# Eval("CountryID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblClusterID" runat="server" Text='<%# Eval("ClusterID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                          <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblIndAlternate" runat="server" Text='<%# Eval("IndicatorAlt") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblUnitID" runat="server" Text='<%# Eval("UnitID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>
            </div>
        </div>

        <table>
            <tr>
                <td>
                    <asp:ModalPopupExtender ID="mpeEditIndicator" BehaviorID="mpeEditIndicator" runat="server" TargetControlID="btnTarget"
                        PopupControlID="pnlOrg" BackgroundCssClass="modalpopupbackground" CancelControlID="btnClose">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="pnlOrg" runat="server" Width="650px">
                        <asp:UpdatePanel ID="uPanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="containerPopup">
                                    <div class="popupheading">
                                        Edit Indicator
                                    </div>
                                    <div class="contentarea">
                                        <div class="formdiv">
                                            <table border="0" style="margin: 0 auto;">
                                               <%-- <tr>
                                                    <td>Country:
                                                    </td>
                                                    <td class="frmControl">
                                                        <asp:DropDownList ID="ddlCountry" runat="server" AppendDataBoundItems="true" Width="300px">
                                                            <asp:ListItem Selected="True" Text="--- Select Country ---" Value="-1">

                                                            </asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                       </td>
                                                </tr>--%>
                                                <tr>
                                                    <td>Indicator (English):
                                                    </td>
                                                    <td class="frmControl">
                                                        <asp:TextBox ID="txtIndicatorEng" runat="server" TextMode="MultiLine" Height="70" Width="450px" MaxLength="4000"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                       
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Indicator (French):
                                                    </td>
                                                    <td class="frmControl">
                                                        <asp:TextBox ID="txtIndicatorFr" runat="server" TextMode="MultiLine" Height="70" Width="450px" MaxLength="4000"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                   
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Target:
                                                    </td>
                                                    <td class="frmControl">
                                                        <asp:TextBox ID="txtTarget"  runat="server" Width="450px" MaxLength="10"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                   
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Unit:
                                                    </td>
                                                    <td class="frmControl">
                                                        <asp:DropDownList runat="server" ID="ddlUnits" Width="450"></asp:DropDownList>
                            
                                                    </td>
                                                    <td>
                                                   
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td></td>
                                                    <td align="left" class="frmControl">
                                                        <br />
                                                        <asp:HiddenField ID="hfClusterIndicatorID" runat="server" />
                                                        <asp:Button ID="btnEdit" runat="server" OnClick="btnEdit_Click" Text="Update"  OnClientClick="return validate();" CssClass="btn btn_primary" />
                                                        <asp:Button ID="btnClose" runat="server" Text="Close" CausesValidation="false" CssClass="btn btn_primary" />
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
                                <asp:PostBackTrigger ControlID="btnEdit" />
                                <asp:PostBackTrigger ControlID="btnClose" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </td>
            </tr>
        </table>

        <div style="display: none">
            <asp:Button ID="btnTarget" runat="server" Width="1px" />
        </div>

    </div>


</asp:Content>
