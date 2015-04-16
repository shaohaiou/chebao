<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="placeordersuccess.aspx.cs"
    Inherits="Chebao.BackAdmin.product.placeordersuccess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>耐磨达产品查询系统-下单成功</title>
    <link href="../css/headfoot2.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <link href="../css/kp.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="header" id="head_01">
        <div class="header_logo">
            <img src="../images/headlogo.jpg" />
        </div>
        <div class="header_nav">
            <ul>
                <li><a href="javascript:void(0);">首页</a></li><li><a href="/product/products.aspx">产品查询</a></li><li>
                    <a href="javascript:void(0);">公司简介</a></li><li><a href="javascript:void(0);">联系我们</a></li><li>
                        <a href="/message/messageboard.aspx">纠错反馈有奖</a></li>
            </ul>
            <div class="header_navinfo">
                <span class="navinfo_user">
                    <%= AdminName %>，您好！</span> <span class="navinfo_opt"><a href="/logout.aspx">安全退出</a><a
                        class="ml10" href="/user/userchangepw.aspx">修改密码</a><a href="/product/myorders.aspx"
                            class="ml10">我的订单</a><%if (Admin.SizeView > 0)
                                                   { %><a href="/product/myorders.aspx" class="cccx">尺寸查询</a><%} %></span>
            </div>
        </div>
    </div>
    <div id="main">
        <div style="text-align: center; font-size: 50px; line-height: 50px; color: #8AB6DD;
            padding: 30px 0;">
            下单成功</div>
        <div style="text-align: center;">
            <a href="shoppingtrolleymg.aspx" style="text-decoration: underline;">去购物车</a> <a
                href="myorders.aspx" style="text-decoration: underline; padding-left: 20px;">我的订单</a>
        </div>
    </div>
    <div class="footer">
        <div class="footmain">
            浙江耐磨达刹车片有限公司 版权所有 2014-2024 Shantui Zhejiang Lamda Brake Pads Co.,Ltd
        </div>
    </div>
</body>
</html>
