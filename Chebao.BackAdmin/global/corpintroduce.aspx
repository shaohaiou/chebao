<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="corpintroduce.aspx.cs"
    Inherits="Chebao.BackAdmin.global.corpintroduce" %>

<%@ Register src="../uc/header.ascx" tagname="header" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>耐磨达产品查询系统</title>
    <link href="../css/kp.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <link href="../css/headfoot2.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <script src="../js/head3.js" type="text/javascript"></script>
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../js/jquery.marquee.js" type="text/javascript"></script>
</head>
<body>
    <uc1:header ID="header1" runat="server" CurrentTag="公司简介" />
    <div id="main">
        <div style="padding: 20px;">
            <%=Sitesetting == null ? string.Empty : Sitesetting.CorpIntroduce%>
        </div>
    </div>
    <div class="footer">
        <div class="footmain">
            浙江耐磨达刹车片有限公司 版权所有 2014-2024 Shantui Zhejiang Lamda Brake Pads Co.,Ltd
        </div>
    </div>
</body>
</html>
