using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;

namespace Chebao.BackAdmin.product
{
    public partial class myorders : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
        }

        public int PageCount { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (GetString("action") == "cancel" && GetInt("id") > 0)
                {
                    int id = GetInt("id");
                    List<OrderInfo> orderlist = Cars.Instance.GetOrderList(true);
                    if (orderlist.Exists(o => o.ID == id && o.UserID == AdminID))
                    {
                        Cars.Instance.UpdateOrderStatus(id.ToString(), OrderStatus.已收款);
                        Cars.Instance.ReloadOrder();
                        Response.Redirect("~/product/myorders.aspx");
                        Response.End();
                    }
                }
                LoadData();
            }
        }

        private void LoadData()
        {
            PageCount = 1;
            List<OrderInfo> list = Cars.Instance.GetOrderList(true);
            list = list.FindAll(l => l.UserID == AdminID);
            PageCount = (list.Count / 10) + (list.Count % 10 > 0 ? 1 : 0);

            list = list.OrderByDescending(l => l.ID).Take(10).ToList();
            
            rptData.DataSource = list;
            rptData.DataBind();
        }

        protected void rptData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                OrderInfo entity = (OrderInfo)e.Item.DataItem;
                Repeater rptOrderProduct = (Repeater)e.Item.FindControl("rptOrderProduct");
                rptOrderProduct.DataSource = entity.OrderProducts;
                rptOrderProduct.DataBind();
            }
        }

        protected void rptOrderProduct_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                OrderProductInfo order = (OrderProductInfo)e.Item.DataItem;
                Repeater rptProductMix = (Repeater)e.Item.FindControl("rptProductMix");
                rptProductMix.DataSource = order.ProductMixList;
                rptProductMix.DataBind();
            }
        }
    }
}