<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="subaccountmg.aspx.cs" Inherits="Chebao.BackAdmin.user.subaccountmg" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>子用户管理</title>
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
        <table width="940" border="0" cellspacing="0" cellpadding="0" class="biaoge4" style="background-color: #f4f8fc;">
            <tr>
                <td>
                    <asp:Button runat="server" ID="btnBack" CssClass="an1" Text="返回" OnClick="btnBack_Click" />
                </td>
            </tr>
        </table>
        <table border="0" cellspacing="0" cellpadding="0" class="biaoge2">
            <asp:Repeater ID="rpadmin" runat="server">
                <HeaderTemplate>
                    <tr class="bgbt">
                        <td class="w100">
                            用户名/密码
                        </td>
                        <td class="w60">
                            联系人
                        </td>
                        <td class="w120">
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
                        <td class="w80">
                            溢价比例
                        </td>
                        <td class="w80">
                            尺寸查询
                        </td>
                        <td class="w80">
                            操作
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            用户名：<%#Eval("UserName")%><br />
                            密码：<%#Eval("PasswordText")%>
                        </td>
                        <td>
                            <%#Eval("LinkName")%>
                        </td>
                        <td>
                            手机：<%#Eval("Mobile")%><br />
                            电话：<%#Eval("TelPhone")%>
                        </td>
                        <td>
                            <%#Eval("Province")%>
                            <%#Eval("City")%>
                            <%#Eval("District")%><br />
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
                            IP:<%#Eval("LastLoginIP")%><br />
                            次数：<%#Eval("LoginTimes") %>
                        </td>
                        <td>
                            <%#Eval("SubDiscount")%>%
                        </td>
                        <td>
                            <%# Eval("SizeView").ToString() == "0" ? "否" : "是"%>
                        </td>
                        <td class="lan5x">
                            <a class="btndel" href="userlist.aspx?id=<%#Eval("ID") %>&action=del&from=<%=UrlEncode(CurrentUrl) %>">
                                删除</a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <webdiyer:aspnetpager id="search_fy" urlpaging="true" nextpagetext="下一页" prevpagetext="上一页"
            currentpagebuttonclass="current" pagesize="10" runat="server" numericbuttontype="Text"
            morebuttontype="Text" showfirstlast="false" horizontalalign="Left" alwaysshow="false"
            showdisabledbuttons="False" pagingbuttonspacing="">
        </webdiyer:aspnetpager>
    </div>
    </form>
</body>
</html>
