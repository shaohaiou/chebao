<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="subaccountpowersetting.aspx.cs"
    Inherits="Chebao.BackAdmin.user.subaccountpowersetting" %>

<%@ Register Src="../uc/header.ascx" TagName="header" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>耐磨达产品查询系统-子用户品牌权限</title>
    <link href="../css/headfoot2.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <link href="../css/kp.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="../js/jquery.marquee.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(function () {
            $("#cbxAll").click(function () {
                $(".cbxCarbrand").attr("checked", this.checked);
                $(".cbxSuball").attr("checked", this.checked);
                setcarbrandvalue();
            });
            $(".cbxCarbrand").click(function () {
                setcarbrandvalue();
            });

            $(".cbxSuball").click(function () {
                $(this).parent().parent().find(".cbxCarbrand").attr("checked", this.checked);
                setcarbrandvalue();
            });
            $(".cbxSuball").each(function () {
                var suball = $(this);
                $(this).parent().parent().find(".cbxCarbrand").each(function () {
                    if (!this.checked)
                        suball.removeAttr("checked");
                });
            });
        });

        function setcarbrandvalue() {
            var carbrand = $(".cbxCarbrand:checked").map(function () {
                return $(this).val();
            }).get().join('|');
            if (carbrand != '')
                carbrand = '|' + carbrand + '|'
            $("#hdnCarbrand").val(carbrand);
        }
    </script>
    <style type="text/css">
    .clpp ul{display:inline-block;width:160px;*display:inline;*zoom:1;vertical-align:top;margin-left:3px;}
    .nh{color:White;font-weight:bold;font-size:18px;background:#222;text-indent:5px;}
    </style>
</head>
<body>
    <form runat="server" id="form1">
    <uc1:header ID="header1" runat="server" />
    <div id="main">
        <div class="usermg-content">
            <div class="usermg-subaccount-field">
                <h3>
                    子用户品牌权限</h3>
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="biaoge3">
                    <tbody>
                        <tr>
                            <td class="vt tr">
                                &nbsp;
                            </td>
                            <td>
                                <label class="block">
                                    <input type="checkbox" id="cbxAll" class="fll" />全选</label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td class="clpp">
                                <asp:Repeater runat="server" ID="rptCarbrand">
                                    <ItemTemplate>
                                        <%# GetNewCharStr(Eval("NameIndex").ToString())%>
                                        <li>
                                            <label class="blockinline" style="line-height: 18px;">
                                                <input type="checkbox" class="cbxCarbrand fll" value="<%# Eval("ID") %>" <%#SetCarbrand(Eval("ID").ToString()) %> /><%# Eval("BrandName")%></label></li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <input type="hidden" runat="server" id="hdnCarbrand" />
                                <asp:Button runat="server" ID="btnSubmit" Text="保存" CssClass="an1" OnClick="btnSubmit_Click" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <div class="footer">
        <div class="footmain">
            浙江耐磨达刹车片有限公司 版权所有 2014-2024 Shantui Zhejiang Lamda Brake Pads Co.,Ltd
        </div>
    </div>
    </form>
</body>
</html>
