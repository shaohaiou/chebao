<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="orders.aspx.cs" Inherits="Chebao.BackAdmin.useradmin.orders" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>耐磨达产品查询系统</title>
    <link href="../css/headfoot2.css" rel="stylesheet" type="text/css" />
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../js/comm.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            $(".btngather").click(function () {
                return confirm("确定该订单已收款吗？");
            });
            $(".btnconsignment,.btncancel").click(function () {
                if (confirm($(this).attr("msg"))) {
                    showLoading("正在处理，请稍候...");
                    $("#hdnAction").val($(this).attr("action"));
                    $("#hdnId").val($(this).attr("vid"));
                    form1.submit();
                } else
                    return false;
            });
            $("#btnFilter").click(function () {
                var query = new Array();
                if ($.trim($("#ddlOrderStatusFilter").val()) != "-1")
                    query[query.length] = "s=" + $.trim($("#ddlOrderStatusFilter").val());
                if ($.trim($("#txtOrderNumberFilter").val()) != "")
                    query[query.length] = "n=" + $.trim($("#txtOrderNumberFilter").val());
                if ($.trim($("#txtUserNameFilter").val()) != "")
                    query[query.length] = "u=" + $.trim($("#txtUserNameFilter").val());
                location = "?" + (query.length > 0 ? $(query).map(function () {
                    return this;
                }).get().join("&") : "");

                return false;
            });
        })
    </script>
</head>
<body style="width:100%;min-height:500px;background:#fff;">
    <form id="form1" runat="server">
    <div class="filter">
        <div class="fbody">
            订单号：<input type="text" id="txtOrderNumberFilter" runat="server" class="srk5 mr10 w80 txt" />
            &nbsp;&nbsp;&nbsp;用户名：<input type="text" runat="server" id="txtUserNameFilter" class="srk5 mr10 w80 txt" />
            &nbsp;&nbsp;&nbsp;订单状态：<asp:DropDownList ID="ddlOrderStatusFilter" runat="server"></asp:DropDownList>
            &nbsp;&nbsp;&nbsp;<input type="submit" id="btnFilter" class="an1 mr10" value=" 查询 " />
        </div>
    </div>
    <div class="tbbody">
        <table border="0" cellspacing="0" cellpadding="0" class="biaoge2">
            <asp:Repeater ID="rptData" runat="server">
                <HeaderTemplate>
                    <tr class="bgbt">
                        <td>
                            订单号/下单时间
                        </td>
                        <td class="w160">
                            联系方式
                        </td>
                        <td class="w300">
                            订单产品
                        </td>
                        <td class="w70">
                            订单总额
                        </td>
                        <td class="w80">
                            状态/操作人
                        </td>
                        <td class="w60">
                            操作
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="<%#Eval("OrderStatus").ToString() == "已取消" ? GetOrderStatusColor((Chebao.Components.OrderStatus)(int)Eval("OrderStatus")) : ""%>">
                        <td style="line-height: 18px;">
                            <a href="orderview.aspx?id=<%#Eval("ID") %>&from=<%=UrlEncode(CurrentUrl) %>"
                                style="text-decoration: underline;" class="<%#Eval("OrderStatus").ToString() == "已取消" ? GetOrderStatusColor((Chebao.Components.OrderStatus)(int)Eval("OrderStatus")) : ""%>"><%#Eval("OrderNumber")%></a>
                                <br />
                            <%#Eval("AddTime") %>
                        </td>
                        <td>
                            用户名：<%#Eval("UserName")%><br />
                            联系人：<%#Eval("LinkName")%><br />
                            手机：<%#Eval("LinkMobile")%><br />
                            电话：<%#Eval("LinkTel")%><br />
                            <%#Eval("Province")%>
                            <%#Eval("City")%>
                            <%#Eval("District")%><br />
                            <%#Eval("Address")%><br />
                            邮编：<%#Eval("PostCode")%>
                        </td>
                        <td>
                            <div style="max-height: 230px; overflow: hidden;">
                                <%# GetOrderProductsStr(Eval("OrderProducts"))%>
                            </div>
                        </td>
                        <td>
                            ¥<%# Chebao.Tools.StrHelper.FormatMoney(Eval("TotalFee").ToString())%>
                        </td>
                        <td>
                            <span class="<%#GetOrderStatusColor((Chebao.Components.OrderStatus)(int)Eval("OrderStatus"))%>">
                                <%# Eval("OrderStatus").ToString()%></span><br />
                                <%#Eval("StatusUpdateUser")%>
                        </td>
                        <td>
                            <a href="?action=gather&ids=<%#Eval("ID") %>&from=<%=UrlEncode(CurrentUrl) %>" class="btngather orange <%#Eval("OrderStatus").ToString() == "未收款" || Eval("OrderStatus").ToString() == "未处理" ? "block" : "hide" %>">
                                收款</a><a href="javascript:void(0);" action="consignment" vid="<%#Eval("ID") %>" msg="确定该订单已发货吗？"
                                    class="btnconsignment green pt5 <%#Eval("OrderStatus").ToString() == "已收款" || Eval("OrderStatus").ToString() == "未处理" ? "block" : "hide" %>">
                                    发货</a><a href="javascript:void(0);" action="cancel" vid="<%#Eval("ID") %>" msg="确定要取消该订单吗？"
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
            <input id="hdnSyncresult" runat="server" type="hidden" />
            <input id="hdnAction" runat="server" type="hidden" />
            <input id="hdnId" runat="server" type="hidden" />
            <input id="hdnFrom" runat="server" type="hidden" />
        </div>
    </div>
    </form>
</body>
</html>
