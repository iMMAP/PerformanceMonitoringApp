<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="SRFROWCA.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function windowOpen1() {
            myWindow = window.open('http://localhost:50464/OPS/OPSDataEntry.aspx?uid=9&pid=59768&ProjectRevision=R&clname=education&cname=mali&key=pUAkWrmUJKCobRxo2HrHSA%3d%3d', '_blank', 'width=1000,height=600, scrollbars=yes,resizable=yes')
            myWindow.focus()
            return false;
        }

        function windowOpen2() {
            myWindow = window.open('http://localhost:50464/OPS/OPSDataEntry.aspx?uid=9&pid=23&ProjectRevision=R&clname=education&cname=mali&key=pUAkWrmUJKCobRxo2HrHSA%3d%3d', '_blank', 'width=1000,height=600, scrollbars=yes,resizable=yes')
            myWindow.focus()
            return false;
        }

        function windowOpen3() {
            myWindow = window.open('http://localhost:50464/ops/opsdataentry.aspx?uid=1883&pid=657445&ProjectRevision=R&clname=foodsecurity&cname=burkinafaso&key=Ka/Cl+36LjhfYQaAZz0QmA==', '_blank', 'width=1000,height=600, scrollbars=yes,resizable=yes')
            myWindow.focus()
            return false;
        }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
    <div class="space"></div>
    <input type="button" value="test"  onclick="windowOpen1()" style="width:200px" />
        <input type="button" value="52472 Education"  onclick="windowOpen2()" style="width:200px" />
        <input type="button" value="new"  onclick="windowOpen3()" style="width:200px" />
        </div>
     
</asp:Content>
