using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;

namespace Chebao.BackAdmin.product
{
    public partial class myordermore : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
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
            int pageindex = GetInt("pageindex");
            List<OrderInfo> list = Cars.Instance.GetOrderList(true);
            list = list.FindAll(l => l.UserID == AdminID);

            list = list.OrderByDescending(l => l.ID).Skip(10 * (pageindex - 1)).Take(10).ToList();

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