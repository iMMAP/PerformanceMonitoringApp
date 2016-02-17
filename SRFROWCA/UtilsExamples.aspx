<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UtilsExamples.aspx.cs" Inherits="SRFROWCA.UtilsExamples" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="assets/css/gritter.css" />
    <script src="/assets/js/gritter.js"></script>
    <script>
        $(function () {
            $('.calctemp').on('click', function () {
                $.gritter.add({
                    // (string | mandatory) the heading of the notification
                    title: 'Calculation Method!',
                    // (string | mandatory) the text inside the notification
                    text: '<b>Sum:</b> Running Sum of monthly achievements.<br/> <b>Average:</b> Averate of monthly achievements.<br/><b>Latest:</b> Latest recorded achievement<br/><b>Max:</b> Maximum value of achievements.',
                    class_name: 'gritter-success',
                    sticky: true,
                });

                return false;
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <span class='calctemp btn'>Gritter Click</span>
</asp:Content>
