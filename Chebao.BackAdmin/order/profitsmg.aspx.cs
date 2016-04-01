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
    public partial class profitsmg : AdminBase
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

        protected decimal TotalFee { get; set; }

        protected decimal CostsTotal { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            int pageindex = GetInt("page", 1);
            if (pageindex < 1)
            {
                pageindex = 1;
            }
            int pagesize = search_fy.PageSize;
            int total = 0;

            List<OrderInfo> list = new List<OrderInfo>();
            if (!string.IsNullOrEmpty(GetString("timeb")) || !string.IsNullOrEmpty(GetString("timee")))
            {
                list = Cars.Instance.GetOrderList(true);
                list = list.FindAll(o => o.OrderStatus == OrderStatus.已发货).ToList();
                DateTime timeb = DateTime.Now;
                if (!string.IsNullOrEmpty(GetString("timeb")) && DateTime.TryParse(GetString("timeb"), out timeb))
                {
                    list = list.FindAll(l => DateTime.Parse(l.DeelTime) >= timeb);
                }
                DateTime timee = DateTime.Now;
                if (!string.IsNullOrEmpty(GetString("timee")) && DateTime.TryParse(GetString("timee"), out timee))
                {
                    list = list.FindAll(l => DateTime.Parse(l.DeelTime) < timee.AddDays(1));
                }
            }

            foreach (OrderInfo order in list)
            {
                TotalFee += DataConvert.SafeDecimal(order.TotalFee);
                foreach (OrderProductInfo p in order.OrderProducts)
                {
                    if (p.ProductMixList != null)
                    {
                        foreach (ProductMixInfo m in p.ProductMixList)
                        {
                            CostsTotal += DataConvert.SafeDecimal(m.CostsSum);
                        }
                    }
                }
            }

            list = list.OrderByDescending(l => l.ID).ToList();
            total = list.Count();
            list = list.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<OrderInfo>();

            rptData.DataSource = list;
            rptData.DataBind();

            search_fy.RecordCount = total;
            search_fy.PageSize = pagesize;
            search_fy.CustomInfoHTML = string.Format("<span style=\"line-height: 34px;\">总记录数：{0} 总页数：{1}</span>", total, search_fy.PageCount);

            if (!string.IsNullOrEmpty(GetString("timeb")))
            {
                txtDateB.Text = GetString("timeb");
            }
            if (!string.IsNullOrEmpty(GetString("timee")))
            {
                txtDateE.Text = GetString("timee");
            }
        }

        protected void rptData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                decimal totalcosts = 0;
                OrderInfo order = (OrderInfo)e.Item.DataItem;

                foreach (OrderProductInfo p in order.OrderProducts)
                {
                    if (p.ProductMixList != null)
                    {
                        foreach (ProductMixInfo m in p.ProductMixList)
                        {
                            totalcosts += DataConvert.SafeDecimal(m.CostsSum);
                        }
                    }
                }

                Label lblProfits = (Label)e.Item.FindControl("lblProfits");
                lblProfits.Text = StrHelper.FormatMoney((DataConvert.SafeDecimal(order.TotalFee) - totalcosts).ToString());
            }
        }
    }
}