using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using Chebao.Tools;
using System.Text;

namespace Chebao.BackAdmin.product
{
    public partial class productedit : AdminBase
    {
        private ProductInfo product = null;
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
                BindControler();
                LoadData();
            }
        }

        private void BindControler()
        {
            ddlBrand.DataSource = Cars.Instance.GetBrandList(true);
            ddlBrand.DataTextField = "BrandNameBind";
            ddlBrand.DataValueField = "ID";
            ddlBrand.DataBind();
            ddlBrand.Items.Insert(0, new ListItem("-品牌-", "-1"));

            ddlCabmodel.Items.Insert(0, new ListItem("-车型-", "-1"));
            ddlCabmodel.Enabled = false;
            ddlPailiang.Items.Insert(0, new ListItem("-排量-", "-1"));
            ddlPailiang.Enabled = false;
            ddlNianfen.Items.Insert(0, new ListItem("-年份-", "-1"));
            ddlNianfen.Enabled = false;

            ddlProductType.DataSource = EnumExtensions.ToTable<ProductType>();
            ddlProductType.DataTextField = "Text";
            ddlProductType.DataValueField = "Value";
            ddlProductType.DataBind();
        }

        private void LoadData()
        {
            int id = GetInt("id");
            if (id > 0)
            {
                product = Cars.Instance.GetProduct(id, true);
                txtProductName.Text = product.Name;
                txtPrice.Text = product.Price;
                txtXSPPrice.Text = product.XSPPrice;
                txtReplacement.Text = product.Replacement;
                txtModelNumber.Text = product.ModelNumber;
                txtOEModelNumber.Text = product.OEModelNumber;
                txtArea.Text = product.Area;
                txtMaterial.Text = product.Material;
                txtStandard.Text = product.Standard;
                txtIntroduce.Text = product.Introduce;
                SetSelectedByValue(ddlProductType, ((int)product.ProductType).ToString());

                string[] cabmodels = product.Cabmodels.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                lbxCabmodelsSel.DataSource = CabmodelList.FindAll(l => cabmodels.Contains(l.ID.ToString()));
                lbxCabmodelsSel.DataTextField = "CabmodelNameBind";
                lbxCabmodelsSel.DataValueField = "ID";
                lbxCabmodelsSel.DataBind();

                hdimage_pic.Value = product.Pic;
                hdimage_pics.Value = product.Pics;
                if (!string.IsNullOrEmpty(product.Pic))
                    imgpic.Src = product.Pic;
                if (!string.IsNullOrEmpty(product.Pics))
                {
                    string[] pics = product.Pics.Split(new char[] { '|' }, StringSplitOptions.None);
                    if (pics.Length == 5)
                    {
                        if (!string.IsNullOrEmpty(pics[0]))
                        {
                            imgpics1.Src = pics[0];
                            imgpics1.Attributes["val"] = pics[0];
                        }
                        if (!string.IsNullOrEmpty(pics[1]))
                        {
                            imgpics2.Src = pics[1];
                            imgpics2.Attributes["val"] = pics[1];
                        }
                        if (!string.IsNullOrEmpty(pics[2]))
                        {
                            imgpics3.Src = pics[2];
                            imgpics3.Attributes["val"] = pics[2];
                        }
                        if (!string.IsNullOrEmpty(pics[3]))
                        {
                            imgpics4.Src = pics[3];
                            imgpics4.Attributes["val"] = pics[3];
                        }
                        if (!string.IsNullOrEmpty(pics[4]))
                        {
                            imgpics5.Src = pics[4];
                            imgpics5.Attributes["val"] = pics[4];
                        }
                    }
                }
            }
        }

        private void FillData(ProductInfo entity)
        {
            entity.Name = txtProductName.Text;
            entity.Price = string.IsNullOrEmpty(txtPrice.Text) ? "0" : txtPrice.Text;
            entity.XSPPrice = string.IsNullOrEmpty(txtXSPPrice.Text) ? "0" : txtXSPPrice.Text;
            entity.Replacement = txtReplacement.Text;
            entity.ModelNumber = txtModelNumber.Text;
            entity.OEModelNumber = txtOEModelNumber.Text;
            entity.Area = txtArea.Text;
            entity.Material = txtMaterial.Text;
            entity.Standard = txtStandard.Text;
            entity.Introduce = txtIntroduce.Text;
            entity.ProductType = (ProductType)DataConvert.SafeInt(ddlProductType.SelectedValue);
            string cabmodels = string.Empty;
            foreach (ListItem item in lbxCabmodelsSel.Items)
            {
                cabmodels += (string.IsNullOrEmpty(cabmodels) ? string.Empty : "|") + item.Value;
            }
            entity.Cabmodels = cabmodels;
            entity.Pic = hdimage_pic.Value;
            entity.Pics = hdimage_pics.Value;
        }

        #region 车型搜索、适用车型设置

        protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPailiang.Items.Clear();
            ddlPailiang.Items.Insert(0, new ListItem("-排量-", "-1"));
            ddlPailiang.Enabled = false;

            ddlNianfen.Items.Clear();
            ddlNianfen.Items.Insert(0, new ListItem("-年份-", "-1"));
            ddlNianfen.Enabled = false;

            if (ddlBrand.SelectedIndex == 0)
            {
                ddlCabmodel.Items.Clear();
                ddlCabmodel.Items.Insert(0, new ListItem("-车型-", "-1"));
                ddlCabmodel.Enabled = false;
            }
            else
            {
                if (!CabmodelList.Exists(l => l.BrandID == DataConvert.SafeInt(ddlBrand.SelectedValue)))
                {
                    ddlCabmodel.Items.Clear();
                    ddlCabmodel.Items.Insert(0, new ListItem("-车型-", "-1"));
                    ddlCabmodel.Enabled = false;
                }
                else
                {
                    ddlCabmodel.DataSource = CabmodelList.FindAll(l => l.BrandID == DataConvert.SafeInt(ddlBrand.SelectedValue)).Select(l => l.CabmodelName).Distinct().Select(s => new KeyValuePair<string, string>(s, s)).ToList();
                    ddlCabmodel.DataTextField = "Key";
                    ddlCabmodel.DataValueField = "Value";
                    ddlCabmodel.DataBind();
                    ddlCabmodel.Items.Insert(0, new ListItem("-车型-", "-1"));
                    ddlCabmodel.Enabled = true;
                }
            }
        }

        protected void ddlCabmodel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlNianfen.Items.Clear();
            ddlNianfen.Items.Insert(0, new ListItem("-年份-", "-1"));
            ddlNianfen.Enabled = false;

            if (ddlCabmodel.SelectedIndex == 0)
            {
                ddlPailiang.Items.Clear();
                ddlPailiang.Items.Insert(0, new ListItem("-排量-", "-1"));
                ddlPailiang.Enabled = false;
            }
            else
            {
                if (!CabmodelList.Exists(l => l.BrandID == DataConvert.SafeInt(ddlBrand.SelectedValue) && l.CabmodelName == ddlCabmodel.SelectedValue))
                {
                    ddlPailiang.Items.Clear();
                    ddlPailiang.Items.Insert(0, new ListItem("-排量-", "-1"));
                    ddlPailiang.Enabled = false;
                }
                else
                {
                    ddlPailiang.DataSource = CabmodelList.FindAll(l => l.BrandID == DataConvert.SafeInt(ddlBrand.SelectedValue) && l.CabmodelName == ddlCabmodel.SelectedValue).Select(l => l.Pailiang).Distinct().Select(s => new KeyValuePair<string, string>(s, s)).ToList();
                    ddlPailiang.DataTextField = "Key";
                    ddlPailiang.DataValueField = "Value";
                    ddlPailiang.DataBind();
                    ddlPailiang.Items.Insert(0, new ListItem("-排量-", "-1"));
                    ddlPailiang.Enabled = true;
                }
            }
        }

        protected void ddlPailiang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPailiang.SelectedIndex == 0)
            {
                ddlNianfen.Items.Clear();
                ddlNianfen.Items.Insert(0, new ListItem("-年份-", "-1"));
                ddlNianfen.Enabled = false;
            }
            else
            {
                if (!CabmodelList.Exists(l => l.BrandID == DataConvert.SafeInt(ddlBrand.SelectedValue) && l.CabmodelName == ddlCabmodel.SelectedValue && l.Pailiang == ddlPailiang.SelectedValue))
                {
                    ddlNianfen.Items.Clear();
                    ddlNianfen.Items.Insert(0, new ListItem("-年份-", "-1"));
                    ddlNianfen.Enabled = false;
                }
                else
                {
                    ddlNianfen.DataSource = CabmodelList.FindAll(l => l.BrandID == DataConvert.SafeInt(ddlBrand.SelectedValue) && l.CabmodelName == ddlCabmodel.SelectedValue && l.Pailiang == ddlPailiang.SelectedValue).Select(l => l.Nianfen.ToString()).Distinct().Select(s => new KeyValuePair<string, string>(s, s)).ToList();
                    ddlNianfen.DataTextField = "Key";
                    ddlNianfen.DataValueField = "Value";
                    ddlNianfen.DataBind();
                    ddlNianfen.Items.Insert(0, new ListItem("-年份-", "-1"));
                    ddlNianfen.Enabled = true;
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            List<CabmodelInfo> list = Cars.Instance.GetCabmodelList(true);
            if (ddlBrand.SelectedIndex > 0)
            {
                list = list.FindAll(l => l.BrandID == DataConvert.SafeInt(ddlBrand.SelectedValue));
            }
            if (ddlCabmodel.SelectedIndex > 0)
            {
                list = list.FindAll(l => l.CabmodelName == ddlCabmodel.SelectedValue);
            }
            if (ddlPailiang.SelectedIndex > 0)
            {
                list = list.FindAll(l => l.Pailiang == ddlPailiang.SelectedValue);
            }
            if (ddlNianfen.SelectedIndex > 0)
            {
                list = list.FindAll(l => l.Nianfen.ToString() == ddlNianfen.SelectedValue);
            }

            lbxCabmodels.DataSource = list.OrderBy(l => l.BrandID).ThenBy(l => l.NameIndex).ThenBy(l => l.Pailiang).ThenBy(l => l.Nianfen).ToList();
            lbxCabmodels.DataTextField = "CabmodelNameBind";
            lbxCabmodels.DataValueField = "ID";
            lbxCabmodels.DataBind();
        }

        protected void btnCabmodelsAdd_Click(object sender, EventArgs e)
        {
            ListItemCollection lic_sel = new ListItemCollection();
            for (int i = 0; i < lbxCabmodels.Items.Count; i++)
            {
                if (lbxCabmodels.Items[i].Selected)
                {
                    lbxCabmodels.Items[i].Selected = false;
                    if (lbxCabmodelsSel.Items.FindByValue(lbxCabmodels.Items[i].Value) == null)
                        lbxCabmodelsSel.Items.Add(lbxCabmodels.Items[i]);
                    lic_sel.Add(lbxCabmodels.Items[i]);
                }
            }
            foreach (ListItem item in lic_sel)
            {
                lbxCabmodels.Items.Remove(item);
            }
            if (lbxCabmodelsSel.Items.Count > 0)
                lbxCabmodelsSel.SelectedIndex = lbxCabmodelsSel.Items.Count - 1;
            if (lbxCabmodels.Items.Count > 0)
                lbxCabmodels.SelectedIndex = 0;
        }

        protected void btnCabmodelsDel_Click(object sender, EventArgs e)
        {
            ListItemCollection lic_sel = new ListItemCollection();
            for (int i = 0; i < lbxCabmodelsSel.Items.Count; i++)
            {
                if (lbxCabmodelsSel.Items[i].Selected)
                {
                    lbxCabmodelsSel.Items[i].Selected = false;
                    lbxCabmodels.Items.Add(lbxCabmodelsSel.Items[i]);
                    lic_sel.Add(lbxCabmodelsSel.Items[i]);
                }
            }
            foreach (ListItem item in lic_sel)
            {
                lbxCabmodelsSel.Items.Remove(item);
            }
            if (lbxCabmodels.Items.Count > 0)
                lbxCabmodels.SelectedIndex = lbxCabmodels.Items.Count - 1;
            if (lbxCabmodelsSel.Items.Count > 0)
                lbxCabmodelsSel.SelectedIndex = 0;
        }

        #endregion

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int id = GetInt("id");
            if (id > 0)
            {
                product = Cars.Instance.GetProduct(id, true);
                if (product == null)
                {
                    WriteMessage("~/message/showmessage.aspx", "操作出错！", "该产品不存在，可能已经被删除！", "", FromUrl);
                }
                else
                {
                    FillData(product);
                    Cars.Instance.UpdateProduct(product);
                }
            }
            else
            {
                product = new ProductInfo();
                FillData(product);
                Cars.Instance.AddProduct(product);
                Cars.Instance.RefreshProductStock(true);
            }

            Cars.Instance.ReloadProductListCache();

            StringBuilder sb = new StringBuilder();
            sb.Append("<span class=\"dalv\">产品保存成功！</span><br />");
            WriteMessage("~/message/showmessage.aspx", "产品保存成功！", sb.ToString(), "", string.IsNullOrEmpty(FromUrl) ? "~/product/productmg.aspx" : FromUrl);
        }
    }
}