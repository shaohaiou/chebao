<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="adminpowersetting.aspx.cs"
    Inherits="Chebao.BackAdmin.user.adminpowersetting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>管理员权限设置</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(function () {
            $("#cbxAll").click(function () {
                $(".cbxModulepower").attr("checked", this.checked);
                $(".cbxSuball").attr("checked", this.checked);
                setcarbrandvalue();
            });
            $(".cbxModulepower").click(function () {
                setcarbrandvalue();
            });

            $(".cbxSuball").click(function () {
                $(this).parent().parent().find(".cbxModulepower").attr("checked", this.checked);
                setcarbrandvalue();
            });
            $(".cbxSuball").each(function () {
                var suball = $(this);
                $(this).parent().parent().find(".cbxModulepower").each(function () {
                    if (!this.checked)
                        suball.removeAttr("checked");
                });
            });
        });

        function setcarbrandvalue() {
            var carbrand = $(".cbxModulepower:checked").map(function () {
                return $(this).val();
            }).get().join('|');
            if (carbrand != '')
                carbrand = '|' + carbrand + '|'
            $("#hdnModulepower").val(carbrand);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="biaoge3">
            <caption class="bt2">
                管理员权限设置</caption>
            <tbody>
                <tr>
                    <td class="tr">
                        用户名：
                    </td>
                    <td>
                        <%= CurrentUser.UserName %>
                    </td>
                </tr>
                <tr>
                    <td class="tr">
                        权限：
                    </td>
                    <td>
                        <label class="blockinline" style="line-height: 18px;">
                            <input type="checkbox" id="cbxAll" class="fll" />全选</label>
                    </td>
                </tr>
                <tr>
                    <td class="bg1">
                        &nbsp;
                    </td>
                    <td class="bg2">
                        <label class="blockinline bold" style="line-height: 18px;background-color:#ccc;width:700px;">
                            <input type="checkbox" class="cbxSuball fll" />用户管理</label>
                        <ul>
                            <li class="blockinline">
                                <label class="blockinline" style="line-height: 18px;">
                                    <input type="checkbox" class="cbxModulepower fll" value="用户管理" <%=SetModulepower("用户管理") %> />用户管理</label></li>
                            <li class="blockinline">
                                <label class="blockinline" style="line-height: 18px;">
                                    <input type="checkbox" class="cbxModulepower fll" value="管理员管理" <%=SetModulepower("管理员管理") %> />管理员管理</label></li>
                            <li class="blockinline">
                                <label class="blockinline" style="line-height: 18px;">
                                    <input type="checkbox" class="cbxModulepower fll" value="折扣模版" <%=SetModulepower("折扣模版") %> />折扣模版</label></li>
                            <li class="blockinline">
                                <label class="blockinline" style="line-height: 18px;">
                                    <input type="checkbox" class="cbxModulepower fll" value="成本折扣" <%=SetModulepower("成本折扣") %> />成本折扣</label></li>
                        </ul>
                    </td>
                </tr>
                <tr>
                    <td class="bg1">
                        &nbsp;
                    </td>
                    <td class="bg2">
                        <label class="blockinline bold" style="line-height: 18px;background-color:#ccc;width:700px;">
                            <input type="checkbox" class="cbxSuball fll" />品牌管理</label>
                        <ul>
                            <li class="blockinline">
                                <label class="blockinline" style="line-height: 18px;">
                                    <input type="checkbox" class="cbxModulepower fll" value="品牌管理" <%=SetModulepower("品牌管理") %> />品牌管理</label></li>
                        </ul>
                    </td>
                </tr>
                <tr>
                    <td class="bg1">
                        &nbsp;
                    </td>
                    <td class="bg2">
                        <label class="blockinline bold" style="line-height: 18px;background-color:#ccc;width:700px;">
                            <input type="checkbox" class="cbxSuball fll" />车型管理</label>
                        <ul>
                            <li class="blockinline">
                                <label class="blockinline" style="line-height: 18px;">
                                    <input type="checkbox" class="cbxModulepower fll" value="车型管理" <%=SetModulepower("车型管理") %> />车型管理</label></li>
                        </ul>
                    </td>
                </tr>
                <tr>
                    <td class="bg1">
                        &nbsp;
                    </td>
                    <td class="bg2">
                        <label class="blockinline bold" style="line-height: 18px;background-color:#ccc;width:700px;">
                            <input type="checkbox" class="cbxSuball fll" />产品管理</label>
                        <ul>
                            <li class="blockinline">
                                <label class="blockinline" style="line-height: 18px;">
                                    <input type="checkbox" class="cbxModulepower fll" value="产品列表" <%=SetModulepower("产品列表") %> />产品列表</label></li>
                            <li class="blockinline">
                                <label class="blockinline" style="line-height: 18px;">
                                    <input type="checkbox" class="cbxModulepower fll" value="新增产品" <%=SetModulepower("新增产品") %> />新增产品</label></li>
                            <li class="blockinline">
                                <label class="blockinline" style="line-height: 18px;">
                                    <input type="checkbox" class="cbxModulepower fll" value="数据导入" <%=SetModulepower("数据导入") %> />数据导入</label></li>
                            <li class="blockinline">
                                <label class="blockinline" style="line-height: 18px;">
                                    <input type="checkbox" class="cbxModulepower fll" value="用户界面" <%=SetModulepower("用户界面") %> />用户界面</label></li>
                        </ul>
                    </td>
                </tr>
                <tr>
                    <td class="bg1">
                        &nbsp;
                    </td>
                    <td class="bg2">
                        <label class="blockinline bold" style="line-height: 18px;background-color:#ccc;width:700px;">
                            <input type="checkbox" class="cbxSuball fll" />订单管理</label>
                        <ul>
                            <li class="blockinline">
                                <label class="blockinline" style="line-height: 18px;">
                                    <input type="checkbox" class="cbxModulepower fll" value="订单列表" <%=SetModulepower("订单列表") %> />订单列表</label></li>
                            <li class="blockinline">
                                <label class="blockinline" style="line-height: 18px;">
                                    <input type="checkbox" class="cbxModulepower fll" value="未收款" <%=SetModulepower("未收款") %> />未收款</label></li>
                            <li class="blockinline">
                                <label class="blockinline" style="line-height: 18px;">
                                    <input type="checkbox" class="cbxModulepower fll" value="已收款" <%=SetModulepower("已收款") %> />已收款</label></li>
                            <li class="blockinline">
                                <label class="blockinline" style="line-height: 18px;">
                                    <input type="checkbox" class="cbxModulepower fll" value="已发货" <%=SetModulepower("已发货") %> />已发货</label></li>
                            <li class="blockinline">
                                <label class="blockinline" style="line-height: 18px;">
                                    <input type="checkbox" class="cbxModulepower fll" value="已取消" <%=SetModulepower("已取消") %> />已取消</label></li>
                            <li class="blockinline">
                                <label class="blockinline" style="line-height: 18px;">
                                    <input type="checkbox" class="cbxModulepower fll" value="金额可见" <%=SetModulepower("金额可见") %> />金额可见</label></li>
                            <li class="blockinline">
                                <label class="blockinline" style="line-height: 18px;">
                                    <input type="checkbox" class="cbxModulepower fll" value="利润查询" <%=SetModulepower("利润查询") %> />利润查询</label></li>
                            <li class="blockinline">
                                <label class="blockinline" style="line-height: 18px;">
                                    <input type="checkbox" class="cbxModulepower fll" value="同步失败记录" <%=SetModulepower("同步失败记录") %> />同步失败记录</label></li>
                        </ul>
                    </td>
                </tr>
                <tr>
                    <td class="bg1">
                        &nbsp;
                    </td>
                    <td class="bg2">
                        <label class="blockinline bold" style="line-height: 18px;background-color:#ccc;width:700px;">
                            <input type="checkbox" class="cbxSuball fll" />系统设置</label>
                        <ul>
                            <li class="blockinline">
                                <label class="blockinline" style="line-height: 18px;">
                                    <input type="checkbox" class="cbxModulepower fll" value="站点设置" <%=SetModulepower("站点设置") %> />站点设置</label></li>
                            <li class="blockinline">
                                <label class="blockinline" style="line-height: 18px;">
                                    <input type="checkbox" class="cbxModulepower fll" value="执行sql" <%=SetModulepower("执行sql") %> />执行sql</label></li>
                            <li class="blockinline">
                                <label class="blockinline" style="line-height: 18px;">
                                    <input type="checkbox" class="cbxModulepower fll" value="数据处理" <%=SetModulepower("数据处理") %> />数据处理</label></li>
                        </ul>
                    </td>
                </tr>
                <tr>
                    <td class="bg1">
                        &nbsp;
                    </td>
                    <td class="bg2">
                        <label class="blockinline bold" style="line-height: 18px;background-color:#ccc;width:700px;">
                            <input type="checkbox" class="cbxSuball fll" />反馈有奖</label>
                        <ul>
                            <li class="blockinline">
                                <label class="blockinline" style="line-height: 18px;">
                                    <input type="checkbox" class="cbxModulepower fll" value="反馈有奖" <%=SetModulepower("反馈有奖") %> />反馈有奖</label></li>
                        </ul>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center">
                        <input type="hidden" runat="server" id="hdnModulepower" />
                        <asp:Button ID="btSave" runat="server" Text="保存" OnClick="btSave_Click" CssClass="an1" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
