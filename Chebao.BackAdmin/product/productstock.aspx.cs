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
            string mnumber = GetString("mnumber");
            if (pid > 0)
            {
                if (ParentAdmin == null || ParentAdmin.IsDistribution == 0)
                {
                    ProductInfo entity = Cars.Instance.GetProduct(pid, true);
                    rptProductMix.DataSource = entity.ProductMix.Select(p => new ProductMixInfo { Name = p.Key, Stock = p.Value, Price = Cars.Instance.GetProductMixPrice(entity.Price, entity.XSPPrice, p.Key, Admin), SID = 0 }).ToList();
                }
                else
                {
                    ProductInfo entity = Cars.Instance.GetProductByUser(Admin.ParentAccountID, pid);
                    rptProductMix.DataSource = entity.UserProductMix.Select(p => new ProductMixInfo { Name = p.Key, Stock = p.Value, Price = Cars.Instance.GetProductMixPrice(entity.Price, entity.XSPPrice, p.Key, ParentAdmin), SID = 0 }).ToList();
                }
                rptProductMix.DataBind();
            }
            else if (!string.IsNullOrEmpty(mnumber))
            {
                List<ProductInfo> plist = Cars.Instance.GetProductList(true);
                if (plist.Exists(p => p.ModelNumber.ToLower() == mnumber.ToLower()))
                {
                    ProductInfo entity = plist.Find(p => p.ModelNumber.ToLower() == mnumber.ToLower());
                    rptProductMix.DataSource = entity.ProductMix.Select(p => new ProductMixInfo { Name = p.Key, Stock = p.Value, SID = entity.SID, UnitPrice = GetUnitPrice(p.Key, entity.Price, entity.XSPPrice), Price = Cars.Instance.GetProductMixPrice(entity.Price, entity.XSPPrice, p.Key, Admin), Costs = GetProductMixCosts(entity.Price, entity.XSPPrice, p.Key) }).ToList();
                    rptProductMix.DataBind();
                }
                else
                {
                    Response.Clear();
                    Response.End();
                }
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
    }
}