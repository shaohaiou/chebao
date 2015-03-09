<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="messageboardmg.aspx.cs" Inherits="Chebao.BackAdmin.message.messageboardmg" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>反馈列表</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="ht_main">
        <table width="500" border="0" cellspacing="0" cellpadding="0" class="biaoge2">
            <asp:Repeater ID="rptData" runat="server">
                <HeaderTemplate>
                    <tr class="bgbt">
                        <td class="w60">
                            用户名
                        </td>
                        <td class="w60">
                            联系人
                        </td>
                        <td class="w200">
                            简述
                        </td>
                        <td class="w80">
                            添加时间
                        </td>
                        <td>
                            操作
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%#Eval("UserName")%>
                        </td>
                        <td>
                            <%#Eval("LinkName")%>
                        </td>
                        <td>
                            <%# Chebao.Tools.StrHelper.GetFuzzyChar(Eval("Title").ToString(),15)%>
                        </td>
                        <td>
                            <%#Eval("AddTime")%>
                        </td>
                        <td>
                            <a href="messageboardview.aspx?id=<%#Eval("ID") %>&from=<%=UrlEncode(CurrentUrl) %>">
                                详情</a>
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
