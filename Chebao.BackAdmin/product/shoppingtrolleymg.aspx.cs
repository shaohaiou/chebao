using System;
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

        private List<string[]> listSelectedsid = null;
        public List<string[]> SelectedSID
        {
            get
            {
                if (listSelectedsid == null)
                {
                    listSelectedsid = new List<string[]>();
                    string strcnumber = ManageCookies.GetCookieValue(GlobalKey.SELECTEDSIDNUMBER_COOKIENAME);
                    if (!string.IsNullOrEmpty(strcnumber) && !string.IsNullOrEmpty(ManageCookies.GetCookieValue(GlobalKey.SELECTEDSID_COOKIENAME + "_1")))
                    {
                        int cnumber = DataConvert.SafeInt(strcnumber);
                        string allsids = string.Empty;
                        for (int i = 1; i <= cnumber; i++)
                        {
                            allsids += ManageCookies.GetCookieValue(GlobalKey.SELECTEDSID_COOKIENAME + "_" + i);
                        }
                        foreach (string s in allsids.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            if (s.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries).Length == 3)
                            {
                                listSelectedsid.Add(s.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries));
                            }
                        }
                    }
                }
                return listSelectedsid;
            }
        }

        public int ItemCount { get; set; }

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
            List<ProductInfo> listAllProduct;
            if (ParentAdmin == null || ParentAdmin.IsDistribution == 0)
                listAllProduct = Cars.Instance.GetProductList(true);
            else
                listAllProduct = Cars.Instance.GetProductListByUser(Admin.ParentAccountID);
            listShoppingTrolley = listShoppingTrolley.FindAll(l => listAllProduct.Exists(p => p.ID == l.ProductID));
            if (listShoppingTrolley.Count > 0)
            {
                string[] pids = listShoppingTrolley.OrderByDescending(s => s.ID).Select(s => s.ProductID + "|" + s.ID).ToArray();
                List<KeyValuePair<int,string>> productmixlist = listShoppingTrolley.OrderByDescending(s => s.ID).Select(s => new KeyValuePair<int,string>(s.ProductID,s.ProductMix)).ToList();
                List<ProductInfo> listProdcutInShoppingTrolley = new List<ProductInfo>();
                for (int i = 0; i < pids.Length; i++)
                {
                    int pid = DataConvert.SafeInt(pids[i].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[0]);
                    int sid = DataConvert.SafeInt(pids[i].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[1]);
                    string productmixsetting = productmixlist.Find(l=>l.Key == pid).Value;
                    ProductInfo entity = listAllProduct.Find(p => p.ID == pid).Clone();
                    entity.SID = sid;
                    entity.CabmodelStr = listShoppingTrolley.FindAll(s => s.ID == sid).Select(s => string.IsNullOrEmpty(s.CabmodelStr) ? string.Empty : (" - " + s.CabmodelStr)).First();
                    entity.ProductMixSetting = productmixsetting;
                    listProdcutInShoppingTrolley.Add(entity);
                }
                ItemCount = listProdcutInShoppingTrolley.Count;
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
                List<KeyValuePair<string, int>> productmixsettinglist = new List<KeyValuePair<string, int>>();
                if (!string.IsNullOrEmpty(entity.ProductMixSetting))
                {
                    string[] productmixsettings = entity.ProductMixSetting.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    productmixsettinglist = productmixsettings.Select(p => new KeyValuePair<string, int>(p.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0], DataConvert.SafeInt(p.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[1]))).ToList();
                }

                if (ParentAdmin == null || ParentAdmin.IsDistribution == 0)
                    rptProductMix.DataSource = entity.ProductMix.Select(p => new ProductMixInfo { Name = p.Key, Amount = (productmixsettinglist.Count > 0 ? productmixsettinglist.Find(l=>l.Key == p.Key).Value : 1), Stock = p.Value, SID = entity.SID, UnitPrice = GetUnitPrice(p.Key, entity.Price, entity.XSPPrice), Price = Cars.Instance.GetProductMixPrice(entity.Price, entity.XSPPrice, p.Key, Admin), Costs = GetProductMixCosts(entity.Price, entity.XSPPrice, p.Key) }).ToList();
                else
                    rptProductMix.DataSource = entity.UserProductMix.Select(p => new ProductMixInfo { Name = p.Key, Amount = (productmixsettinglist.Count > 0 ? productmixsettinglist.Find(l => l.Key == p.Key).Value : 1), Stock = p.Value, SID = entity.SID, UnitPrice = Cars.Instance.GetProductMixPrice(entity.Price, entity.XSPPrice, p.Key, ParentAdmin), Price = Cars.Instance.GetProductMixPrice(entity.Price, entity.XSPPrice, p.Key, ParentAdmin), Costs = GetUserProductMixCosts(entity.Price, entity.XSPPrice, p.Key) }).ToList();
                rptProductMix.DataBind();
            }
        }

        protected void rptProductMix_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlInputText txtAmount = (HtmlInputText)e.Item.FindControl("txtAmount");
                HtmlInputCheckBox cbxSelect = (HtmlInputCheckBox)e.Item.FindControl("cbxSelect");
                HtmlAnchor spStock = (HtmlAnchor)e.Item.FindControl("spStock");
                ProductMixInfo pminfo = (ProductMixInfo)e.Item.DataItem;
                if (txtAmount != null)
                {
                    txtAmount.Attributes["data-id"] = ((ProductMixInfo)e.Item.DataItem).SID.ToString() + "_" + e.Item.ItemIndex;
                    txtAmount.Value = pminfo.Amount.ToString();
                    ProductMixInfo entity = ((ProductMixInfo)e.Item.DataItem);
                    int stock = entity.Stock;
                    //if (OrderAll.Exists(o => o.SyncStatus == 0 && o.OrderStatus != OrderStatus.已取消 && o.OrderProducts != null && o.OrderProducts.Exists(p => p.ProductMixList != null && p.ProductMixList.Exists(pm => pm.Name == entity.Name))))
                    //{
                    //    int amount = 0;
                    //    List<OrderInfo> orderlist = OrderAll.FindAll(o => o.SyncStatus == 0 && o.OrderStatus != OrderStatus.已取消 && o.OrderProducts != null && o.OrderProducts.Exists(p => p.ProductMixList != null && p.ProductMixList.Exists(pm => pm.Name == entity.Name)));
                    //    orderlist.ForEach(delegate(OrderInfo o)
                    //    {
                    //        amount += o.OrderProducts.FindAll(p => p.ProductMixList != null && p.ProductMixList.Exists(pm => pm.Name == entity.Name)).Sum(p => p.ProductMixList.FindAll(pm => pm.Name == entity.Name).Sum(pm => pm.Amount));
                    //    });
                    //    stock -= amount;
                    //} 

                    if (ParentAdmin == null || ParentAdmin.IsDistribution == 0) //代理商或未开启分销功能的子用户库存计算
                    {
                        if (OrderAll.Exists(o => o.ParentID == 0 && o.SyncStatus == 0 && o.OrderStatus != OrderStatus.已取消 && o.OrderProducts != null && o.OrderProducts.Exists(p => p.ProductMixList != null && p.ProductMixList.Exists(pm => pm.Name == entity.Name))))
                        {
                            int amount = 0;
                            List<OrderInfo> orderlist = OrderAll.FindAll(o => o.ParentID == 0 && o.SyncStatus == 0 && o.OrderStatus != OrderStatus.已取消 && o.OrderProducts != null && o.OrderProducts.Exists(p => p.ProductMixList != null && p.ProductMixList.Exists(pm => pm.Name == entity.Name)));
                            orderlist.ForEach(delegate(OrderInfo o)
                            {
                                amount += o.OrderProducts.FindAll(p => p.ProductMixList != null && p.ProductMixList.Exists(pm => pm.Name == entity.Name)).Sum(p => p.ProductMixList.FindAll(pm => pm.Name == entity.Name).Sum(pm => pm.Amount));
                            });
                            stock -= amount;
                        }
                    }
                    else //零售商库存计算
                    {
                        DateTime datevertionorder = DateTime.Parse(GlobalKey.VERSIONUPDATETIME_ORDER);
                        if (OrderAll.Exists(o => o.ParentID == Admin.ParentAccountID && o.OrderStatus != OrderStatus.已取消 && o.OrderStatus != OrderStatus.已发货 && DateTime.Parse(o.AddTime) > datevertionorder && o.OrderProducts != null && o.OrderProducts.Exists(p => p.ProductMixList != null && p.ProductMixList.Exists(pm => pm.Name == entity.Name))))
                        {
                            int amount = 0;
                            List<OrderInfo> orderlist = OrderAll.FindAll(o => o.ParentID == Admin.ParentAccountID && o.OrderStatus != OrderStatus.已取消 && o.OrderStatus != OrderStatus.已发货 && DateTime.Parse(o.AddTime) > datevertionorder && o.OrderProducts != null && o.OrderProducts.Exists(p => p.ProductMixList != null && p.ProductMixList.Exists(pm => pm.Name == entity.Name)));
                            orderlist.ForEach(delegate(OrderInfo o)
                            {
                                amount += o.OrderProducts.FindAll(p => p.ProductMixList != null && p.ProductMixList.Exists(pm => pm.Name == entity.Name)).Sum(p => p.ProductMixList.FindAll(pm => pm.Name == entity.Name).Sum(pm => pm.Amount));
                            });
                            stock -= amount;
                        }
                    }
                    if (stock < 0)
                        stock = 0;
                    entity.Stock = stock;
                    txtAmount.Attributes["data-max"] = stock.ToString();
                    spStock.InnerHtml = stock.ToString();
                }
                if (SelectedSID.Exists(s => s[0] == pminfo.SID.ToString() && s[1] == pminfo.Name))
                {
                    cbxSelect.Checked = true;
                    if (txtAmount != null)
                    {
                        txtAmount.Value = SelectedSID.Find(s => s[0] == pminfo.SID.ToString() && s[1] == pminfo.Name)[2];
                    }
                }
            }
        }

        protected string SetPMSelected(string sid, string name)
        {
            string result = string.Empty;

            if (SelectedSID.Exists(s => s[0] == sid && s[1] == name))
                result = " cart-checkbox-checked";
            else
                result = "";

            return result;
        }

        private string GetUnitPrice(string name, string price, string xspprice)
        {
            if (name.IndexOf("xsp") > 0)
                return xspprice.StartsWith("¥") ? xspprice.Substring(1) : xspprice;
            else
                return price.StartsWith("¥") ? price.Substring(1) : price;
        }

        private string GetProductMixCosts(string pricestr, string xsppricestr, string mn)
        {
            decimal price = 0;
            DiscountStencilInfo discountinfo = Cars.Instance.GetCostsDiscount(true);
            if (discountinfo != null)
            {
                price = Cars.Instance.GetDiscountPrice(pricestr, xsppricestr, mn, discountinfo);
            }
            return Math.Round(price, 2).ToString();
        }

        private string GetUserProductMixCosts(string pricestr, string xsppricestr, string mn)
        {
            decimal price = 0;
            DiscountStencilInfo discountinfo = null;
            if (ParentAdmin.DiscountStencilID > 0)
                discountinfo = Cars.Instance.GetDiscountStencil(ParentAdmin.DiscountStencilID, true);
            if (discountinfo == null)
            {
                discountinfo = new DiscountStencilInfo(ParentAdmin);
            }
            price = Cars.Instance.GetDiscountPrice(pricestr, xsppricestr, mn, discountinfo);
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
                    AdminInfo validAdmin = ParentAdmin == null ? Admin : ParentAdmin;
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
                        DiscountM = validAdmin.DiscountM,
                        DiscountY = validAdmin.DiscountY,
                        DiscountH = validAdmin.DiscountH,
                        AdditemW = validAdmin.AdditemW,
                        AdditemF = validAdmin.AdditemF,
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
                            HtmlInputHidden hdnPMCosts = (HtmlInputHidden)mitem.FindControl("hdnPMCosts");
                            HtmlInputHidden hdnPMUnitPrice = (HtmlInputHidden)mitem.FindControl("hdnPMUnitPrice");
                            pm.Amount = DataConvert.SafeInt(txtAmount.Value);
                            pm.Name = hdnPMName.Value;
                            pm.Price = hdnPMPrice.Value;
                            pm.Costs = hdnPMCosts.Value;
                            pm.UnitPrice = hdnPMUnitPrice.Value;
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