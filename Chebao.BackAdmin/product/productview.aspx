<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="productview.aspx.cs" Inherits="Chebao.BackAdmin.product.productview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>耐磨达产品查询系统</title>
    <link href="../css/kp.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <link href="../css/headfoot2.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="../js/head3.js" type="text/javascript"></script>
    <script src="../js/jquery.jqzoom.js" type="text/javascript"></script>
    <script src="../js/base2.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            if ($(".items ul li img").length > 0)
                preview($(".items ul li img").first()[0]);
            $("#imgpic").click(function () {
                var _this = $(this);
                imgShow("#outerdiv", "#innerdiv", "#bigimg", _this);
            });

            $(".tb-toolbar-item-hd").hover(function () {
                $(".tb-toolbar-item-tip", $(this)).show();
            }, function () {
                $(".tb-toolbar-item-tip", $(this)).hide();
            });
            $(".J_TBToolbarCart").click(function () {
                if ($(".tb-toolbar-mini-cart").is(":hidden")) {
                    $(".tb-toolbar-mini-cart").show();
                    $("#J_Toolbar").animate({ right: 330 }, 200);
                    $(this).css({ color: "#0971B2!important" });
                } else {
                    $("#J_Toolbar").animate({ right: 0 }, 200, function () {
                        $(".tb-toolbar-mini-cart").hide();
                    })
                }
            });
            $(".J_ShrinkMiniCartToolBar").click(function () {
                $("#J_Toolbar").animate({ right: 0 }, 200, function () {
                    $(".tb-toolbar-mini-cart").hide();
                })
            });
            $(".J_DelItem").click(function () {
                var ids = $(this).attr("data-cartid");
                var item = $(this).parent().parent().parent();
                $.ajax({
                    url: "/remoteaction.ashx",
                    data: { action: "deleteshoppingtrolley", ids: ids, d: new Date() },
                    type: 'GET',
                    dataType: "json",
                    error: function (msg) {
                        alert("发生错误");
                    },
                    success: function (data) {
                        if (data.Value == "success") {
                            item.remove();
                            var count = parseInt($.trim($(".J_ToolbarCartNum").html()))
                            $(".J_ToolbarCartNum").html(count - 1);
                        }
                        else {
                            alert(data.Msg);
                        }
                    }
                });
            });
            $(window).resize(function () {
                var windowheight = $(this).height();
                if (windowheight > 200) {
                    $(".mini-cart-items-list").height(141 + (parseInt((windowheight - 309) / 71)) * 71);
                }
            });
            $("#btnGotop").click(function () {
                $("body").animate({ scrollTop: 0 }, 300);
            });
            $(".mini-cart-items-list").height(141 + (parseInt(($(window).height() - 309) / 71)) * 71);

            $(".J_Increase").click(function () {
                var amount = parseInt($("#J_IptAmount").val());
                $("#J_IptAmount").val(amount + 1);
                CheckAmount();
            });
            $("#J_IptAmount").change(function () {
                CheckAmount();
            });
            $("#J_IptAmount").keyup(function () {
                CheckAmount();
            });
            $(".J_LinkAdd").click(function () {
                var productid = $(this).attr("pid");
                var amount = parseInt($("#J_IptAmount").val());
                $.ajax({
                    url: "/remoteaction.ashx",
                    data: { action: "addshoppingtrolley", pid: productid, amount: amount, d: new Date() },
                    type: 'GET',
                    dataType: "json",
                    error: function (msg) {
                        alert("发生错误");
                    },
                    success: function (data) {
                        if (data.Value == "success") {
                            location.href = "addproductsucceed.aspx";
                        }
                        else {
                            alert(data.Msg);
                        }
                    }
                });
            });
            $(".J_LinkBuy").click(function () {
                var productid = $(this).attr("pid");
                var amount = parseInt($("#J_IptAmount").val());
                $.ajax({
                    url: "/remoteaction.ashx",
                    data: { action: "buyproduct", pid: productid, amount: amount, d: new Date() },
                    type: 'GET',
                    dataType: "json",
                    error: function (msg) {
                        alert("发生错误");
                    },
                    success: function (data) {
                        if (data.Value == "success") {
                            location.href = "placeorder.aspx";
                        }
                        else {
                            alert(data.Msg);
                        }
                    }
                });
            });
        });
        document.onkeydown = function () {
            if (event.ctrlKey && event.keyCode == 67) {
                event.returnValue = false;
            }

            if ((event.keyCode == 116) || (window.event.ctrlKey) || (window.event.shiftKey) || (event.keyCode == 122)) {
                event.returnValue = false;
            }
        }
        document.oncontextmenu = function () { event.returnValue = false; }
        function imgShow(outerdiv, innerdiv, bigimg, _this) {
            var src = _this.attr("src");
            $(bigimg).attr("src", src);

            $("<img />").attr("src", src).one("load", function () {
                var windowW = $(window).width();
                var windowH = $(window).height();
                var realWidth = this.width;
                var realHeight = this.height;
                var imgWidth, imgHeight;
                var scale = 0.8;

                if (realHeight > windowH * scale) {
                    imgHeight = windowH * scale;
                    imgWidth = imgHeight / realHeight * realWidth;
                    if (imgWidth > windowW * scale) {
                        imgWidth = windowW * scale;
                    }
                } else if (realWidth > windowW * scale) {
                    imgWidth = windowW * scale;
                    imgHeight = imgWidth / realWidth * realHeight;
                } else {
                    imgWidth = realWidth;
                    imgHeight = realHeight;
                }
                $(bigimg).css("width", imgWidth);

                var w = (windowW - imgWidth) / 2;
                var h = (windowH - imgHeight) / 2;
                $(innerdiv).css({ "top": h, "left": w });
                $(outerdiv).fadeIn("fast");
            }).each(function () {
                if (this.complete) $(this).load();
            });

            $(outerdiv).click(function () {
                $(this).fadeOut("fast");
            });
        }
        function CheckAmount() {
            var amount = parseInt($("#J_IptAmount").val());
            var stock = parseInt($("#J_SpanStock").text());
            if (amount <= 0 || !amount) {
                $("#J_IptAmount").val(1);
            }
            else if (stock <= amount) {
                $("#J_IptAmount").val(stock);
            }
            else {
                $("#J_IptAmount").val(amount);
            }
            if (parseInt($("#J_IptAmount").val()) == 1) {
                if (!$(".J_Reduce").hasClass("tb-disable-reduce")) {
                    $(".J_Reduce").addClass("tb-disable-reduce");
                }
                $(".J_Reduce").unbind("click");
            } else if (parseInt($("#J_IptAmount").val()) > 1) {
                $(".J_Reduce").unbind("click");
                $(".J_Reduce").click(function () {
                    var amount = parseInt($("#J_IptAmount").val());
                    $("#J_IptAmount").val(amount - 1);
                    CheckAmount();
                });
                if ($(".J_Reduce").hasClass("tb-disable-reduce")) {
                    $(".J_Reduce").removeClass("tb-disable-reduce");
                }
            }
        }
    </script>
    <style type="text/css">
