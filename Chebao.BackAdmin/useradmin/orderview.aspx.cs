using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Tools;
using System.Web.UI.HtmlControls;
using System.Text;
using Chebao.Components;

namespace Chebao.BackAdmin.useradmin
{
    public partial class orderview : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            if (Admin.ParentAccountID > 0)
            {
                Response.Clear();
                Response.Write("您没有权限操作！");
                Response.End();
                return;
            }
        }

        private OrderInfo currentorder = null;
        protected OrderInfo CurrentOrder
        {
            get
            {
                if (currentorder == null && GetInt("id") > 0)
                {
                    currentorder = Cars.Instance.GetOrder(GetInt("id"), true);
                }
                return currentorder;
            }
        }

        protected OrderProductInfo CurrentProductInfo { get; set; }

        protected ProductMixInfo CurrentProductMixInfo { get; set; }

        protected decimal CostsTotal { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (GetInt("id") == 0 || CurrentOrder == null || CurrentOrder.ParentID != AdminID)
                {
                    WriteMessage("~/message/showmessage.aspx", "错误！", "非法订单！", "", FromUrl);
                    Response.End();
                    return;
                }
                LoadData();
            }
        }

        private void LoadData()
        {
            if (CurrentOrder != null)
            {
                rptData.DataSource = CurrentOrder.OrderProducts.OrderBy(p => p.ModelNumber).ToList();
                rptData.DataBind();
            }
        }

        protected void rptData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                OrderProductInfo p = (OrderProductInfo)e.Item.DataItem;
                CurrentProductInfo = p;

                Repeater rptProductMix = (Repeater)e.Item.FindControl("rptProductMix");
                rptProductMix.DataSource = p.ProductMixList;
                rptProductMix.DataBind();
            }
        }

        protected void rptProductMix_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                ProductMixInfo p = (ProductMixInfo)e.Item.DataItem;
                CurrentProductMixInfo = p;

                Label spOriginalPrice = (Label)e.Item.FindControl("spOriginalPrice");
                if (string.IsNullOrEmpty(p.UnitPrice))
                    spOriginalPrice.Text = Math.Round(CurrentProductInfo.Price, 2).ToString();
                else
                    spOriginalPrice.Text = p.UnitPrice;
                CostsTotal += DataConvert.SafeDecimal(p.CostsSum);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(FromUrl);
        }
    }
}