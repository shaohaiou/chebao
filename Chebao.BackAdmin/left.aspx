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
  <%if (CheckModulePower("用户管理,管理员管理,折扣模版,成本折扣"))
    { %>
	<a href="user/main.aspx" target="mainFrame" runat="server" id="user_main">用户管理</a>
    <%} %>
  <%if (CheckModulePower("品牌管理"))
    { %>
	<a href="car/main1.aspx" target="mainFrame" runat="server" id="car_main1">品牌管理</a>
    <%} %>
  <%if (CheckModulePower("车型管理"))
    { %>
	<a href="car/main.aspx" target="mainFrame" runat="server" id="car_main">车型管理</a>
    <%} %>
  <%if (CheckModulePower("产品列表,新增产品,数据导入,用户界面"))
    { %>
	<a href="product/main.aspx" target="mainFrame" runat="server" id="product_main">产品管理</a>
    <%} %>
  <%if (CheckModulePower("订单列表,利润查询,同步失败记录"))
    { %>
	<a href="order/main.aspx" target="mainFrame" runat="server" id="order_main">订单管理</a>
    <%} %>
  <%if (CheckModulePower("盘库审核,库存查询,出入库记录"))
    { %>
	<a href="userstock/main.aspx" target="mainFrame" runat="server" id="userstock_main">用户库存</a>
    <%} %>
  <%if (CheckModulePower("站点设置,执行sql,数据处理"))
    { %>
	<a href="global/main.aspx" target="mainFrame" runat="server" id="global_main">系统设置</a>
    <%} %>
  <%if (CheckModulePower("反馈有奖"))
    { %>
	<a href="message/main.aspx" target="mainFrame" runat="server" id="messageboard_main">反馈有奖</a>
    <%} %>
</div>
</body>
</html>
