<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="messageboardview.aspx.cs" Inherits="Chebao.BackAdmin.message.messageboardview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>反馈有奖详情</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
</head>
<body>
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="biaoge3">
        <caption class="bt2">
            反馈详情</caption>
        <tbody>
            <tr>
                <td class="bg1">
                    简述：
                </td>
                <td>
                    <%= CurrentMessage.Title%>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    反馈时间：
                </td>
                <td>
                    <%= CurrentMessage.AddTime%>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    用户名：
                </td>
                <td>
                    <%= CurrentMessage.UserName%>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    联系人：
                </td>
                <td>
                    <%= CurrentMessage.LinkName%>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    详细描述：
                </td>
                <td>
                    <%= CurrentMessage.Content%>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                </td>
                <td>
                    <form id="Form1" runat="server">
                    <asp:Button ID="btnBack" runat="server" Text="返回" CssClass="an1" OnClick="btnBack_Click" />
                    </form>
                </td>
            </tr>
        </tbody>
    </table>
</body>
</html>
