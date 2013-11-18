<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true"
    CodeBehind="TargetsAchievedByLocData.aspx.cs" Inherits="SRFROWCA.Reports.TargetsAchievedByLocData" %>

<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="http://code.highcharts.com/highcharts.js" type="text/javascript"></script>
    <script src="http://code.highcharts.com/modules/exporting.js" type="text/javascript"></script>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="containerchart">
        <div class="divleftchart">
            <div class="singalselect">
                <label>
                    Data:</label>
                <div>
                    <asp:DropDownList ID="ddlData" runat="server" AutoPostBack="true" Width="100px" OnSelectedIndexChanged="ddlData_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="singalselect">
                <label>
                    Country:</label>
                <div>
                    <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" Width="100px"
                        OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="singalselect">
                <label>
                    Admin1:</label>
                <div>
                    <cc:DropDownCheckBoxes ID="ddlAdmin1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAdmin1_SelectedIndexChanged"
                        AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1" UseButtons="False"
                        UseSelectAllNode="True">
                        <Style SelectBoxWidth="100px" DropDownBoxBoxWidth="200px" DropDownBoxBoxHeight=""></Style>
                        <Texts SelectBoxCaption="Select Location" />
                    </cc:DropDownCheckBoxes>
                </div>
            </div>
            <div class="singalselect">
                <label>
                    Admin2:</label>
                <div>
                    <cc:DropDownCheckBoxes ID="ddlLocations" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlLocations_SelectedIndexChanged" AddJQueryReference="True"
                        meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                        <Style SelectBoxWidth="100px" DropDownBoxBoxWidth="200px" DropDownBoxBoxHeight=""></Style>
                        <Texts SelectBoxCaption="Select Location" />
                    </cc:DropDownCheckBoxes>
                </div>
            </div>
        </div>
        <div class="divrightchart">
            <div style="width: 54%; float: left;">
                <asp:Literal ID="ltrChart" runat="server" ViewStateMode="Disabled"></asp:Literal>
                <asp:HiddenField ID="hfChart" runat="server" ViewStateMode="Disabled" />
            </div>
            <div style="width: 44%; float: right;">
                <asp:Literal ID="ltrChartPercentage" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
</asp:Content>
