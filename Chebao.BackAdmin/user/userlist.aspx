<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userlist.aspx.cs" Inherits="Chebao.BackAdmin.user.userlist" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>普通用户管理</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(".btndel").click(function () {
                return confirm("确定要删除此用户吗？");
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="ht_main">
        <table width="920" border="0" cellspacing="0" cellpadding="0" class="biaoge4" style="background-color: #f4f8fc;">
            <tr>
                <td class="w40 bold">
                    查询：
                </td>
                <td>
                    用户名：<asp:TextBox ID="txtUserName" runat="server" CssClass="srk6"></asp:TextBox>
                    联系人：<asp:TextBox ID="txtLinkName" runat="server" CssClass="srk6"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button runat="server" ID="btnExportExcel" CssClass="an1 fll mr10" Text=" 导出Excel "
                        OnClick="btnExportExcel_Click" />
                    <asp:Button runat="server" ID="btnFilter" CssClass="an1" Text="确定" OnClick="btnFilter_Click" />
                </td>
            </tr>
        </table>
        <table width="920" border="0" cellspacing="0" cellpadding="0" class="biaoge2">
            <asp:Repeater ID="rpadmin" runat="server">
                <HeaderTemplate>
                    <tr class="bgbt">
                        <td class="w100">
                            用户名
                        </td>
                        <td class="w60">
                            联系人
                        </td>
                        <td class="w100">
                            联系方式
                        </td>
                        <td class="w200">
                            联系地址
                        </td>
                        <td class="w60">
                            邮政编码
                        </td>
                        <td class="w100">
                            有效期至
                        </td>
                        <td class="w160">
                            最后登录信息
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
                            手机：<%#Eval("Mobile")%><br />
                            电话：<%#Eval("TelPhone")%>
                        </td>
                        <td>
                            <%#Eval("Province")%> <%#Eval("City")%> <%#Eval("District")%><br />
                            <%#Eval("Address")%>
                        </td>
                        <td>
                            <%#Eval("PostCode")%>
                        </td>
                        <td>
                            <%# ((DateTime)Eval("ValidDate")).ToString("yyyyMMdd") == DateTime.MaxValue.ToString("yyyyMMdd") ? "无限制" : ((DateTime)Eval("ValidDate") > DateTime.Today ? Eval("ValidDate", "{0:yyyy年MM月dd日}") : "已过期")%>
                        </td>
                        <td>
                            时间：<%#Eval("LastLoginTime", "{0:yyyy-MM-dd HH:mm}")%><br />
                            IP:<%#Eval("LastLoginIP")%>
                        </td>
                        <td class="lan5x">
                            <a class="btndel" href="userlist.aspx?id=<%#Eval("ID") %>&action=del&from=<%=UrlEncode(CurrentUrl) %>">
                                删除</a><a href="useredit.aspx?id=<%#Eval("ID") %>&action=update&from=<%=UrlEncode(CurrentUrl) %>">编辑</a>
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
        <div style="text-align: center; width: 600px;" class="lan5x">
            <a href="useredit.aspx?from=<%=UrlEncode(CurrentUrl) %>">添加用户</a></div>
    </div>
    </form>
</body>
</html>
