using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using Chebao.Tools;

namespace Chebao.BackAdmin.userstock
{
    public partial class userstockcheck : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            if (ChebaoContext.Current.AdminUser.UserRole != Components.UserRoleType.管理员 || !CheckModulePower("盘库审核"))
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
            UserStockChangeQuery query = new UserStockChangeQuery();
            if(!string.IsNullOrEmpty(GetString("n")))
                query.UserName = GetString("n");
            if(!string.IsNullOrEmpty(GetString("s")))
                query.CheckStatus = GetInt("s");

            List<UserStockChangeInfo> list = Cars.Instance.GetUserStockChangeList(pageindex, pagesize, query, out total);
            list = list.OrderBy(l => l.CheckStatus).ThenByDescending(l => DataConvert.SafeDate(l.AddTime)).ToList();

            rptData.DataSource = list;
            rptData.DataBind();

            search_fy.RecordCount = total;
            search_fy.PageSize = pagesize;
            search_fy.CustomInfoHTML = string.Format("<span style=\"line-height: 34px;\">总记录数：{0} 总页数：{1}</span>", total, search_fy.PageCount);

            if (!string.IsNullOrEmpty(GetString("n")))
                txtUserName.Text = GetString("n");
            if (!string.IsNullOrEmpty(GetString("s")))
                SetSelectedByValue(ddlCheckStatus, GetString("s"));
        }

        protected string GetOrderProductsStr(object orderproducts)
        {
            string result = string.Empty;
            List<OrderProductInfo> list = orderproducts as List<OrderProductInfo>;
            if (list != null)
            {
                foreach (OrderProductInfo p in list)
                {
                    result += p.ProductName + "<br />";
                    if (p.ProductMixList != null)
                    {
                        foreach (ProductMixInfo pi in p.ProductMixList)
                        {
                            result += string.Format("<span class=\"pl10\">{0}</span>", pi.Amount + " × " + pi.Name + "<br />");
                        }
                    }
                }
            }

            return result;
        }
    }
}