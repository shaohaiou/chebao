using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using System.Web.UI.HtmlControls;

namespace Chebao.BackAdmin.useradmin
{
    public partial class productmix : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
        }

        private int productid = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            string mnumber = GetString("mnumber");
            if (!string.IsNullOrEmpty(mnumber))
            {
                List<ProductInfo> plist = Cars.Instance.GetProductList(true);
                if (plist.Exists(p => p.ModelNumber.ToLower() == mnumber.ToLower()))
                {
                    ProductInfo entity = plist.Find(p => p.ModelNumber.ToLower() == mnumber.ToLower());
                    productid = entity.ID;
                    rptProductMix.DataSource = entity.ProductMix.Select(p => new ProductMixInfo { Name = p.Key }).ToList();
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
                ProductMixInfo pminfo = (ProductMixInfo)e.Item.DataItem;
                if (txtAmount != null)
                {
                    txtAmount.Value = "0";
                    if (GetInt("t") == 0)
                    {
                        ProductMixInfo entity = ((ProductMixInfo)e.Item.DataItem);
                        int  stock = 0;
                        UserProductInfo upinfo = Cars.Instance.GetUserProductInfo(Admin.ID, productid,true);
                        if (upinfo != null && upinfo.ProductMix.Exists(p=>p.Key == entity.Name))
                        {
                            stock = upinfo.ProductMix.Find(p => p.Key == entity.Name).Value;
                        }
                        txtAmount.Attributes["data-max"] = stock.ToString();
                    }
                    else
                    {
                        txtAmount.Attributes["data-max"] = int.MaxValue.ToString();
                    }
                }
            }
        }
    }
}