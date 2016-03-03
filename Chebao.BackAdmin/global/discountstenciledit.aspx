<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="discountstenciledit.aspx.cs" Inherits="Chebao.BackAdmin.global.discountstenciledit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>添加/编辑折扣模版</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="biaoge3">
            <caption class="bt2">
                添加/编辑折扣模版</caption>
            <tbody>
                <tr>
                    <td class="bg1">
                        名称：
                    </td>
                    <td class="bg2">
                        <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="bg1">
                        折扣设置：
                    </td>
                    <td class="bg2">A产品分类：
                         M：<asp:TextBox ID="txtDiscountM" runat="server" CssClass="srk4 mr10"></asp:TextBox>折
                         Y：<asp:TextBox ID="txtDiscountY" runat="server" CssClass="srk4 mr10"></asp:TextBox>折
                         H：<asp:TextBox ID="txtDiscountH" runat="server" CssClass="srk4 mr10"></asp:TextBox>折
                         XSP：<asp:TextBox ID="txtDiscountXSP" runat="server" CssClass="srk4 mr10"></asp:TextBox>折
                         MT：<asp:TextBox ID="txtDiscountMT" runat="server" CssClass="srk4 mr10"></asp:TextBox>折<br />
                         C产品分类：
                         S：<asp:TextBox ID="txtDiscountS" runat="server" CssClass="srk4 mr10"></asp:TextBox>折
                         K：<asp:TextBox ID="txtDiscountK" runat="server" CssClass="srk4 mr10"></asp:TextBox>折
                         P：<asp:TextBox ID="txtDiscountP" runat="server" CssClass="srk4 mr10"></asp:TextBox>折<br />
                         其他产品：
                         LS：<asp:TextBox ID="txtDiscountLS" runat="server" CssClass="srk4 mr10"></asp:TextBox>折
                         B：<asp:TextBox ID="txtDiscountB" runat="server" CssClass="srk4 mr10"></asp:TextBox>折
                    </td>
                </tr>
                <tr>
                    <td class="bg1">
                        附加项设置：
                    </td>
                    <td class="bg2">
                        A产品附件：
                         W：<asp:TextBox ID="txtAdditemW" runat="server" CssClass="srk4 mr10"></asp:TextBox>折
                         F：<asp:TextBox ID="txtAdditemF" runat="server" CssClass="srk4 mr10"></asp:TextBox>折
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center">
                        <asp:Button ID="btSave" runat="server" Text="保存" OnClick="btSave_Click" class="an1" />
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:HiddenField ID="hdid" runat="server" />
        <asp:HiddenField ID="HfRid" runat="server" />
    </div>
    </form>
</body>
</html>
