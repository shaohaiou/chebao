<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="subaccountlist.aspx.cs"
    Inherits="Chebao.BackAdmin.user.subaccountlist" %>

<%@ Register Src="../uc/header.ascx" TagName="header" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>耐磨达产品查询系统-子用户</title>
    <link href="../css/headfoot2.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <link href="../css/kp.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="../js/jquery.marquee.js" type="text/javascript"></script>
</head>
<body>
    <form runat="server" id="form1">
    <uc1:header ID="header1" runat="server" />
    <div id="main">
        <div class="usermg-content">
            <div class="usermg-subaccount-field">
                <h3>
                    子用户管理 <a href="subaccountadd.aspx?from=<%=UrlEncode(CurrentUrl) %>" class="pl10">+新增子用户</a></h3>
                <div style="padding-left: 20px;">
                    <asp:Repeater runat="server" ID="rptSubAccount">
                        <HeaderTemplate>
                            <div class="usermg-th-line clearfix">
                                <span class="w100">用户名</span> <span class="w60">联系人</span>
                                <span class="w120">联系方式</span> <span class="w200">联系地址</span>
                                <span class="w60">邮政编码</span> <span class="w100">有效期至</span>
                                <span class="w80">溢价比例</span> <span class="w80">尺寸查询</span>
                                <span class="w80">操作</span>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="usermg-tc-line clearfix">
                                <span class="w100">
                                    <%#Eval("UserName")%></span> <span class="w60">
                                        <%#Eval("LinkName")%></span> <span class="w120">手机：<%#Eval("Mobile")%><br />
                                            电话：<%#Eval("TelPhone")%></span> <span class="w200">
                                                <%#Eval("Province")%>
                                                <%#Eval("City")%>
                                                <%#Eval("District")%><br />
                                                <%#Eval("Address")%></span> <span class="w60">
                                                    <%#Eval("PostCode")%></span> <span class="w100">
                                                        <%# ((DateTime)Eval("ValidDate")).ToString("yyyyMMdd") == DateTime.MaxValue.ToString("yyyyMMdd") ? "无限制" : ((DateTime)Eval("ValidDate") > DateTime.Today ? Eval("ValidDate", "{0:yyyy年MM月dd日}") : "已过期")%></span>
                                <span class="w80">
                                    <%#Eval("SubDiscount")%>%</span> <span class="usermg-item w80">
                                        <%# Eval("SizeView").ToString() == "0" ? "否" : "是"%></span> <span class="usermg-item w80">
                                            <a href="subaccountadd.aspx?id=<%#Eval("ID") %>">编辑</a> <a href="?action=del&id=<%#Eval("ID") %>"
                                                class="red pl10 hide" onclick="javascript:return confirm('确定要删除该用户吗？');">删除</a> </span>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
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
