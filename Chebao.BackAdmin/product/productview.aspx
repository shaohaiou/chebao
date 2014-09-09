<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="productview.aspx.cs" Inherits="Chebao.BackAdmin.product.productview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>耐磨达产品查询系统</title>
    <link href=../css/kp.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %> rel="stylesheet"
        type="text/css" />
    <link href=../css/headfoot2.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %> rel="stylesheet"
        type="text/css" />
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
                        联系我们</a></li><li><a href="javascript:void(0);">纠错反馈有奖</a></li>
            </ul>
            <div class="header_navinfo">
                <span class="navinfo_user">
                    <%= AdminName %>，您好！</span> <span class="navinfo_opt"><a href="/logout.aspx">安全退出</a><a
                        class="ml40" href="/user/userchangepw.aspx">修改密码</a></span>
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
</body>
<noscript>
    <iframe src="*.htm"></iframe>
</noscript>
</html>
