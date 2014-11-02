<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="EmergencyObjectives.aspx.cs" Inherits="SRFROWCA.Admin.EmergencyObjectives" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ID="cntMainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">Home</a> </li>
            <li class="active">Emergency Objectives</li>
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

                                        <asp:Button ID="btnAddObjectives" runat="server" Text="Add Objective" CausesValidation="false"
                                            CssClass="btn btn-yellow pull-right" />
                                    </h6>
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
                                                                    Emergency Name:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtEmergencyName" runat="server" CssClass="width-80"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    Objective Name:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtObjectiveName" runat="server" CssClass="width-80"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td style="padding-top: 20px;">
                                                                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" CssClass="btn btn-primary" CausesValidation="false" />
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

            <div class="table-responsive">
            <div style="overflow-x: auto; width: 100%">
                <asp:GridView ID="gvEmergencyObjectives" runat="server" AutoGenerateColumns="false" AllowSorting="True"
                    CssClass=" table-striped table-bordered table-hover"
                    OnRowCommand="gvEmergencyObjectives_RowCommand" OnRowDataBound="gvEmergencyObjectives_RowDataBound" OnSorting="gvEmergencyObjectives_Sorting" >

                    <Columns>
                        <asp:TemplateField ItemStyle-Width="5%" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-Width="5%" DataField="EmergencyObjectiveId" HeaderText="ID" SortExpression="EmergencyObjectiveId" />

                        <asp:BoundField ItemStyle-Width="25%" DataField="EmergencyName" HeaderText="Emergency Name" SortExpression="EmergencyName" />
                        <asp:BoundField ItemStyle-Width="45%" DataField="Objective" HeaderText="Objective Name" SortExpression="Objective" />
                        <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>

                                <asp:LinkButton runat="server" ID="btnEdit" CausesValidation="false"
                                    CommandName="EditObjective" CommandArgument='<%# Eval("EmergencyObjectiveId") %>' Text="Edit">

                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" Width="80px" CausesValidation="false"
                                    CommandName="DeleteObjective" CommandArgument='<%# Eval("EmergencyObjectiveId") %>' >

                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                   
                </asp:GridView>
            </div>
        </div>

    </div>
</asp:Content>
