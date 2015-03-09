<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="left.aspx.cs" Inherits="Chebao.BackAdmin.left" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>耐磨达产品查询系统</title>
    <link href="css/admin.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.3.2.min.js" type="text/javascript"></script>
<style type="text/css">
body{background:url(images/ldi.gif) repeat-y;}
</style>
<script type="text/javascript">
    $(function () {
        $(".left_nav a").click(function () {
            $(".left_nav a").removeClass("current");
            $(this).addClass("current");
            return true;
        });
    });

    function SetNavStyle(text) {
        $(".left_nav a").each(function () {
            if ($(this).html() == text) {
                $(".left_nav a").removeClass("current");
                $(this).addClass("current");
            }
        });
    }
</script>
</head>

<body style="text-align:center;">

<a href="index.aspx" target="_parent" style="display: inline-block;margin: 10px 0 0 30px;*display:inline;*zoom:1;"><img src="images/logo.jpg" width="82" height="82" /></a>
<div class="left_nav">
	<a href="main.aspx" target="mainFrame" class="current" runat="server" id="index_page">管理首页</a>
	<a href="user/main.aspx" target="mainFrame" runat="server" id="user_main">用户管理</a>
	<a href="car/main1.aspx" target="mainFrame" runat="server" id="car_main1">品牌管理</a>
	<a href="car/main.aspx" target="mainFrame" runat="server" id="car_main">车型管理</a>
	<a href="product/main.aspx" target="mainFrame" runat="server" id="product_main">产品管理</a>
	<a href="order/main.aspx" target="mainFrame" runat="server" id="order_main">订单管理</a>
	<a href="global/main.aspx" target="mainFrame" runat="server" id="global_main">系统设置</a>
	<a href="message/main.aspx" target="mainFrame" runat="server" id="messageboard_main">反馈有奖</a>
</div>
</body>
</html>
