using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;

namespace Chebao.BackAdmin.useradmin
{
    public partial class stockmg : AdminBase
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

            List<UserStockChangeInfo> list = Cars.Instance.GetUserStockChangeList(AdminID);

            rptData.DataSource = list;
            rptData.DataBind();

            search_fy.RecordCount = total;
            search_fy.PageSize = pagesize;
            search_fy.CustomInfoHTML = string.Format("<span style=\"line-height: 34px;\">总记录数：{0} 总页数：{1}</span>", total, search_fy.PageCount);
        }

        protected string GetOrderProductsStr(object orderproducts)
        {
            string result = string.Empty;
            List<OrderProductInfo> list = orderproducts as List<OrderProductInfo>;
            if (list != null)
            {
                list = list.OrderBy(p => p.ModelNumber).ToList();
                foreach (OrderProductInfo p in list)
                {
                    result += string.Format("<span class=\"bold\">{0}</span><br />", p.ModelNumber);
                    if (p.ProductMixList != null)
                    {
                        foreach (ProductMixInfo pi in p.ProductMixList)
                        {
                            result += string.Format("<span class=\"pl10\">{0}</span>", pi.Name + " × " + pi.Amount + "<br />");
                        }
                    }
                }
            }

            return result;
        }
    }
}