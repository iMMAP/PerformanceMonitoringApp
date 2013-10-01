<%@ Page Title="" Language="C#" MasterPageFile="~/NestedMasterPage2.master" AutoEventWireup="true"
    CodeBehind="TargetAchievedByLocDataMap.aspx.cs" Inherits="SRFROWCA.Maps.TargetAchievedByLocDataMap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%@ register assembly="DropDownCheckBoxes" namespace="Saplin.Controls" tagprefix="cc" %>
    <table width="100%" class="label1" border='0'>
        <tr>
            <td class="formh01">
                Data:
            </td>
            <td colspan="5">
                <asp:DropDownList ID="ddlData" runat="server" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="ddlData_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="formh01">
                Country:
            </td>
            <td>
                <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" Width="200px"
                    OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="formh01">
                Admin1:
            </td>
            <td>
                <cc:DropDownCheckBoxes ID="ddlAdmin1" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlAdmin1_SelectedIndexChanged" AddJQueryReference="True"
                    meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                    <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                    <Texts SelectBoxCaption="Select Location" />
                </cc:DropDownCheckBoxes>
            </td>
            <td>
                Admin2:
            </td>
            <td>
                <cc:DropDownCheckBoxes ID="ddlLocations" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlLocations_SelectedIndexChanged" AddJQueryReference="True"
                    meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                    <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                    <Texts SelectBoxCaption="Select Location" />
                </cc:DropDownCheckBoxes>
            </td>
        </tr>
        <%--<tr>
            <td>
            </td>
            <td colspan="2">
                <asp:RadioButton ID="rdAdmin1" runat="server" Text="Admin1" />
                <asp:RadioButton ID="rdAdmin2" runat="server" Text="Admin2" />
            </td>
            <td colspan="3">
                <asp:RadioButton ID="rdTarget" runat="server" Text="Target" />
                <asp:RadioButton ID="rdAchieved" runat="server" Text="Achieved" />
            </td>
        </tr>--%>
    </table>
    <div style="width: 100%">
        <div style="width:60%; float:left;">
            <iframe width="100%" height="500" frameborder="1" scrolling="no" marginheight="0"
                marginwidth="0" src="http://linux.oasiswebservice.org/crf/googleKMZ.html?query=SELECT%20*%20FROM%20[rowca].[dbo].[MapResults]"
                style="color: #0000FF; text-align: left"></iframe>
        </div>
        <div style="width:39%; float:right;">
            <script src="//www.gmodules.com/ig/ifr?url=http://dl.google.com/developers/maps/embedkmlgadget.xml&amp;up_kml_url=http%3A%2F%2Frowca.oasiswebservice.org%2Fmaps%2Fgetkmz.php%3Fquery%3Dselect%2520*%2520from%2520%5Browca%5D.%5Bdbo%5D.mapresults&amp;up_view_mode=earth&amp;up_earth_2d_fallback=0&amp;up_earth_fly_from_space=1&amp;up_earth_show_nav_controls=1&amp;up_earth_show_buildings=1&amp;up_earth_show_terrain=1&amp;up_earth_show_roads=1&amp;up_earth_show_borders=1&amp;up_earth_sphere=earth&amp;up_maps_zoom_out=0&amp;up_maps_default_type=map&amp;synd=open&amp;w=600&amp;h=450&amp;title=Mali+Demo+Map&amp;border=http%3A%2F%2Fwww.gmodules.com%2Fig%2Fimages%2F&amp;output=js"></script>
        </div>
    </div>
</asp:Content>
