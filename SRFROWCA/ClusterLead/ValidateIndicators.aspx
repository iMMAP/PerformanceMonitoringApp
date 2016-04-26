<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ValidateIndicators.aspx.cs" Inherits="SRFROWCA.ClusterLead.ValidateIndicators" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function CheckAll(Checkbox) {
            var GridVwHeaderChckbox = document.getElementById("<%=gvIndicators.ClientID %>");

            for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                GridVwHeaderChckbox.rows[i].cells[1].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }

        </script>
</asp:Content>
<asp:Content ID="vIndicatorContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div id="divMsg">
        </div>
        <table width="100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box no-border">
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="form-group">
                                            <div class="row">
                                                <table class="width-100" border="0">
                                                    <tr>
                                                        <td>
                                                            <b>
                                                                <label>Project:</label>
                                                            </b>
                                                            <asp:Label ID="lblProjectTitle" runat="server" Text=""></asp:Label>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <label>Organization:</label>
                                                            </b>
                                                            <asp:Label ID="lblOrganization" runat="server" Text=""></asp:Label>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <label>Reporting Period:</label>
                                                            </b>

                                                            <asp:Label ID="lblReportingPeriod" runat="server" Text=""></asp:Label>
                                                            <asp:Label ID="lblUpdatedBy" runat="server" Text="" Visible="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">


                                <button runat="server" id="btnApprove" onserverclick="btnApprove_Click" class="btn btn-sm btn-success pull-right">
                                    <i class="icon-ok bigger-110"></i>Approve
                                       
                                </button>

                                <div class="widget-main">
                                    <div class="form-group">
                                        <div class="row">
                                            <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
                                                <asp:GridView ID="gvIndicators" runat="server" AutoGenerateColumns="False" HeaderStyle-BackColor="ButtonFace"
                                                    DataKeyNames="ReportId,ActivityDataId,ReportDetailId" CssClass="imagetable" Width="100%" OnRowDataBound="gvIndicators_RowDataBound">
                                                    <HeaderStyle BackColor="Control"></HeaderStyle>
                                                    <RowStyle CssClass="istrow" />
                                                    <AlternatingRowStyle CssClass="altcolor" />
                                                    <Columns>
                                                        <asp:BoundField DataField="ObjectiveId" HeaderText="Obj" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                                        <asp:TemplateField HeaderText="Select" ItemStyle-Width="70px">
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this);" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkApproved" runat="server" Checked='<%# Eval("IsApproved") %>'
                                                                    CssClass="testcb" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="2%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-Wrap="false" meta:resourcekey="TemplateFieldResource2">
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgObjective" runat="server" AlternateText="Obj" />
                                                            </ItemTemplate>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Activity" HeaderText="Activity" />
                                                        <asp:BoundField DataField="Indicator" HeaderText="Indicator" />
                                                        <asp:BoundField DataField="Unit" HeaderText="Unit" />
                                                        <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location" />
                                                        <asp:TemplateField HeaderText="Target Total" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTarget" runat="server" Text=' <%# Eval("AnnualTarget")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Target Male" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTargetMale" runat="server" Text=' <%# Eval("TargetMale")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Target Female" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTargetFemale" runat="server" Text=' <%# Eval("TargetFemale")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Achieved Total" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAchieved" runat="server" Text=' <%# Eval("AchievedTotal")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Achieved Male" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAchievedMale" runat="server" Text=' <%# Eval("AchievedMale")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Achieved Female" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAchievedFemale" runat="server" Text=' <%# Eval("AchievedMale")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Running Value" ItemStyle-HorizontalAlign="Right" SortExpression="RunningValue">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCountrySum" runat="server" Text=' <%# Eval("RunningValue")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="CalculationMethod" HeaderText="Calc Method" />
                                                    </Columns>
                                                </asp:GridView>
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
