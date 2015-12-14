<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="products.aspx.cs" Inherits="Chebao.BackAdmin.product.products" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="../uc/header.ascx" TagName="header" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>耐磨达产品查询系统</title>
    <link href="../css/kp.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <link href="../css/headfoot2.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="../js/jquery.marquee.js" type="text/javascript"></script>
    <script src="../js/head3.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#txtsearch").focus(function () {
                if ($(this).val() == "请输入lamda型号") {
                    $(this).val("");
                    $(this).css("color", "Black");
                }
            }).blur(function () {
                if ($(this).val() == "") {
                    $(this).val("请输入lamda型号");
                    $(this).css("color", "Gray");
                }
            });
            $("#btnsearch").click(function () {
                location.href = "?n=" + ($.trim($("#txtsearch").val()) == "请输入lamda型号" ? "" : escape($.trim($("#txtsearch").val())));
                return false;
            });
            $("#imgpic").click(function () {
                var _this = $(this);
                imgShow("#outerdiv", "#innerdiv", "#bigimg", _this);
            });
            $(".tb-toolbar-item-hd").hover(function () {
                $(".tb-toolbar-item-tip", $(this)).show();
            }, function () {
                $(".tb-toolbar-item-tip", $(this)).hide();
            });
            $(".J_LinkAdd").click(function () {
                var productid = $(this).attr("pid");
                var amount = 1;
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
                            //                            alert("已成功加入购物车！");
                            location.href = location.href.replace("#", "");
                        }
                        else {
                            alert(data.Msg);
                        }
                    }
                });
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

            var layertop = ($(window).height() - $(".brandLayer").height()) / 2;
            var layerleft = ($(window).width() - $(".brandLayer").width()) / 2;
            $(".brandLayer").css({ "left": layerleft + "px", "top": layertop + "px" });
            
            $("#btnlayerclose").click(function () {
                $("#ddlBrand").show();

                $("#framflay").css({ "visibility": "hidden" });
                $("#divflay").css({ "visibility": "hidden" });
                $(".brandLayer").css({ "visibility": "hidden" });
            });

            $(".brandsel").click(function () {
                $("#ddlBrand").show();
                $("#ddlBrand").val($(this).attr("val"));
                setTimeout('__doPostBack(\'ddlBrand\',\'\')', 0);

                $("#framflay").css({ "visibility": "hidden" });
                $("#divflay").css({ "visibility": "hidden" });
                $(".brandLayer").css({ "visibility": "hidden" });
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

        function showbrands() {
            $("#ddlBrand").hide();

            $("#framflay").css({ "visibility": "visible" });
            $("#divflay").css({ "visibility": "visible" });
            $(".brandLayer").css({ "visibility": "visible" });
        }
    </script>
</head>
<body>
    <uc1:header ID="header1" runat="server" CurrentTag="产品查询" />
    <div id="main">
        <%--<div class="n_nav">
            当前位置：<a href="/product/products.aspx">产品查询</a>
            <%= string.IsNullOrEmpty(GetString("t")) ? "" : (" &gt;<a href=\"products.aspx?t=" + GetString("t") + "\">" + ((Chebao.Components.ProductType)GetInt("t")).ToString() + "</a>")%></div>--%>
        <div class="goodslist">
            <div class="g_list_r">
                <div class="FilterNavForm">
                    <div class="filter_car">
                        <form id="form1" runat="server">
                        <asp:ScriptManager runat="server" ID="smCabmodels">
                        </asp:ScriptManager>
                        <asp:UpdatePanel runat="server" ID="upnCabmodels">
                            <ContentTemplate>
                                <dl>
                                    <dt>请选择车型：</dt>
                                    <dd>
                                        <span id="carbrands">
                                            <asp:DropDownList runat="server" ID="ddlBrand" CssClass="rideselect" AutoPostBack="true" 
                                                Style="width: 213px!important;" OnSelectedIndexChanged="ddlBrand_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </span>
                                    </dd>
                                    <dd>
                                        <span id="carmodels">
                                            <asp:DropDownList ID="ddlCabmodel" runat="server" CssClass="rideselect" AutoPostBack="true"
                                                Style="width: 160px!important;" Enabled="false" OnSelectedIndexChanged="ddlCabmodel_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </span>
                                    </dd>
                                    <dd>
                                        <span id="caroutputs">
                                            <asp:DropDownList ID="ddlPailiang" runat="server" CssClass="rideselect" AutoPostBack="true"
                                                Style="width: 100px!important;" Enabled="false" OnSelectedIndexChanged="ddlPailiang_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </span>
                                    </dd>
                                    <dd>
                                        <span id="caryears">
                                            <asp:DropDownList ID="ddlNianfen" runat="server" CssClass="rideselect" Enabled="false"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlNianfen_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </span>
                                    </dd>
                                </dl>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <input type="text" runat="server" value="请输入lamda型号" class="f_c_search" style="color: Gray;"
                            id="txtsearch" />
                        <input type="submit" class="f_c_btn" id="btnsearch" value="搜索" />
                        </form>
                    </div>
                    <div class="g_filter  ">
                        <div class="filter_tit">
                            类&nbsp;&nbsp;型：</div>
                        <div class="filter_con">
                            <dl class="gl_hover brand_list">
                                <dd <%= string.IsNullOrEmpty(GetString("t")) ? "class=\"elected\"" : "" %>>
                                    <a href="products.aspx?<%= GetTypeQueryStr()%>">全部</a></dd>
                                <asp:Repeater runat="server" ID="rptProductType">
                                    <ItemTemplate>
                                        <dd <%# GetString("t") == Eval("Value").ToString() ? "class=\"elected\"" : "" %>>
                                            <a class="all_brand get_one_brand" href="products.aspx?t=<%# Eval("Value")%>&<%#GetTypeQueryStr() %>">
                                                <%# Eval("Text")%>&nbsp;<span class="del_btn"></span></a></dd>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </dl>
                        </div>
                    </div>
                </div>
                <webdiyer:AspNetPager ID="search_fy1" UrlPaging="true" NextPageText="下一页" PrevPageText="上一页"
                    CurrentPageButtonClass="current" PageSize="20" runat="server" NumericButtonType="Text"
                    MoreButtonType="Text" ShowFirstLast="false" HorizontalAlign="Right" AlwaysShow="false"
                    ShowDisabledButtons="False" PagingButtonSpacing="">
                </webdiyer:AspNetPager>
                <%if (HasProduct)
                  { %>
                <!--商品显示-->
                <div class="sm_list goods_p_list">
                    <ul class="">
                        <asp:Repeater runat="server" ID="rptProduct">
                            <HeaderTemplate>
                                <%if (SearchCabmodel != null && !string.IsNullOrEmpty(SearchCabmodel.Imgpath))
                                  { %>
                                <li class="" style="padding: 0 10px; height: 308px; border: 1px #d7d7d7 solid;">
                                    <div class="carstatus2">
                                        <span id="carstatus2" class="customGal">
                                            <h5>
                                                产品适用于：</h5>
                                            <img id="imgpic" src="<%= SearchCabmodel.Imgpath %>" alt="车型图片" /><span class="car_name">
                                                <%= SearchCabmodel.CabmodelNameBind%></span></span>
                                        <p class="g_picc" style="text-align: center; display: none;">
                                            <img src="../images/picc.png" style="height: 30px; width: 77px;" /><br />
                                            <b>PICC</b>产品承保</p>
                                        <div class="fwbz" style="margin: 50px 0 0 0;">
                                            <span>安全<br>
                                                保障</span>
                                            <label>
                                                本公司每一款刹车片均受<b><img src="../images/picc.png" style="height: 14px; width: 30px; float: right;
                                                    margin: 2px 0 0 0;" /></b>承保保险</label>
                                        </div>
                                    </div>
                                </li>
                                <%} %>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li class="">
                                    <!--商品标签-->
                                    <div class="pro_tag">
                                    </div>
                                    <p class="s_h">
                                        <%#Eval("ProductType").ToString()%></p>
                                    <!--商品标签-->
                                    <a class="sp_img" href="productview.aspx?id=<%#Eval("ID") %>" <%if (IsShowCabmodel)
                                          { %> title="<%# Eval("Name")%>" <%} %> target="_blank">
                                        <img src="<%#Eval("Pic") %>" <%if (IsShowCabmodel)
                                          { %>title="<%# Eval("Name")%>" <%} %>width="192" height="144" <%if (IsShowCabmodel)
                                          { %>alt="<%# Eval("Name")%>" <%} %>></a>
                                    <% if (IsShowPrice)
                                       { %>
                                    <p class="s_t1 tb-btn-add">
                                        ￥<%# Eval("Price").ToString().StartsWith("¥") ? Eval("Price").ToString().Substring(1) : Eval("Price").ToString()%>
                                        <i class="tb-iconfont tb-iconfont-list J_LinkAdd" pid="<%#Eval("ID") %>" title="加入购物车">
                                            ŭ</i>
                                    </p>
                                    <%} %>
                                    <p class="s_t2">
                                        <%if (IsShowCabmodel)
                                          { %>
                                        <a href="productview.aspx?id=<%#Eval("ID") %>" title="<%# Eval("Name")%>" target="_blank">
                                            <%# Eval("Name")%></a>
                                        <%} %></p>
                                    <p class="s_t3">
                                        型号：<%# Eval("ModelNumber")%>
                                    </p>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
                <%}
                  else
                  { %>
                <div class="pros_item">
                    <div class="ooss">
                        <p class="sold_out">
                            对不起，您要的产品未找到！</p>
                        <div class="sold_out_info">
                            <span>找不到产品？</span>
                        </div>
                    </div>
                </div>
                <%} %>
                <webdiyer:AspNetPager ID="search_fy" UrlPaging="true" NextPageText="下一页" PrevPageText="上一页"
                    CurrentPageButtonClass="current" PageSize="20" runat="server" NumericButtonType="Text"
                    MoreButtonType="Text" ShowFirstLast="false" HorizontalAlign="Right" AlwaysShow="false"
                    ShowDisabledButtons="False" PagingButtonSpacing="">
                </webdiyer:AspNetPager>
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
    <%if (IsShowPrice)
      { %>
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
                                    <ul class="mini-cart-items-list" style="height: 212px;overflow-y:auto;">
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
    <%} %>
    <%--<div id="divflay" style="border: 0px; z-index: 1109; opacity: 0.7; position: absolute;
        visibility: hidden; display: block; top: 0px; left: 0px; width: 100%; height: 100%;
        background-color: rgb(0, 0, 0);">
    </div>--%>
   <%-- <iframe id="framflay" style="border: 0px; opacity: 0; position: absolute; visibility: hidden;
        display: block; z-index: 1108; top: 0px; left: 0px; width: 100%; height: 100%;
        background-color: rgb(255, 255, 255);"></iframe>
    <div class="brandLayer" style="border: 0px; z-index: 1111; position: absolute; visibility: hidden;
        display: block; background-color: rgb(255, 255, 255);">
        <table border="0" cellpadding="0" cellspacing="0" style="font-size: 12px;">
            <tbody>
                <tr class="title">
                    <td>
                        <span style="line-height: 14px;">&nbsp;请选择车辆品牌</span><span class="ccType"><span id="btnlayerclose" cctype="close"
                            style="cursor: pointer; line-height: 14px;">[关闭]</span><span></span></span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="max-height:500px;overflow-y:scroll;">
                        <table style="width: 780px;">
                            <tbody>
                                <%= GetBrandSelHtml() %>
                            </tbody>
                        </table>
                        </div>
                    </td>
                </tr>
                <tr class="bottomLine">
                    <td>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>--%>
</body>
<noscript>
    <iframe src="*.htm"></iframe>
</noscript>
</html>
