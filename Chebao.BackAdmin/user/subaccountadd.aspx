<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="subaccountadd.aspx.cs"
    Inherits="Chebao.BackAdmin.user.subaccountadd" %>

<%@ Register Src="../uc/header.ascx" TagName="header" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>耐磨达产品查询系统-新增子用户</title>
    <link href="../css/headfoot2.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <link href="../css/kp.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="../js/jquery.marquee.js" type="text/javascript"></script>
    <script src="../js/comm.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#txtValidDays").change(function () {
                var reg = new RegExp("\\d+");
                if ($.trim($(this).val()) != "" && !reg.test($(this).val())) {
                    $(this).focus();
                    $(this).val("");
                    prompts($(this), "请输入正确天数");
                }else if (parseInt($.trim($(this).val())) < 0) {
                    $(this).focus();
                    $(this).val("");
                    prompts($(this), "请输入正数");
                } else {
                    if ($(this).attr("max-data") != "") {
                        if (parseInt($(this).attr("max-data") < parseInt($(this).val())))
                            $(this).val($(this).attr("max-data"));
                    }
                    var date = new Date();
                    date.setDate(date.getDate() + parseInt($.trim($(this).val())));
                    $("#lblValidDays").text("（有效期至：" + date.getFullYear() + "年" + (date.getMonth() + 1) + "月" + date.getDate() + "日）");
                }
            });
            $("#txtSubDiscountt").change(function () {
                var reg = new RegExp("^(\\d*\.)?\\d+$");
                if ($.trim($(this).val()) != "" && !reg.test($(this).val())) {
                    $(this).focus();
                    $(this).val("");
                    prompts($(this), "请输入正确的溢价比例");
                }
                if (parseFloat($.trim($(this).val())) < 0) {
                    $(this).focus();
                    $(this).val("");
                    prompts($(this), "请输入正数");
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:header ID="header1" runat="server" />
    <div id="main">
        <div class="usermg-content">
            <div class="usermg-subaccount-field">
                <h3>
                    <%= GetInt("id") > 0 ? "编辑" : "新增" %>子用户</h3>
                <div style="padding-left: 20px;">
                    <div class="usermg-add-title">
                        <span>用户信息</span></div>
                    <ul class="usermg-add-ul">
                        <li><span class="tl">最后登录时间：</span>
                            <asp:Label runat="server" ID="lblLastLoginTime"></asp:Label></li>
                        <li><span class="tl">用户名：</span>
                            <asp:TextBox ID="txtUserName" runat="server" ReadOnly="true"></asp:TextBox></li>
                        <li><span class="tl">密码：</span>
                            <asp:TextBox ID="txtPassword" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rePassword" runat="server" ErrorMessage="密码必须填写"
                                CssClass="red" ControlToValidate="txtPassword"></asp:RequiredFieldValidator></li>
                        <li><span class="tl">有效天数：</span>
                            <asp:TextBox runat="server" ID="txtValidDays" CssClass="srk4 mr10" max-data=""></asp:TextBox>
                            天
                            <asp:Label runat="server" ID="lblValidDays" CssClass="gray"></asp:Label></li>
                        <li><span class="tl">联系手机：</span>
                            <asp:TextBox ID="txtMobile" runat="server"></asp:TextBox></li>
                        <li><span class="tl">联系电话：</span>
                            <asp:TextBox ID="txtTelPhone" runat="server"></asp:TextBox></li>
                        <li><span class="tl">联系人姓名：</span>
                            <asp:TextBox ID="txtLinkName" runat="server"></asp:TextBox></li>
                        <li><span class="tl">地区：</span>
                            <asp:ScriptManager ID="smArea" runat="server">
                            </asp:ScriptManager>
                            <asp:UpdatePanel runat="server" ID="upl1">
                                <ContentTemplate>
                                    <asp:DropDownList runat="server" ID="ddlProvince" AutoPostBack="true" OnSelectedIndexChanged="ddlProvince_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:DropDownList runat="server" ID="ddlCity" AutoPostBack="true" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:DropDownList runat="server" ID="ddlDistrict" AutoPostBack="true" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </li>
                        <li><span class="tl">街道小区：</span>
                            <asp:TextBox runat="server" ID="txtAddress" CssClass="w200"></asp:TextBox>
                        </li>
                        <li><span class="tl">邮政编码：</span>
                            <asp:UpdatePanel runat="server" ID="upl2">
                                <ContentTemplate>
                                    <input type="text" runat="server" id="txtPostCode" class="w100" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </li>
                        <li class="<%= Admin.SizeView > 0 ? string.Empty : "hide" %>"><span class="tl">尺寸查询权限：</span>
                            <asp:CheckBox runat="server" ID="cbxSizeView" /></li>
                        <li><span class="tl">溢价比例：</span><asp:TextBox runat="server" ID="txtSubDiscountt" Text="90"
                            CssClass="srk4 mr10"></asp:TextBox>
                            %<span class="gray pl10">子用户的购买价格 = 主用户购买价格 × （1 + 溢价比例）</span></li>
                        <li class="<%= Admin.IsShowCabmodel > 0 ? string.Empty : "hide" %>"><span class="tl">显示适用车型：</span>
                            <asp:CheckBox runat="server" ID="cbxIsShowCabmodel" /></li>
                        <li class="<%= Admin.IsShowPrice > 0 ? string.Empty : "hide" %>"><span class="tl">显示价格：</span>
                            <asp:CheckBox runat="server" ID="cbxIsShowPrice" /></li>
                        <li>
                            <asp:HiddenField ID="hdid" runat="server" />
                            <div style="text-align: center">
                                <asp:CustomValidator ID="cvmess" Display="Dynamic" runat="server" ErrorMessage=""></asp:CustomValidator></div>
                            <asp:Button ID="btSave" runat="server" Text="保存" OnClick="btSave_Click" class="an1" />
                        </li>
                    </ul>
                </div>
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
