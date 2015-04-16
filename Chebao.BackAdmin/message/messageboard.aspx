<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="messageboard.aspx.cs" Inherits="Chebao.BackAdmin.message.messageboard"
    ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>耐磨达产品查询系统-纠错反馈有奖</title>
    <link href="../css/headfoot2.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <link href="../css/kp.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../js/ajaxupload.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(function () {
            $(".uploadbtpics").each(function () {
                var imgpath_pics;
                var button1 = $(this), interval1;
                new AjaxUpload(button1, {
                    action: '/cutimage.ashx',
                    name: 'picfile',
                    responseType: 'json',
                    data: { action: 'customerupload' },
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
                        button1.val('上传图片');
                        window.clearInterval(interval1);
                        this.enable();

                        $("#txtMessageContent").html($("#txtMessageContent").html() + "<br /><img src=" + response.src + " alt=\"\" />");
                    }
                });
            });

            $("#btnAddToMessage").click(function () {
                $("#txtMessageContent").html($("#txtMessageContent").html()
                + "<br />" + $("#ddlNianfen").val() + " "
                + $("#ddlBrand").val() + " "
                + $("#ddlCabmodel").val() + " "
                + $("#ddlPailiang").val() + " "
                + $("#ddlProductType option:checked").html() + " "
                + $("#ddlProducts").val());
            });

            $("#btnSubmit").click(function () {
                $("#hdnMessageContent").val($("#txtMessageContent").html());
                if ($.trim($("#txtMessageTitle").val()) == "") {
                    alert("请填写简述信息");
                    return false;
                }
                if ($.trim($("#txtMessageContent").html()) == "") {
                    alert("请填写详细描述信息");
                    return false;
                }
            });
        })
    </script>
</head>
<body>
    <div class="header" id="head_01">
        <div class="header_logo">
            <img src="../images/headlogo.jpg" />
        </div>
        <div class="header_nav">
            <ul>
                <li><a href="javascript:void(0);">首页</a></li><li><a href="/product/products.aspx">产品查询</a></li><li>
                    <a href="javascript:void(0);">公司简介</a></li><li><a href="javascript:void(0);">联系我们</a></li><li
                        class="navcurrent"><a href="/message/messageboard.aspx">纠错反馈有奖</a></li>
            </ul>
            <div class="header_navinfo">
                <span class="navinfo_user">
                    <%= AdminName %>，您好！</span> <span class="navinfo_opt"><a href="/logout.aspx">安全退出</a><a
                        class="ml10" href="/user/userchangepw.aspx">修改密码</a><a href="/product/myorders.aspx"
                            class="ml10">我的订单</a><%if (Admin.SizeView > 0)
                                                   { %><a href="/product/myorders.aspx" class="cccx">尺寸查询</a><%} %></span>
            </div>
        </div>
    </div>
    <div id="main">
        <form runat="server" id="form1">
        <div class="address-bar" style="padding-top: 20px;">
            <ul>
                <li style="margin-bottom: 10px;"><span><em class="red">*</em>简述：</span>
                    <div>
                        <asp:TextBox runat="server" ID="txtMessageTitle" CssClass="srk1"></asp:TextBox>
                    </div>
                </li>
                <li style="margin-bottom: 10px;"><span>产品类型：</span>
                    <div>
                        <asp:DropDownList ID="ddlProductType" runat="server">
                        </asp:DropDownList>
                    </div>
                </li>
                <li style="margin-bottom: 10px;"><span>车型：</span>
                    <div>
                        <asp:ScriptManager runat="server" ID="smCabmodels">
                        </asp:ScriptManager>
                        <asp:UpdatePanel runat="server" ID="upnCabmodels">
                            <ContentTemplate>
                                <asp:DropDownList runat="server" ID="ddlBrand" AutoPostBack="true" Style="width: 213px!important;"
                                    OnSelectedIndexChanged="ddlBrand_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlCabmodel" runat="server" AutoPostBack="true" Style="width: 160px!important;"
                                    Enabled="false" OnSelectedIndexChanged="ddlCabmodel_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlPailiang" runat="server" AutoPostBack="true" Style="width: 100px!important;"
                                    Enabled="false" OnSelectedIndexChanged="ddlPailiang_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlNianfen" runat="server" Enabled="false" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlNianfen_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </li>
                <li style="margin-bottom: 10px;"><span>产品：</span>
                    <div>
                        <asp:UpdatePanel runat="server" ID="upnProducts">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlProducts" runat="server" Enabled="false">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </li>
                <li style="margin-bottom: 10px;">
                    <input type="button" id="btnAddToMessage" value="添加到详细描述" />
                </li>
                <li><span><em class="red">*</em>详细描述：</span>
                    <div>
                        <div contenteditable="true" id="txtMessageContent">
                        </div>
                        &nbsp;
                        <input type="hidden" runat="server" id="hdnMessageContent" />
                    </div>
                </li>
                <li><span>&nbsp;</span>
                    <div>
                        <input type="button" name="uploadbtpic" id="uploadbtpic" value="上传图片" class="an3 uploadbtpics" />
                        <asp:Button runat="server" ID="btnSubmit" Text=" 提 交 " CssClass="an3" OnClick="btnSubmit_Click" />
                    </div>
                </li>
            </ul>
        </div>
        </form>
    </div>
    <div class="footer">
        <div class="footmain">
            浙江耐磨达刹车片有限公司 版权所有 2014-2024 Shantui Zhejiang Lamda Brake Pads Co.,Ltd
        </div>
    </div>
</body>
</html>