<!--
.ddd {
	border-bottom-width: 1px;
	border-bottom-style: dotted;
	border-bottom-color: #999;
	font-size: 12px;
	color: #06F;
	font-weight: bold;
}
--></style>
</head>
<body>
    <div class="header" id="head_01">
        <div class="header_logo">
            <img src="../images/headlogo.jpg" />
        </div>
        <div class="header_nav">
            <ul>
                <li><a href="javascript:void(0);">首页</a></li><li class="navcurrent"><a href="/product/products.aspx">
                    产品查询</a></li><li><a href="javascript:void(0);">公司简介</a></li><li><a href="javascript:void(0);">
                        联系我们</a></li><li><a href="/message/messageboard.aspx">纠错反馈有奖</a></li>
            </ul>
            <div class="header_navinfo">
                <span class="navinfo_user">
                    <%= AdminName %>，您好！</span> <span class="navinfo_opt"><a href="/logout.aspx">安全退出</a><a
                        class="ml10" href="/user/userchangepw.aspx">修改密码</a><a href="/product/myorders.aspx"
                            class="ml10">我的订单</a></span>
            </div>
        </div>
        <!--end-->
    </div>
    <div id="main">
        <div class="n_nav">
            当前位置：<a href="/product/products.aspx">首页</a> &gt;<a href="products.aspx?t=<%= (int)Product.ProductType %>"><%=Product.ProductType.ToString()%></a>
            <%if (SearchCabmodel != null)
              { %>&gt;<a href="products.aspx?id=<%=SearchCabmodel.ID %>"><%=SearchCabmodel.CabmodelNameBind%></a><%} %>
            &gt;
            <%= Product.Name %></div>
        <div class="goods_p_r1">
            <!--产品参数开始-->
            <div class="goods_r_a">
                <!--产品标签-->
                <div class="pro_tag">
                </div>
                <div id="preview" class="spec-preview">
                    <span class="jqzoom">
                        <img jqimg="<%=GetFirstPic() %>" src="<%=GetFirstPic() %>" alt=""></span>
                </div>
                <!--缩略图开始-->
                <div class="spec-scroll">
                    <a class="prev"></a><a class="next"></a>
                    <div class="items">
                        <ul>
                            <asp:Repeater runat="server" ID="rptPics">
                                <ItemTemplate>
                                    <li>
                                        <img alt="<%= Product.Name %>" title="<%= Product.Name %>" bimg="<%# Eval("Value") %>"
                                            src="<%# Eval("Key") %>" onmousemove="preview(this);"></li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </div>
                <!--缩图结束-->
            </div>
            <!--产品参数结束-->
            <div class="goods_r_b">
                <div class="name">
                    <h1 id="dsp_goods_name7076">
                        <%=Product.Name %></h1>
                    <!--<strong>适用于：</strong>-->
                </div>
                <div class="gprice">
                    <ul>
                        <li>
                            <div class="gp_r1">
                                价&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;格：</div>
                            <span class="m_price_1" style="width: 450px;">¥<%=Product.Price.StartsWith("¥") ? Product.Price.Substring(1) : Product.Price%></span></li>
                        <li>
                            <div class="gp_r1">
                                型&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;号：</div>
                            <div class="gp_r1" style="width: 450px;">
                                <%= Product.ModelNumber %></div>
                        </li>
                        <li></li>
                        <li>
                            <div class="gp_r1">
                                产&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;地：</div>
                            <div class="gp_r1" style="width: 450px;">
                                <%=Product.Area %></div>
                        </li>
                        <li>
                            <div class="gp_r1">
                                材&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;质：</div>
                            <div class="gp_r1" style="width: 450px;">
                                <%=Product.Material %></div>
                        </li>
                        <li>
                            <div class="gp_r1">
                                更换周期：</div>
                            <div class="gp_r1" style="width: 450px;">
                                <%=Product.Replacement %></div>
                        </li>
                        <li>
                            <div class="gp_r1">
                                规&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;格：</div>
                            <div class="gp_r1" style="width: 450px;">
                                <%= Product.Standard %></div>
                        </li>
                    </ul>
                </div>
                <div>
                    &nbsp;
                </div>
                <div id="J_SepLine" class="sep-line">
                </div>
                <%if (Stock > 0)
                  { %>
                <div id="J_isku" class="tb-key tb-key-sku">
                    <div class="tb-skin">
                        <dl class="tb-amount tb-clearfix">
                            <dt class="tb-property-type">数量</dt>
                            <dd>
                                <span class="tb-stock" id="J_Stock"><a href="javascript:void(0);" class="tb-reduce J_Reduce tb-iconfont tb-disable-reduce">
                                    ƛ</a><input id="J_IptAmount" type="text" class="tb-text" value="1" maxlength="8"
                                        title="请输入购买量" /><a href="javascript:void(0);" class="tb-increase J_Increase tb-iconfont">ƚ</a>件
                                </span><em>(库存<span id="J_SpanStock" class="tb-count"><%=Stock%></span>件)</em>
                            </dd>
                        </dl>
                        <div id="J_juValid" class="tb-action tb-clearfix ">
                            <div class="tb-btn-buy">
                                <a href="javascript:void(0);" title="点击此按钮，到下一步确认购买信息" class="J_LinkBuy" pid="<%=Product.ID %>">立即购买</a></div>
                            <div class="tb-btn-add">
                                <a href="javascript:void(0);" title="加入购物车" class="J_LinkAdd" pid="<%=Product.ID %>">
                                    <i class="tb-iconfont">ŭ</i>加入购物车</a></div>
                            <div id="Div1" style="display: none;">
                            </div>
                        </div>
                    </div>
                </div>
                <%}
                  else
                  { %>
                <div>
                    该商品已经售罄</div>
                <%} %>
                <div id="gtips" style="display: none;">
                </div>
            </div>
        </div>
        <div class="goods_p_r1_1">
            <div class="carstatus">
                <%if (SearchCabmodel != null && !string.IsNullOrEmpty(SearchCabmodel.Imgpath))
                  { %>
                <span id="carstatus" class="customGal">
                    <h5>
                        此产品适用于：</h5>
                    <img id="imgpic" src="<%= SearchCabmodel.Imgpath %>" alt="车型图片" /><span class="car_name">
                        <%= SearchCabmodel.CabmodelNameBind%></span></span>
                <%} %>
                <p class="g_picc" style="text-align: center;">
                    <img src="../images/picc.png" style="height: 30px; width: 77px;" /><br />
                    <b>PICC</b>产品承保</p>
                <div class="fwbz">
                    <span>安全<br>
                        保障</span>
                    <ul>
                        <li>本公司每一款刹车片均受<b>PICC</b>承保保险</li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="goods_p_r2">
            <!--随心配-->
            <span id="comb_goods_id"></span>
            <!--商品详情内容-->
            <div class="g_detail" id="comment">
                <ul class="tab clearfix1">
                    <li onclick="tabs('#comment',0)" class="curr">产品描述<strong></strong><span></span></li>
                    <li onclick="tabs('#comment',1)">适用车型<span></span></li>
                </ul>
                <div class="mc tabcon hide" style="display: block;">
                    <div class="norecode">
                        <div id="con_one_1" class="hover" style="display: block;">
                            <div class="textHight">
                            </div>
                            <p>
                            </p>
                            <%=Product.Introduce %>
                            <!---->
                        </div>
                    </div>
                </div>
                <div class="mc tabcon hide" style="display: none;">
                    <div id="con_one_2" style="display: block;">
                        <p>
                            &nbsp;</p>
                        <ul style="width: 960px;">
                            <asp:Repeater runat="server" ID="rptCabmodels">
                                <ItemTemplate>
                                    <li style="width: 180px; display: inline-block; *display: inline; *zoom: 1;">
                                        <%# Eval("CabmodelNameBind")%></li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                        <p>
                            &nbsp;</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="footer">
        <div class="footmain">
            浙江耐磨达刹车片有限公司 版权所有 2014-2024 Shantui Zhejiang Lamda Brake Pads Co.,Ltd
        </div>
    </div>
    <div id="outerdiv" style="position: fixed; top: 0; left: 0; background-color: rgba(0,0,0,0.7);
        filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=#BF000000,endColorstr=#BF000000);
        z-index: 200; width: 100%; height: 100%; display: none;">
        <div id="innerdiv" style="position: absolute;">
            <img id="bigimg" style="border: 5px solid #fff;" src="" />
        </div>
    </div>
    <div class="tb-toolbar tb-toolbar-right" id="J_Toolbar" style="right: 0px;">
        <div class="tb-toolbar-space" style="height: 25%;">
        </div>
        <ul class="tb-toolbar-list tb-toolbar-list-with-ww tb-toolbar-list-with-cart tb-toolbar-list-with-pk">
            <li class="tb-toolbar-item tb-toolbar-item-cart"><a href="#" class="tb-toolbar-item-hd tb-toolbar-item-hd-cart J_TBToolbarCart tb-toolbar-item-hd-active">
                <div class="tb-toolbar-item-icon tb-toolbar-item-icon-cart">
                    </div>
                <div class="tb-toolbar-item-label tb-toolbar-item-label-cart">
                    购物车</div>
                <div class="J_ToolbarCartNum tb-toolbar-item-badge-cart">
                    <%=ShoppingTrolleyCount%></div>
                <div class="tb-toolbar-item-tip">
                    我的购物车<div class="tb-toolbar-item-arrow">
                        ◆</div>
                </div>
            </a>
                <div class="tb-toolbar-item-bd tb-toolbar-mini-cart">
                    <div class="toolbar-main toolbar-mini-cart-main">
                        <div class="toolbar-hd">
                            <div class="toolbar-hd-title">
                                购物车</div>
                            <span class="toolbar-shrink J_ShrinkMiniCartToolBar">收起</span></div>
                        <div class="toolbar-bd">
                            <div class="mini-cart-list">
                                <div class="mini-cart-list-hd">
                                    <div class="mini-cart-list-title">
                                        最新加入的宝贝</div>
                                    <a href="shoppingtrolleymg.aspx" target="_blank">查看全部</a></div>
                                <div class="mini-cart-list-bd">
                                    <ul class="mini-cart-items-list" style="height: 212px;">
                                        <asp:Repeater runat="server" ID="rptShoppingTrolley">
                                            <ItemTemplate>
                                                <li>
                                                    <div class="mini-cart-item">
                                                        <div class="mini-cart-item-pic">
                                                            <a title="<%# Eval("Name")%>" href="productview.aspx?id=<%#Eval("ID") %>" target="_blank">
                                                                <img src="<%#Eval("Pic") %>"></a></div>
                                                        <div class="mini-cart-item-info">
                                                            <div class="mini-cart-item-title">
                                                                <a title="<%# Eval("Name")%>" href="productview.aspx?id=<%#Eval("ID") %>" target="_blank">
                                                                    <%# Eval("Name")%></a>
                                                            </div>
                                                            <div class="mini-cart-item-price">
                                                                ¥<em><%# Eval("Price").ToString().StartsWith("¥") ? Eval("Price").ToString().Substring(1) : Eval("Price").ToString()%></em></div>
                                                            <a class="J_DelItem mini-cart-item-del" href="#" data-cartid="<%#Eval("SID") %>">删除商品</a></div>
                                                    </div>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </div>
                                <a href="shoppingtrolleymg.aspx" class="mini-cart-submit" target="_blank"><i></i>去购物车结算</a></div>
                        </div>
                    </div>
                </div>
            </li>
        </ul>
        <div class="tb-toolbar-space" style="height: 7%;">
        </div>
        <ul class="tb-toolbar-list tb-toolbar-list-with-feedback tb-toolbar-list-with-gotop">
            <li class="tb-toolbar-item"><a href="#" class="tb-toolbar-item-hd">
                <div class="tb-toolbar-item-icon">
                    </div>
                <div class="tb-toolbar-item-tip">
                    <span class="tb-toolbar-item-tip-text">纠错反馈有奖</span><div class="tb-toolbar-item-arrow">
                        ◆</div>
                </div>
            </a></li>
            <li class="tb-toolbar-item"><a id="btnGotop" href="javascrit:void(0);" class="tb-toolbar-item-hd">
                <div class="tb-toolbar-item-icon">
                    </div>
                <div class="tb-toolbar-item-tip">
                    <span class="tb-toolbar-item-tip-text">顶部</span><div class="tb-toolbar-item-arrow">
                        ◆</div>
                </div>
            </a></li>
        </ul>
    </div>
</body>
<noscript>
    <iframe src="*.htm"></iframe>
</noscript>
</html>
