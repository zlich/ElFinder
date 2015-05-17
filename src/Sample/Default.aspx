<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Sample._Default" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="screen" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.18/themes/smoothness/jquery-ui.css">
    <link rel="stylesheet" type="text/css" media="screen" href="Content/css/elfinder.min.css">
    <link rel="stylesheet" type="text/css" media="screen" href="Content/css/theme.css">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.18/jquery-ui.min.js"></script>
    <script type="text/javascript" src="Content/js/elfinder.min.js"></script>
    <script type="text/javascript" charset="utf-8">
        $().ready(function () {
            var elf = $('#elfinder').elfinder({
                url: 'Files.ashx'  // connector URL (REQUIRED)
                // lang: 'ru',             // language (OPTIONAL)
            }).elfinder('instance');
        });
		</script>
    <div id="elfinder" />
</asp:Content>
