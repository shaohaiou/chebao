using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using System.Web.UI.HtmlControls;

namespace Chebao.BackAdmin.product
{
    public partial class productstock : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
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
                LoadData();
            }
        }

        private void LoadData()
        {
            int pid = GetInt("pid");
            if (pid > 0)
            {
                ProductInfo entity = Cars.Instance.GetProduct(pid,true);
                //if (Admin.ParentAccountID == 0)
                rptProductMix.DataSource = entity.ProductMix.Select(p => new ProductMixInfo { Name = p.Key, Stock = p.Value, SID = entity.SID, UnitPrice = GetUnitPrice(p.Key, entity.Price, entity.XSPPrice), Price = Cars.Instance.GetProductMixPrice(entity.Price, entity.XSPPrice, p.Key, Admin), Costs = GetProductMixCosts(entity.Price, entity.XSPPrice, p.Key) }).ToList();
                //else
                //    rptProductMix.DataSource = entity.UserProductMix.Select(p => new ProductMixInfo { Name = p.Key, Stock = p.Value, SID = entity.SID, UnitPrice = Cars.Instance.GetProductMixPrice(entity.Price, entity.XSPPrice, p.Key, ParentAdmin), Price = Cars.Instance.GetProductMixPrice(entity.Price, entity.XSPPrice, p.Key, ParentAdmin), Costs = GetUserProductMixCosts(entity.Price, entity.XSPPrice, p.Key) }).ToList();
                rptProductMix.DataBind();
            }
            else
            {
                Response.Clear();
                Response.End();
            }
        }

        protected void rptProductMix_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlInputText txtAmount = (HtmlInputText)e.Item.FindControl("txtAmount");
                HtmlAnchor spStock = (HtmlAnchor)e.Item.FindControl("spStock");
                ProductMixInfo pminfo = (ProductMixInfo)e.Item.DataItem;
                if (txtAmount != null)
                {
                    txtAmount.Attributes["data-id"] = ((ProductMixInfo)e.Item.DataItem).SID.ToString() + "_" + e.Item.ItemIndex;
                    txtAmount.Value = "1";
                    ProductMixInfo entity = ((ProductMixInfo)e.Item.DataItem);
                    int stock = entity.Stock;
                    if (OrderAll.Exists(o => o.SyncStatus == 0 && o.OrderStatus != OrderStatus.已取消 && o.OrderProducts != null && o.OrderProducts.Exists(p => p.ProductMixList != null && p.ProductMixList.Exists(pm => pm.Name == entity.Name))))
                    {
                        int amount = 0;
                        List<OrderInfo> orderlist = OrderAll.FindAll(o => o.SyncStatus == 0 && o.OrderStatus != OrderStatus.已取消 && o.OrderProducts != null && o.OrderProducts.Exists(p => p.ProductMixList != null && p.ProductMixList.Exists(pm => pm.Name == entity.Name)));
                        orderlist.ForEach(delegate(OrderInfo o)
                        {
                            amount += o.OrderProducts.FindAll(p => p.ProductMixList != null && p.ProductMixList.Exists(pm => pm.Name == entity.Name)).Sum(p => p.ProductMixList.FindAll(pm => pm.Name == entity.Name).Sum(pm => pm.Amount));
                        });
                        stock -= amount;
                    }
                    //if (Admin.ParentAccountID == 0)
                    //{
                    //    if (OrderAll.Exists(o => o.ParentID == 0 && o.SyncStatus == 0 && o.OrderStatus != OrderStatus.已取消 && o.OrderProducts != null && o.OrderProducts.Exists(p => p.ProductMixList != null && p.ProductMixList.Exists(pm => pm.Name == entity.Name))))
                    //    {
                    //        int amount = 0;
                    //        List<OrderInfo> orderlist = OrderAll.FindAll(o => o.ParentID == 0 && o.SyncStatus == 0 && o.OrderStatus != OrderStatus.已取消 && o.OrderProducts != null && o.OrderProducts.Exists(p => p.ProductMixList != null && p.ProductMixList.Exists(pm => pm.Name == entity.Name)));
                    //        orderlist.ForEach(delegate(OrderInfo o)
                    //        {
                    //            amount += o.OrderProducts.FindAll(p => p.ProductMixList != null && p.ProductMixList.Exists(pm => pm.Name == entity.Name)).Sum(p => p.ProductMixList.FindAll(pm => pm.Name == entity.Name).Sum(pm => pm.Amount));
                    //        });
                    //        stock -= amount;
                    //    }
                    //}
                    if (stock < 0)
                        stock = 0;
                    entity.Stock = stock;
                    txtAmount.Attributes["data-max"] = stock.ToString();
                    spStock.InnerHtml = stock.ToString();
                }
            }
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
    }
}