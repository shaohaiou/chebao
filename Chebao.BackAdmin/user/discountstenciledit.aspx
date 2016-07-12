<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="discountstenciledit.aspx.cs" Inherits="Chebao.BackAdmin.user.discountstenciledit" %>

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
                <%if (GetInt("costs") > 0)
                  { %>成本折扣设置<%}else{ %>添加/编辑折扣模版<%} %></caption>
            <tbody>
                <%if (GetInt("costs") == 0){%>
                <tr>
                    <td class="bg1">
                        名称：
                    </td>
                    <td class="bg2">
                        <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <%} %>
                <tr>
                    <td class="bg1">
                        折扣设置：
                    </td>
                    <td class="bg2">
                        <span class="bold">A产品分类：</span><br />
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;M：+ <asp:TextBox ID="txtDiscountMAdd" runat="server" CssClass="srk4"></asp:TextBox> × <asp:TextBox ID="txtDiscountM" runat="server" CssClass="srk4 mr10"></asp:TextBox>折<br />
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Y：+ <asp:TextBox ID="txtDiscountYAdd" runat="server" CssClass="srk4"></asp:TextBox> × <asp:TextBox ID="txtDiscountY" runat="server" CssClass="srk4 mr10"></asp:TextBox>折<br />
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;H：+ <asp:TextBox ID="txtDiscountHAdd" runat="server" CssClass="srk4"></asp:TextBox> × <asp:TextBox ID="txtDiscountH" runat="server" CssClass="srk4 mr10"></asp:TextBox>折<br />
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;XSP：+ <asp:TextBox ID="txtDiscountXSPAdd" runat="server" CssClass="srk4"></asp:TextBox> × <asp:TextBox ID="txtDiscountXSP" runat="server" CssClass="srk4 mr10"></asp:TextBox>折<br />
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;MT：+ <asp:TextBox ID="txtDiscountMTAdd" runat="server" CssClass="srk4"></asp:TextBox> × <asp:TextBox ID="txtDiscountMT" runat="server" CssClass="srk4 mr10"></asp:TextBox>折<br />
                         <span class="bold">C产品分类：</span><br />
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;S：+ <asp:TextBox ID="txtDiscountSAdd" runat="server" CssClass="srk4"></asp:TextBox> × <asp:TextBox ID="txtDiscountS" runat="server" CssClass="srk4 mr10"></asp:TextBox>折<br />
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;K：+ <asp:TextBox ID="txtDiscountKAdd" runat="server" CssClass="srk4"></asp:TextBox> × <asp:TextBox ID="txtDiscountK" runat="server" CssClass="srk4 mr10"></asp:TextBox>折<br /> 
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;P：+ <asp:TextBox ID="txtDiscountPAdd" runat="server" CssClass="srk4"></asp:TextBox> × <asp:TextBox ID="txtDiscountP" runat="server" CssClass="srk4 mr10"></asp:TextBox>折<br /> 
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;PY：+ <asp:TextBox ID="txtDiscountPYAdd" runat="server" CssClass="srk4"></asp:TextBox> × <asp:TextBox ID="txtDiscountPY" runat="server" CssClass="srk4 mr10"></asp:TextBox>折<br />
                         <span class="bold">其他产品：</span><br />
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;LS：+ <asp:TextBox ID="txtDiscountLSAdd" runat="server" CssClass="srk4"></asp:TextBox> × <asp:TextBox ID="txtDiscountLS" runat="server" CssClass="srk4 mr10"></asp:TextBox>折<br />
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;B：+ <asp:TextBox ID="txtDiscountBAdd" runat="server" CssClass="srk4"></asp:TextBox> × <asp:TextBox ID="txtDiscountB" runat="server" CssClass="srk4 mr10"></asp:TextBox>折<br />
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
