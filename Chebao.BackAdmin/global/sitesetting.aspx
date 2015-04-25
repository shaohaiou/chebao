<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sitesetting.aspx.cs" ValidateRequest="false" Inherits="Chebao.BackAdmin.global.sitesetting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>站点设置</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../js/ckeditor/ckeditor.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            CKEDITOR.replace('txtCorpIntroduce', { toolbar: 'Basic', height: 480 });
            CKEDITOR.replace('txtContact', { toolbar: 'Basic', height: 480 });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="biaoge3">
        <caption class="bt2">
            反馈详情</caption>
        <tbody>
            <tr>
                <td class="bg1">
                    网站公告：
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtNotice" CssClass="srk1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    公司简介：
                </td>
                <td>
                    <asp:TextBox ID="txtCorpIntroduce" runat="server" TextMode="MultiLine" CssClass="ckeditor"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    联系我们：
                </td>
                <td>
                    <asp:TextBox ID="txtContact" runat="server" TextMode="MultiLine" CssClass="ckeditor"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                </td>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" Text="提交" CssClass="an1" OnClick="btnSubmit_Click" />
                </td>
            </tr>
        </tbody>
    </table>
    </form>
</body>
</html>
