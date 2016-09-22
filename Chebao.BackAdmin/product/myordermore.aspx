<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="myordermore.aspx.cs" Inherits="Chebao.BackAdmin.product.myordermore" %>

<asp:Repeater runat="server" ID="rptData" OnItemDataBound="rptData_ItemDataBound">
    <ItemTemplate>
    <div class="orderbody" style="<%#Eval("OrderStatus").ToString() == "已发货" ? "border-color:green;" : (Eval("OrderStatus").ToString() == "未收款" ? "border-color:red;" : "")%>">
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
    </div>
    </ItemTemplate>
</asp:Repeater>
