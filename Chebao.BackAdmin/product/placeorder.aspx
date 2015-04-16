<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="placeorder.aspx.cs" Inherits="Chebao.BackAdmin.product.placeorder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>耐磨达产品查询系统-提交订单</title>
    <link href="../css/headfoot2.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <link href="../css/kp.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            $(".memo .J_MakePoint").hover(function () {
                $(this).toggleClass("c2c-text-hover");
            }).focus(function () {
                if ($(this).val() == "") {
                    $(this).toggleClass("memo-close");
                }
            }).blur(function () {
                if ($(this).val() == "") {
                    $(this).toggleClass("memo-close");
                }
            });

            $("#J_Go").click(function () {
                if ($.trim($("#txtAddress").val()) == "") {
                    $("#txtAddress").focus();
                    alert("请输入详细地址");
                    return;
                }
                if ($.trim($("#txtPostCode").val()) == "") {
                    $("#txtPostCode").focus();
                    alert("请输入邮政编码");
                    return;
                }
                if ($.trim($("#txtLinkName").val()) == "") {
                    $("#txtLinkName").focus();
                    alert("请输入收货人姓名");
                    return;
                }
                if ($.trim($("#txtLinkTel").val()) == "" && $.trim($("#txtLinkMobile").val()) == "") {
                    $("#txtLinkMobile").focus();
                    alert("手机号码与电话号码至少填一项");
                    return;
                }
                form1.submit();
            });

            setLinkInfo();
        });

        function setLinkInfo() {
            setTimeout(function () {
                $("#txtLinkShow").text($("#txtLinkName").val() + " " + $("#txtLinkMobile").val() + " " + $("#txtLinkTel").val());
                setLinkInfo();
            }, 1000);
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
                            class="ml10">我的订单</a><%if (Admin.SizeView > 0)
                                                   { %><a href="/product/myorders.aspx" class="cccx">尺寸查询</a><%} %></span>
            </div>
        </div>
    </div>
    <div id="main">
        <form runat="server" id="form1">
        <div class="buy-content">
            <div id="jigsaw12" class="address">
                <h3>
                    确认收货地址
                </h3>
                <div class="address-bar">
                    <ul>
                        <li><span><em class="red">*</em>寄送至：</span>
                            <div>
                                <asp:ScriptManager ID="smArea" runat="server">
                                </asp:ScriptManager>
                                <asp:UpdatePanel runat="server" ID="upl1">
                                    <ContentTemplate>
                                        <asp:DropDownList runat="server" ID="ddlProvince" AutoPostBack="true" OnSelectedIndexChanged="ddlProvince_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:DropDownList runat="server" ID="ddlCity" AutoPostBack="true" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:DropDownList runat="server" ID="ddlDistrict" AutoPostBack="true" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:TextBox runat="server" ID="txtAddress" CssClass="w200" OnTextChanged="txtAddress_TextChanged"
                                            AutoPostBack="true"></asp:TextBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </li>
                        <li><span><em class="red">*</em>邮政编码：</span>
                            <div>
                                <asp:UpdatePanel runat="server" ID="upl2">
                                    <ContentTemplate>
                                        <input type="text" runat="server" id="txtPostCode" class="w100" />
                                        &nbsp;
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </li>
                        <li><span><em class="red">*</em>收货人姓名：</span>
                            <div>
                                <input type="text" runat="server" id="txtLinkName" class="w100" />&nbsp;</div>
                        </li>
                        <li><span>手机号码：</span>
                            <div>
                                <input type="text" runat="server" id="txtLinkMobile" class="w100" /><em class="gray pl10">手机号码与电话号码至少填一项</em></div>
                        </li>
                        <li><span>电话号码：</span>
                            <div>
                                <input type="text" runat="server" id="txtLinkTel" class="w100" /><em class="gray pl10">手机号码与电话号码至少填一项</em></div>
                        </li>
                    </ul>
                </div>
            </div>
            <div id="jigsaw15" class="tips">
            </div>
            <div class="buy-order-field" data-spm="3">
                <h3>
                    确认订单信息</h3>
                <div class="buy-th-line clearfix">
                    <span class="buy-th-title">商品</span> <span class="buy-th-price">单价(元)</span> <span
                        class="buy-th-quantity">数量</span> <span class="buy-th-total">小计(元)</span>
                </div>
                <asp:Repeater runat="server" ID="rptData" OnItemDataBound="rptData_ItemDataBound">
                    <ItemTemplate>
                        <input type="hidden" runat="server" id="hdnSID" value='<%#Eval("SID") %>' />
                        <div id="jigsaw28" class="order">
                            <div id="jigsaw22" class="item blue-line clearfix">
                                <div id="jigsaw17" class="itemInfo item-title">
                                    <a target="_blank" href="productview.aspx?id=<%#Eval("ProductID") %>" title="<%# Eval("ProductName")%>"
                                        class="itemInfo-link J_MakePoint"><span class="item-pic"><span>
                                            <img class="itemInfo-pic" src="<%#Eval("ProductPic") %>" alt="">
                                        </span></span><span class="itemInfo-title J_MakePoint">
                                            <%# Eval("ProductName")%></span><span class="gray">
                                                <%#Eval("CabmodelStr")%></span> </a>
                                    <asp:Repeater runat="server" ID="rptProductMix">
                                        <ItemTemplate>
                                            <div>
                                                <div class="itemInfo-title" style="display: table-cell; padding: 5px 0;">
                                                    <%#Eval("Name") %></div>
                                                <div class="item-price">
                                                    <span id="jigsaw18" class="itemInfo price"><em class="style-normal-small-black J_ItemPrice">
                                                        <%# Eval("Price")%></em> </span>
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
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
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
                                            给卖家留言：</label>
                                        <textarea runat="server" id="txtRemark" title="选填：对本次交易的说明（建议填写已经和卖家达成一致的说明）" placeholder="选填：对本次交易的说明（建议填写已经和卖家达成一致的说明）"
                                            class="memo-input J_MakePoint c2c-text-default memo-close" autocomplete="off"></textarea>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        <div class="buy-footer">
            <div class="order-go clearfix" data-spm="4">
                <div class="address-confirm clearfix">
                    <div class="box">
                        <div id="jigsaw34" class="realPay" tabindex="0">
                            <em class="t">实付款：</em> <span class="price g_price "><span>¥</span> <em class="style-large-bold-red"
                                runat="server" id="txtTotalFee"></em></span>
                        </div>
                        <div id="jigsaw35" class="address">
                            <asp:UpdatePanel runat="server" ID="upl3">
                                <ContentTemplate>
                                    <p class="buy-footer-address">
                                        <span class="buy-line-title buy-line-title-type">寄送至：</span> <span class="buy-footer-address-detail J_BuyFooterAddressDetail"
                                            runat="server" id="txtAddressShow"></span>
                                    </p>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <p class="buy-footer-address">
                                <span class="buy-line-title">收货人：</span> <span class="buy-footer-address-detail J_BuyFooterAddressRec"
                                    id="txtLinkShow"></span>
                            </p>
                        </div>
                    </div>
                    <div id="jigsaw36" class="submitOrder">
                        <a id="J_Go" href="javascript:void(0);" class="btn-go " tabindex="0" title="点击此按钮，提交订单">
                            提交订单</a></div>
                    <a href="shoppingtrolleymg.aspx" class="return-cart J_MakePoint" target="_self">返回购物车</a>
                </div>
                <div id="jigsaw37" class="tips steppay-notice">
                </div>
            </div>
        </div>
        </form>
    </div>
    <div class="footer">
        <div class="footmain">
            浙江耐磨达刹车片有限公司 版权所有 2014-2024 Shantui Zhejiang Lamda Brake Pads Co.,Ltd
        </div>
    </div>
</body>
</html>
