using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using System.Web.UI.HtmlControls;
using Chebao.Tools;

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
                ShoppingTrolleyInfo st = CurrentShoppingTrolleyList.Find(s => s.ID == entity.SID);
                HtmlInputText txtAmount = (HtmlInputText)e.Item.FindControl("txtAmount");
                if (txtAmount != null && entity != null && entity.SID > 0)
                {
                    txtAmount.Attributes["data-id"] = entity.SID.ToString();
                    txtAmount.Value = st.Amount.ToString();
                    int stock = entity.Stock;
                    if (OrderAll.Exists(o => o.OrderStatus == OrderStatus.未处理 && o.OrderProducts != null && o.OrderProducts.Exists(p => p.ProductID == entity.ID)))
                    {
                        int amount = 0;
                        List<OrderInfo> orderlist = OrderAll.FindAll(o => o.OrderStatus == OrderStatus.未处理 && o.OrderProducts != null && o.OrderProducts.Exists(p => p.ProductID == entity.ID));
                        orderlist.ForEach(delegate(OrderInfo o)
                        {
                            amount += o.OrderProducts.FindAll(p => p.ProductID == entity.ID).Sum(p => p.Amount);
                        });
                        stock -= amount;
                        if (stock < 0)
                            stock = 0;
                    }
                    txtAmount.Attributes["data-max"] = stock.ToString();
                }
            }
        }

        private void Account()
        {
            List<OrderProductInfo> listOrderProduct = new List<OrderProductInfo>();
            foreach (RepeaterItem item in rptData.Items)
            {
                HtmlInputCheckBox cbxSelect = (HtmlInputCheckBox)item.FindControl("cbxSelect");
                if (cbxSelect != null && cbxSelect.Checked)
                {
                    HtmlInputText txtAmount = (HtmlInputText)item.FindControl("txtAmount");
                    HtmlInputHidden hdnModelNumber = (HtmlInputHidden)item.FindControl("hdnModelNumber");
                    HtmlInputHidden hdnOEModelNumber = (HtmlInputHidden)item.FindControl("hdnOEModelNumber");
                    HtmlInputHidden hdnStandard = (HtmlInputHidden)item.FindControl("hdnStandard");
                    HtmlInputHidden hdnPrice = (HtmlInputHidden)item.FindControl("hdnPrice");
                    HtmlInputHidden hdnName = (HtmlInputHidden)item.FindControl("hdnName");
                    HtmlInputHidden hdnPic = (HtmlInputHidden)item.FindControl("hdnPic");
                    HtmlInputHidden hdnSID = (HtmlInputHidden)item.FindControl("hdnSID");
                    HtmlInputHidden hdnID = (HtmlInputHidden)item.FindControl("hdnID");

                    OrderProductInfo oinfo = new OrderProductInfo() 
                    {
                        SID = DataConvert.SafeInt(hdnSID.Value),
                        ProductID = DataConvert.SafeInt(hdnID.Value),
                        ProductName = hdnName.Value,
                        ModelNumber = hdnModelNumber.Value,
                        OEModelNumber = hdnOEModelNumber.Value,
                        Standard = hdnStandard.Value,
                        ProductPic = hdnPic.Value,
                        Introduce = string.Format("Lamda型号：{0} 规格：{1}", hdnModelNumber.Value, hdnStandard.Value),
                        Price = DataConvert.SafeDecimal(hdnPrice.Value),
                        Amount = DataConvert.SafeInt(txtAmount.Value),
                        Remark = string.Empty
                    };

                    listOrderProduct.Add(oinfo);
                }
            }

            string key = GlobalKey.ORDERPRODUCT_LIST + "_" + AdminID;
            MangaCache.Add(key, listOrderProduct);

            Response.Redirect("placeorder.aspx");
        }
    }
}