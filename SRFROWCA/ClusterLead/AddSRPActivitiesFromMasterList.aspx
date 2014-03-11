<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AddSRPActivitiesFromMasterList.aspx.cs" Inherits="SRFROWCA.ClusterLead.AddSRPActivitiesFromMasterList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .highlight
        {
            background-color: yellow;
        }
        
        .highlight2
        {
            background-color: #ADFF2F;
        }
        
        .highlightRow
        {
            background-color: #F08080;
        }
    </style>
    <script type="text/javascript" src="../Scripts/ShowHideObJAndPr.js"></script>
    <script type="text/javascript" src="../Scripts/jq-highlight.js"></script>
    <script>
        $(function () {
            showHideObj();
            showHidePriority();

            $('#txtActivity').keyup(function () {
                $(".testact").unhighlight();
                var searchTerm = $(this).val().toLowerCase();
                if (searchTerm) {
                    var terms = searchTerm.split(' ');
                    if (terms.length > 0) {
                        $(".testact").highlight(terms);
                    }
                }

            });

            $('#txtIndicator').keyup(function () {
                $(".testind").unhighlight({ element: 'span', className: 'highlight2' });
                var searchTerm = $(this).val().toLowerCase();
                if (searchTerm) {
                    var terms = searchTerm.split(' ');
                    if (terms.length > 0) {
                        $(".testind").highlight(terms, { element: 'span', className: 'highlight2' });
                    }
                }
            });

            $('tr .testcb').click(function () {
                $(this).parent().parent().toggleClass('highlightRow');
            });

            $("tr .testcb").each(function () {
                var checkBox = $(this).find("input[type='checkbox']");
                if ($(checkBox).is(':checked')) {
                    $(this).parent().parent().addClass('highlightRow');
                }
                else {
                    $(this).parent().removeClass('highlightRow');
                }
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="containerDataEntryMain">
        <div class="containerDataEntryProjects">
            <div class="containerDataEntryProjectsInner">
                <fieldset>
                    <legend>Strategic Objectives</legend>
                    <asp:CheckBoxList ID="cblObjectives" runat="server" CssClass="checkObj">
                    </asp:CheckBoxList>
                </fieldset>
                <fieldset>
                    <legend>Humanitarian Priorities</legend>
                    <asp:CheckBoxList ID="cblPriorities" runat="server" CssClass="checkPr">
                    </asp:CheckBoxList>
                </fieldset>
            </div>
        </div>
        <div class="containerDataEntryGrid">
            <div class="tablegrid">
                <div>
                    <table>
                        <tr>
                            <td>
                                <label class="highlight">
                                    Highlight Activity:</label>
                            </td>
                            <td>
                                <input id="txtActivity" type="text" style="width: 350px;" />
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                <label class="highlight2">
                                    Highlight Indicator:</label>
                            </td>
                            <td>
                                <input id="txtIndicator" type="text" style="width: 350px;" />
                            </td>
                            <td align="right">
                                <asp:Button ID="btnAddSRPActivity" runat="server" OnClick="btnAddSRPActivity_Click"
                                    Text="Add New Activity" CssClass="button_example" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
                    <asp:GridView ID="gvSRPIndicators" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                        HeaderStyle-BackColor="ButtonFace" DataKeyNames="ActivityDataId" CssClass="imagetable"
                        Width="100%" OnRowDataBound="gvSRPIndicators_RowDataBound" EmptyDataText="Your Cluster Doesn Not Have Mastre List or SRP List Of Activities"
                        AllowSorting="true" OnSorting="gvSRPIndicators_Sorting">
                        <HeaderStyle BackColor="Control"></HeaderStyle>
                        <RowStyle CssClass="istrow" />
                        <AlternatingRowStyle CssClass="altcolor" />
                        <Columns>
                            <asp:BoundField DataField="ObjectiveId" HeaderText="ObjectiveId" ItemStyle-Width="1px"
                                ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement" meta:resourcekey="BoundFieldResource1">
                                <HeaderStyle CssClass="hiddenelement"></HeaderStyle>
                                <ItemStyle Wrap="False" CssClass="hiddenelement"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="HumanitarianPriorityId" HeaderText="HumanitarianPriorityId"
                                ItemStyle-Width="1px" ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement"
                                meta:resourcekey="BoundFieldResource2">
                                <HeaderStyle CssClass="hiddenelement"></HeaderStyle>
                                <ItemStyle Wrap="False" CssClass="hiddenelement"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ObjAndPrId" HeaderText="objprid" ItemStyle-Width="1px"
                                ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement" meta:resourcekey="BoundFieldResource2">
                                <HeaderStyle CssClass="hiddenelement"></HeaderStyle>
                                <ItemStyle Wrap="False" CssClass="hiddenelement"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Objective & Priority">
                                <ItemTemplate>
                                    <asp:Image ID="imgObjective" runat="server" AlternateText="O" />
                                    <asp:Image ID="imgPriority" runat="server" AlternateText="P" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ActivityName" HeaderText="Activity" ItemStyle-CssClass="testact"
                                SortExpression="ActivityName"></asp:BoundField>
                            <asp:TemplateField HeaderText="Regional Indicator" meta:resourcekey="TemplateFieldResource2"
                                SortExpression="IsRegional" ItemStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkRegional" runat="server" AutoPostBack="true" OnCheckedChanged="chkRegional_CheckedChanged"
                                        Checked='<%# Eval("IsRegional") %>' CssClass="testcb" />
                                </ItemTemplate>
                                <ItemStyle Width="2%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Country Indicator" meta:resourcekey="TemplateFieldResource2"
                                SortExpression="IsSRP" ItemStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSRP" runat="server" AutoPostBack="true" OnCheckedChanged="chkSRP_CheckedChanged"
                                        Checked='<%# Eval("IsSRP") %>' CssClass="testcb" />
                                </ItemTemplate>
                                <ItemStyle Width="2%" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="DataName" HeaderText="Output Indicator" ItemStyle-CssClass="testind"
                                SortExpression="DataName" ItemStyle-Wrap="true"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
