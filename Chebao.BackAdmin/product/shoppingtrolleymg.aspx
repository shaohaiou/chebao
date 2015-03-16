﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="shoppingtrolleymg.aspx.cs"
    Inherits="Chebao.BackAdmin.product.shoppingtrolleymg" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>耐磨达产品查询系统-购物车</title>
    <link href="../css/headfoot2.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <link href="../css/kp.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(".cart-checkbox label").click(function () {
                $(this).parent().toggleClass("cart-checkbox-checked");
                if ($(this).parent().hasClass("cart-checkbox-checked")) {
                    $(this).prev().attr("checked", true);
                } else {
                    $(this).prev().attr("checked", false);
                }
                Account();

                var notcheckedcount = $(".cart-checkbox input[type='checkbox']", $(".item-content")).not($(".cart-checkbox input[type='checkbox']:checked", $(".item-content"))).length;
                if (notcheckedcount == 0) {
                    $("#J_SelectAll1 .cart-checkbox,#J_SelectAll2 .cart-checkbox").each(function () {
                        if (!$(this).hasClass("cart-checkbox-checked"))
                            $(this).addClass("cart-checkbox-checked")
                    });
                } else {
                    $("#J_SelectAll1 .cart-checkbox,#J_SelectAll2 .cart-checkbox").each(function () {
                        if ($(this).hasClass("cart-checkbox-checked"))
                            $(this).removeClass("cart-checkbox-checked")
                    });
                }
            });

            $(".J_SelectAll").click(function () {
                $(".cart-checkbox", $(this)).toggleClass("cart-checkbox-checked");
                var haschecked = $(".cart-checkbox", $(this)).hasClass("cart-checkbox-checked");
                $(".cart-checkbox").not($(".cart-checkbox", $(this))).each(function () {
                    if (!$(this).hasClass("cart-checkbox-checked") && haschecked) {
                        $(this).addClass("cart-checkbox-checked")
                        $("input[type='checkbox']", $(this)).attr("checked", true);
                    } else if (!haschecked && $(this).hasClass("cart-checkbox-checked")) {
                        $(this).removeClass("cart-checkbox-checked")
                        $("input[type='checkbox']", $(this)).attr("checked", false);
                    }
                });
                Account();
            });

            $(".J_DeleteSelected").click(function () {
                if (confirm("确认要删除这些宝贝吗？")) {
                    var ids = $(".cart-checkbox input[type='checkbox']:checked", $(".item-content")).map(function () {
                        return $(this).val();
                    }).get().join(",");
                    if (ids != "")
                        location.href = "?action=del&ids=" + ids;
                }
            });

            $(".J_Del").click(function () {
                if (confirm("确认要删除该宝贝吗？")) {
                    var ids = $(this).attr("val");
                    if (ids != "")
                        location.href = "?action=del&ids=" + ids;
                }
            });

            $(".J_Plus").click(function () {
                var amount = parseInt($(this).prev().val());
                $(this).prev().val(amount + 1);
                CheckAmount($(this).prev());
            });
            $(".J_Minus").click(function () {
                var amount = parseInt($(this).next().val());
                $(this).next().val(amount - 1);
                CheckAmount($(this).next());
            });
            $(".J_ItemAmount").change(function () {
                CheckAmount(this);
            });
            $(".J_ItemAmount").keyup(function () {
                CheckAmount(this);
            });

            $(".J_ItemAmount").each(function () {
                CheckAmount(this);
                var amount = parseInt($(this).val());
                var stock = parseInt($(this).attr("data-max"));
                var price = $("#txtprice" + $(this).attr("data-id")).html();
                var sum = parseFloat(price) * parseInt($(this).val());
                $("#txtsum" + $(this).attr("data-id")).html(sum.toFixed(2));
            });
            $("#J_Go").click(function () {
                if ($(".cart-checkbox input[type='checkbox']:checked", $(".item-content")).length == 0) {
                    alert("请选择要结算的宝贝");
                    return;
                }
                form1.submit();
            });

            setTimeout(function () {
                Account();
            }, 1000);
        })

        function CheckAmount(b) {
            var amount = parseInt($(b).val());
            var stock = parseInt($(b).attr("data-max"));
            var price = $("#txtprice" + $(b).attr("data-id")).html();
            if (amount <= 0 || !amount) {
                $(b).val(1);
            }
            else if (stock < amount) {
                $(b).val(stock);
                $(b).parent().next().html("<em class=\"error-msg\">最多只可购买" + stock + "件</em>");
                setTimeout(function () {
                    $(b).parent().next().html("");
                }, 1000);
            }
            else {
                $(b).val(amount);
            }
            if (stock == 0)
                $(b).val(0);
            var sum = parseFloat(price) * parseInt($(b).val());
            $("#txtsum" + $(b).attr("data-id")).html(sum.toFixed(2));
            if (parseInt($(b).val()) == 1) {
                if (!$(b).prev().hasClass("no-minus")) {
                    $(b).prev().addClass("no-minus");
                }
                if ($(b).prev().hasClass("minus")) {
                    $(b).prev().removeClass("minus");
                }
                $(b).prev().unbind("click");
            } else if (parseInt($(b).val()) == stock) {
                if (!$(b).next().hasClass("no-plus")) {
                    $(b).next().addClass("no-plus");
                }
                if ($(b).next().hasClass("plus")) {
                    $(b).next().removeClass("plus");
                }
                $(b).next().unbind("click");
            }
            if (parseInt($(b).val()) > 1) {
                $(b).prev().unbind("click");
                $(b).prev().click(function () {
                    var amount = parseInt($(b).val());
                    $(b).val(amount - 1);
                    CheckAmount(b);
                });
                if ($(b).prev().hasClass("no-minus")) {
                    $(b).prev().removeClass("no-minus");
                }
                if (!$(b).prev().hasClass("minus")) {
                    $(b).prev().addClass("minus");
                }
            }
            if (parseInt($(b).val()) < stock) {
                $(b).next().unbind("click");
                $(b).next().click(function () {
                    var amount = parseInt($(b).val());
                    $(b).val(amount + 1);
                    CheckAmount(b);
                });
                if ($(b).next().hasClass("no-plus")) {
                    $(b).next().removeClass("no-plus");
                }
                if (!$(b).next().hasClass("plus")) {
                    $(b).next().addClass("plus");
                }
            }
            Account();
        }

        function Account() {
            $("#J_SelectedItemsCount").html($(".cart-checkbox input[type='checkbox']:checked", $(".item-content")).length);
            var account = 0.00;
            $(".cart-checkbox input[type='checkbox']:checked", $(".item-content")).each(function () {
                var id = $(this).val();
                account += parseFloat($("#txtsum" + id).html());
            });
            $("#J_Total").html(account.toFixed(2));
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
        <form runat="server" id="form1">
        <div id="J_Cart" class="cart">
            <div id="J_CartMain" class="cart-main">
                <div class="cart-table-th">
                    <div class="wp">
                        <div class="th th-chk">
                            <div id="J_SelectAll1" class="select-all J_SelectAll">
                                <div class="cart-checkbox">
                                    <input class="J_CheckBoxShop" id="J_SelectAllCbx1" type="checkbox" name="select-all" /><label>勾选购物车内所有商品</label></div>
                                &nbsp;&nbsp;全选</div>
                        </div>
                        <div class="th th-item">
                            <div class="td-inner">
                                商品信息</div>
                        </div>
                        <div class="th th-info">
                            <div class="td-inner">
                                &nbsp;</div>
                        </div>
                        <div class="th th-price">
                            <div class="td-inner">
                                单价（元）</div>
                        </div>
                        <div class="th th-amount">
                            <div class="td-inner">
                                数量</div>
                        </div>
                        <div class="th th-sum">
                            <div class="td-inner">
                                金额（元）</div>
                        </div>
                        <div class="th th-op">
                            <div class="td-inner">
                                操作</div>
                        </div>
                    </div>
                </div>
                <div id="J_OrderList">
                    <div id="J_OrderHolder_s_90766069_1" style="height: auto;">
                        <div id="J_Order_s_90766069_1" class="J_Order clearfix order-body  all-select">
                            <div class="order-content">
                                <div id="J_BundleList_s_90766069_1" class="item-list">
                                    <div id="J_Bundle_s_90766069_1_0" class="bundle  bundle-last ">
                                        <asp:Repeater runat="server" ID="rptData" OnItemDataBound="rptData_ItemDataBound">
                                            <ItemTemplate>
                                                <div id="J_ItemHolder_104672630049">
                                                    <div id="J_Item_104672630049" class="J_ItemBody item-body clearfix item-normal  first-item  last-item    selected item-selected ">
                                                        <ul class="item-content clearfix">
                                                            <li class="td td-chk">
                                                                <div class="td-inner">
                                                                    <div class="cart-checkbox">
                                                                        <input class="J_CheckBoxItem" id="cbxSelect" runat="server" type="checkbox" value='<%#Eval("SID") %>' /><label>勾选商品</label></div>
                                                                </div>
                                                                <input type="hidden" runat="server" id="hdnModelNumber" value='<%#Eval("ModelNumber") %>' />
                                                                <input type="hidden" runat="server" id="hdnOEModelNumber" value='<%#Eval("OEModelNumber") %>' />
                                                                <input type="hidden" runat="server" id="hdnStandard" value='<%#Eval("Standard") %>' />
                                                                <input type="hidden" runat="server" id="hdnPrice" value='<%#decimal.Parse(Eval("Price").ToString().StartsWith("¥") ? Eval("Price").ToString().Substring(1) : Eval("Price").ToString()) %>' />
                                                                <input type="hidden" runat="server" id="hdnName" value='<%#Eval("Name") %>' />
                                                                <input type="hidden" runat="server" id="hdnPic" value='<%#Eval("Pic") %>' />
                                                                <input type="hidden" runat="server" id="hdnSID" value='<%#Eval("SID") %>' />
                                                                <input type="hidden" runat="server" id="hdnID" value='<%#Eval("ID") %>' />
                                                            </li>
                                                            <li class="td td-item">
                                                                <div class="td-inner">
                                                                    <div class="item-pic J_ItemPic img-loaded">
                                                                        <a href="productview.aspx?id=<%#Eval("ID") %>" target="_blank" title="<%# Eval("Name")%>"
                                                                            class="J_MakePoint">
                                                                            <img src="<%#Eval("Pic") %>" class="itempic J_ItemImg"></a></div>
                                                                    <div class="item-info">
                                                                        <div class="item-basic-info">
                                                                            <a href="productview.aspx?id=<%#Eval("ID") %>" target="_blank" title="<%# Eval("Name")%>"
                                                                                class="item-title J_MakePoint">
                                                                                <%# Eval("Name")%></a>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </li>
                                                            <li class="td td-info">
                                                                <div class="item-props ">
                                                                </div>
                                                            </li>
                                                            <li class="td td-price">
                                                                <div class="td-inner">
                                                                    <div class="item-price price-promo-promo">
                                                                        <div class="price-content">
                                                                            <div class="price-line">
                                                                                <em class="J_Price price-now" id="txtprice<%# Eval("SID") %>">
                                                                                    <%# Math.Round(decimal.Parse(Eval("Price").ToString().StartsWith("¥") ? Eval("Price").ToString().Substring(1) : Eval("Price").ToString()),2)%></em></div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </li>
                                                            <li class="td td-amount">
                                                                <div class="td-inner">
                                                                    <div class="amount-wrapper ">
                                                                        <div class="item-amount ">
                                                                            <a href="javascript:void(0);" class="J_Minus minus">-</a><input type="text" value=""
                                                                                id="txtAmount" runat="server" class="text text-amount J_ItemAmount" data-max=""
                                                                                data-id="" /><a href="javascript:void(0);" class="J_Plus plus">+</a></div>
                                                                        <div class="amount-msg J_AmountMsg">
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </li>
                                                            <li class="td td-sum">
                                                                <div class="td-inner">
                                                                    <em class="J_ItemSum number" id="txtsum<%# Eval("SID") %>"></em>
                                                                    <div class="J_ItemLottery">
                                                                    </div>
                                                                </div>
                                                            </li>
                                                            <li class="td td-op">
                                                                <div class="td-inner">
                                                                    <a href="javascript:void(0);" val="<%#Eval("SID") %>" class="J_Del J_MakePoint">删除</a></div>
                                                            </li>
                                                        </ul>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="J_FloatBarHolder" class="float-bar-holder">
            </div>
            <div id="J_FloatBar" class="float-bar clearfix has-items fixed-bottom" style="position: relative;">
                <div id="J_SelectedItems" class="group-wrapper group-popup hidden" style="display: none;">
                    <div id="J_SelectedItemsList" class="group-content">
                    </div>
                    <span class="arrow" style="left: 636.453125px;"></span>
                </div>
                <div class="float-bar-wrapper">
                    <div id="J_SelectAll2" class="select-all J_SelectAll">
                        <div class="cart-checkbox">
                            <input class="J_CheckBoxShop" id="J_SelectAllCbx2" type="checkbox" name="select-all" /><label
                                for="J_SelectAllCbx2">勾选购物车内所有商品</label></div>
                        &nbsp;全选</div>
                    <div class="operations">
                        <a href="javascript:void(0);" class="J_DeleteSelected">删除</a></div>
                    <div class="float-bar-right">
                        <div id="J_ShowSelectedItems" class="amount-sum">
                            <span class="txt">已选商品</span><em id="J_SelectedItemsCount"></em><span class="txt">件</span>
                        </div>
                        <div class="pipe">
                        </div>
                        <div class="price-sum">
                            <span class="txt">合计：</span><strong class="price">¥<em id="J_Total"></em></strong></div>
                        <div class="btn-area">
                            <a href="javascript:void(0)" id="J_Go" class="submit-btn"><span>结&nbsp;算</span><b></b></a></div>
                    </div>
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