<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="stockchange.aspx.cs" Inherits="Chebao.BackAdmin.useradmin.stockchange" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>耐磨达产品查询系统</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <link href="../css/kp.css" rel="stylesheet" type="text/css" />
    <link href="../css/headfoot2.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../js/comm.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.7.3.custom.min.js" type="text/javascript"></script>
    <link href="../css/newstart/jquery-ui-1.7.3.custom.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">
        var diagbuynumber;
        $(function () {
            diagbuynumber = $("#buynumber"); //弹出区域显示框
            diagbuynumber.dialog({
                width: 300,
                resizable: false,
                autoOpen: false,
                title: '设置数量'
            });
            $("#btnAddProduct").click(function () {
                var left = $(this).offset().left - $("body").scrollLeft();
                if (($(this).offset().left + 300) > $("body").width()) left = left - 300;
                var top = $(this).offset().top - $("body").scrollTop() + 30;
                $.ajax({
                    url: "productmix.aspx",
                    data: { mnumber: $("#txtModelNumber").val(),t:<%=GetInt("t") %>, d: new Date() },
                    type: 'GET',
                    dataType: "text",
                    error: function (msg) {
                        alert("发生错误");
                    },
                    success: function (data) {
                        if (data == "") {
                            alert("产品型号错误");
                        } else {
                            $("#buybody").html(data);
                            BindBuynumberEvent();
                            diagbuynumber.dialog('option', 'position', [left, top]);
                            diagbuynumber.dialog("open");
                        }
                    }
                });
            });
            $("#btnBuysubmit").click(function () {
                var hassetamount = false;
                var productmix = $(".J_ItemAmount").map(function () {
                    if(parseInt($(this).val()) > 0) hassetamount = true;
                    return $(this).attr("data-name") + "," + $(this).val();
                }).get().join("|");
                if(!hassetamount) {
                    alert("请设置数量");
                    return;
                }
                $.ajax({
                    url: "/remoteaction.ashx",
                    data: { action: "adduserstockchange", mnumber: $("#txtModelNumber").val(),t:<%=GetInt("t") %>, productmix: productmix, d: new Date() },
                    type: 'GET',
                    dataType: "json",
                    error: function (msg) {
                        alert("发生错误");
                    },
                    success: function (data) {
                        if (data.Value == "success") {
                            var productmixstr = $(".J_ItemAmount").map(function () {
                                if(parseInt($(this).val()) > 0) hassetamount = true;
                                return $(this).attr("data-name") + " × " + $(this).val();
                            }).get().join("<br />");
                            $("#tbData").append("<tr><td><a href=\"javascript:void(0)\" class=\"btndelpro red\" style=\"text-decoration:none;\">删除</a></td>"
                                    + "<td>" + $("#txtModelNumber").val() + "</td>"
                                    + "<td>" + productmixstr  + "</td></tr>");
                            diagbuynumber.dialog("close");
                            iFrameHeight(top.document.getElementById("ibody"));
                        }
                        else {
                            alert(data.Msg);
                        }
                    }
                });
            });
            $("#btnSubmit").click(function(){
                if($(".btndelpro").length == 0){
                    alert("未添加产品");
                    return false;
                }
                return true;
            });
        });

        function BindBuynumberEvent() {
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
            });
        }

        function CheckAmount(b) {
            var amount = parseInt($(b).val());
            var stock = parseInt($(b).attr("data-max"));
            if (amount < 0 || !amount || stock == 0) {
                $(b).val(0);
            }
            else if (stock < amount && stock > 0) {
                $(b).val(stock);
                $(b).parent().next().html("<em class=\"error-msg\">最多只可输入" + stock + "件</em>");
                setTimeout(function () {
                    $(b).parent().next().html("");
                }, 1000);
            }
            else {
                $(b).val(amount);
            }
            //            if (stock == 0)
            //                $(b).val(0);
            if (parseInt($(b).val()) < 1) {
                if (!$(b).prev().hasClass("no-minus")) {
                    $(b).prev().addClass("no-minus");
                }
                if ($(b).prev().hasClass("minus")) {
                    $(b).prev().removeClass("minus");
                }
                $(b).prev().unbind("click");
            }
            if (parseInt($(b).val()) == stock) {
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
        }
    </script>
</head>
<body style="width: 100%; min-height: 500px; background: #fff;">
    <form id="form1" runat="server">
    <div class="nav_stockmg">
        <ul>
            <li><a href="stockmg.aspx">出入库记录</a></li><!--
            --><li<%=GetInt("t") == 0 ? " class=\"current_stockmg\"" : "" %>><a href="stockchange.aspx?t=0">出库</a></li><!--
            --><li<%=GetInt("t") == 1 ? " class=\"current_stockmg\"" : "" %>><a href="stockchange.aspx?t=1">入库</a></li>
        </ul>
    </div>
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="biaoge3">
        <caption class="bt2">
            <%=GetInt("t") == 1 ? "入库申请" : "出库申请" %></caption>
        <tbody>
            <tr>
                <td class="bg1">
                    <%=GetInt("t") == 0 ? "出库" : "入库" %>说明
                </td>
                <td>
                   <input type="text" id="txtRemark" runat="server" class="srk3" />
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    产品型号：
                </td>
                <td>
                    <input type="text" id="txtModelNumber" class="srk5" />
                    <input type="button" id="btnAddProduct" value="添加" class="an1" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <table id="tbData" width="100%" border="0" cellspacing="0" cellpadding="0" class="biaoge2">
                        <asp:Repeater ID="rptData" runat="server">
                            <HeaderTemplate>
                                <tr class="bgbt">
                                    <td class="w60">
                                        操作
                                    </td>
                                    <td class="w100">
                                        产品型号
                                    </td>
                                    <td>
                                        <%=GetInt("t") == 1 ? "入库" : "出库" %>详情
                                    </td>
                                </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <a href="javascript:void(0)" class="btndelpro red" style="text-decoration: none;">删除</a>
                                    </td>
                                    <td>
                                        <%# Eval("ModelNumber")%>
                                    </td>
                                    <td>
                                        <%# GetProductMix(Eval("ProductMix").ToString()) %>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    &nbsp;
                </td>
                <td>
                    <asp:Button runat="server" ID="btnSubmit" CssClass="an1" Text="提交" OnClick="btnSubmit_Click" />
                </td>
            </tr>
        </tbody>
    </table>
    <div id="buynumber" style="display: none;">
        <div id="buybody">
        </div>
        <div id="buysubmit" style="padding: 10px;">
            <input type="button" id="btnBuysubmit" value="确定" class="an1" />
        </div>
    </div>
    </form>
</body>
</html>
