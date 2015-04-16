<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="cabmodelmg.aspx.cs"
    Inherits="Chebao.BackAdmin.car.cabmodelmg" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>车型管理</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../js/jquery.magnifier.js" type="text/javascript"></script>
    <script src="../js/ajaxupload.js" type="text/javascript"></script>
    <script src="../js/comm.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(".btnDel").click(function () {
                DelRow(this);
            });

            $("#btnAdd").click(function () {
                $("#hdnAddCount").val(parseInt($("#hdnAddCount").val()) + 1);
                $("#tblCabmodel").append("<tr><td><select id=\"ddlBrand" + $("#hdnAddCount").val() + "\" name=\"ddlBrand" + $("#hdnAddCount").val() + "\">" + $("#ddlBrand").html() + "</select></td>"
                 + "<td><input type=\"text\" id=\"txtCabmodelName" + $("#hdnAddCount").val() + "\" name=\"txtCabmodelName" + $("#hdnAddCount").val() + "\" class=\"srk1 w120\" value=\"\" /></td>"
                 + "<td><input type=\"text\" id=\"txtPailiang" + $("#hdnAddCount").val() + "\" name=\"txtPailiang" + $("#hdnAddCount").val() + "\" class=\"pailiang required srk1 w80\" value=\"\" /></td>"
                 + "<td><input type=\"text\" id=\"txtNianfen" + $("#hdnAddCount").val() + "\" name=\"txtNianfen" + $("#hdnAddCount").val() + "\" class=\"srk1 w60\" value=\"\" /></td>"
                 + "<td><input type=\"button\" value=\"上传图片\" id=\"btnUploadImg" + $("#hdnAddCount").val() + "\" class=\"an3 uploadbtpics\" /><br /><img src=\"../images/fm.jpg\" alt=\"车型图片\" class=\"imgpics\" id=\"imgpics" + $("#hdnAddCount").val() + "\" style=\"width: 80px;height: 60px;\" /><input type=\"hidden\" id=\"hdnImgpath" + $("#hdnAddCount").val() + "\" name=\"hdnImgpath" + $("#hdnAddCount").val() + "\" class=\"hdnImgpath\" /></td>"
                 + "<td><a href=\"javascript:void(0);\" class=\"btnDel\" val=\"0\">删除</a> </td></tr>");
                $(".btnDel").unbind("click").click(function () {
                    DelRow(this);
                });

                UploadImg($("#btnUploadImg" + $("#hdnAddCount").val())[0]);
                jQuery($("#imgpics" + $("#hdnAddCount").val())[0]).imageMagnify({
                    magnifyby: 8
                });
            });

            $("#btnSubmit").click(function () {
                return CheckForm();
            });

            $("#btnFilter").click(function () {
                var query = new Array();
                if ($.trim($("#ddlBrandFilter").val()) != "-1")
                    query[query.length] = "brand=" + $.trim($("#ddlBrandFilter").val());
                if ($.trim($("#ddlCabmodelFilter").val()) != "-1")
                    query[query.length] = "cabmodel=" + $.trim($("#ddlCabmodelFilter").val());
                if ($.trim($("#ddlPailiangFilter").val()) != "-1")
                    query[query.length] = "pailiang=" + $.trim($("#ddlPailiangFilter").val());
                if ($("#chxNoimgFilter").attr("checked"))
                    query[query.length] = "noimg=1";
                location = "?" + (query.length > 0 ? $(query).map(function () {
                    return this;
                }).get().join("&") : "");

                return false;
            });

            $(".uploadbtpics").each(function () {
                UploadImg(this);
            });
            $(".imgpics").each(function () {
                jQuery(this).imageMagnify({
                    magnifyby: 8
                });
            });
        });

        function CheckForm() {
            var pass = true;

            for (var i = 0; i < $(".required").length; i++) {
                var t = $($(".required")[i]);
                if ($.trim(t.val()) == "") {
                    t.focus();
                    prompts(t, "请填写此字段");
                    pass = false;
                    break;
                }
            }

            if (pass) {
                var reg = new RegExp("^\\d\\.\\d[\\s\\S]*?$");
                for (var i = 0; i < $(".pailiang").length; i++) {
                    var t = $($(".pailiang")[i]);
                    if ($.trim(t.val()) != "" && !reg.test(t.val())) {
                        t.focus();
                        prompts(t, "请填写正确排量");
                        pass = false;
                        break;
                    }
                }
            }

            return pass;
        }

        function DelRow(obj) {
            if ($(obj).attr("val") > 0) {
                $("#hdnDelIds").val($("#hdnDelIds").val() + ($("#hdnDelIds").val() == "" ? "" : ",") + $(obj).attr("val"));
            }
            $(obj).parent().parent().remove();
        }

        function UploadImg(btn) {
            var imgpath_pics;
            var button1 = $(btn), interval1;
            new AjaxUpload(button1, {
                action: '/cutimage.ashx',
                name: 'picfile',
                responseType: 'json',
                data: { action: 'upload' },
                onSubmit: function (file, ext) {
                    if (!(ext && /^(jpg|png|jpeg|gif)$/i.test(ext))) {
                        alert('只能上传图片！');
                        return false;
                    }
                    button1.val('上传中');
                    this.disable();
                    interval1 = window.setInterval(function () {
                        var text = button1.val();
                        if (text.length < 13) {
                            button1.val(text + '.');
                        } else {
                            button1.val('上传中');
                        }
                    }, 200);
                },
                onComplete: function (file, response) {
                    button1.val('修改图片');
                    window.clearInterval(interval1);
                    this.enable();
                    $("img", button1.parent()).attr("src", response.src).attr("val", response.src);
                    $(".hdnImgpath", button1.parent()).val(response.src);
                    jQuery($("img", button1.parent())[0]).imageMagnify({
                        magnifyby: 8
                    });
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="ht_main">
        <table width="750" border="0" cellspacing="0" cellpadding="0" class="biaoge4" style="background-color: #f4f8fc;">
            <tr>
                <td class="w40 bold">
                    查询：
                </td>
                <td>
                    <asp:ScriptManager ID="smCabmodels" runat="server">
                    </asp:ScriptManager>
                    <asp:UpdatePanel ID="upnCabmodels" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlBrandFilter" runat="server" CssClass="mr10" AutoPostBack="true"
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
                    <label class="block" style="line-height:18px;"><input type="checkbox" runat="server" id="chxNoimgFilter" class="fll" />无图数据</label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <input type="submit" id="btnFilter" class="an1" value=" 查询 " />
                </td>
            </tr>
        </table>
        <table width="750" border="0" cellspacing="0" cellpadding="0" class="biaoge2" id="tblCabmodel">
            <asp:Repeater ID="rptCabmodel" runat="server" OnItemDataBound="rptCabmodel_ItemDataBound">
                <HeaderTemplate>
                    <tr class="bgbt">
                        <td>
                            品牌
                        </td>
                        <td class="w120">
                            车型
                        </td>
                        <td class="w80">
                            排量
                        </td>
                        <td class="w80">
                            年份
                        </td>
                        <td class="w80">
                            图片
                        </td>
                        <td class="w50">
                            操作
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <input type="hidden" runat="server" id="hdnID" value='<%#Eval("ID") %>' />
                            <asp:DropDownList runat="server" ID="ddlBrand">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <input type="text" runat="server" id="txtCabmodelName" class="srk1 w120" value='<%#Eval("CabmodelName") %>' />
                        </td>
                        <td>
                            <input type="text" runat="server" id="txtPailiang" class="pailiang required srk1 w80"
                                value='<%#Eval("Pailiang") %>' />
                        </td>
                        <td>
                            <input type="text" runat="server" id="txtNianfen" class="srk1 w60" value='<%#Eval("Nianfen") %>' />
                        </td>
                        <td>
                            <input type="button" value="上传图片" class="an3 uploadbtpics" /><br />
                            <img src="<%# string.IsNullOrEmpty(Eval("Imgpath").ToString()) ? "../images/fm.jpg" : Eval("Imgpath").ToString() %>"
                                alt="车型图片" class="imgpics" style="width: 80px; height: 60px;" />
                            <input type="hidden" value='<%# Eval("Imgpath") %>' runat="server" id="hdnImgpath"
                                class="hdnImgpath" />
                        </td>
                        <td class="lan5x">
                            <a href="javascript:void(0);" class="btnDel pl10" val="<%#Eval("ID") %>">删除</a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <webdiyer:AspNetPager ID="search_fy" UrlPaging="true" NextPageText="下一页" PrevPageText="上一页"
            CurrentPageButtonClass="current" PageSize="10" runat="server" NumericButtonType="Text"
            MoreButtonType="Text" HorizontalAlign="Left" AlwaysShow="false" ShowCustomInfoSection="Right"
            ShowDisabledButtons="False" PagingButtonSpacing="">
        </webdiyer:AspNetPager>
        <div class="lan5x" style="padding-top: 10px;">
            <input type="hidden" runat="server" id="hdnAddCount" value="0" />
            <input type="hidden" runat="server" id="hdnDelIds" value="" />
            <input class="an1" type="button" value="添加" id="btnAdd" />
            <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" CssClass="an1" runat="server"
                Text="保存" />
        </div>
        <div class="hidden">
            <asp:DropDownList runat="server" ID="ddlBrand">
            </asp:DropDownList>
            <asp:DropDownList runat="server" ID="ddlNianfen">
            </asp:DropDownList>
        </div>
    </div>
    </form>
</body>
</html>
