<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="orderview.aspx.cs" Inherits="Chebao.BackAdmin.order.orderview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>订单详情</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
</head>
<body>
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="biaoge3">
        <caption class="bt2">
            订单详情</caption>
        <tbody>
            <tr>
                <td class="bg1">
                    订单号：
                </td>
                <td>
                    <%= CurrentOrder.OrderNumber%>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    订单状态：
                </td>
                <td>
                    <%= CurrentOrder.OrderStatus.ToString()%>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    收货地址：
                </td>
                <td>
                    <%=CurrentOrder.Province%>
                    <%=CurrentOrder.City%>
                    <%=CurrentOrder.District%>
                    <%=CurrentOrder.Address%>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    邮政编码：
                </td>
                <td>
                    <%=CurrentOrder.PostCode%>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    收货人姓名：
                </td>
                <td>
                    <%=CurrentOrder.LinkName%>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    手机号码：
                </td>
                <td>
                    <%=CurrentOrder.LinkMobile%>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    电话号码：
                </td>
                <td>
                    <%=CurrentOrder.LinkTel%>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    订单信息：
                </td>
                <td>
                    <asp:Repeater runat="server" ID="rptData" OnItemDataBound="rptData_ItemDataBound">
                        <HeaderTemplate>
                            <ul style="width: 940px; background-color: #9BD1F7; font-weight: bold; height: 16px;
                                padding: 2px 0 0 2px">
                                <li class="fll w300">产品名称/型号</li>
                                <li class="fll w40">数量</li>
                                <li class="fll w100">单价</li>
                                <li class="fll w100">折扣/组合单价</li>
                                <li class="fll" style="width: 400px">留言</li>
                            </ul>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <ul style="width: 942px; height: 16px;">
                                <li class="fll">
                                    <div style="background-color: #D2EBFC;width:540px;">
                                        <%# Eval("ProductName")%><span class="gray">
                                            <%#Eval("CabmodelStr")%></span></div>
                                    <asp:Repeater runat="server" ID="rptProductMix">
                                        <ItemTemplate>
                                            <div class="pl10">
                                                <span style="width: 290px; display: table-cell;">
                                                    <%#Eval("Name") %></span> <span class="w40" style="display: table-cell;">
                                                        <%#Eval("Amount") %></span> <span class="w100" style="display: table-cell;">
                                                            <%= GetOriginalPrice()%></span> <span class="w100" style="display: table-cell;">
                                                                <%#Eval("Price")%></span>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </li>
                                <li class="fll" style="width: 400px">
                                    <%#Eval("Remark") %>&nbsp;</li>
                            </ul>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    实付款：
                </td>
                <td>
                    <%=CurrentOrder.TotalFee%>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                </td>
                <td>
                    <form runat="server">
                    <asp:Button ID="btnBack" runat="server" Text="返回" CssClass="an1" OnClick="btnBack_Click" />
                    </form>
                </td>
            </tr>
        </tbody>
    </table>
</body>
</html>
