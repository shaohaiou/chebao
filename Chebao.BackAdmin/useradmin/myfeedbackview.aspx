<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="myfeedbackview.aspx.cs" Inherits="Chebao.BackAdmin.useradmin.myfeedbackview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>反馈有奖详情</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            $("img").not("#flay img").click(function () {
                if ($(this).attr("src") != "../images/nopic.png") {
                    $("#flay img").attr("src", $(this).attr("src")).css("max-height", $(window).height() - 100);
                    $("#flay").fadeIn();
                    $("#flay img").css("margin-top", parseInt(($(window).height() - $("#flay img").height()) / 2) + "px");
                }
            });
            $("#flay img").click(function () {
                $("#flay").fadeOut();
            });
            $("#flay").width($(document).width()).height($(window).height());
        })
    </script>
</head>
<body style="width:100%;min-height:500px;background:#fff;">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="biaoge6">
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
                    回复信息：
                </td>
                <td>
                    <%= CurrentMessage.Reply%>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <form runat="server" id="form1">
                        <asp:Button ID="btnBack" runat="server" Text="返回" CssClass="an1" OnClick="btnBack_Click" />
                    </form>
                </td>
            </tr>
        </tbody>
    </table>
    <div id="flay" style="display: none; position: fixed; top: 0; left: 0; z-index: 999;
        text-align: center; background: rgba(0, 0, 0, 0.65);">
        <img src="" />
    </div>
</body>
</html>
