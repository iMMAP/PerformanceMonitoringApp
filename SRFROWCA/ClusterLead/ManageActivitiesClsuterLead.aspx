<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ManageActivitiesClsuterLead.aspx.cs" Inherits="SRFROWCA.ClusterLead.ManageActivitiesClsuterLead" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../Scripts/jq-highlight.js" type="text/javascript"></script>
    <script>
        $(function () {
            $('.txtacteng').keyup(function () {
                $(".acteng").unhighlight();
                var searchTerm = $(this).val().toLowerCase();
                if (searchTerm) {
                    var terms = searchTerm.split(' ');
                    if (terms.length > 0) {
                        $(".acteng").highlight(terms);
                    }
                }

            });

            $('.txtactfr').keyup(function () {
                $(".actfr").unhighlight();
                var searchTerm = $(this).val().toLowerCase();
                if (searchTerm) {
                    var terms = searchTerm.split(' ');
                    if (terms.length > 0) {
                        $(".actfr").highlight(terms);
                    }
                }

            });
        });
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="containerDataEntryMain">
        <asp:Wizard ID="wzActivities" runat="server" DisplaySideBar="false" OnNextButtonClick="wzActivities_NextButtonClick"
            OnPreviousButtonClick="wzActivities_PreviousButtonClick" OnFinishButtonClick="wzActivities_FinishButtonClick">
            <WizardSteps>
                <asp:WizardStep ID="wsObjAndPr" runat="server" Title="">
                    <div class="containerWizardStep">
                        <div>
                            <div class="graybar">
                                <asp:Label ID="lblCluster" runat="server" Text="Education"></asp:Label>
                            </div>
                            <div class="divMargin">
                                <asp:RadioButtonList ID="RadioButtonList1" runat="server">
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div>
                            <div class="graybar">
                                Strategic Objective
                            </div>
                            <div class="divMargin">
                                <asp:RadioButtonList ID="rbObjectives" runat="server">
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div>
                            <div class="graybar">
                                Humanitarian Priorities
                            </div>
                            <div class="divMargin">
                                <asp:RadioButtonList ID="rbPriorities" runat="server">
                                </asp:RadioButtonList>
                            </div>
                        </div>
                    </div>
                </asp:WizardStep>
                <asp:WizardStep ID="wsActivities" runat="server" Title="">
                    <div class="containerWizardStep">
                        <div class="graybar">
                            Add New Activity OR Select From List
                        </div>
                        <table>
                            <tr>
                                <td>
                                    <label>
                                        <b>Activity (English):</b></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtActivityEng" runat="server" Width="600px" TextMode="MultiLine"
                                        CssClass="txtacteng"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        <b>Activity (French):</b></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtActivityFr" runat="server" Width="600px" TextMode="MultiLine" CssClass="txtactfr"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td colspan="2">
                                    <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
                                        <asp:GridView ID="gvActivities" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                            HeaderStyle-BackColor="ButtonFace" DataKeyNames="PriorityActivityId" CssClass="imagetable"
                                            Width="100%">
                                            <HeaderStyle BackColor="Control"></HeaderStyle>
                                            <RowStyle CssClass="istrow" />
                                            <AlternatingRowStyle CssClass="altcolor" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select" meta:resourcekey="TemplateFieldResource2">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkActivitySelect" runat="server" meta:resourcekey="chkActivitySelectResource1" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="2%" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ActivityName_En" HeaderText="Activity (English)" ItemStyle-CssClass="acteng" />
                                                <asp:BoundField DataField="ActivityName_Fr" HeaderText="Activity (French)" ItemStyle-CssClass="actfr" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:WizardStep>
                <asp:WizardStep ID="wsIndicators" runat="server" Title="">
                </asp:WizardStep>
                <asp:WizardStep ID="wsSRP" runat="server" Title="">
                </asp:WizardStep>
            </WizardSteps>
        </asp:Wizard>
    </div>
</asp:Content>
