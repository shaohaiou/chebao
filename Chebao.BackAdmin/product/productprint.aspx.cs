using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using Chebao.Tools;

namespace Chebao.BackAdmin.product
{
    public partial class productprint : AdminBase
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            List<ProductInfo> productlist = Cars.Instance.GetProductList(true);
            if (GetInt("pt", -1) >= 0)
            {
                int producttype = GetInt("pt");
                productlist = productlist.FindAll(l => (int)l.ProductType == producttype);
            }
            if (!string.IsNullOrEmpty(GetString("name")))
            {
                string name = GetString("name");
                productlist = productlist.FindAll(l => l.Name.ToLower().IndexOf(name.ToLower()) >= 0);
            }
            if (!string.IsNullOrEmpty(GetString("mn")))
            {
                string modelnumber = GetString("mn");
                productlist = productlist.FindAll(l => l.ModelNumber.ToLower().IndexOf(modelnumber.ToLower()) >= 0 || l.OEModelNumber.ToLower().IndexOf(modelnumber.ToLower()) >= 0);
            }
            if (GetInt("id") > 0)
            {
                int id = GetInt("id");
                productlist = productlist.FindAll(p => p.Cabmodels.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Contains(id.ToString()));
            }

            rptProduct.DataSource = productlist;
            rptProduct.DataBind();
        }

        protected string GetProductTypeName(object type)
        {
            return EnumExtensions.GetDescription<ProductType>(type.ToString());
        }
    }
}