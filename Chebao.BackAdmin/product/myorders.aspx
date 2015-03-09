<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="myorders.aspx.cs" Inherits="Chebao.BackAdmin.product.myorders" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>耐磨达产品查询系统-我的订单</title>
    <link href="../css/headfoot2.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <link href="../css/kp.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        var pageindex = 1;
        var pagecount = <%=PageCount %>;
        $(function () {
            //更多订单
            $("#btnMore").click(function () {
                GetMore();
            });
        })

        function GetMore() { 
            if(pageindex >= pagecount){
            alert("没有更多评论了");
            return;
        }
        else{
            $("#dvLoading").show();
            pageindex++;
            $.ajax({
                url: "myordermore.aspx",
                data: { pageindex:pageindex, d: new Date() },
                type: 'GET',
                dataType: "text",
                error: function (msg) {
                    $("#dvLoading").hide();
                    alert("发生错误");
                },
                success: function (data) {
                    if(data == ""){
                        alert("数据获取失败");
                    }else{
                        var order = $(data);
                        order.hide();
                        $("#J_More").prepend(order);
                        order.fadeIn();
                    }

                    $("#dvLoading").hide();
                }
            });
        }
        }
    </script>
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
                            class="ml10">我的订单</a></span>
            </div>
        </div>
    </div>
    <div id="main">
        <div class="buy-content">
            <div class="buy-order-field" data-spm="3" id="J_Order">
                <h3>
                    我的订单</h3>
                <asp:Repeater runat="server" ID="rptData" OnItemDataBound="rptData_ItemDataBound">
                    <ItemTemplate>
                        <div style="padding-left: 10px; color: #3d3d3d; margin-top: 10px;">
                        <span style="font-weight:bold;"><%#Eval("AddTime") %></span>&nbsp;
                            订单号：<%#Eval("OrderNumber") %><span style="float: right; padding-right: 20px;">订单金额：<em
                                style="color: #F50"><%#Eval("TotalFee") %></em></span></div>
                        <div style="padding-left: 20px;">
                            <div class="buy-th-line clearfix" style="margin-top: 0;">
                                <span class="buy-th-title">商品</span> <span class="buy-th-price">单价(元)</span> <span
                                    class="buy-th-quantity">数量</span> <span class="buy-th-total">小计(元)</span>
                            </div>
                            <asp:Repeater runat="server" ID="rptOrderProduct">
                                <ItemTemplate>
                                    <div id="jigsaw28" class="order">
                                        <div id="jigsaw22" class="item blue-line clearfix">
                                            <div id="jigsaw17" class="itemInfo item-title">
                                                <a target="_blank" href="productview.aspx?id=<%# Eval("ProductID") %>" title="<%# Eval("ProductName")%>"
                                                    class="itemInfo-link J_MakePoint"><span class="item-pic"><span>
                                                        <img class="itemInfo-pic" src="<%# Eval("ProductPic") %>" alt="">
                                                    </span></span><span class="itemInfo-title J_MakePoint">
                                                        <%# Eval("ProductName")%></span> </a>
                                                <div class="itemInfo-sku">
                                                    <span></span>
                                                </div>
                                                <p class="c2c-extraInfo-container promo-extraInfo">
                                                </p>
                                            </div>
                                            <div class="item-price">
                                                <span id="jigsaw18" class="itemInfo price"><em class="style-normal-small-black J_ItemPrice">
                                                    <%# Math.Round(decimal.Parse(Eval("Price").ToString().StartsWith("¥") ? Eval("Price").ToString().Substring(1) : Eval("Price").ToString()),2)%></em>
                                                </span>
                                            </div>
                                            <div id="jigsaw19" class="quantity item-quantity">
                                                <p>
                                                    <%#Eval("Amount")%></p>
                                            </div>
                                            <div id="jigsaw21" class="itemPay item-total">
                                                <p class="itemPay-price price">
                                                    <em class="style-normal-bold-red J_ItemTotal">
                                                        <%#Eval("Sum") %></em>
                                                </p>
                                            </div>
                                            <div class="item-form-desc blue-line clearfix">
                                                <div class="item-form-eticketDesc">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="order-extra blue-line">
                                            <div class="order-user-info">
                                                <div id="jigsaw23" class="memo">
                                                    <label>
                                                        给卖家留言：<%#Eval("Remark")%></label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <%if (PageCount > 1)
                  {%>
                <div style="padding-top: 10px; padding-left: 10px;" id="J_More">
                    <a href="javascript:void(0);" id="btnMore">更多>></a></div>
                <%} %>
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
