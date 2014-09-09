<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userchangepw.aspx.cs" Inherits="Chebao.BackAdmin.user.userchangepw" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>修改密码</title>
    <link href=../css/style2.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %> rel="stylesheet" type="text/css" />
    <link href=../css/headfoot2.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %> rel="stylesheet" type="text/css" />
    <link href=../css/kp.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %> rel="stylesheet"
        type="text/css" />
</head>
<body>
    <div class="header" id="head_01">
        <div class="header_logo">
            <img src="../images/headlogo.jpg" />
        </div>
        <div class="header_nav">
            <ul>
                <li><a href="javascript:void(0);">首页</a></li><li><a href="/product/products.aspx">产品查询</a></li><li>
                    <a href="javascript:void(0);">公司简介</a></li><li><a href="javascript:void(0);">联系我们</a></li><li>
                        <a href="javascript:void(0);">纠错反馈有奖</a></li>
            </ul>
            <div class="header_navinfo">
                <span class="navinfo_user">
                    <%= AdminName %>，您好！</span> <span class="navinfo_opt"><a href="/logout.aspx">安全退出</a><a
                        class="ml40" href="/user/userchangepw.aspx">修改密码</a></span>
            </div>
        </div>
        <!--end-->
    </div>
    <div id="main">
        <div id="member">
            <div class="list">
                <h1>
                    <span>修改密码</span></h1>
                <div class="tables tb-void tb-line">
                    <form id="form1" runat="server">
                    <input name="id" id="id" type="hidden" value="23097" class="text">
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tbody>
                            <tr>
                                <td style="text-align: right; width: 130px;">
                                    旧密码：
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtOldPassword" runat="server" TextMode="password" Style="float: left;"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="rfvUserName" ControlToValidate="TxtOldPassword" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;width: 130px;">
                                    新密码：
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtUserPassword" runat="server" TextMode="password" Style="float: left;"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="ValrUserPassword" ControlToValidate="TxtUserPassword"
                                        runat="server" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;width: 130px;">
                                    重复新密码：
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="TxtNewUserPassword" runat="server" TextMode="password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvTxtCoe" ControlToValidate="TxtNewUserPassword"
                                        runat="server" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator><asp:CompareValidator
                                            ID="CValidator" ControlToValidate="TxtNewUserPassword" ControlToCompare="TxtUserPassword"
                                            runat="server" ErrorMessage="密码不一致"></asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;width: 130px;">
                                    <asp:Literal ID="lerrorMes" runat="server"></asp:Literal>
                                </td>
                                <td style="text-align: left;">
                                    <asp:Button ID="btSave" runat="server" Text=" 提交 " OnClick="btSave_Click" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <div class="footer">
        <div class="footmain">
            浙江耐磨达刹车片有限公司 版权所有 2014-2024 Shantui Zhejiang Lamda Brake Pads Co.,Ltd
        </div>
    </div>
</body>
</html>
