﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using System.Web.UI.HtmlControls;
using Chebao.Tools;
using System.Data;

namespace Chebao.BackAdmin.product
{
    public partial class shoppingtrolleymg : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
        }

        private List<ShoppingTrolleyInfo> currentshoppingtrolleylist = null;
        public List<ShoppingTrolleyInfo> CurrentShoppingTrolleyList
        {
            get
            {
                if (currentshoppingtrolleylist == null)
                {
                    currentshoppingtrolleylist = Cars.Instance.GetShoppingTrolleyByUserID(AdminID);
                }
                return currentshoppingtrolleylist;
            }
        }

        private List<OrderInfo> orderall = null;
        public List<OrderInfo> OrderAll
        {
            get
            {
                if (orderall == null)
                {
                    orderall = Cars.Instance.GetOrderList(true);
                }
                return orderall;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (GetString("action") == "del")
                {
                    string ids = GetString("ids");
                    Cars.Instance.DeleteShoppingTrolley(ids, AdminID);
                    Cars.Instance.ReloadShoppingTrolley(AdminID);
                }

                LoadData();
            }
            else
            {
                Account();
            }
        }

        private void LoadData()
        {
            List<ShoppingTrolleyInfo> listShoppingTrolley = Cars.Instance.GetShoppingTrolleyByUserID(AdminID);
            if (listShoppingTrolley.Count > 0)
            {
                List<ProductInfo> listAllProduct = Cars.Instance.GetProductList(true);
                string[] pids = listShoppingTrolley.OrderByDescending(s => s.ID).Select(s => s.ProductID + "|" + s.ID).ToArray();
                List<ProductInfo> listProdcutInShoppingTrolley = new List<ProductInfo>();
                for (int i = 0; i < pids.Length; i++)
                {
                    int pid = DataConvert.SafeInt(pids[i].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[0]);
                    int sid = DataConvert.SafeInt(pids[i].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[1]);
                    ProductInfo entity = listAllProduct.Find(p => p.ID == pid).Clone();
                    entity.SID = sid;
                    entity.CabmodelStr = listShoppingTrolley.FindAll(s => s.ID == sid).Select(s => string.IsNullOrEmpty(s.CabmodelStr) ? string.Empty : (" - " + s.CabmodelStr)).First();
                    listProdcutInShoppingTrolley.Add(entity);
                }
                rptData.DataSource = listProdcutInShoppingTrolley;
                rptData.DataBind();
            }
        }

        protected void rptData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ProductInfo entity = e.Item.DataItem as ProductInfo;
                Repeater rptProductMix = (Repeater)e.Item.FindControl("rptProductMix");
                ShoppingTrolleyInfo st = CurrentShoppingTrolleyList.Find(s => s.ID == entity.SID);

                rptProductMix.DataSource = entity.ProductMix.Select(p => new ProductMixInfo { Name = p.Key, Stock = p.Value, SID = entity.SID, Price = GetProductMixPrice(entity.Price, p.Key) }).ToList();
                rptProductMix.DataBind();
            }
        }

        protected void rptProductMix_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlInputText txtAmount = (HtmlInputText)e.Item.FindControl("txtAmount");
                HtmlAnchor spStock = (HtmlAnchor)e.Item.FindControl("spStock");
                if (txtAmount != null)
                {
                    txtAmount.Attributes["data-id"] = ((ProductMixInfo)e.Item.DataItem).SID.ToString() + "_" + e.Item.ItemIndex;
                    txtAmount.Value = "1";
                    ProductMixInfo entity = ((ProductMixInfo)e.Item.DataItem);
                    int stock = entity.Stock;
                    if (OrderAll.Exists(o => o.OrderStatus == OrderStatus.未收款 && o.OrderProducts != null && o.OrderProducts.Exists(p => p.ProductMixList != null && p.ProductMixList.Exists(pm => pm.Name == entity.Name))))
                    {
                        int amount = 0;
                        List<OrderInfo> orderlist = OrderAll.FindAll(o => o.OrderStatus == OrderStatus.未收款 && o.OrderProducts != null && o.OrderProducts.Exists(p => p.ProductMixList != null && p.ProductMixList.Exists(pm => pm.Name == entity.Name)));
                        orderlist.ForEach(delegate(OrderInfo o)
                        {
                            amount += o.OrderProducts.FindAll(p => p.ProductMixList != null && p.ProductMixList.Exists(pm => pm.Name == entity.Name)).Sum(p => p.ProductMixList.FindAll(pm => pm.Name == entity.Name).Sum(pm => pm.Amount));
                        });
                        stock -= amount;
                    }
                    if (stock < 0)
                        stock = 0;
                    entity.Stock = stock;
                    txtAmount.Attributes["data-max"] = stock.ToString();
                    spStock.InnerHtml = stock.ToString();
                }
            }
        }

        private string GetProductMixPrice(string pricestr, string mn)
        {
            decimal price = DataConvert.SafeDecimal(pricestr.StartsWith("¥") ? pricestr.Substring(1) : pricestr);
            if (mn.ToLower().Replace("w", string.Empty).Replace("f", string.Empty).EndsWith("m"))
                price = price * Admin.DiscountM / 10;
            else if (mn.ToLower().Replace("w", string.Empty).Replace("f", string.Empty).EndsWith("y"))
                price = price * Admin.DiscountY / 10;
            else if (mn.ToLower().Replace("w", string.Empty).Replace("f", string.Empty).EndsWith("h"))
                price = price * Admin.DiscountH / 10;
            else if (mn.ToLower().Replace("w", string.Empty).Replace("f", string.Empty).EndsWith("s"))
                price = price * Admin.DiscountS / 10;
            else if (mn.ToLower().Replace("w", string.Empty).Replace("f", string.Empty).EndsWith("k"))
                price = price * Admin.DiscountK / 10;
            else if (mn.ToLower().Replace("w", string.Empty).Replace("f", string.Empty).EndsWith("p"))
                price = price * Admin.DiscountP / 10;
            else if (mn.ToLower().StartsWith("ls"))
                price = price * Admin.DiscountLS / 10;
            else if (mn.ToLower().StartsWith("xsp"))
                price = price * Admin.DiscountXSP / 10;
            else if (mn.ToLower().StartsWith("b"))
                price = price * Admin.DiscountB / 10;
            if (mn.ToLower().IndexOf("w") >= 0)
                price = price + price * Admin.AdditemW / 10;
            if (mn.ToLower().IndexOf("f") >= 0)
                price = price + price * Admin.AdditemF / 10;

            return Math.Round(price, 2).ToString();
        }

        private void Account()
        {
            List<OrderProductInfo> listOrderProduct = new List<OrderProductInfo>();
            foreach (RepeaterItem item in rptData.Items)
            {
                HtmlInputHidden hdnModelNumber = (HtmlInputHidden)item.FindControl("hdnModelNumber");
                HtmlInputHidden hdnOEModelNumber = (HtmlInputHidden)item.FindControl("hdnOEModelNumber");
                HtmlInputHidden hdnStandard = (HtmlInputHidden)item.FindControl("hdnStandard");
                HtmlInputHidden hdnPrice = (HtmlInputHidden)item.FindControl("hdnPrice");
                HtmlInputHidden hdnName = (HtmlInputHidden)item.FindControl("hdnName");
                HtmlInputHidden hdnPic = (HtmlInputHidden)item.FindControl("hdnPic");
                HtmlInputHidden hdnSID = (HtmlInputHidden)item.FindControl("hdnSID");
                HtmlInputHidden hdnID = (HtmlInputHidden)item.FindControl("hdnID");
                HtmlInputHidden hdnCabmodelStr = (HtmlInputHidden)item.FindControl("hdnCabmodelStr");
                HtmlInputHidden hdnProductType = (HtmlInputHidden)item.FindControl("hdnProductType");

                Repeater rptProductMix = (Repeater)item.FindControl("rptProductMix");
                if (rptProductMix != null)
                {
                    OrderProductInfo oinfo = new OrderProductInfo()
                    {
                        SID = DataConvert.SafeInt(hdnSID.Value),
                        ProductID = DataConvert.SafeInt(hdnID.Value),
                        ProductName = hdnName.Value,
                        ProductType = (ProductType)DataConvert.SafeInt(hdnProductType.Value),
                        ModelNumber = hdnModelNumber.Value,
                        OEModelNumber = hdnOEModelNumber.Value,
                        Standard = hdnStandard.Value,
                        ProductPic = hdnPic.Value,
                        Introduce = string.Format("Lamda型号：{0} 规格：{1}", hdnModelNumber.Value, hdnStandard.Value),
                        Price = DataConvert.SafeDecimal(hdnPrice.Value),
                        Remark = string.Empty,
                        DiscountM = Admin.DiscountM,
                        DiscountY = Admin.DiscountY,
                        DiscountH = Admin.DiscountH,
                        AdditemW = Admin.AdditemW,
                        AdditemF = Admin.AdditemF,
                        CabmodelStr = hdnCabmodelStr.Value
                    };
                    List<ProductMixInfo> ProductMixList = new List<ProductMixInfo>();
                    foreach (RepeaterItem mitem in rptProductMix.Items)
                    {
                        HtmlInputCheckBox cbxSelect = (HtmlInputCheckBox)mitem.FindControl("cbxSelect");
                        if (cbxSelect.Checked)
                        {
                            ProductMixInfo pm = new ProductMixInfo();
                            HtmlInputText txtAmount = (HtmlInputText)mitem.FindControl("txtAmount");
                            HtmlInputHidden hdnPMName = (HtmlInputHidden)mitem.FindControl("hdnPMName");
                            HtmlInputHidden hdnPMPrice = (HtmlInputHidden)mitem.FindControl("hdnPMPrice");
                            pm.Amount = DataConvert.SafeInt(txtAmount.Value);
                            pm.Name = hdnPMName.Value;
                            pm.Price = hdnPMPrice.Value;
                            ProductMixList.Add(pm);
                        }
                    }
                    if (ProductMixList.Count > 0)
                    {
                        oinfo.ProductMixList = ProductMixList;
                        listOrderProduct.Add(oinfo);
                    }
                }
            }

            string key = GlobalKey.ORDERPRODUCT_LIST + "_" + AdminID;
            MangaCache.Add(key, listOrderProduct);

            Response.Redirect("placeorder.aspx");
        }
    }
}