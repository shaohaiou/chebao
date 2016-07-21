<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userloginrecords.aspx.cs" Inherits="Chebao.BackAdmin.user.userloginrecords" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>用户登录记录</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">

    <div class="ht_main">
        <asp:Button ID="btnBack" runat="server" Text="返回" CssClass="an1" OnClick="btnBack_Click" />
        <table border="0" cellspacing="0" cellpadding="0" class="biaoge2">
            <asp:Repeater ID="rpadmin" runat="server">
                <HeaderTemplate>
                    <tr class="bgbt">
                        <td class="w100">
                            用户名
                        </td>
                        <td class="w120">
                            登录时间
                        </td>
                        <td class="w120">
                            登录ip
                        </td>
                        <td class="w200">
                            登录地区
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%#Eval("AdminName")%>
                        </td>
                        <td>
                            <%#Eval("LoginTime","{0:yyyy-MM-dd HH:mm:ss}")%>
                        </td>
                        <td>
                            <%#Eval("LoginIP")%>
                        </td>
                        <td>
                            <%#Eval("LoginPosition")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <webdiyer:AspNetPager ID="search_fy" UrlPaging="true" NextPageText="下一页" PrevPageText="上一页"
            CurrentPageButtonClass="current" PageSize="10" runat="server" NumericButtonType="Text"
            MoreButtonType="Text" ShowFirstLast="false" HorizontalAlign="Left" AlwaysShow="false"
            ShowDisabledButtons="False" PagingButtonSpacing="">
        </webdiyer:AspNetPager>
    </div>
    </form>
</body>
</html>
