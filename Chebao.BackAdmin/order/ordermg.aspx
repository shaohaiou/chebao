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
        var copyorderid = 0;
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
            $(".btnconsignment,.btncancel").click(function () {
                if (confirm($(this).attr("msg"))) {
                    $(".overlay").css({ 'display': 'block', 'opacity': '0.8' });
                    $("#AjaxLoading").stop(true).animate({ 'margin-top': '200px', 'opacity': '1' }, 200);
                    $("#hdnAction").val($(this).attr("action"));
                    $("#hdnId").val($(this).attr("vid"));
                    form1.submit();
                } else
                    return false;
            });
            if ($("#hdnSyncresult").val() != "") {
                alert($("#hdnSyncresult").val());
            }

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

            $(".btncopy").click(function () {
                copyorderid = $(this).attr("data-id");
                $("#txtUserNameCopy").val($(this).attr("data-uname"));
                $(".overlay").css({ 'display': 'block', 'opacity': '0.8' });
                $("#copyflay").stop(true).animate({ 'margin-top': '200px', 'opacity': '1' }, 200);
            });
            $(".btncopyclose").click(function () {
                $(".overlay").css({ 'display': 'none', 'opacity': '0.8' });
                $("#copyflay").stop(true).animate({ 'margin-top': '0px', 'opacity': '0' }, 200);
            });
            $("#btnSubmitCopy").click(function () {
                window.open("copyorder.aspx?id=" + copyorderid + "&uname=" + $("#txtUserNameCopy").val());
                $(".overlay").css({ 'display': 'none', 'opacity': '0.8' });
                $("#copyflay").stop(true).animate({ 'margin-top': '0px', 'opacity': '0' }, 200);
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
    <style type="text/css">
        a, img
        {
            border: 0;
        }
        .demo
        {
            margin: 100px auto 0 auto;
            width: 400px;
            text-align: center;
            font-size: 18px;
        }
        .demo .action
        {
            color: #3366cc;
            text-decoration: none;
            font-family: "微软雅黑" , "宋体";
        }
        
        .overlay
        {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            z-index: 998;
            width: 100%;
            height: 100%;
            _padding: 0 20px 0 0;
            background: #f6f4f5;
            display: none;
        }
        .showbox
        {
            position: fixed;
            top: 0;
            left: 50%;
            z-index: 9999;
            opacity: 0;
            filter: alpha(opacity=0);
            margin-left: -80px;
        }
        *html, *html body
        {
            background-image: url(about:blank);
            background-attachment: fixed;
        }
        *html .showbox, *html .overlay
        {
            position: absolute;
            top: expression(eval(document.documentElement.scrollTop));
        }
        #AjaxLoading
        {
            border: 1px solid #8CBEDA;
            color: #37a;
            font-size: 12px;
            font-weight: bold;
        }
        #AjaxLoading div.loadingWord
        {
            width: 240px;
            height: 50px;
            line-height: 50px;
            border: 2px solid #D6E7F2;
            background: #fff;
        }
        #AjaxLoading img
        {
            margin: 10px 15px;
            float: left;
            display: inline;
        }
        li
        {
            line-height: 32px;
        }
    </style>
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
                    <tr class="<%#Eval("OrderStatus").ToString() == "已取消" ? GetOrderStatusColor((Chebao.Components.OrderStatus)(int)Eval("OrderStatus")) : ""%>">
                        <td style="line-height: 18px;">
                            <input type="checkbox" class="fll cbxSub" value="<%#Eval("ID") %>" /><a href="orderview.aspx?id=<%#Eval("ID") %>&from=<%=UrlEncode(CurrentUrl) %>"
                                style="text-decoration: underline;" class="<%#Eval("OrderStatus").ToString() == "已取消" ? GetOrderStatusColor((Chebao.Components.OrderStatus)(int)Eval("OrderStatus")) : ""%>"><%#Eval("OrderNumber")%></a>
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
                            <div style="max-height: 230px; overflow: hidden;">
                                <%# GetOrderProductsStr(Eval("OrderProducts"))%>
                            </div>
                        </td>
                        <td>
                            <%# Chebao.Tools.StrHelper.FormatMoney(Eval("TotalFee").ToString())%>
                        </td>
                        <td>
                            <span class="<%#GetOrderStatusColor((Chebao.Components.OrderStatus)(int)Eval("OrderStatus"))%>">
                                <%# Eval("OrderStatus").ToString()%></span>
                        </td>
                        <td>
                            <a href="?action=gather&ids=<%#Eval("ID") %>&from=<%=UrlEncode(CurrentUrl) %>" class="btngather orange <%#Eval("OrderStatus").ToString() == "未收款" || Eval("OrderStatus").ToString() == "未处理" ? "block" : "hide" %>">
                                收款</a><a href="javascript:void(0);" action="consignment" vid="<%#Eval("ID") %>" msg="确定该订单已发货吗？"
                                    class="btnconsignment green pt5 <%#Eval("OrderStatus").ToString() == "已收款" || Eval("OrderStatus").ToString() == "未处理" ? "block" : "hide" %>">
                                    发货</a><a href="javascript:void(0);" action="cancel" vid="<%#Eval("ID") %>" msg="确定要取消该订单吗？"
                                        class="btncancel red pt5 <%#Eval("OrderStatus").ToString() == "已取消" ? "hide" : "block" %>">
                                        取消</a><a href="javascript:void(0);" target="_blank" class="green btncopy" data-id="<%#Eval("ID") %>"
                                data-uname="<%#Eval("UserName") %>">复制</a>
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
            <span id="spMsg" class="red"></span>
        </div>
    </div>
    </form>
    <div class="overlay">
    </div>
    <div id="AjaxLoading" class="showbox">
        <div class="loadingWord">
            <img src="/images/waiting.gif">正在处理，请稍候...</div>
    </div>
    <div id="copyflay" class="showbox" style="background: wheat;padding:3px;">
        <div style="line-height:24px;font-weight:bold;padding:0 3px;background: #aaa;">
            订单复制<a style="float:right;text-decoration:none;" class="btncopyclose" href="javascript:void(0)">[关闭]</a>
        </div>
        <div style="width:160px;">
            <ul>
                <li class="fll">用户名：</li>
                <li><input type="text" class="srk6 w60" id="txtUserNameCopy" /></li>
            </ul>
            <input type="button" value="确定" id="btnSubmitCopy" />
        </div>
    </div>
</body>
</html>
