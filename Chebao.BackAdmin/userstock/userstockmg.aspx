<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userstockmg.aspx.cs" Inherits="Chebao.BackAdmin.userstock.userstockmg" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>库存查询</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../js/comm.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#btnFilterModelNumber").click(function () {
                var query = new Array();
                if ($.trim($("#txtModelNumber").val()) != "")
                    query[query.length] = "mn=" + $.trim($("#txtModelNumber").val());
                location = "?" + (query.length > 0 ? $(query).map(function () {
                    return this;
                }).get().join("&") : "");

                return false;
            });
            $("#btnFilterUser").click(function () {
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
                <td class="w100 bold">
                    查询产品库存：
                </td>
                <td class="w80">
                    <asp:TextBox runat="server" ID="txtModelNumber" CssClass="srk6 w60" placeholder="产品型号"></asp:TextBox>
                </td>
                <td>
                    <input type="submit" id="btnFilterModelNumber" class="an1 mr10" value=" 查询 " />
                </td>
            </tr>
            <tr>
                <td class="bold">
                    查询用户库存：
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtUserName" CssClass="srk6 w60" placeholder="用户名"></asp:TextBox>
                </td>
                <td>
                    <input type="submit" id="btnFilterUser" class="an1 mr10" value=" 查询 " />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <span id="spMsg" class="red"></span>
                </td>
            </tr>
        </table>
        <%if (!string.IsNullOrEmpty(GetString("mn")))
          { %>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="biaoge2">
            <asp:Repeater ID="rptDataModelNumber" runat="server">
                <HeaderTemplate>
                    <tr>
                        <td class="w100">
                            总库存：
                        </td>
                        <td>
                            <%=ModelNumberAmountCount%>
                        </td>
                    </tr>
                    <tr class="bgbt">
                        <td>
                            用户
                        </td>
                        <td>
                            库存
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%#Eval("UserName")%>
                        </td>
                        <td>
                            <%#Eval("Amount")%>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr style="background: #F9F4C7;">
                        <td>
                            <%#Eval("UserName")%>
                        </td>
                        <td>
                            <%#Eval("Amount")%>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
        </table>
        <%}
          else if (!string.IsNullOrEmpty(GetString("n")))
          { %>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="biaoge2">
            <asp:Repeater ID="rptDataUserName" runat="server">
                <HeaderTemplate>
                    <tr class="bgbt">
                        <td class="w100">
                            产品型号
                        </td>
                        <td>
                            库存
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%#GetModelNumber(Eval("ProductID"))%>
                        </td>
                        <td>
                            <%#GetProductMixStr(Eval("ProductMix"))%>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr style="background: #F9F4C7;">
                        <td>
                            <%#GetModelNumber(Eval("ProductID"))%>
                        </td>
                        <td>
                            <%#GetProductMixStr(Eval("ProductMix"))%>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
        </table>
        <%} %>
        <webdiyer:AspNetPager ID="search_fy" UrlPaging="true" NextPageText="下一页" PrevPageText="上一页"
            CurrentPageButtonClass="current" PageSize="10" runat="server" NumericButtonType="Text"
            MoreButtonType="Text" ShowFirstLast="false" HorizontalAlign="Left" AlwaysShow="false"
            ShowCustomInfoSection="Right" ShowDisabledButtons="False" PagingButtonSpacing="">
        </webdiyer:AspNetPager>
        <div class="lan5x">
        </div>
    </div>
    </form>
</body>
</html>
