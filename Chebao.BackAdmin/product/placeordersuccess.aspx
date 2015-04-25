<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="placeordersuccess.aspx.cs"
    Inherits="Chebao.BackAdmin.product.placeordersuccess" %>

<%@ Register Src="../uc/header.ascx" TagName="header" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>耐磨达产品查询系统-下单成功</title>
    <link href="../css/headfoot2.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <link href="../css/kp.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="../js/jquery.marquee.js" type="text/javascript"></script>
</head>
<body>
    <uc1:header ID="header1" runat="server" />
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
