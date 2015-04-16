using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using Chebao.Tools;

namespace Chebao.BackAdmin.order
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
            if (ChebaoContext.Current.AdminUser.UserRole != Components.UserRoleType.管理员)
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
                    currentorder = Cars.Instance.GetOrder(GetInt("id"),true);
                }
                return currentorder;
            }
        }

        protected OrderProductInfo CurrentProductInfo { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (GetInt("id") == 0 || CurrentOrder == null)
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
                rptData.DataSource = CurrentOrder.OrderProducts;
                rptData.DataBind();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(FromUrl);
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

        protected string GetOriginalPrice()
        {
            return Math.Round(CurrentProductInfo.Price,2).ToString();
        }
    }
}