using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using Chebao.Tools;

namespace Chebao.BackAdmin.useradmin
{
    public partial class stocks : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            if (!Admin.Administrator && (ChebaoContext.Current.AdminUser.UserRole != Components.UserRoleType.普通用户 || Admin.ParentAccountID > 0))
            {
                Response.Clear();
                Response.Write("您没有权限操作！");
                Response.End();
                return;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write("功能正在研发中，敬请期待！");
            Response.End();
            if (!IsPostBack)
            {
                BindControler();
                LoadData();
            }
        }

        private void BindControler()
        {
            ddlProductTypeFilter.DataSource = EnumExtensions.ToTable<ProductType>();
            ddlProductTypeFilter.DataTextField = "Text";
            ddlProductTypeFilter.DataValueField = "Value";
            ddlProductTypeFilter.DataBind();
            ddlProductTypeFilter.Items.Insert(0, new ListItem("-产品类型-", "-1"));
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

            List<ProductInfo> plist = Cars.Instance.GetProductListByUser(AdminID);
            if (GetInt("pt", -1) >= 0)
            {
                int producttype = GetInt("pt");
                plist = plist.FindAll(l => (int)l.ProductType == producttype);
                SetSelectedByValue(ddlProductTypeFilter, producttype.ToString());
            }
            if (!string.IsNullOrEmpty(GetString("name")))
            {
                string name = GetString("name");
                plist = plist.FindAll(l => l.Name.ToLower().IndexOf(name.ToLower()) >= 0);
                txtNameFilter.Value = name;
            }
            if (!string.IsNullOrEmpty(GetString("mn")))
            {
                string modelnumber = GetString("mn");
                plist = plist.FindAll(l => l.ModelNumber.ToLower().IndexOf(modelnumber.ToLower()) >= 0);
                txtModelNumberFilter.Value = modelnumber;
            }

            total = plist.Count();
            plist = plist.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<ProductInfo>();

            rptData.DataSource = plist;
            rptData.DataBind(); 
            
            search_fy.RecordCount = total;
            search_fy.PageSize = pagesize;
            search_fy.CustomInfoHTML = string.Format("<span style=\"line-height: 34px;\">总记录数：{0} 总页数：{1}</span>", total, search_fy.PageCount);
        }

        protected void rptData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                ProductInfo entity = e.Item.DataItem as ProductInfo;
                Repeater rptProductMix = (Repeater)e.Item.FindControl("rptProductMix");

                rptProductMix.DataSource = entity.UserProductMix.Select(p => new ProductMixInfo { Name = p.Key, Price = Cars.Instance.GetProductMixPrice(entity.Price, entity.XSPPrice, p.Key, ParentAdmin == null ? Admin : ParentAdmin), Stock = p.Value }).ToList();
                rptProductMix.DataBind();
            }
        }

        protected string GetProductTypeName(object type)
        {
            return EnumExtensions.GetDescription<ProductType>(type.ToString());
        }
    }
}