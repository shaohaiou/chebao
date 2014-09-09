<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="productprint.aspx.cs" Inherits="Chebao.BackAdmin.product.productprint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>产品数据打印</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        @media Print
        {
            .notprint
            {
                display: none;
            }
        }
    </style>
</head>
<body>
    <table width="700" border="0" cellspacing="0" cellpadding="0" class="biaoge2" style="margin:0 auto;">
        <asp:Repeater ID="rptProduct" runat="server">
            <HeaderTemplate>
                <tr class="notprint">
                    <td colspan="4">
                        <input type="button" id="btnPrint" value="打印" class="an1 mr10" onclick="javascript:window.print();" />
                    </td>
                </tr>
                <tr class="bgbt">
                    <td>
                        名称
                    </td>
                    <td class="w60">
                        类型
                    </td>
                    <td class="w60">
                        价格
                    </td>
                    <td class="w120">
                        Lamda型号
                    </td>
                    <td class="w120">
                        OE型号
                    </td>
                </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%# Eval("Name")%>
                    </td>
                    <td class="lan5x">
                        <%# GetProductTypeName(Eval("ProductType"))%>
                    </td>
                    <td>
                        <span style="font-size: 14px; font-weight: bold; padding-right: 4px;">¥</span><span
                            style="color: Red;"><%# Eval("Price")%></span>
                    </td>
                    <td>
                        <%# Eval("ModelNumber")%>
                    </td>
                    <td>
                        <%# Eval("OEModelNumber")%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</body>
</html>
