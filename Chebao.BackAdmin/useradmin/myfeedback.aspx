<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="myfeedback.aspx.cs" Inherits="Chebao.BackAdmin.useradmin.myfeedback" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>耐磨达产品查询系统</title>
    <link href="../css/headfoot2.css" rel="stylesheet" type="text/css" />
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../js/comm.js" type="text/javascript"></script>
</head>
<body style="width:100%;min-height:500px;background:#fff;">
    <form id="form1" runat="server">
    <div class="tbbody">
        <table border="0" cellspacing="0" cellpadding="0" class="biaoge2">
            <asp:Repeater ID="rptData" runat="server">
                <HeaderTemplate>
                    <tr class="bgbt">
                        <td>
                            简述
                        </td>
                        <td class="w120">
                            反馈时间
                        </td>
                        <td class="w80">
                            回复状态
                        </td>
                        <td class="w60">
                            操作
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%# Chebao.Tools.StrHelper.GetFuzzyChar(Eval("Title").ToString(),25)%>
                        </td>
                        <td>
                            <%#Eval("AddTime")%>
                        </td>
                        <td>
                            <%#Eval("Status").ToString() == "0" ? "<span class=\"red\">未回复</span>" : "<span class=\"green\">已回复</span>"%>
                        </td>
                        <td>
                            <a href="myfeedbackview.aspx?id=<%#Eval("ID") %>&from=<%=UrlEncode(CurrentUrl) %>">
                                查看</a>
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
        </div>
    </div>
    </form>
</body>
</html>

