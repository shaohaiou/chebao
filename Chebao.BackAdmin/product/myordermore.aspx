<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="myordermore.aspx.cs" Inherits="Chebao.BackAdmin.product.myordermore" %>

<asp:repeater runat="server" id="rptData" onitemdatabound="rptData_ItemDataBound">
    <ItemTemplate>
        <div style="padding-left: 10px; color: #3d3d3d; margin-top: 10px;">
            订单号：<%#Eval("OrderNumber") %><span style="float: right; padding-right: 20px;">订单金额：<em
                                style="color: #F50"><%#Eval("TotalFee") %></em></span></div>
        <div style="padding-left: 20px;">
            <div class="buy-th-line clearfix" style="margin-top: 0;">
                <span class="buy-th-title">商品</span> <span class="buy-th-price">单价(元)</span> <span
                    class="buy-th-quantity">数量</span> <span class="buy-th-total">小计(元)</span>
            </div>
            <asp:Repeater runat="server" ID="rptOrderProduct">
                <ItemTemplate>
                    <div id="jigsaw28" class="order">
                        <div id="jigsaw22" class="item blue-line clearfix">
                            <div id="jigsaw17" class="itemInfo item-title">
                                <a target="_blank" href="productview.aspx?id=<%# Eval("ProductID") %>" title="<%# Eval("ProductName")%>"
                                    class="itemInfo-link J_MakePoint"><span class="item-pic"><span>
                                        <img class="itemInfo-pic" src="<%# Eval("ProductPic") %>" alt="">
                                    </span></span><span class="itemInfo-title J_MakePoint">
                                        <%# Eval("ProductName")%></span> </a>
                                <div class="itemInfo-sku">
                                    <span></span>
                                </div>
                                <p class="c2c-extraInfo-container promo-extraInfo">
                                </p>
                            </div>
                            <div class="item-price">
                                <span id="jigsaw18" class="itemInfo price"><em class="style-normal-small-black J_ItemPrice">
                                    <%# Math.Round(decimal.Parse(Eval("Price").ToString().StartsWith("¥") ? Eval("Price").ToString().Substring(1) : Eval("Price").ToString()),2)%></em>
                                </span>
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
    </ItemTemplate>
</asp:repeater>
