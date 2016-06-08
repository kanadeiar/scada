﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Table.aspx.cs" Inherits="Scada.Web.Plugins.Table.WFrmTable" EnableViewState="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Table - Rapid SCADA</title>
    <link href="~/lib/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="~/lib/open-sans/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="~/css/controls/notifier.min.css" rel="stylesheet" type="text/css" />
    <link href="~/plugins/Table/css/tablecommon.min.css" rel="stylesheet" type="text/css" />
    <link href="~/plugins/Table/css/table.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../lib/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../../js/api/utils.js"></script>
    <script type="text/javascript" src="../../js/api/clientapi.js"></script>
    <script type="text/javascript" src="../../js/api/eventtypes.js"></script>
    <script type="text/javascript" src="../../js/api/viewhub.js"></script>
    <script type="text/javascript" src="../../js/controls/notifier.js"></script>
    <script type="text/javascript" src="js/tablecommon.js"></script>
    <script type="text/javascript">
        var DEBUG_MODE = <%= debugMode ? "true" : "false" %>;
        var viewID = <%= viewID %>;
        var refrRate = <%= refrRate %>;
        var phrases = <%= phrases %>;
        var today = <%= today %>;
        var locale = "<%= Scada.Localization.Culture.Name %>";
    </script>
    <script type="text/javascript" src="js/table.js"></script>
</head>
<body>
    <div id="divNotif" class="notifier">
    </div>
    <div id="divToolbar"><span 
        id="spanDate" class="tool-ctrl"><input id="txtDate" type="text" /><i class="fa fa-calendar" aria-hidden="true"></i></span><span 
        id="spanTime" class="tool-ctrl"><%= selTimeFromHtml %> - <%= selTimeToHtml %></span><span 
        id="spanExportBtn" class="tool-btn"><i class="fa fa-print" aria-hidden="true"></i></span><div id="divDebugTools"><span 
            id="spanTitleChangedBtn" class="tool-btn">TitleChanged</span><span 
            id="spanNavigateBtn" class="tool-btn">Navigate</span><span 
            id="spanDateChangedBtn" class="tool-btn">DateChanged</span>
        </div>
    </div>
    <div id="divTblWrapper">
        <%= tableViewHtml %>
    </div>
</body>
</html>