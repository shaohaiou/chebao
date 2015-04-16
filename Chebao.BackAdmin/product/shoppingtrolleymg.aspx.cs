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
                if (txtAmount != null)
                {
                    txtAmount.Attributes["data-id"] = ((ProductMixInfo)e.Item.DataItem).SID.ToString() + "_" + e.Item.ItemIndex;
                    txtAmount.Value = "1";
                    int stock = ((ProductMixInfo)e.Item.DataItem).Stock;
                    //if (OrderAll.Exists(o => o.OrderStatus == OrderStatus.未处理 && o.OrderProducts != null && o.OrderProducts.Exists(p => p.ProductID == entity.ID)))
                    //{
                    //    int amount = 0;
                    //    List<OrderInfo> orderlist = OrderAll.FindAll(o => o.OrderStatus == OrderStatus.未处理 && o.OrderProducts != null && o.OrderProducts.Exists(p => p.ProductID == entity.ID));
                    //    orderlist.ForEach(delegate(OrderInfo o)
                    //    {
                    //        amount += o.OrderProducts.FindAll(p => p.ProductID == entity.ID).Sum(p => p.Amount);
                    //    });
                    //    stock -= amount;
                    //    if (stock < 0)
                    //        stock = 0;
                    //}
                    txtAmount.Attributes["data-max"] = stock.ToString();
                }
            }
        }

        private string GetProductMixPrice(string pricestr, string mn)
        {
            decimal price = DataConvert.SafeDecimal(pricestr.StartsWith("¥") ? pricestr.Substring(1) : pricestr);
            if (mn.ToLower().IndexOf("m") >= 0)
                price = price * Admin.DiscountM / 10;
            else if (mn.ToLower().IndexOf("y") >= 0)
                price = price * Admin.DiscountY / 10;
            else if (mn.ToLower().IndexOf("h") >= 0)
                price = price * Admin.DiscountH / 10;
            if (mn.ToLower().IndexOf("w") >= 0)
                price = price + Admin.AdditemW;
            if (mn.ToLower().IndexOf("f") >= 0)
                price = price + Admin.AdditemF;

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