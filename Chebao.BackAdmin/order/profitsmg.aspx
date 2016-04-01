<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="profitsmg.aspx.cs" Inherits="Chebao.BackAdmin.order.profitsmg" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>利润查询</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../js/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        var copyorderid = 0;
        $(function () {
            $(".txtDate").click(function () {
                WdatePicker({ 'readOnly': 'true', dateFmt: 'yyyy-MM-dd', maxDate: '<%=DateTime.Today.ToString("yyyy-MM-dd") %>' });
            });

            $("#btnFilter").click(function () {
                var query = new Array();
                if ($.trim($("#txtDateB").val()) != "")
                    query[query.length] = "timeb=" + $.trim($("#txtDateB").val());
                if ($.trim($("#txtDateE").val()) != "")
                    query[query.length] = "timee=" + $.trim($("#txtDateE").val());
                location = "?" + (query.length > 0 ? $(query).map(function () {
                    return this;
                }).get().join("&") : "");

                return false;
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="ht_main">
        <table width="420" border="0" cellspacing="0" cellpadding="0" class="biaoge4" style="background-color: #f4f8fc;">
            <tr>
                <td>
                    发货时间：<asp:TextBox runat="server" ID="txtDateB" CssClass="srk6 txtDate"></asp:TextBox>
                    -
                    <asp:TextBox runat="server" ID="txtDateE" CssClass="srk6 txtDate"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    利润合计：<%=TotalFee - CostsTotal%>
                </td>
            </tr>
            <tr>
                <td>
                    <input type="submit" id="btnFilter" class="an1 mr10" value=" 查询 " />
                </td>
            </tr>
        </table>
        <table width="420" border="0" cellspacing="0" cellpadding="0" class="biaoge2">
            <asp:Repeater ID="rptData" runat="server" OnItemDataBound="rptData_ItemDataBound">
                <HeaderTemplate>
                    <tr class="bgbt">
                        <td class="w160">
                            <label style="line-height: 20px;">
                                <input type="checkbox" id="cbxAll" class="fll" />
                                订单号</label>
                        </td>
                        <td class="w80">
                            发货时间
                        </td>
                        <td class="w60">
                            用户名
                        </td>
                        <td>
                            订单总额/利润
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="line-height: 18px;">
                            <%#Eval("OrderNumber")%>
                        </td>
                        <td>
                            <%#Eval("DeelTime") %>
                        </td>
                        <td>
                            <%#Eval("UserName")%>
                        </td>
                        <td>
                            <%# Chebao.Tools.StrHelper.FormatMoney(Eval("TotalFee").ToString())%><br />
                            <asp:Label runat="server" ID="lblProfits"></asp:Label>
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
</body>
</html>
