<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="products.aspx.cs" Inherits="Chebao.BackAdmin.product.products" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
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
                                                OnSelectedIndexChanged="ddlBrand_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </span>
                                    </dd>
                                    <dd>
                                        <span id="carmodels">
                                            <asp:DropDownList ID="ddlCabmodel" runat="server" CssClass="rideselect" AutoPostBack="true"
                                                Enabled="false" OnSelectedIndexChanged="ddlCabmodel_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </span>
                                    </dd>
                                    <dd>
                                        <span id="caroutputs">
                                            <asp:DropDownList ID="ddlPailiang" runat="server" CssClass="rideselect" AutoPostBack="true"
                                                Enabled="false" OnSelectedIndexChanged="ddlPailiang_SelectedIndexChanged">
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
                                        <p class="g_picc" style="text-align: center;">
                                            <img src="../images/picc.png" style="height: 30px; width: 77px;" /><br />
                                            <b>PICC</b>产品承保</p>
                                        <div class="fwbz">
                                            <span>安全<br>
                                                保障</span>
                                            <label>
                                                本公司每一款刹车片均受<b>PICC</b>承保保险</label>
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
                                    <a class="sp_img" href="productview.aspx?id=<%#Eval("ID") %>" title="<%# Eval("Name")%>"
                                        target="_blank">
                                        <img src="<%#Eval("Pic") %>" title="<%# Eval("Name")%>" width="192" height="144"
                                            alt="<%# Eval("Name")%>"></a>
                                    <p class="s_t1">
                                        ￥<%# Eval("Price").ToString().StartsWith("¥") ? Eval("Price").ToString().Substring(1) : Eval("Price").ToString()%></p>
                                    <p class="s_t2">
                                        <a href="productview.aspx?id=<%#Eval("ID") %>" title="<%# Eval("Name")%>" target="_blank">
                                            <%# Eval("Name")%></a></p>
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
</body>
<noscript>
    <iframe src="*.htm"></iframe>
</noscript>
</html>
