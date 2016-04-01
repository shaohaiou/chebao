<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="productedit.aspx.cs" ValidateRequest="false" Inherits="Chebao.BackAdmin.product.productedit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加/编辑产品</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <link href="../css/newstart/jquery-ui-1.7.3.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/bitmapcutter/jquery.bitmapcutter.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.7.3.custom.min.js" type="text/javascript"></script>
    <script src="../js/jquery.bitmapcutter.js" type="text/javascript"></script>
    <script src="../js/ajaxupload.js" type="text/javascript"></script>
    <script src="../js/ckeditor/ckeditor.js" type="text/javascript"></script>
    <script src="../js/comm.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            CKEDITOR.replace('txtStandard', { toolbar: 'Basic', height: 140 });
            CKEDITOR.replace('txtIntroduce', { toolbar: 'Basic', height: 480 });

            $("#cutImg").dialog({
                bgiframe: true,
                modal: true,
                autoOpen: false,
                width: 820,
                height: 560
            });

            var imgpath_pic;
            var button = $('#uploadbtpic'), interval;
            new AjaxUpload(button, {
                action: '/cutimage.ashx',
                name: 'picfile',
                responseType: 'json',
                data: { action: 'upload' },
                onSubmit: function (file, ext) {
                    if (!(ext && /^(jpg|png|jpeg|gif)$/i.test(ext))) {
                        alert('只能上传图片！');
                        return false;
                    }
                    button.val('上传中');
                    this.disable();
                    interval = window.setInterval(function () {
                        var text = button.val();
                        if (text.length < 13) {
                            button.val(text + '.');
                        } else {
                            button.val('上传中');
                        }
                    }, 200);
                },
                onComplete: function (file, response) {
                    button.val('修改图片');
                    window.clearInterval(interval);
                    this.enable();

                    $("#imgpic").attr("src", response.src);
                    $("#hdimage_pic").val(response.src);

                    if ($("#hdimage_pics").val().split('|')[0] == "") {
                        var button1 = $($(".uploadbtpics")[0]);
                        button1.val("修改图片");
                        $("img", button1.parent()).attr("src", response.src).attr("val", response.src);
                        var imgs = $(".imgpics").map(function () {
                            return $(this).attr("val");
                        }).get().join("|")
                        $("#hdimage_pics").val(imgs);
                    }
                }
            });

            $(".uploadbtpics").each(function () {
                var imgpath_pics;
                var button1 = $(this), interval1;
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
                        var imgs = $(".imgpics").map(function () {
                            return $(this).attr("val");
                        }).get().join("|")
                        $("#hdimage_pics").val(imgs);
                    }
                });
            });

            //            var imgpath_pic;
            //            var button = $('#uploadbtpic'), interval;
            //            new AjaxUpload(button, {
            //                action: 'cutimage.ashx',
            //                name: 'picfile',
            //                responseType: 'json',
            //                data: { action: 'upload' },
            //                onSubmit: function (file, ext) {
            //                    if (!(ext && /^(jpg|png|jpeg|gif)$/i.test(ext))) {
            //                        alert('只能上传图片！');
            //                        return false;
            //                    }
            //                    button.val('上传中');
            //                    this.disable();
            //                    interval = window.setInterval(function () {
            //                        var text = button.val();
            //                        if (text.length < 13) {
            //                            button.val(text + '.');
            //                        } else {
            //                            button.val('上传中');
            //                        }
            //                    }, 200);
            //                },
            //                onComplete: function (file, response) {
            //                    $("#hdnAction").val("GenerateBitmap");
            //                    button.val('修改图片');
            //                    window.clearInterval(interval);
            //                    this.enable();
            //                    $("#cutImg").empty();
            //                    $.fn.bitmapCutter({
            //                        src: response.src,
            //                        id: response.id,
            //                        renderTo: '#cutImg',
            //                        cutterSize: { width: 200, height: 150 },
            //                        holderSize: { width: 500, height: 450 },
            //                        zoomStep: 0.05,
            //                        zoomIn: 2.0,
            //                        zoomOut: 0.05,
            //                        onGenerated: function (src) {
            //                            if (src.c)
            //                                imgpath_pic = src.c;
            //                            else
            //                                imgpath_pic = src;
            //                        },
            //                        onComplete: function () {
            //                            $(".newimgcheck:checked").each(function () {
            //                                if ($(this).attr("val") == "c") {
            //                                    $("#imgpic").attr("src", imgpath_pic);
            //                                    $("#hdimage_pic").val(imgpath_pic);
            //                                }
            //                            });
            //                            $("#cutImg").dialog('close');
            //                        },
            //                        rotateAngle: 90,
            //                        lang: {
            //                            clockwise: '顺时针旋转{0}度.',
            //                            counterclockwise: '逆时针旋转{0}度.',
            //                            generate: '生成!',
            //                            process: '请稍后，正在生成封面.....',
            //                            zoomout: '缩小',
            //                            zoomin: '放大',
            //                            left: '向左',
            //                            right: '向右',
            //                            up: '向上',
            //                            down: '向下'
            //                        }
            //                    });
            //                    $('#cutImg').dialog('open');
            //                }
            //            });

            //            $(".uploadbtpics").each(function () {
            //                var imgpath_pics;
            //                var button1 = $(this), interval1;
            //                new AjaxUpload(button1, {
            //                    action: 'cutimage.ashx',
            //                    name: 'picfile',
            //                    responseType: 'json',
            //                    data: { action: 'upload' },
            //                    onSubmit: function (file, ext) {
            //                        if (!(ext && /^(jpg|png|jpeg|gif)$/i.test(ext))) {
            //                            alert('只能上传图片！');
            //                            return false;
            //                        }
            //                        button1.val('上传中');
            //                        this.disable();
            //                        interval1 = window.setInterval(function () {
            //                            var text = button1.val();
            //                            if (text.length < 13) {
            //                                button1.val(text + '.');
            //                            } else {
            //                                button1.val('上传中');
            //                            }
            //                        }, 200);
            //                    },
            //                    onComplete: function (file, response) {
            //                        $("#hdnAction").val("GenerateBitmaps");
            //                        button1.val('修改图片');
            //                        window.clearInterval(interval1);
            //                        this.enable();
            //                        $("#cutImg").empty();
            //                        $.fn.bitmapCutter({
            //                            src: response.src,
            //                            id: response.id,
            //                            renderTo: '#cutImg',
            //                            cutterSize: { width: 460, height: 345 },
            //                            holderSize: { width: 600, height: 450 },
            //                            zoomStep: 0.05,
            //                            zoomIn: 2.0,
            //                            zoomOut: 0.05,
            //                            onGenerated: function (src) {
            //                                if (src.c)
            //                                    imgpath_pics = src.c;
            //                                else
            //                                    imgpath_pics = src;
            //                            },
            //                            onComplete: function () {
            //                                $(".newimgcheck:checked").each(function () {
            //                                    if ($(this).attr("val") == "c") {
            //                                        $("img", button1.parent()).attr("src", imgpath_pics).attr("val", imgpath_pics);
            //                                        var imgs = $(".imgpics").map(function () {
            //                                            return $(this).attr("val");
            //                                        }).get().join("|")
            //                                        $("#hdimage_pics").val(imgs);
            //                                    }
            //                                });
            //                                $("#cutImg").dialog('close');
            //                            },
            //                            rotateAngle: 90,
            //                            lang: {
            //                                clockwise: '顺时针旋转{0}度.',
            //                                counterclockwise: '逆时针旋转{0}度.',
            //                                generate: '生成!',
            //                                process: '请稍后，正在生成封面.....',
            //                                zoomout: '缩小',
            //                                zoomin: '放大',
            //                                left: '向左',
            //                                right: '向右',
            //                                up: '向上',
            //                                down: '向下'
            //                            }
            //                        });
            //                        $('#cutImg').dialog('open');
            //                    }
            //                });
            //            });


            $("#btnSubmit").click(function () {
                return CheckForm();
            });
        });

        function CheckForm() {
            var pass = true;

//            for (var i = 0; i < $(".required").length; i++) {
//                var t = $($(".required")[i]);
//                if ($.trim(t.val()) == "") {
//                    t.focus();
//                    prompts(t, "请填写此字段");
//                    pass = false;
//                    break;
//                }
//            }
//            if (pass && $("#hdimage_pic").val() == "") {
//                var t = $("#uploadbtpic");
//                t.focus();
//                prompts(t, "请上传列表图片");
//                pass = false;
//            }

            return pass;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ValidationSummary ID="vsProduct" runat="server" ShowMessageBox="True" ShowSummary="false" />
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="biaoge3">
        <caption class="bt2">
            <%= GetInt("id") > 0 ? "编辑" : "新增" %>产品</caption>
        <tbody>
            <tr>
                <td class="bg1">
                    产品名称：
                </td>
                <td class="bg2">
                    <asp:TextBox runat="server" ID="txtProductName" CssClass="srk1 w160"></asp:TextBox>
                </td>
                <td class="bg1">
                    产品价格：
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtPrice" CssClass="srk1 w160" onkeyup="value=value.replace(/[^\0-9\.]/g,'')" onpaste="value=value.replace(/[^\0-9\.]/g,'')" oncontextmenu = "value=value.replace(/[^\0-9\.]/g,'')"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="bg1" colspan="3">
                    消声片价格：
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtXSPPrice" CssClass="srk1 w160" onkeyup="value=value.replace(/[^\0-9\.]/g,'')" onpaste="value=value.replace(/[^\0-9\.]/g,'')" oncontextmenu = "value=value.replace(/[^\0-9\.]/g,'')"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    Lamda型号：
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtModelNumber" CssClass="srk1 w160"></asp:TextBox>
                </td>
                <td class="bg1">
                    OE型号：
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtOEModelNumber" CssClass="srk1 w160"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    产地：
                </td>
                <td class="bg2">
                    <asp:TextBox runat="server" ID="txtArea" CssClass="srk1 w160"></asp:TextBox>
                </td>
                <td class="bg1">
                    材质：
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtMaterial" CssClass="srk1 w160"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    产品类型：
                </td>
                <td class="bg2">
                    <asp:DropDownList runat="server" ID="ddlProductType">
                    </asp:DropDownList>
                </td>
                <td class="bg1">
                    建议更换周期：
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtReplacement" CssClass="srk1 w160"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="bg1 vt">
                    适用车型：
                </td>
                <td colspan="3">
                    <asp:ScriptManager runat="server" ID="smCabmodels">
                    </asp:ScriptManager>
                    <asp:UpdatePanel runat="server" ID="upnCabmodels">
                        <ContentTemplate>
                            <div>
                                <asp:DropDownList ID="ddlBrand" runat="server" CssClass="mr10" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlBrand_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlCabmodel" runat="server" CssClass="mr10 w100" AutoPostBack="true"
                                    Enabled="false" OnSelectedIndexChanged="ddlCabmodel_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlPailiang" runat="server" CssClass="mr10 w100" AutoPostBack="true"
                                    Enabled="false" OnSelectedIndexChanged="ddlPailiang_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlNianfen" runat="server" CssClass="mr10 w60" Enabled="false">
                                </asp:DropDownList>
                                <asp:Button runat="server" ID="btnSearch" Text="搜索" CssClass="an2" OnClick="btnSearch_Click" />
                            </div>
                            <div class="mt10">
                                <asp:ListBox runat="server" ID="lbxCabmodels" SelectionMode="Multiple" Width="360"
                                    Rows="10"></asp:ListBox>
                                <asp:Button runat="server" ID="btnCabmodelsDel" Text="<<" CssClass="vt" Style="margin-top: 75px;"
                                    OnClick="btnCabmodelsDel_Click" />
                                <asp:Button runat="server" ID="btnCabmodelsAdd" Text=">>" CssClass="vt" Style="margin-top: 75px;"
                                    OnClick="btnCabmodelsAdd_Click" />
                                <asp:ListBox runat="server" ID="lbxCabmodelsSel" SelectionMode="Multiple" Width="360"
                                    Rows="10"></asp:ListBox>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    列表图片：
                </td>
                <td colspan="3">
                    <input type="button" name="uploadbtpic" id="uploadbtpic" value="上传图片" class="an3" /><br />
                    <img src="../images/fm.jpg" alt="列表图片" id="imgpic" style="width: 192px; height: 144px;"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    展示图片：
                </td>
                <td colspan="3">
                    <ul>
                        <li class="fll" style="padding-right: 10px;">
                            <input type="button" value="上传图片" class="an3 uploadbtpics" /><br />
                            <img src="../images/fm.jpg" alt="展示图片1" id="imgpics1" class="imgpics" style="width: 64px;
                                height: 48px;" val="" runat="server" />
                        </li>
                        <li class="fll" style="padding-right: 10px;">
                            <input type="button" value="上传图片" class="an3 uploadbtpics" /><br />
                            <img src="../images/fm.jpg" alt="展示图片2" id="imgpics2" class="imgpics" style="width: 64px;
                                height: 48px;" val="" runat="server" />
                        </li>
                        <li class="fll" style="padding-right: 10px;">
                            <input type="button" value="上传图片" class="an3 uploadbtpics" /><br />
                            <img src="../images/fm.jpg" alt="展示图片3" id="imgpics3" class="imgpics" style="width: 64px;
                                height: 48px;" val="" runat="server" />
                        </li>
                        <li class="fll" style="padding-right: 10px;">
                            <input type="button" value="上传图片" class="an3 uploadbtpics" /><br />
                            <img src="../images/fm.jpg" alt="展示图片4" id="imgpics4" class="imgpics" style="width: 64px;
                                height: 48px;" val="" runat="server" />
                        </li>
                        <li class="fll" style="padding-right: 10px;">
                            <input type="button" value="上传图片" class="an3 uploadbtpics" /><br />
                            <img src="../images/fm.jpg" alt="展示图片5" id="imgpics5" class="imgpics" style="width: 64px;
                                height: 48px;" val="" runat="server" />
                        </li>
                    </ul>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    规格：
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtStandard" runat="server" TextMode="MultiLine" CssClass="ckeditor"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    产品介绍：
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtIntroduce" runat="server" TextMode="MultiLine" CssClass="ckeditor"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                </td>
                <td colspan="3">
                    <asp:Button ID="btnSubmit" runat="server" Text="确认提交" CssClass="an1" OnClick="btnSubmit_Click" />
                </td>
            </tr>
        </tbody>
    </table>
    <input id="hdimage_pic" runat="server" type="hidden" />
    <input id="hdimage_pics" runat="server" type="hidden" />
    <input id="hdnAction" type="hidden" value="GenerateBitmap" />
    <div id="cutImg" title="裁剪图片">
    </div>
    </form>
</body>
</html>
