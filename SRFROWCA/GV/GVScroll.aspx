<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GVScroll.aspx.cs" Inherits="SRFROWCA.GV.GVScroll" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <style>
        .GridviewScrollHeader TH, .GridviewScrollHeader TD {
            padding: 5px;
            font-weight: bold;
            white-space: nowrap;
            border-right: 1px solid #AAAAAA;
            border-bottom: 1px solid #AAAAAA;
            background-color: #EFEFEF;
            text-align: left;
            vertical-align: bottom;
        }

        .GridviewScrollItem TD {
            padding: 5px;
            white-space: nowrap;
            border-right: 1px solid #AAAAAA;
            border-bottom: 1px solid #AAAAAA;
            background-color: #FFFFFF;
        }

        .GridviewScrollPager {
            border-top: 1px solid #AAAAAA;
            background-color: #FFFFFF;
        }

            .GridviewScrollPager TD {
                padding-top: 3px;
                font-size: 14px;
                padding-left: 5px;
                padding-right: 5px;
            }

            .GridviewScrollPager A {
                color: #666666;
            }

            .GridviewScrollPager SPAN {
                font-size: 16px;
                font-weight: bold;
            }
    </style>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.9.1/jquery-ui.min.js"></script>
    <script type="text/javascript" src="gridviewScroll.js"></script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="gv" runat="server" HeaderStyle-BackColor="ButtonFace" OnRowCreated="gv_RowCreated" AutoGenerateColumns="false"
        CssClass="imagetable" Width="100%">
        <HeaderStyle CssClass="GridviewScrollHeader" />
        <RowStyle CssClass="GridviewScrollItem" />
        <PagerStyle CssClass="GridviewScrollPager" />
        <Columns>
            <asp:BoundField DataField="ProjectId" HeaderText="Id" ItemStyle-Width="100px" />
            <asp:BoundField DataField="ProjectCode" HeaderText="Code" ItemStyle-Width="100px" />
            <asp:BoundField DataField="ProjectTitle" HeaderText="Title" />
            <asp:BoundField DataField="ProjectStatus" HeaderText="LocationId" />
            <asp:BoundField DataField="ProjectContactName" HeaderText="Name" />
            <asp:BoundField DataField="ProjectContactEmail" HeaderText="Email" />
            <asp:BoundField DataField="Type" HeaderText="Type" />
            <asp:BoundField DataField="Year" HeaderText="Year" />
            <asp:BoundField DataField="Type" HeaderText="Type" />
            <asp:BoundField DataField="Year" HeaderText="Year" />
        </Columns>
    </asp:GridView>

    <script type="text/javascript">
        $(document).ready(function () {
            gridviewScroll();
        });

        function gridviewScroll() {
            $('#<%=gv.ClientID%>').gridviewScroll({
                width: 660,
                height: 300,
                freezesize: 1,
                headerrowcount: 3
            });
        }
    </script>
</asp:Content>

