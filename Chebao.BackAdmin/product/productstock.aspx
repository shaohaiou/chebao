<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="productstock.aspx.cs" Inherits="Chebao.BackAdmin.product.productstock" %>

<asp:repeater runat="server" id="rptProductMix" onitemdatabound="rptProductMix_ItemDataBound">
    <HeaderTemplate>
        <div  class="itemtype-info bold">
        <ul>
            <li class="th" style="width: 160px;">
                商品信息</li>
            <li class="th th-price" style="padding-left:18px;">
                单价
            </li>
            <li class="th th-amount">
                数量
            </li>
        </ul>
        </div>
        <div class="itemtype-info item-content">
    </HeaderTemplate>
    <ItemTemplate>
        <ul>
            <li class="th" style="width: 160px;">
                <%#Eval("Name") %>
                <span class="gray">(库存<a id="spStock" runat="server" href="javascript:void(0);"></a>)</span> </li>
            <li class="th th-price">
                <div class="td-inner">
                    <div class="item-price price-promo-promo">
                        <div class="price-content">
                            <div class="price-line">
                                <em class="J_Price price-now" id="txtprice_<%#Container.ItemIndex %>">
                                    <%# Eval("Price") %></em></div>
                        </div>
                    </div>
                </div>
            </li>
            <li class="th th-amount">
                <div class="td-inner">
                    <div class="amount-wrapper ">
                        <div class="item-amount ">
                            <a href="javascript:void(0);" class="J_Minus minus">-</a><input type="text" value=""
                                id="txtAmount" runat="server" class="text text-amount J_ItemAmount" data-max=""
                                data-id="" data-name='<%#Eval("Name") %>' /><a href="javascript:void(0);" class="J_Plus plus">+</a></div>
                        <div class="amount-msg J_AmountMsg">
                        </div>
                    </div>
                </div>
            </li>
        </ul>
    </ItemTemplate>
    <FooterTemplate>
        </div>
    </FooterTemplate>
</asp:repeater>
