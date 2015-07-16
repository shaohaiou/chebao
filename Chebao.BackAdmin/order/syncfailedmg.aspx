<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="syncfailedmg.aspx.cs" Inherits="Chebao.BackAdmin.order.syncfailedmg" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>同步失败记录</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#cbxAll").click(function () {
                $(".cbxSub").attr("checked", $(this).attr("checked"));
                setSeldata();
            });
            $(".cbxSub").click(function () {
                setSeldata();
            });

            $("#btnSyncfailed").click(function () {
                return CheckForm();
            });
            $("#btnSyncfailed1").click(function () {
                return CheckForm();
            });
        });
        function setSeldata() {
            $("#hdnIds").val($(".cbxSub:checked").map(function () {
                return $(this).attr("value");
            }).get().join(","));
        }
        function CheckForm() {
            var msg = "";
            if ($("#hdnIds").val() == "")
                msg = "请至少选择一条记录";
            if (msg != "") {
                alert(msg);
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="ht_main">
        <div class="lan5x">
            <asp:Button runat="server" ID="btnSyncfailed" OnClick="btnSyncfailed_Click" CssClass="an1" Text="同步" />
        </div>
        <table border="0" cellspacing="0" cellpadding="0" class="biaoge2">
            <asp:Repeater ID="rptData" runat="server">
                <HeaderTemplate>
                    <tr class="bgbt">
                        <td class="w80">
                            <label style="line-height: 20px;">
                                <input type="checkbox" id="cbxAll" class="fll" />
                                选择</label>
                        </td>
                        <td class="w80">
                            名称
                        </td>
                        <td class="w60">
                            数量
                        </td>
                        <td class="w100">
                            用户名
                        </td>
                        <td class="w80">
                            操作类型
                        </td>
                        <td class="w120">
                            操作时间
                        </td>
                        <td class="w60">
                            操作
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="line-height: 18px;">
                            <input type="checkbox" class="fll cbxSub" value="<%#Eval("ID") %>" />
                        </td>
                        <td>
                            <%#Eval("Name")%>
                        </td>
                        <td>
                            <%#Eval("Amount")%>
                        </td>
                        <td>
                            <%#Eval("UserName")%>
                        </td>
                        <td>
                            <%#Eval("AType").ToString() == "c" ? "出库" : "入库"%>
                        </td>
                        <td>
                            <%#Eval("AddTime")%>
                        </td>
                        <td>
                            <a href="?action=sync&ids=<%#Eval("ID") %>&from=<%=UrlEncode(CurrentUrl) %>" onclick="return confirm('确定要同步吗？')">同步</a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <webdiyer:aspnetpager id="search_fy" urlpaging="true" nextpagetext="下一页" prevpagetext="上一页"
            currentpagebuttonclass="current" pagesize="10" runat="server" numericbuttontype="Text"
            morebuttontype="Text" showfirstlast="false" horizontalalign="Left" alwaysshow="false"
            showcustominfosection="Right" showdisabledbuttons="False" pagingbuttonspacing="">
        </webdiyer:aspnetpager>
        <div class="lan5x">
            <input id="hdnIds" runat="server" type="hidden" />
            <asp:Button runat="server" ID="btnSyncfailed1" OnClick="btnSyncfailed_Click" CssClass="an1" Text="同步"  />
        </div>
    </div>
    </form>
</body>
</html>
