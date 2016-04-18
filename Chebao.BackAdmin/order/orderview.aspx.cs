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
            if (ChebaoContext.Current.AdminUser.UserRole != Components.UserRoleType.管理员 || !CheckModulePower("订单列表"))
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

        protected ProductMixInfo CurrentProductMixInfo { get; set; }

        protected decimal CostsTotal { get; set; }

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
                TextBox txtAmount = (TextBox)e.Item.FindControl("txtAmount");
                txtAmount.Text = p.Amount.ToString();
                txtAmount.Attributes["data-price"] = p.Price;
                txtAmount.Attributes["data-costs"] = p.Costs;
                txtAmount.Attributes["data-source"] = p.Amount.ToString();
                if (CheckModulePower("金额可见"))
                {
                    if (string.IsNullOrEmpty(p.UnitPrice))
                        spOriginalPrice.Text = Math.Round(CurrentProductInfo.Price, 2).ToString();
                    else
                        spOriginalPrice.Text = p.UnitPrice;
                }
                CostsTotal += DataConvert.SafeDecimal(p.CostsSum);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(FromUrl);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<OrderProductInfo> listOrderProduct = new List<OrderProductInfo>();
            foreach (RepeaterItem pitem in rptData.Items)
            {
                HtmlInputHidden hdnProductID = (HtmlInputHidden)pitem.FindControl("hdnProductID");
                Repeater rptProductMix = (Repeater)pitem.FindControl("rptProductMix");
                int productid = DataConvert.SafeInt(hdnProductID.Value);
                OrderProductInfo pinfo = CurrentOrder.OrderProducts.Find(p => p.ProductID == productid);
                foreach (RepeaterItem mitem in rptProductMix.Items)
                {
                    HtmlInputHidden hdnProductMixName = (HtmlInputHidden)mitem.FindControl("hdnProductMixName");
                    TextBox txtAmount = (TextBox)mitem.FindControl("txtAmount");
                    if (pinfo.ProductMixList != null && pinfo.ProductMixList.Exists(m => m.Name == hdnProductMixName.Value))
                    {
                        pinfo.ProductMixList[pinfo.ProductMixList.FindIndex(m => m.Name == hdnProductMixName.Value)].Amount = DataConvert.SafeInt(txtAmount.Text);
                    }
                }

                listOrderProduct.Add(pinfo);
            }

            System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();
            CurrentOrder.TotalFee = Math.Round(listOrderProduct.Sum(p => DataConvert.SafeDecimal(p.Sum)), 2).ToString();
            CurrentOrder.OrderProductJson = json.Serialize(listOrderProduct);
            if (CurrentOrder.OrderProductJson.Length > 65500)
                CurrentOrder.OrderProductJson1 = CurrentOrder.OrderProductJson.Substring(0, 65500);
            else
                CurrentOrder.OrderProductJson1 = CurrentOrder.OrderProductJson;

            if (CurrentOrder.OrderProductJson.Length > 65500 * 2)
                CurrentOrder.OrderProductJson2 = CurrentOrder.OrderProductJson.Substring(65500, 65500);
            else
                CurrentOrder.OrderProductJson2 = CurrentOrder.OrderProductJson.Length > 65500 ? CurrentOrder.OrderProductJson.Substring(65500) : string.Empty;

            if (CurrentOrder.OrderProductJson.Length > 65500 * 3)
                CurrentOrder.OrderProductJson3 = CurrentOrder.OrderProductJson.Substring(65500 * 2, 65500);
            else
                CurrentOrder.OrderProductJson3 = CurrentOrder.OrderProductJson.Length > 65500 * 2 ? CurrentOrder.OrderProductJson.Substring(65500 * 2) : string.Empty;

            if (CurrentOrder.OrderProductJson.Length > 65500 * 4)
                CurrentOrder.OrderProductJson4 = CurrentOrder.OrderProductJson.Substring(65500 * 3, 65500);
            else
                CurrentOrder.OrderProductJson4 = CurrentOrder.OrderProductJson.Length > 65500 * 3 ? CurrentOrder.OrderProductJson.Substring(65500 * 3) : string.Empty;

            if (CurrentOrder.OrderProductJson.Length > 65500 * 5)
                CurrentOrder.OrderProductJson5 = CurrentOrder.OrderProductJson.Substring(65500 * 4, 65500);
            else
                CurrentOrder.OrderProductJson5 = CurrentOrder.OrderProductJson.Length > 65500 * 4 ? CurrentOrder.OrderProductJson.Substring(65500 * 4) : string.Empty;

            if (CurrentOrder.OrderProductJson.Length > 65500 * 6)
                CurrentOrder.OrderProductJson6 = CurrentOrder.OrderProductJson.Substring(65500 * 5, 65500);
            else
                CurrentOrder.OrderProductJson6 = CurrentOrder.OrderProductJson.Length > 65500 * 5 ? CurrentOrder.OrderProductJson.Substring(65500 * 5) : string.Empty;

            if (CurrentOrder.OrderProductJson.Length > 65500 * 7)
                CurrentOrder.OrderProductJson7 = CurrentOrder.OrderProductJson.Substring(65500 * 6, 65500);
            else
                CurrentOrder.OrderProductJson7 = CurrentOrder.OrderProductJson.Length > 65500 * 6 ? CurrentOrder.OrderProductJson.Substring(65500 * 6) : string.Empty;

            string result = Cars.Instance.UpdateOrderProducts(CurrentOrder);
            if (!string.IsNullOrEmpty(result))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("<span class=\"dalv\">{0}</span><br />",result);
                WriteErrorMessage("保存失败！", sb.ToString(), "", CurrentUrl);
                return;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<span class=\"dalv\">信息保存成功！</span><br />");
                WriteMessage("~/message/showmessage.aspx", "保存成功！", sb.ToString(), "", CurrentUrl);
            }
        }
    }
}