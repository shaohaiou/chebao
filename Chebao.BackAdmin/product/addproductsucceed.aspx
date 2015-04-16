<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addproductsucceed.aspx.cs"
    Inherits="Chebao.BackAdmin.product.addproductsucceed" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>耐磨达产品查询系统-加入购物车成功</title>
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
    <!--end-->
    <div id="main">
        <div id="J_ResultMain" class="result-main clearfix" data-spm="1998302264">
            <div class="item-info">
                <%if (CurrentProduct != null)
                  { %>
                <div class="item-title">
                    <a href="productview.aspx?id=<%= CurrentProduct.ID%>" target="_blank">
                        <%=CurrentProduct.Name %></a>
                </div>
                <%} %>
                <br />
                <div class="sku-info">
                </div>
                <div class="quantity-info">
                    数量：<%if (CurrentShoppingTrolley != null)
                         {%><%= CurrentShoppingTrolley.Amount%><%} %></div>
                <div class="item-pic cart-pic s140">
                    <a href="<%if(CurrentProduct != null){ %>productview.aspx?id=<%=CurrentProduct.ID
                %><%}else{ %>javascript:void(0);<%} %>" target="_blank">
                        <img src="<% if(CurrentProduct
                != null){ %><%= CurrentProduct.Pic %><%} %>" alt=""></a></div>
            </div>
            <div id="J_ResultSummary" class="result-summary">
                <div class="result-hint">
                    <i class="cart-icon icon-success"></i>已成功加入购物车</div>
                <div class="price-summary">
                    小计：<span class="price">¥<em><% if (CurrentProduct != null && CurrentShoppingTrolley != null)
                                                   {%><%= Math.Round(decimal.Parse(CurrentProduct.Price.ToString().StartsWith("¥") ? CurrentProduct.Price.ToString().Substring(1) : CurrentProduct.Price.ToString()) * CurrentShoppingTrolley.Amount,2)%><%} %></em></span></div>
                <div class="cart-summary">
                    购物车共有<em><%=ShoppingTrolleyCount%>件</em>商品</div>
                <div class="result-op">
                    <a href="<%if(CurrentProduct != null){ %>productview.aspx?id=<%=CurrentProduct.ID
                %><%}else{ %>javascript:void(0);<%} %>" class="op-return"><i class="cart-icon icon-left">
                </i>返回商品详情</a> <a href="shoppingtrolleymg.aspx" class="op-cart">去购物车结算</a>
                </div>
            </div>
        </div>
    </div>
    <div class="footer">
        <div class="footmain">
            浙江耐磨达刹车片有限公司 版权所有 2014-2024 Shantui Zhejiang Lamda Brake Pads Co.,Ltd
        </div>
    </div>
</body>
</html>
