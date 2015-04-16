<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ordermg.aspx.cs" Inherits="Chebao.BackAdmin.order.ordermg" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>订单列表</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../js/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#cbxAll").click(function () {
                $(".cbxSub").attr("checked", $(this).attr("checked"));
                setSeldata();
            });
            $(".cbxSub").click(function () {
                setSeldata();
            });
            $(".btngather").click(function () {
                return confirm("确定该订单已收款吗？");
            });
            $(".btnconsignment").click(function () {
                return confirm("确定该订单已发货吗？");
            });
            $(".btncancel").click(function () {
                return confirm("确定要取消该订单吗？");
            });

            $(".txtDate").click(function () {
                WdatePicker({ 'readOnly': 'true', dateFmt: 'yyyy-MM-dd', maxDate: '<%=DateTime.Today.ToString("yyyy-MM-dd") %>' });
            });

            $("#btnFilter").click(function () {
                var query = new Array();
                if ($.trim($("#txtOrderNumber").val()) != "")
                    query[query.length] = "ordernumber=" + $.trim($("#txtOrderNumber").val());
                if ($.trim($("#txtUserName").val()) != "")
                    query[query.length] = "username=" + $.trim($("#txtUserName").val());
                if ($.trim($("#txtLinkName").val()) != "")
                    query[query.length] = "linkname=" + $.trim($("#txtLinkName").val());
                if ($.trim($("#ddlOrderStatus").val()) != "-1")
                    query[query.length] = "orderstatus=" + $.trim($("#ddlOrderStatus").val());
                if ($.trim($("#txtDateB").val()) != "")
                    query[query.length] = "timeb=" + $.trim($("#txtDateB").val());
                if ($.trim($("#txtDateE").val()) != "")
                    query[query.length] = "timee=" + $.trim($("#txtDateE").val());
                location = "?" + (query.length > 0 ? $(query).map(function () {
                    return this;
                }).get().join("&") : "");

                return false;
            });

            $("#btnExportExcel").click(function () {
                if ($(".cbxSub:checked").length == 0) {
                    alert("请选择一条需要导出的订单");
                    return false;
                }
                if ($(".cbxSub:checked").length > 1) {
                    alert("只能选择一条记录导出");
                    return false;
                }
                return true;
            });
        });

        function setSeldata() {
            $("#hdnIds").val($(".cbxSub:checked").map(function () {
                return $(this).attr("value");
            }).get().join(","));
        }

        function CheckForm() {
            var msg = "";
            if ($("#hdnIds").val() == "")
                msg = "请至少选择一条记录";
            if (msg != "") {
                $("#spMsg").text(msg);
                setTimeout(function () {
                    $("#spMsg").text("");
                }, 1000);
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="ht_main">
        <table width="1120" border="0" cellspacing="0" cellpadding="0" class="biaoge4" style="background-color: #f4f8fc;">
            <tr>
                <td class="w40 bold">
                    查询：
                </td>
                <td>
                    订单号：<asp:TextBox runat="server" ID="txtOrderNumber" CssClass="srk6"></asp:TextBox>
                    下单时间：<asp:TextBox runat="server" ID="txtDateB" CssClass="srk6 txtDate"></asp:TextBox>
                    -
                    <asp:TextBox runat="server" ID="txtDateE" CssClass="srk6 txtDate"></asp:TextBox>
                    <div class="pt5">
                        用户名：<asp:TextBox runat="server" ID="txtUserName" CssClass="srk6 w60"></asp:TextBox>
                        联系人姓名：<asp:TextBox runat="server" ID="txtLinkName" CssClass="srk6 w60"></asp:TextBox>
                        订单状态：<asp:DropDownList runat="server" ID="ddlOrderStatus">
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button runat="server" ID="btnExportExcel" CssClass="an1 fll mr10" Text=" 导出Excel "
                        OnClick="btnExportExcel_Click" />
                    <input type="submit" id="btnFilter" class="an1 mr10" value=" 查询 " />
                </td>
            </tr>
        </table>
        <table width="1120" border="0" cellspacing="0" cellpadding="0" class="biaoge2">
            <asp:Repeater ID="rptData" runat="server">
                <HeaderTemplate>
                    <tr class="bgbt">
                        <td class="w160">
                            <label style="line-height: 20px;">
                                <input type="checkbox" id="cbxAll" class="fll" />
                                订单号</label>
                        </td>
                        <td class="w80">
                            下单时间
                        </td>
                        <td class="w60">
                            用户名
                        </td>
                        <td class="w120">
                            联系方式
                        </td>
                        <td class="w200">
                            收货地址
                        </td>
                        <td class="w220">
                            订单产品
                        </td>
                        <td class="w100">
                            订单总额
                        </td>
                        <td class="w60">
                            订单状态
                        </td>
                        <td>
                            操作
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="line-height: 18px;">
                            <input type="checkbox" class="fll cbxSub" value="<%#Eval("ID") %>" /><a href="orderview.aspx?id=<%#Eval("ID") %>&from=<%=UrlEncode(CurrentUrl) %>"
                                style="text-decoration: underline;"><%#Eval("OrderNumber")%></a>
                        </td>
                        <td>
                            <%#Eval("AddTime") %>
                        </td>
                        <td>
                            <%#Eval("UserName")%>
                        </td>
                        <td>
                            联系人：<%#Eval("LinkName")%><br />
                            手机：<%#Eval("LinkMobile")%><br />
                            电话：<%#Eval("LinkTel")%>
                        </td>
                        <td>
                            <%#Eval("Province")%>
                            <%#Eval("City")%>
                            <%#Eval("District")%><br />
                            <%#Eval("Address")%><br />
                            邮编：<%#Eval("PostCode")%>
                        </td>
                        <td>
                            <%# GetOrderProductsStr(Eval("OrderProducts"))%>
                        </td>
                        <td>
                            <%# Chebao.Tools.StrHelper.FormatMoney(Eval("TotalFee").ToString())%>
                        </td>
                        <td>
                            <span class="<%#GetOrderStatusColor((Chebao.Components.OrderStatus)(int)Eval("OrderStatus"))%>">
                                <%# Eval("OrderStatus").ToString()%></span>
                        </td>
                        <td>
                            <a href="?action=gather&ids=<%#Eval("ID") %>&from=<%=UrlEncode(CurrentUrl) %>"
                                class="btngather orange <%#Eval("OrderStatus").ToString() == "未收款" || Eval("OrderStatus").ToString() == "未处理" ? "block" : "hide" %>">
                                收款</a><a href="?action=consignment&ids=<%#Eval("ID") %>&from=<%=UrlEncode(CurrentUrl) %>"
                                    class="btnconsignment green pt5 <%#Eval("OrderStatus").ToString() == "已收款" || Eval("OrderStatus").ToString() == "未处理" ? "block" : "hide" %>">
                                    发货</a><a href="?action=cancel&ids=<%#Eval("ID") %>&from=<%=UrlEncode(CurrentUrl) %>"
                                        class="btncancel red pt5 <%#Eval("OrderStatus").ToString() == "已取消" ? "hide" : "block" %>">
                                        取消</a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <webdiyer:AspNetPager ID="search_fy" UrlPaging="true" NextPageText="下一页" PrevPageText="上一页"
            CurrentPageButtonClass="current" PageSize="10" runat="server" NumericButtonType="Text"
            MoreButtonType="Text" ShowFirstLast="false" HorizontalAlign="Left" AlwaysShow="false"
            ShowCustomInfoSection="Right" ShowDisabledButtons="False" PagingButtonSpacing="">
        </webdiyer:AspNetPager>
        <div class="lan5x">
            <input id="hdnIds" runat="server" type="hidden" />
            <span id="spMsg" class="red"></span>
        </div>
    </div>
    </form>
</body>
</html>
