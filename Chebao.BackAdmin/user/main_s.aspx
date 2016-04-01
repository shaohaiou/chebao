﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main_s.aspx.cs" Inherits="Chebao.BackAdmin.user.main_s" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>无标题文档</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function setdaohan(t, url) {
            $("#daohan_sp").html("<a href='" + url + "'>" + t + "</a>");
        }
    </script>
</head>
<body style="background: url(../images/xdd.gif) repeat-x;">
    <div class="right_nav">
        <a href="userlist.aspx" target="ztk" id="userlist" runat="server" class="current">用户管理</a>
        <a href="adminlist.aspx" target="ztk" id="adminlist" runat="server">管理员管理</a>
        <a href="discountstencilmg.aspx" target="ztk" id="discountstencilmg" runat="server">折扣模版</a>
        <a href="discountstenciledit.aspx?costs=1" target="ztk" id="costsdiscount" runat="server">成本折扣</a>
    </div>
    <div class="r_sy" id="daohan">
        当前位置：用户管理 &gt;&gt; <span id="daohan_sp">
            <%if (Admin.Administrator)
              { %>管理员管理<%}
              else
              { %>用户管理<%} %></span>
    </div>
</body>
<script language="javascript" type="text/javascript">
    $(function () {
        $(".right_nav a").each(function () {
            $(this).click(function () {
                $(".r_sy").html("当前位置：用户管理 &gt;&gt; " + $(this).html());
                $(".right_nav a").removeClass("current");
                $(this).addClass("current");
            });
        });
    });
</script>
</html>
