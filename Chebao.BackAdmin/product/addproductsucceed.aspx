<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addproductsucceed.aspx.cs"
    Inherits="Chebao.BackAdmin.product.addproductsucceed" %>
<%@ Register src="../uc/header.ascx" tagname="header" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>耐磨达产品查询系统-加入购物车成功</title>
    <link href="../css/headfoot2.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <link href="../css/kp.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="../js/jquery.marquee.js" type="text/javascript"></script>
</head>
<body>
    <uc1:header ID="header1" runat="server" />
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
