<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="productmix.aspx.cs" Inherits="Chebao.BackAdmin.useradmin.productmix" %>

<asp:repeater runat="server" id="rptProductMix" onitemdatabound="rptProductMix_ItemDataBound">
    <HeaderTemplate>
        <div  class="itemtype-info bold">
        <ul>
            <li class="th" style="width: 160px;">
                商品信息</li>
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
                <%#Eval("Name") %></li>
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

