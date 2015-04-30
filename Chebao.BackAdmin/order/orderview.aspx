<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="orderview.aspx.cs" Inherits="Chebao.BackAdmin.order.orderview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>订单详情</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
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
                        button1.val('修改图片');
                        window.clearInterval(interval1);
                        this.enable();

                        var img = button1.parent().find("img");
                        $.ajax({
                            url: "/remoteaction.ashx",
                            data: { action: "updateorderpic",col:button1.attr("col"), id: <%=CurrentOrder.ID %>, src: response.src, d: new Date() },
                            type: 'GET',
                            dataType: "json",
                            error: function (msg) {
                                alert("发生错误");
                            },
                            success: function (data) {
                                img.attr("src", response.src);
                            }
                        });
                    }
                });
            });
            $(".img").click(function(){
                if($(this).attr("src") != "../images/nopic.png"){
                    $("#flay img").attr("src",$(this).attr("src"));
                    $("#flay").fadeIn();
                    $("#flay img").css("margin-top",parseInt(($(window).height() - $("#flay img").height()) / 2) + "px");
                }
            });
            $("#flay img").click(function(){
                $("#flay").fadeOut();
            });
            $("#flay").width($(document).width()).height($(window).height());
        });
    </script>
</head>
<body>
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="biaoge3">
        <caption class="bt2">
            订单详情</caption>
        <tbody>
            <tr>
                <td class="bg1">
                    订单号：
                </td>
                <td>
                    <%= CurrentOrder.OrderNumber%>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    订单状态：
                </td>
                <td>
                    <%= CurrentOrder.OrderStatus.ToString()%>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    收货地址：
                </td>
                <td>
                    <%=CurrentOrder.Province%>
                    <%=CurrentOrder.City%>
                    <%=CurrentOrder.District%>
                    <%=CurrentOrder.Address%>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    邮政编码：
                </td>
                <td>
                    <%=CurrentOrder.PostCode%>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    收货人姓名：
                </td>
                <td>
                    <%=CurrentOrder.LinkName%>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    手机号码：
                </td>
                <td>
                    <%=CurrentOrder.LinkMobile%>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    电话号码：
                </td>
                <td>
                    <%=CurrentOrder.LinkTel%>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    订单信息：
                </td>
                <td>
                    <asp:Repeater runat="server" ID="rptData" OnItemDataBound="rptData_ItemDataBound">
                        <HeaderTemplate>
                            <ul style="width: 940px; background-color: #9BD1F7; font-weight: bold; height: 16px;
                                padding: 2px 0 0 2px">
                                <li class="fll w300">产品名称/型号</li>
                                <li class="fll w40">数量</li>
                                <li class="fll w100">单价</li>
                                <li class="fll w100">折扣/组合单价</li>
                                <li class="fll" style="width: 400px">留言</li>
                            </ul>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <ul style="width: 942px; height: 16px;">
                                <li class="fll">
                                    <div style="background-color: #D2EBFC; width: 540px;">
                                        <%# Eval("ProductName")%><span class="gray">
                                            <%#Eval("CabmodelStr")%></span></div>
                                    <asp:Repeater runat="server" ID="rptProductMix">
                                        <ItemTemplate>
                                            <div class="pl10">
                                                <span style="width: 290px; display: table-cell;">
                                                    <%#Eval("Name") %></span> <span class="w40" style="display: table-cell;">
                                                        <%#Eval("Amount") %></span> <span class="w100" style="display: table-cell;">
                                                            <%= GetOriginalPrice()%></span> <span class="w100" style="display: table-cell;">
                                                                <%#Eval("Price")%></span>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </li>
                                <li class="fll" style="width: 400px">
                                    <%#Eval("Remark") %>&nbsp;</li>
                            </ul>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    实付款：
                </td>
                <td>
                    <%=CurrentOrder.TotalFee%>
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    汇款单：
                </td>
                <td>
                    <img class="img" style="width:160px;height:72px;" src="<%=string.IsNullOrEmpty(CurrentOrder.PicRemittanceAdvice) ? "../images/nopic.png" : CurrentOrder.PicRemittanceAdvice %>" />
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    发货单：
                </td>
                <td>
                    <input type="button" col="invoice" value="<%=string.IsNullOrEmpty(CurrentOrder.PicInvoice) ? "上传图片" : "修改图片" %>" class="an3 uploadbtpics" /><br />
                    <img class="img" style="width:160px;height:72px;" src="<%=string.IsNullOrEmpty(CurrentOrder.PicInvoice) ? "../images/nopic.png" : CurrentOrder.PicInvoice %>" />
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    清单：
                </td>
                <td>
                    <input type="button" col="listitem" value="<%=string.IsNullOrEmpty(CurrentOrder.PicListItem) ? "上传图片" : "修改图片" %>" class="an3 uploadbtpics" /><br />
                    <img class="img" style="width:160px;height:72px;" src="<%=string.IsNullOrEmpty(CurrentOrder.PicListItem) ? "../images/nopic.png" : CurrentOrder.PicListItem %>" />
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    托运单：
                </td>
                <td>
                    <input type="button" col="bookingnote" value="<%=string.IsNullOrEmpty(CurrentOrder.PicBookingnote) ? "上传图片" : "修改图片" %>" class="an3 uploadbtpics" /><br />
                    <img class="img" style="width:160px;height:72px;" src="<%=string.IsNullOrEmpty(CurrentOrder.PicBookingnote) ? "../images/nopic.png" : CurrentOrder.PicBookingnote %>" />
                </td>
            </tr>
            <tr>
                <td class="bg1">
                </td>
                <td>
                    <form runat="server">
                    <asp:Button ID="btnBack" runat="server" Text="返回" CssClass="an1" OnClick="btnBack_Click" />
                    </form>
                </td>
            </tr>
        </tbody>
    </table>
    <div id="flay" style="display:none;position:fixed;top:0;left:0;z-index:999;text-align:center;">
        <img src=""/>
    </div>
</body>
</html>
