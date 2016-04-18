﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="Chebao.BackAdmin.main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>无标题文档</title>
    <link href="css/admin.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            $(".J_Quick a").click(function () {
                self.parent.leftFrame.SetNavStyle($(this).html());
            });
        });
    </script>
</head>
<body style="background: url(images/xdd.gif) repeat-x;">
    <div class="right_nav">
        <span><a href="#">管理首页</a></span>
    </div>
    <div class="r_sy">
        当前位置：管理首页
    </div>
    <div class="ht_main">
        <div class="bt0">
            管理中心首页</div>
        <div class="bt2">
            您的资料</div>
        <div class="bt2x lan5">
            用户名：<%= AdminName %></div>
        <div class="bt2">
            快捷访问</div>
        <div class="bt2x lan5 J_Quick">
            <%if (CheckModulePower("产品列表,新增产品,数据导入,用户界面"))
              { %>
            <a href="/product/main.aspx">产品管理</a> <%} %>
            <%if(CheckModulePower("订单列表,利润查询,同步失败记录")){ %>
            <a href="/order/main.aspx">订单管理</a> 
            <%} %>
            <%if (CheckModulePower("用户界面"))
              { %>
            <a href="/product/products.aspx"
                target="_blank">用户界面</a>
                <%} %>
        </div>
    </div>
</body>
</html>
