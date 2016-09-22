<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="stockmg.aspx.cs" Inherits="Chebao.BackAdmin.useradmin.stockmg" %>

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
            
        });
    </script>
</head>
<body style="width:100%;min-height:500px;background:#fff;">
    <form id="form1" runat="server">
    <div class="nav_stockmg">
        <ul>
            <li class="current_stockmg"><a href="stockmg.aspx">我的申请</a></li><!--
            --><li><a href="stockchange.aspx?t=0">出库</a></li><!--
            --><li><a href="stockchange.aspx?t=1">入库</a></li>
        </ul>
    </div>
    <div class="tbbody" style="margin-top:10px;">
        <table border="0" cellspacing="0" cellpadding="0" class="biaoge2">
            <asp:Repeater ID="rptData" runat="server">
                <HeaderTemplate>
                    <tr class="bgbt">
                        <td class="w60">
                            出/入库
                        </td>
                        <td class="w120">
                            申请时间
                        </td>
                        <td class="w80">
                            审核状态
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
                            <%#Eval("Action").ToString() == "0" ? "出库" : "入库"%>
                        </td>
                        <td>
                            <%# Eval("AddTime") %>
                        </td>
                        <td>
                            <%# Eval("CheckStatus").ToString() == "0" ? "<span class=\"gray\">未审核</span>" : (Eval("CheckStatus").ToString() == "1" ? "<span class=\"green\">审核通过</span>" : "<span class=\"red\">审核不通过</span>")%>
                        </td>
                        <td>
                            <%#Eval("Remark") %>
                        </td>
                        <td>
                            <%# GetOrderProductsStr(Eval("OrderProducts"))%>
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
    </div>
    </form>
</body>
</html>
