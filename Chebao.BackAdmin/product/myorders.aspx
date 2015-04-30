<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="myorders.aspx.cs" Inherits="Chebao.BackAdmin.product.myorders" %>

<%@ Register Src="../uc/header.ascx" TagName="header" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>耐磨达产品查询系统-我的订单</title>
    <link href="../css/headfoot2.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <link href="../css/kp.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="../js/jquery.marquee.js" type="text/javascript"></script>
    <script src="../js/ajaxupload.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        var pageindex = 1;
        var pagecount = <%=PageCount %>;
        $(function () {
            //更多订单
            $("#btnMore").click(function () {
                GetMore();
            });

            $(".cancel").click(function(){
                return confirm("确定要取消此订单吗？");
            });

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

                        var img = button1.parent().prev().find("img");
                        var id = button1.prev().val();
                        $.ajax({
                            url: "/remoteaction.ashx",
                            data: { action: "updateorderpic",col:"remittanceadvice", id: id, src: response.src, d: new Date() },
                            type: 'GET',
                            dataType: "json",
                            error: function (msg) {
                                alert("发生错误");
                            },
                            success: function (data) {
                                img.attr("src",response.src);
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
        })

        function GetMore() { 
            if(pageindex >= pagecount){
            alert("没有更多评论了");
            return;
        }
        else{
            $("#dvLoading").show();
            pageindex++;
            $.ajax({
                url: "myordermore.aspx",
                data: { pageindex:pageindex, d: new Date() },
                type: 'GET',
                dataType: "text",
                error: function (msg) {
                    $("#dvLoading").hide();
                    alert("发生错误");
                },
                success: function (data) {
                    if(data == ""){
                        alert("数据获取失败");
                    }else{
                        var order = $(data);
                        order.hide();
                        $("#J_More").prepend(order);
                        order.fadeIn();
                    }

                    $("#dvLoading").hide();
                }
            });
        }
        }
    </script>
</head>
<body>
    <uc1:header ID="header1" runat="server" />
    <div id="main">
        <div class="buy-content">
            <div class="buy-order-field" data-spm="3" id="J_Order">
                <h3>
                    我的订单</h3>
                <asp:Repeater runat="server" ID="rptData" OnItemDataBound="rptData_ItemDataBound">
                    <ItemTemplate>
                        <div style="padding-left: 10px; color: #3d3d3d; margin-top: 10px;">
                            <span style="font-weight: bold;">
                                <%#Eval("AddTime") %></span>&nbsp; 订单号：<%#Eval("OrderNumber") %>&nbsp; 订单状态：<%#Eval("OrderStatus").ToString() == "未收款" ? "未付款" : Eval("OrderStatus").ToString()%><a
                                    href="?action=cancel&id=<%#Eval("ID") %>" class="pl10 cancel <%#Eval("OrderStatus").ToString() == "未收款" ? "" : "hide" %>"
                                    style="color: Blue;">取消订单</a><span style="float: right; padding-right: 20px;">订单金额：<em
                                        style="color: #F50"><%#Eval("TotalFee") %></em></span></div>
                        <div style="padding-left: 20px;">
                            <div class="buy-th-line clearfix" style="margin-top: 0;">
                                <span class="buy-th-title">商品</span> <span class="buy-th-price">单价(元)</span> <span
                                    class="buy-th-quantity">数量</span> <span class="buy-th-total">小计(元)</span>
                            </div>
                            <asp:Repeater runat="server" ID="rptOrderProduct" OnItemDataBound="rptOrderProduct_ItemDataBound">
                                <ItemTemplate>
                                    <div id="jigsaw28" class="order">
                                        <div id="jigsaw22" class="item blue-line clearfix">
                                            <div id="jigsaw17" class="itemInfo item-title">
                                                <a target="_blank" href="productview.aspx?id=<%# Eval("ProductID") %>" title="<%# Eval("ProductName")%>"
                                                    class="itemInfo-link J_MakePoint"><span class="item-pic"><span>
                                                        <img class="itemInfo-pic" src="<%# Eval("ProductPic") %>" alt="">
                                                    </span></span><span class="itemInfo-title J_MakePoint">
                                                        <%# Eval("ProductName")%></span> <span class="gray">
                                                            <%#Eval("CabmodelStr")%></span></a>
                                                <asp:Repeater runat="server" ID="rptProductMix">
                                                    <ItemTemplate>
                                                        <div>
                                                            <div class="itemInfo-title" style="display: table-cell; padding: 5px 0;">
                                                                <%#Eval("Name") %></div>
                                                            <div class="item-price" style="display: table-cell; padding: 5px 0;">
                                                                <span id="jigsaw18" class="itemInfo price"><em class="style-normal-small-black J_ItemPrice">
                                                                    <%# Eval("Price")%></em> </span>
                                                            </div>
                                                            <div id="jigsaw19" class="quantity item-quantity">
                                                                <p>
                                                                    <%#Eval("Amount")%></p>
                                                            </div>
                                                            <div id="jigsaw21" class="itemPay item-total">
                                                                <p class="itemPay-price price">
                                                                    <em class="style-normal-bold-red J_ItemTotal">
                                                                        <%#Eval("Sum") %></em>
                                                                </p>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                            <div class="item-form-desc blue-line clearfix">
                                                <div class="item-form-eticketDesc">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="order-extra blue-line">
                                            <div class="order-user-info">
                                                <div id="jigsaw23" class="memo">
                                                    <label>
                                                        给卖家留言：<%#Eval("Remark")%></label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <div class="blue-line order-pic">
                            <ul>
                                <li><a href="javascript:void(0);" class="order-pic-info">
                                    <img class="img" src="<%#string.IsNullOrEmpty(Eval("PicRemittanceAdvice") as string) ? "../images/nopic.png" : Eval("PicRemittanceAdvice").ToString() %>"
                                        alt="汇款单" />汇款单</a> <span class="order-pic-opt"><input type="hidden" value="<%#Eval("ID") %>" />
                                            <input type="button" value="<%#string.IsNullOrEmpty(Eval("PicRemittanceAdvice") as string) ? "上传图片" : "修改图片" %>"
                                                class="an3<%# Eval("OrderStatus").ToString() == "未收款" ? string.Empty : " hide" %> uploadbtpics" /></span></li>
                                <li><a href="javascript:void(0);" class="order-pic-info">
                                    <img class="img" src="<%#string.IsNullOrEmpty(Eval("PicInvoice") as string) ? "../images/nopic.png" : Eval("PicInvoice").ToString() %>"
                                        alt="发货单" />发货单</a> <span class="order-pic-opt">&nbsp;</span></li>
                                <li><a href="javascript:void(0);" class="order-pic-info">
                                    <img class="img" src="<%#string.IsNullOrEmpty(Eval("PicListItem") as string) ? "../images/nopic.png" : Eval("PicListItem").ToString() %>"
                                        alt="清单" />清单</a> <span class="order-pic-opt">&nbsp;</span></li>
                                <li><a href="javascript:void(0);" class="order-pic-info">
                                    <img class="img" src="<%#string.IsNullOrEmpty(Eval("PicBookingnote") as string) ? "../images/nopic.png" : Eval("PicBookingnote").ToString() %>"
                                        alt="托运单" />托运单</a> <span class="order-pic-opt">&nbsp;</span></li>
                            </ul>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <%if (PageCount > 1)
                  {%>
                <div style="padding-top: 10px; padding-left: 10px;" id="J_More">
                    <a href="javascript:void(0);" id="btnMore">更多>></a></div>
                <%} %>
            </div>
        </div>
    </div>
    <div class="footer">
        <div class="footmain">
            浙江耐磨达刹车片有限公司 版权所有 2014-2024 Shantui Zhejiang Lamda Brake Pads Co.,Ltd
        </div>
    </div>
</body>
    <div id="flay" style="display:none;position:fixed;top:0;left:0;z-index:999;text-align:center;">
        <img src=""/>
    </div>
</html>
