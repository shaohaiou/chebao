<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userstockcheck.aspx.cs"
    Inherits="Chebao.BackAdmin.userstock.userstockcheck" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>盘库审核</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../js/comm.js" type="text/javascript"></script>
    <script type="text/javascript">
    
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
                    用户名：<asp:TextBox runat="server" ID="txtUserName" CssClass="srk6 w60"></asp:TextBox>
                    审核状态：<asp:DropDownList runat="server" ID="ddlCheckStatus">
                        <asp:ListItem Text="-请选择-" Value=""></asp:ListItem>
                        <asp:ListItem Text="未审核" Value="0"></asp:ListItem>
                        <asp:ListItem Text="审核通过" Value="1"></asp:ListItem>
                        <asp:ListItem Text="审核不通过" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <input type="submit" id="btnFilter" class="an1 mr10" value=" 查询 " />
                    <input type="button" class="an1 mr10 btnmulpass" value="审核通过" />
                    <input type="button" class="an1 mr10 btnmulunpass" value="审核不通过" />
                    <span id="spMsg" class="red"></span>
                </td>
            </tr>
        </table>
        <table width="1120" border="0" cellspacing="0" cellpadding="0" class="biaoge2">
            <asp:Repeater ID="rptData" runat="server">
                <HeaderTemplate>
                    <tr class="bgbt">
                        <td class="w100">
                            <label style="line-height: 20px;">
                                <input type="checkbox" id="cbxAll" class="fll" />
                                申请人</label>
                        </td>
                        <td class="w120">
                            申请时间
                        </td>
                        <td class="w60">
                            出/入库
                        </td>
                        <td class="w200">
                            备注
                        </td>
                        <td class="w200">
                            产品
                        </td>
                        <td class="w200">
                            审核情况
                        </td>
                        <td>
                            操作
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="line-height: 18px;">
                            <input type="checkbox" class="fll cbxSub" value="<%#Eval("ID") %>" />
                            <%#Eval("UserName")%>
                        </td>
                        <td>
                            <%#Eval("AddTime") %>
                        </td>
                        <td>
                            <%#Eval("Action").ToString() == "0" ? "出库" : "入库"%>
                        </td>
                        <td>
                            <%#Eval("Remark") %>
                        </td>
                        <td>
                            <%# GetOrderProductsStr(Eval("OrderProducts"))%>
                        </td>
                        <td>
                            审核状态：<%# Eval("CheckStatus").ToString() == "0" ? "<span class=\"gray\">未审核</span>" : (Eval("CheckStatus").ToString() == "1" ? "<span class=\"green\">审核通过</span>" : "<span class=\"red\">审核不通过</span>")%><br />
                            审核人：<%#Eval("CheckUser") %><br />
                            审核时间：<%#Eval("CheckTime")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr style="background: #F9F4C7;">
                        <td style="line-height: 18px;">
                            <input type="checkbox" class="fll cbxSub" value="<%#Eval("ID") %>" />
                            <%#Eval("UserName")%>
                        </td>
                        <td>
                            <%#Eval("AddTime") %>
                        </td>
                        <td>
                            <%#Eval("Action").ToString() == "0" ? "出库" : "入库"%>
                        </td>
                        <td>
                            <%#Eval("Remark") %>
                        </td>
                        <td>
                            <%# GetOrderProductsStr(Eval("OrderProducts"))%>
                        </td>
                        <td>
                            <%# Eval("CheckStatus").ToString() == "0" ? "<span class=\"gray\">未审核</span>" : (Eval("CheckStatus").ToString() == "1" ? "<span class=\"green\">审核通过</span>" : "<span class=\"red\">审核不通过</span>")%>
                        </td>
                        <td>
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
