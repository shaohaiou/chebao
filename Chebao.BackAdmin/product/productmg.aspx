<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="productmg.aspx.cs" EnableEventValidation="false"
    Inherits="Chebao.BackAdmin.product.productmg" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>产品管理</title>
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
            $("#btnDel").click(function () {
                if (!CheckForm()) return;
                if (confirm("确定删除吗？"))
                    window.location.href = "?ids=" + $("#hdnIds").val() + "&action=del&from=<%=UrlEncode(CurrentUrl)%>";
            });
            $(".btnDel").click(function () {
                return confirm("确定要删除吗？");
            });
            $(".btncopy").click(function () {
                return confirm("确定要复制吗？");
            });

            $("#btnFilter").click(function () {
                var query = new Array();
                if ($.trim($("#ddlProductTypeFilter").val()) != "-1")
                    query[query.length] = "pt=" + $.trim($("#ddlProductTypeFilter").val());
                if ($.trim($("#txtNameFilter").val()) != "-1")
                    query[query.length] = "name=" + $.trim($("#txtNameFilter").val());
                if ($.trim($("#txtModelNumberFilter").val()) != "-1")
                    query[query.length] = "mn=" + $.trim($("#txtModelNumberFilter").val());
                location = "?" + (query.length > 0 ? $(query).map(function () {
                    return this;
                }).get().join("&") : "");

                return false;
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
                $("#spMsg").text(msg);
                setTimeout(function () {
                    $("#spMsg").text("");
                }, 1000);
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="ht_main">
        <table width="1040" border="0" cellspacing="0" cellpadding="0" class="biaoge4" style="background-color: #f4f8fc;">
            <tr>
                <td class="w40 bold">
                    查询：
                </td>
                <td>
                    <asp:ScriptManager ID="smCabmodels" runat="server">
                    </asp:ScriptManager>
                    <asp:UpdatePanel ID="upnCabmodels" runat="server">
                        <ContentTemplate>
                            类型：<asp:DropDownList ID="ddlProductTypeFilter" runat="server" CssClass="mr10">
                            </asp:DropDownList>
                            名称：<asp:TextBox runat="server" ID="txtNameFilter" CssClass="srk1 mr10 w80"></asp:TextBox>
                            型号：<asp:TextBox runat="server" ID="txtModelNumberFilter" CssClass="srk1 mr10 w80"></asp:TextBox>
                            <input type="submit" id="btnFilter" class="an1 mr10" value=" 查询 " /><br />
                            使用车型：<asp:DropDownList ID="ddlBrandFilter" runat="server" CssClass="mr10" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlBrandFilter_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlCabmodelFilter" runat="server" CssClass="mr10" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlCabmodelFilter_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:DropDownList runat="server" ID="ddlPailiangFilter" CssClass="mr10" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlPailiangFilter_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlNianfenFilter" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlNianfenFilter_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button runat="server" ID="btnExportExcel" CssClass="an1 fll mr10" Text=" 导出Excel "
                        OnClick="btnExportExcel_Click" />
                    <a href="productprint.aspx<%=Request.Url.Query %>" target="_blank" class="an1 fll"
                        style="display: block; line-height: 24px; padding: 0 6px;">打印</a>
                </td>
            </tr>
        </table>
        <table width="1040" border="0" cellspacing="0" cellpadding="0" class="biaoge2">
            <asp:Repeater ID="rptProduct" runat="server">
                <HeaderTemplate>
                    <tr class="bgbt">
                        <td>
                            <label style="line-height: 20px;">
                                <input type="checkbox" id="cbxAll" class="fll" />
                                名称</label>
                        </td>
                        <td class="w60">
                            首字母
                        </td>
                        <td class="w60">
                            类型
                        </td>
                        <td class="w160">
                            图片
                        </td>
                        <td class="w60">
                            价格
                        </td>
                        <td class="w120">
                            Lamda型号
                        </td>
                        <td class="w120">
                            OE型号
                        </td>
                        <td class="w100">
                            库存
                        </td>
                        <td class="w120">
                            操作
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <label style="line-height: 18px;">
                                <input type="checkbox" class="fll cbxSub" value="<%#Eval("ID") %>" /><%# Eval("Name")%>
                            </label>
                        </td>
                        <td>
                            <%#Eval("NameIndex") %>
                        </td>
                        <td class="lan5x">
                            <a href="productmg.aspx?pt=<%# (int)Eval("ProductType")%>">
                                <%# GetProductTypeName(Eval("ProductType"))%></a>
                        </td>
                        <td>
                            <img src="<%#Eval("Pic") %>" width="128" height="96" alt="" />
                        </td>
                        <td>
                            <span style="font-size: 14px; font-weight: bold; padding-right: 4px;">¥</span><span
                                style="color: Red;"><%# Eval("Price").ToString().StartsWith("¥") ? Eval("Price").ToString().Substring(1) : Eval("Price").ToString()%></span>
                        </td>
                        <td>
                            <%# Eval("ModelNumber")%>
                        </td>
                        <td>
                            <%# Eval("OEModelNumber")%>
                        </td>
                        <td>
                            <%# GetStock(Eval("ProductMixStr"))%><br />
                            <%#Eval("StockLastUpdateTime")%>
                        </td>
                        <td class="lan5x">
                            <a href="?action=copy&pid=<%#Eval("ID") %>&from=<%=UrlEncode(CurrentUrl) %>" class="btncopy">复制</a>
                            <a href="productedit.aspx?id=<%#Eval("ID") %>&from=<%=UrlEncode(CurrentUrl) %>">编辑</a>
                            <a href="?action=del&ids=<%#Eval("ID") %>&from=<%=UrlEncode(CurrentUrl) %>" class="btnDel">
                                删除</a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <webdiyer:AspNetPager ID="search_fy" UrlPaging="true" NextPageText="下一页" PrevPageText="上一页"
            CurrentPageButtonClass="current" PageSize="10" runat="server" NumericButtonType="Text"
            MoreButtonType="Text" ShowFirstLast="false" HorizontalAlign="Left" AlwaysShow="false"
            ShowCustomInfoSection="Right" ShowDisabledButtons="False" PagingButtonSpacing="">
        </webdiyer:AspNetPager>
        <div class="lan5x">
            <input id="hdnIds" runat="server" type="hidden" />
            <input type="button" value="删除" id="btnDel" class="an1" />
            <span id="spMsg" class="red"></span>
        </div>
    </div>
    </form>
</body>
</html>
