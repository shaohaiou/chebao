<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="brandmg.aspx.cs" Inherits="Chebao.BackAdmin.car.brandmg" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>品牌管理</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(".btnDel").click(function () {
                DelRow(this);
            });

            $("#btnAdd").click(function () {
                $("#hdnAddCount").val(parseInt($("#hdnAddCount").val()) + 1);
                $("#tblBrand").append("<tr><td><input type=\"text\" id=\"txtBrandName" + $("#hdnAddCount").val() + "\" name=\"txtBrandName" + $("#hdnAddCount").val() + "\" class=\"srk1 w120\" value=\"\" /></td><td></td>"
                 + "<td><a href=\"javascript:void(0);\" class=\"btnDel\" val=\"0\">删除</a> </td></tr>");
                $(".btnDel").unbind("click").click(function () {
                    DelRow(this);
                });
            });
        });

        function DelRow(obj) {
            if ($(obj).attr("val") > 0) {
                $("#hdnDelIds").val($("#hdnDelIds").val() + ($("#hdnDelIds").val() == "" ? "" : ",") + $(obj).attr("val"));
            }
            $(obj).parent().parent().remove();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="ht_main">
        <table width="240" border="0" cellspacing="0" cellpadding="0" class="biaoge2" id="tblBrand">
            <asp:Repeater ID="rptBrand" runat="server">
                <HeaderTemplate>
                    <tr class="bgbt">
                        <td class="w120">
                            品牌名称
                        </td>
                        <td class="w40">
                            首字母
                        </td>
                        <td>
                            操作
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <input type="hidden" runat="server" id="hdnID" value='<%#Eval("ID") %>' />
                            <input type="text" runat="server" id="txtBrandName" class="srk1 w120" value='<%#Eval("BrandName") %>' />
                        </td>
                        <td>
                            <input type="text" runat="server" id="txtNameIndex" class="srk1 w40" value='<%#Eval("NameIndex") %>' />
                        </td>
                        <td class="lan5x">
                            <a href="javascript:void(0);" class="btnDel pl10" val="<%#Eval("ID") %>">删除</a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <webdiyer:aspnetpager id="search_fy" urlpaging="true" nextpagetext="下一页" prevpagetext="上一页"
            currentpagebuttonclass="current" pagesize="10" runat="server" numericbuttontype="Text"
            morebuttontype="Text" showfirstlast="false" horizontalalign="Left" alwaysshow="false"
            showdisabledbuttons="False" pagingbuttonspacing="">
        </webdiyer:aspnetpager>
        <div class="lan5x" style="padding-top: 10px;">
            <input type="hidden" runat="server" id="hdnAddCount" value="0" />
            <input type="hidden" runat="server" id="hdnDelIds" value="" />
            <input class="an1" type="button" value="添加" id="btnAdd" />
            <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" CssClass="an1" runat="server"
                Text="保存" />
        </div>
    </div>
    </form>
</body>
</html>
