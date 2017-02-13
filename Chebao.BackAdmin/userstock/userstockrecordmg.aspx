<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userstockrecordmg.aspx.cs" Inherits="Chebao.BackAdmin.userstock.userstockrecordmg" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>出入库记录</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../js/comm.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#btnFilter").click(function () {
                var query = new Array();
                if ($.trim($("#txtUserName").val()) != "")
                    query[query.length] = "n=" + $.trim($("#txtUserName").val());
                location = "?" + (query.length > 0 ? $(query).map(function () {
                    return this;
                }).get().join("&") : "");

                return false;
            });
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="ht_main" style="padding-right: 20px;">
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="biaoge4" style="background-color: #f4f8fc;">
            <tr>
                <td class="w40 bold">
                    查询：
                </td>
                <td>
                    用户：<asp:TextBox runat="server" ID="txtUserName" CssClass="srk6 w60"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <input type="submit" id="btnFilter" class="an1 mr10" value=" 查询 " />
                    <span id="spMsg" class="red"></span>
                </td>
            </tr>
        </table>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="biaoge2">
            <asp:Repeater ID="rptData" runat="server">
                <HeaderTemplate>
                    <tr class="bgbt">
                        <td class="w100">
                            用户
                        </td>
                        <td class="w120">
                            提交时间
                        </td>
                        <td class="w60">
                            出/入库
                        </td>
                        <td class="w200">
                            备注
                        </td>
                        <td>
                            产品
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%#Eval("UserName")%>
                        </td>
                        <td>
                            <%#Eval("AddTime") %>
                        </td>
                        <td>
                            <%#Eval("Action").ToString() == "0" ? "出库" : "入库"%>
                        </td>
                        <td>
                            <span class="gray"><%# string.IsNullOrEmpty(Eval("SysRemark").ToString()) ? string.Empty : (Eval("SysRemark").ToString() + " - ")%></span><%#Eval("Remark") %>
                        </td>
                        <td>
                            <%# GetOrderProductsStr(Eval("OrderProducts"))%>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr style="background: #F9F4C7;">
                        <td>
                            <%#Eval("UserName")%>
                        </td>
                        <td>
                            <%#Eval("AddTime") %>
                        </td>
                        <td>
                            <%#Eval("Action").ToString() == "0" ? "出库" : "入库"%>
                        </td>
                        <td>
                            <span class="gray"><%# string.IsNullOrEmpty(Eval("SysRemark").ToString()) ? string.Empty : (Eval("SysRemark").ToString() + " - ")%></span><%#Eval("Remark") %>
                        </td>
                        <td>
                            <%# GetOrderProductsStr(Eval("OrderProducts"))%>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
        </table>
        <webdiyer:AspNetPager ID="search_fy" UrlPaging="true" NextPageText="下一页" PrevPageText="上一页"
            CurrentPageButtonClass="current" PageSize="10" runat="server" NumericButtonType="Text"
            MoreButtonType="Text" ShowFirstLast="false" HorizontalAlign="Left" AlwaysShow="false"
            ShowCustomInfoSection="Right" ShowDisabledButtons="False" PagingButtonSpacing="">
        </webdiyer:AspNetPager>
        <div class="lan5x">
            <input id="hdnIds" runat="server" type="hidden" />
        </div>
    </div>
    </form>
</body>
</html>
