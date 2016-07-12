<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="stocks.aspx.cs" Inherits="Chebao.BackAdmin.useradmin.stocks" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>耐磨达产品查询系统</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <link href="../css/headfoot2.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            $("#btnFilter").click(function () {
                var query = new Array(); 
                if ($.trim($("#ddlProductTypeFilter").val()) != "-1")
                    query[query.length] = "pt=" + $.trim($("#ddlProductTypeFilter").val());
                if ($.trim($("#txtNameFilter").val()) != "")
                    query[query.length] = "name=" + $.trim($("#txtNameFilter").val());
                if ($.trim($("#txtModelNumberFilter").val()) != "")
                    query[query.length] = "mn=" + $.trim($("#txtModelNumberFilter").val());
                location = "?" + (query.length > 0 ? $(query).map(function () {
                    return this;
                }).get().join("&") : "");

                return false;
            });
        })
    </script>
</head>
<body style="width: 100%;min-height:500px; background: #fff;">
    <form id="form1" runat="server">
    <div class="filter">
        <div class="fbody">
            产品名称：<input type="text" id="txtNameFilter" runat="server" class="srk5 mr10 w80 txt" />
            &nbsp;&nbsp;&nbsp;产品型号：<input type="text" runat="server" id="txtModelNumberFilter" class="srk5 mr10 w80 txt" />
            &nbsp;&nbsp;&nbsp;产品类型：<asp:DropDownList ID="ddlProductTypeFilter" runat="server" CssClass="mr10"></asp:DropDownList>
            &nbsp;&nbsp;&nbsp;<input type="submit" id="btnFilter" class="an1 mr10" value=" 查询 " />
        </div>
    </div>
    <div class="tbbody">
        <table border="0" cellspacing="0" cellpadding="0" class="biaoge2">
            <asp:Repeater ID="rptData" runat="server" OnItemDataBound="rptData_ItemDataBound">
                <HeaderTemplate>
                    <tr class="bgbt">
                        <td>
                            产品名称
                        </td>
                        <td class="w60">
                            产品类型
                        </td>
                        <td class="w120">
                            产品型号
                        </td>
                        <td class="w200">
                            库存/价格
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
                            <%# Eval("ModelNumber")%>
                        </td>
                        <td>
                            <ul style="width: 200px;">
                                <asp:Repeater runat="server" ID="rptProductMix">
                                    <ItemTemplate>
                                        <li style="width: 200px;">
                                            <span style="width:70px;" class="block fll"><%#Eval("Name") %></span>
                                            <span style="width:60px;" class="block fll">库存：<%#Eval("Stock") %></span>
                                            <span style="width:70px;" class="blockinline tr">¥<%#Eval("Price") %></span>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
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
