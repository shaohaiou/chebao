﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main_s.aspx.cs" Inherits="Chebao.BackAdmin.message.main_s" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
     <script type="text/javascript">
         function setdaohan(t, url) {
             $("#daohan_sp").html("<a href='" + url + "'>" + t + "</a>");
         }
     </script>
</head>
<body style="background:url(../images/xdd.gif) repeat-x;">
<div class="right_nav">
     <a href="messageboardmg.aspx" target="ztk" id="messageboardmg" runat="server" class="current">信息列表</a>
</div>

<div class="r_sy" id="daohan">
	当前位置：反馈有奖 &gt;&gt; <span id="daohan_sp">信息列表 </span>
</div>
</body>
<script language="javascript" type="text/javascript">
    $(function () {
        $(".right_nav a").not("#products").each(function () {
            $(this).click(function () {
                $(".r_sy").html("当前位置：反馈有奖 &gt;&gt; " + $(this).html());
                $(".right_nav a").removeClass("current");
                $(this).addClass("current");
            });
        });
    });
</script>
</html>