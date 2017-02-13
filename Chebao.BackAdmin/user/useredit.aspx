<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="useredit.aspx.cs" Inherits="Chebao.BackAdmin.user.useredit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>添加/编辑用户</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../js/comm.js" type="text/javascript"></script>
    <script type="text/javascript">
        var username = '';
        $(function () {
            username = $("#txtUserName").val();
            $("#cbIsAdmin").click(function () { setrole(); });
            $("#txtValidDays").change(function () {
                var reg = new RegExp("\\d+");
                if ($.trim($(this).val()) != "" && !reg.test($(this).val())) {
                    $(this).focus();
                    $(this).val("");
                    prompts($(this), "请输入正确天数");
                } else {
                    if ($.trim($(this).val()) == "") {
                        $("#lblValidDays").text("（有效期至：无限制）");
                    }
                    else {
                        var date = new Date();
                        date.setDate(date.getDate() + parseInt($.trim($(this).val())));
                        $("#lblValidDays").text("（有效期至：" + date.getFullYear() + "年" + (date.getMonth() + 1) + "月" + date.getDate() + "日）");
                    }
                }
            });

            $("#ddlDiscountStencilID").change(function () {
                if ($("#ddlDiscountStencilID option:selected").val() != "0") {
                    $(".trdiscount").hide();
                } else {
                    $(".trdiscount").show();
                }
            });
        });
        function ValidationName(source, arguments) {
            var v = false;
            if (username == $("#txtUserName").val()) {
                arguments.IsValid = true;
                return;
            }
            $.ajax({
                url: '/checkadmin.ashx?d=' + new Date(),
                async: false,
                dataType: "json",
                data: { name: $("#txtUserName").val() },
                error: function (msg) {
                    alert("发生错误！");
                },
                success: function (data) {
                    if (data.result == 'success') {
                        v = true;
                    }
                    else {
                        v = false;
                    }
                }
            });
            arguments.IsValid = v;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="biaoge3">
            <caption class="bt2">
                添加/编辑用户</caption>
            <tbody>
                <tr>
                    <td class="bg1">
                        用户账户名：
                    </td>
                    <td class="bg2">
                        <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox><asp:RequiredFieldValidator
                            ID="rfvname" runat="server" CssClass="red" ErrorMessage="账户名必须填写" ControlToValidate="txtUserName"
                            Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cvUserName" runat="server" ClientValidationFunction="ValidationName"
                            CssClass="red" ErrorMessage="该账户名已经被使用" Text="该账户名已经被使用" SetFocusOnError="True"
                            ControlToValidate="txtUserName" EnableClientScript="true" Display="Dynamic"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td class="tr">
                        密码：
                    </td>
                    <td>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox><span class="gray">（原密码：<asp:Label ID="lblPassword" runat="server"></asp:Label>）</span><asp:RequiredFieldValidator
                            ID="rePassword" runat="server" ErrorMessage="密码必须填写" CssClass="red" ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="tr">
                        确认密码：
                    </td>
                    <td>
                        <asp:TextBox ID="txtTruePassword" runat="server" TextMode="Password"></asp:TextBox><asp:RequiredFieldValidator
                            ID="reTruePassword" runat="server" CssClass="red" ErrorMessage="确认密码必须填写" ControlToValidate="txtTruePassword"></asp:RequiredFieldValidator>
                        <asp:CompareValidator runat="server" ID="cvPassword" ControlToCompare="txtPassword"
                            ControlToValidate="txtTruePassword" ErrorMessage="两次密码输入不一致" CssClass="red"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td class="bg1">
                        开启分销：
                    </td>
                    <td class="bg2">
                        <asp:CheckBox runat="server" ID="cbxIsDistribution" />
                    </td>
                </tr>
                <tr>
                    <td class="bg1">
                        子用户权限：
                    </td>
                    <td class="bg2">
                        <asp:CheckBox runat="server" ID="cbxIsAddAccount" />
                    </td>
                </tr>
                <tr>
                    <td class="tr">
                        有效天数：
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtValidDays" CssClass="srk4 mr10"></asp:TextBox>天
                        <asp:Label runat="server" ID="lblValidDays" CssClass="gray"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tr">
                        联系手机：
                    </td>
                    <td >
                        <asp:TextBox ID="txtMobile" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="bg1">
                        联系电话：
                    </td>
                    <td class="bg2">
                        <asp:TextBox ID="txtTelPhone" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="bg1">
                        联系人姓名：
                    </td>
                    <td class="bg2">
                        <asp:TextBox ID="txtLinkName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="bg1">
                        联系地址：
                    </td>
                    <td class="bg2">
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
                                <asp:TextBox runat="server" ID="txtAddress" CssClass="w200"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td class="bg1">
                        邮政编码：
                    </td>
                    <td class="bg2">
                        <asp:UpdatePanel runat="server" ID="upl2">
                            <ContentTemplate>
                                <input type="text" runat="server" id="txtPostCode" class="w100" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td class="bg1">
                        折扣模版：
                    </td>
                    <td class="bg2">
                        <asp:DropDownList runat="server" ID="ddlDiscountStencilID"></asp:DropDownList>
                    </td>
                </tr>
                <tr class="trdiscount" <%if (discountstencilid > 0){%>style="display:none;"<%} %>>
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
                <tr class="trdiscount" <%if (discountstencilid > 0){%>style="display:none;"<%} %>>
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
                    <td class="bg1">
                        尺寸查询权限：
                    </td>
                    <td class="bg2">
                        <asp:CheckBox runat="server" ID="cbxSizeView" />
                    </td>
                </tr>
                <tr>
                    <td class="bg1">
                        显示产品介绍：
                    </td>
                    <td class="bg2">
                        <asp:CheckBox runat="server" ID="cbxIsShowIntroduce" />
                    </td>
                </tr>
                <tr>
                    <td class="bg1">
                        显示适用车型：
                    </td>
                    <td class="bg2">
                        <asp:CheckBox runat="server" ID="cbxIsShowCabmodel" />
                    </td>
                </tr>
                <tr>
                    <td class="bg1">
                        显示价格：
                    </td>
                    <td class="bg2">
                        <asp:CheckBox runat="server" ID="cbxIsShowPrice" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center">
                        <div style="text-align: center">
                            <asp:CustomValidator ID="cvmess" Display="Dynamic" runat="server" ErrorMessage=""></asp:CustomValidator></div>
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
