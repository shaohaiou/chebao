using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using Chebao.Tools;

namespace Chebao.BackAdmin.message
{
    public partial class messageboard : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
        }

        private List<CabmodelInfo> cabmodellist = null;
        public List<CabmodelInfo> CabmodelList
        {
            get
            {
                if (cabmodellist == null)
                {
                    cabmodellist = Cars.Instance.GetCabmodelList(true);
                }
                return cabmodellist;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindControler();
            }
        }

        private void BindControler()
        {
            ddlBrand.DataSource = Cars.Instance.GetBrandList(true);
            ddlBrand.DataTextField = "BrandNameBind";
            ddlBrand.DataValueField = "BrandName";
            ddlBrand.DataBind();
            ddlBrand.Items.Insert(0, new ListItem("请选择爱车品牌", ""));

            ddlCabmodel.Items.Insert(0, new ListItem("请选择型号", ""));
            ddlCabmodel.Enabled = false;
            ddlPailiang.Items.Insert(0, new ListItem("请选择排量", ""));
            ddlPailiang.Enabled = false;
            ddlNianfen.Items.Insert(0, new ListItem("请选择出厂年份", ""));
            ddlNianfen.Enabled = false;
            ddlProducts.Items.Insert(0, new ListItem("请选择产品型号", ""));
            ddlProducts.Enabled = false;

            ddlProductType.DataSource = EnumExtensions.ToTable<ProductType>();
            ddlProductType.DataTextField = "Text";
            ddlProductType.DataValueField = "Value";
            ddlProductType.DataBind();
            ddlProductType.Items.Insert(0, new ListItem("全部", ""));
        }

        #region 车型搜索

        protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPailiang.Items.Clear();
            ddlPailiang.Items.Insert(0, new ListItem("请选择排量", ""));
            ddlPailiang.Enabled = false;

            ddlNianfen.Items.Clear();
            ddlNianfen.Items.Insert(0, new ListItem("请选择出厂年份", ""));
            ddlNianfen.Enabled = false;

            ddlProducts.Items.Clear();
            ddlProducts.Items.Insert(0, new ListItem("请选择产品型号", ""));
            ddlProducts.Enabled = false;

            if (ddlBrand.SelectedIndex == 0)
            {
                ddlCabmodel.Items.Clear();
                ddlCabmodel.Items.Insert(0, new ListItem("请选择型号", ""));
                ddlCabmodel.Enabled = false;
            }
            else
            {
                if (!CabmodelList.Exists(l => l.BrandName == ddlBrand.SelectedValue))
                {
                    ddlCabmodel.Items.Clear();
                    ddlCabmodel.Items.Insert(0, new ListItem("请选择型号", ""));
                    ddlCabmodel.Enabled = false;
                }
                else
                {
                    ddlCabmodel.DataSource = CabmodelList.FindAll(l => l.BrandName == ddlBrand.SelectedValue).Select(l => l.CabmodelName).Distinct().Select(s => new KeyValuePair<string, string>(s, s)).ToList();
                    ddlCabmodel.DataTextField = "Key";
                    ddlCabmodel.DataValueField = "Value";
                    ddlCabmodel.DataBind();
                    ddlCabmodel.Items.Insert(0, new ListItem("请选择型号", ""));
                    ddlCabmodel.Enabled = true;
                }
            }
        }

        protected void ddlCabmodel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlNianfen.Items.Clear();
            ddlNianfen.Items.Insert(0, new ListItem("请选择出厂年份", ""));
            ddlNianfen.Enabled = false;

            ddlProducts.Items.Clear();
            ddlProducts.Items.Insert(0, new ListItem("请选择产品型号", ""));
            ddlProducts.Enabled = false;

            if (ddlCabmodel.SelectedIndex == 0)
            {
                ddlPailiang.Items.Clear();
                ddlPailiang.Items.Insert(0, new ListItem("请选择排量", ""));
                ddlPailiang.Enabled = false;
            }
            else
            {
                if (!CabmodelList.Exists(l => l.BrandName == ddlBrand.SelectedValue && l.CabmodelName == ddlCabmodel.SelectedValue))
                {
                    ddlPailiang.Items.Clear();
                    ddlPailiang.Items.Insert(0, new ListItem("请选择排量", ""));
                    ddlPailiang.Enabled = false;
                }
                else
                {
                    ddlPailiang.DataSource = CabmodelList.FindAll(l => l.BrandName == ddlBrand.SelectedValue && l.CabmodelName == ddlCabmodel.SelectedValue).Select(l => l.Pailiang).Distinct().Select(s => new KeyValuePair<string, string>(s, s)).ToList();
                    ddlPailiang.DataTextField = "Key";
                    ddlPailiang.DataValueField = "Value";
                    ddlPailiang.DataBind();
                    ddlPailiang.Items.Insert(0, new ListItem("请选择排量", ""));
                    ddlPailiang.Enabled = true;
                }
            }
        }

        protected void ddlPailiang_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlProducts.Items.Clear();
            ddlProducts.Items.Insert(0, new ListItem("请选择产品型号", ""));
            ddlProducts.Enabled = false;

            if (ddlPailiang.SelectedIndex == 0)
            {
                ddlNianfen.Items.Clear();
                ddlNianfen.Items.Insert(0, new ListItem("请选择出厂年份", ""));
                ddlNianfen.Enabled = false;
            }
            else
            {
                if (!CabmodelList.Exists(l => l.BrandName == ddlBrand.SelectedValue && l.CabmodelName == ddlCabmodel.SelectedValue && l.Pailiang == ddlPailiang.SelectedValue))
                {
                    ddlNianfen.Items.Clear();
                    ddlNianfen.Items.Insert(0, new ListItem("请选择出厂年份", ""));
                    ddlNianfen.Enabled = false;
                }
                else
                {
                    ddlNianfen.DataSource = CabmodelList.FindAll(l => l.BrandName == ddlBrand.SelectedValue && l.CabmodelName == ddlCabmodel.SelectedValue && l.Pailiang == ddlPailiang.SelectedValue).Select(l => l.Nianfen.ToString()).Distinct().OrderBy(l => l).Select(s => new KeyValuePair<string, string>(s, s)).ToList();
                    ddlNianfen.DataTextField = "Key";
                    ddlNianfen.DataValueField = "Value";
                    ddlNianfen.DataBind();
                    ddlNianfen.Items.Insert(0, new ListItem("请选择出厂年份", ""));
                    ddlNianfen.Enabled = true;
                }
            }
        }

        protected void ddlNianfen_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlNianfen.SelectedIndex == 0)
            {
                ddlProducts.Items.Clear();
                ddlProducts.Items.Insert(0, new ListItem("请选择产品型号", ""));
                ddlProducts.Enabled = false;
            }
            else
            {
                CabmodelInfo cab = CabmodelList.Find(l => l.BrandName == ddlBrand.SelectedValue
                && l.CabmodelName == ddlCabmodel.SelectedValue
                && l.Pailiang == ddlPailiang.SelectedValue
                && l.Nianfen.ToString() == ddlNianfen.SelectedValue);
                if (cab != null)
                {
                    List<ProductInfo> productlist = Cars.Instance.GetProductList(true);
                    productlist = productlist.FindAll(p => p.Cabmodels.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Contains(cab.ID.ToString()));

                    productlist = productlist.FindAll(p => p.ProductType == (ProductType)DataConvert.SafeInt(ddlProductType.SelectedValue));
                    productlist = productlist.OrderBy(p => (int)p.ProductType).ToList();
                    productlist = productlist.Count > 8 ? productlist.GetRange(0, 8) : productlist;
                    ddlProducts.DataSource = productlist;
                    ddlProducts.DataTextField = "ModelNumber";
                    ddlProducts.DataValueField = "ModelNumber";
                    ddlProducts.DataBind();
                    ddlProducts.Items.Insert(0, new ListItem("请选择产品型号", ""));
                    ddlProducts.Enabled = true;

                }
            }
        }

        #endregion

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMessageTitle.Text.Trim()))
            {
                ClientScript.RegisterStartupScript(typeof(string), "aa", "alert(\"请填写简述信息\");", true);
                return;
            }
            if (string.IsNullOrEmpty(hdnMessageContent.Value.Trim()))
            {
                ClientScript.RegisterStartupScript(typeof(string), "aa", "alert(\"请填写详细描述信息\");", true);
                return;
            }

            MessageBoardInfo entity = new MessageBoardInfo()
            {
                UserID = AdminID,
                UserName = AdminName,
                LinkName = Admin.LinkName,
                Title = txtMessageTitle.Text,
                Content = hdnMessageContent.Value,
                AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            Cars.Instance.AddMessageBoard(entity);
            txtMessageTitle.Text = string.Empty;
            hdnMessageContent.Value = string.Empty;
            ClientScript.RegisterStartupScript(typeof(string), "aa", "alert(\"提交成功，感谢您的反馈！\");", true);
        }
    }
}