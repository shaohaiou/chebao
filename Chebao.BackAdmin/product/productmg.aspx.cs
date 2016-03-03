using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using Chebao.Tools;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using System.Text;

namespace Chebao.BackAdmin.product
{
    public partial class productmg : AdminBase
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
        private List<CabmodelInfo> _cabmodellist = null;
        public List<CabmodelInfo> CabmodelList
        {
            get
            {
                if (_cabmodellist == null)
                {
                    _cabmodellist = Cars.Instance.GetCabmodelList(true);
                }
                return _cabmodellist;
            }
        }

        private List<BrandInfo> brandlist = null;
        private List<BrandInfo> BrandList
        {
            get
            {
                if (brandlist == null)
                {
                    brandlist = Cars.Instance.GetBrandList(true);
                    brandlist = brandlist.OrderBy(l => l.NameIndex).ToList();
                }
                return brandlist;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (GetString("action") == "del")
                {
                    Cars.Instance.DeletetProducts(GetString("ids"));
                    Cars.Instance.ReloadProductListCache();
                    Response.Redirect(FromUrl);
                    Response.End();
                }
                else if (GetString("action") == "copy")
                {
                    int id = GetInt("pid");
                    ProductInfo pinfo = Cars.Instance.GetProduct(id ,true);
                    if (pinfo != null)
                    {
                        Cars.Instance.AddProduct(pinfo);
                        Cars.Instance.ReloadProductListCache();
                    }
                    Response.Redirect(FromUrl);
                    Response.End();
                }
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

            ddlBrandFilter.DataSource = BrandList;
            ddlBrandFilter.DataTextField = "BrandNameBind";
            ddlBrandFilter.DataValueField = "ID";
            ddlBrandFilter.DataBind();
            ddlBrandFilter.Items.Insert(0, new ListItem("-品牌-", "-1"));

            if (GetInt("id") > 0)
            {
                int id = GetInt("id");

                CabmodelInfo cab = CabmodelList.Find(l => l.ID == id);
                if (cab != null)
                {
                    SetSelectedByValue(ddlBrandFilter, cab.BrandID.ToString());

                    ddlCabmodelFilter.DataSource = CabmodelList.FindAll(l => l.BrandID == cab.BrandID).Select(l => l.CabmodelName).Distinct().Select(s => new KeyValuePair<string, string>(s, s)).ToList();
                    ddlCabmodelFilter.DataTextField = "Key";
                    ddlCabmodelFilter.DataValueField = "Value";
                    ddlCabmodelFilter.DataBind();
                    ddlCabmodelFilter.Items.Insert(0, new ListItem("-型号-", "-1"));
                    ddlCabmodelFilter.Enabled = true;
                    SetSelectedByValue(ddlCabmodelFilter, cab.CabmodelName);

                    ddlPailiangFilter.DataSource = CabmodelList.FindAll(l => l.BrandID == cab.BrandID && l.CabmodelName == cab.CabmodelName).Select(l => l.Pailiang).Distinct().Select(s => new KeyValuePair<string, string>(s, s)).ToList();
                    ddlPailiangFilter.DataTextField = "Key";
                    ddlPailiangFilter.DataValueField = "Value";
                    ddlPailiangFilter.DataBind();
                    ddlPailiangFilter.Items.Insert(0, new ListItem("-排量-", "-1"));
                    ddlPailiangFilter.Enabled = true;
                    SetSelectedByValue(ddlPailiangFilter, cab.Pailiang);

                    ddlNianfenFilter.DataSource = CabmodelList.FindAll(l => l.BrandID == cab.BrandID && l.CabmodelName == cab.CabmodelName && l.Pailiang == cab.Pailiang).Select(l => l.Nianfen.ToString()).Distinct().Select(s => new KeyValuePair<string, string>(s, s)).ToList();
                    ddlNianfenFilter.DataTextField = "Key";
                    ddlNianfenFilter.DataValueField = "Value";
                    ddlNianfenFilter.DataBind();
                    ddlNianfenFilter.Items.Insert(0, new ListItem("-年份-", "-1"));
                    ddlNianfenFilter.Enabled = true;
                    SetSelectedByValue(ddlNianfenFilter, cab.Nianfen.ToString());

                    productlist = productlist.FindAll(p => p.Cabmodels.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Contains(cab.ID.ToString()));
                }
            }
            else
            {
                ddlCabmodelFilter.Items.Insert(0, new ListItem("-型号-", "-1"));
                ddlCabmodelFilter.Enabled = false;
                ddlPailiangFilter.Items.Insert(0, new ListItem("-排量-", "-1"));
                ddlPailiangFilter.Enabled = false;
                ddlNianfenFilter.Items.Insert(0, new ListItem("-年份-", "-1"));
                ddlNianfenFilter.Enabled = false;
            }

            productlist = productlist.OrderBy(l => l.NameOrder).ThenBy(l=>l.ModelNumber).ThenBy(l => l.ID).ToList();
            total = productlist.Count();
            productlist = productlist.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<ProductInfo>();

            rptProduct.DataSource = productlist;
            rptProduct.DataBind();

            search_fy.RecordCount = total;
            search_fy.PageSize = pagesize;
            search_fy.CustomInfoHTML = string.Format("<span style=\"line-height: 34px;\">总记录数：{0} 总页数：{1}</span>", total, search_fy.PageCount);

            ddlProductTypeFilter.DataSource = EnumExtensions.ToTable<ProductType>();
            ddlProductTypeFilter.DataTextField = "Text";
            ddlProductTypeFilter.DataValueField = "Value";
            ddlProductTypeFilter.DataBind();
            ddlProductTypeFilter.Items.Insert(0, new ListItem("-产品类型-", "-1"));

            if (GetInt("pt", -1) >= 0)
            {
                SetSelectedByValue(ddlProductTypeFilter, GetString("pt"));
            }
            if (!string.IsNullOrEmpty(GetString("name")))
            {
                txtNameFilter.Text = GetString("name");
            }
            if (!string.IsNullOrEmpty(GetString("mn")))
            {
                txtModelNumberFilter.Text = GetString("mn");
            }
        }

        #region 车型搜索

        protected void ddlBrandFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPailiangFilter.Items.Clear();
            ddlPailiangFilter.Items.Insert(0, new ListItem("-排量-", "-1"));
            ddlPailiangFilter.Enabled = false;

            ddlNianfenFilter.Items.Clear();
            ddlNianfenFilter.Items.Insert(0, new ListItem("-年份-", "-1"));
            ddlNianfenFilter.Enabled = false;

            if (ddlBrandFilter.SelectedIndex == 0)
            {
                ddlCabmodelFilter.Items.Clear();
                ddlCabmodelFilter.Items.Insert(0, new ListItem("-型号-", "-1"));
                ddlCabmodelFilter.Enabled = false;
            }
            else
            {
                if (!CabmodelList.Exists(l => l.BrandID == DataConvert.SafeInt(ddlBrandFilter.SelectedValue)))
                {
                    ddlCabmodelFilter.Items.Clear();
                    ddlCabmodelFilter.Items.Insert(0, new ListItem("-型号-", "-1"));
                    ddlCabmodelFilter.Enabled = false;
                }
                else
                {
                    ddlCabmodelFilter.DataSource = CabmodelList.FindAll(l => l.BrandID == DataConvert.SafeInt(ddlBrandFilter.SelectedValue)).Select(l => l.CabmodelName).Distinct().Select(s => new KeyValuePair<string, string>(s, s)).ToList();
                    ddlCabmodelFilter.DataTextField = "Key";
                    ddlCabmodelFilter.DataValueField = "Value";
                    ddlCabmodelFilter.DataBind();
                    ddlCabmodelFilter.Items.Insert(0, new ListItem("-型号-", "-1"));
                    ddlCabmodelFilter.Enabled = true;
                }
            }
        }

        protected void ddlCabmodelFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlNianfenFilter.Items.Clear();
            ddlNianfenFilter.Items.Insert(0, new ListItem("-年份-", "-1"));
            ddlNianfenFilter.Enabled = false;

            if (ddlCabmodelFilter.SelectedIndex == 0)
            {
                ddlPailiangFilter.Items.Clear();
                ddlPailiangFilter.Items.Insert(0, new ListItem("-排量-", "-1"));
                ddlPailiangFilter.Enabled = false;
            }
            else
            {
                if (!CabmodelList.Exists(l => l.BrandID == DataConvert.SafeInt(ddlBrandFilter.SelectedValue) && l.CabmodelName == ddlCabmodelFilter.SelectedValue))
                {
                    ddlPailiangFilter.Items.Clear();
                    ddlPailiangFilter.Items.Insert(0, new ListItem("-排量-", "-1"));
                    ddlPailiangFilter.Enabled = false;
                }
                else
                {
                    ddlPailiangFilter.DataSource = CabmodelList.FindAll(l => l.BrandID == DataConvert.SafeInt(ddlBrandFilter.SelectedValue) && l.CabmodelName == ddlCabmodelFilter.SelectedValue).Select(l => l.Pailiang).Distinct().Select(s => new KeyValuePair<string, string>(s, s)).ToList();
                    ddlPailiangFilter.DataTextField = "Key";
                    ddlPailiangFilter.DataValueField = "Value";
                    ddlPailiangFilter.DataBind();
                    ddlPailiangFilter.Items.Insert(0, new ListItem("-排量-", "-1"));
                    ddlPailiangFilter.Enabled = true;
                }
            }
        }

        protected void ddlPailiangFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPailiangFilter.SelectedIndex == 0)
            {
                ddlNianfenFilter.Items.Clear();
                ddlNianfenFilter.Items.Insert(0, new ListItem("-年份-", "-1"));
                ddlNianfenFilter.Enabled = false;
            }
            else
            {
                if (!CabmodelList.Exists(l => l.BrandID == DataConvert.SafeInt(ddlBrandFilter.SelectedValue) && l.CabmodelName == ddlCabmodelFilter.SelectedValue && l.Pailiang == ddlPailiangFilter.SelectedValue))
                {
                    ddlNianfenFilter.Items.Clear();
                    ddlNianfenFilter.Items.Insert(0, new ListItem("-年份-", "-1"));
                    ddlNianfenFilter.Enabled = false;
                }
                else
                {
                    ddlNianfenFilter.DataSource = CabmodelList.FindAll(l => l.BrandID == DataConvert.SafeInt(ddlBrandFilter.SelectedValue) && l.CabmodelName == ddlCabmodelFilter.SelectedValue && l.Pailiang == ddlPailiangFilter.SelectedValue).Select(l => l.Nianfen.ToString()).Distinct().Select(s => new KeyValuePair<string, string>(s, s)).ToList();
                    ddlNianfenFilter.DataTextField = "Key";
                    ddlNianfenFilter.DataValueField = "Value";
                    ddlNianfenFilter.DataBind();
                    ddlNianfenFilter.Items.Insert(0, new ListItem("-年份-", "-1"));
                    ddlNianfenFilter.Enabled = true;
                }
            }
        }

        protected void ddlNianfenFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            CabmodelInfo cab = CabmodelList.Find(l => l.BrandID == DataConvert.SafeInt(ddlBrandFilter.SelectedValue)
                && l.CabmodelName == ddlCabmodelFilter.SelectedValue
                && l.Pailiang == ddlPailiangFilter.SelectedValue
                && l.Nianfen.ToString() == ddlNianfenFilter.SelectedValue);
            List<string> query = new List<string>();
            if (cab != null)
                query.Add("id=" + cab.ID);
            if (ddlProductTypeFilter.SelectedIndex > 0)
                query.Add("pt=" + ddlProductTypeFilter.SelectedValue);
            if (!string.IsNullOrEmpty(txtNameFilter.Text))
                query.Add("name=" + txtNameFilter.Text);
            if (!string.IsNullOrEmpty(txtModelNumberFilter.Text))
                query.Add("mn=" + txtModelNumberFilter.Text);
            ScriptManager.RegisterClientScriptBlock(upnCabmodels, this.GetType(), "redirect", "location='productmg.aspx?" + string.Join("&", query) + "'", true);
        }

        #endregion

        #region 导出Excel

        static HSSFWorkbook hssfworkbook;

        public void InitializeWorkbook()
        {
            hssfworkbook = new HSSFWorkbook();
            ////create a entry of DocumentSummaryInformation
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "耐磨达";
            hssfworkbook.DocumentSummaryInformation = dsi;
            ////create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "xxx";
            hssfworkbook.SummaryInformation = si;
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
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
            InitializeWorkbook();
            HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet1");
            HSSFRow rowHeader = (HSSFRow)sheet1.CreateRow(0);
            rowHeader.CreateCell(0).SetCellValue("名称");
            rowHeader.CreateCell(1).SetCellValue("类型");
            rowHeader.CreateCell(2).SetCellValue("价格");
            rowHeader.CreateCell(3).SetCellValue("lamda型号");
            rowHeader.CreateCell(4).SetCellValue("OE型号");
            sheet1.SetColumnWidth(0, 35 * 256);
            sheet1.SetColumnWidth(1, 9 * 256);
            sheet1.SetColumnWidth(2, 9 * 256);
            sheet1.SetColumnWidth(3, 20 * 256);
            sheet1.SetColumnWidth(4, 20 * 256);

            for (int i = 0; i < productlist.Count; i++)
            {
                HSSFRow row = (HSSFRow)sheet1.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(productlist[i].Name);
                row.CreateCell(1).SetCellValue(productlist[i].ProductType.ToString());
                row.CreateCell(2).SetCellValue("￥" + productlist[i].Price);
                row.CreateCell(3).SetCellValue(productlist[i].ModelNumber);
                row.CreateCell(4).SetCellValue(productlist[i].OEModelNumber);

            }

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                hssfworkbook.Write(ms);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode("产品数据", Encoding.UTF8).ToString() + ".xls");
                Response.BinaryWrite(ms.ToArray());
                Response.End();
                hssfworkbook = null;
            }
        }

        #endregion

        protected string GetProductTypeName(object type)
        {
            return EnumExtensions.GetDescription<ProductType>(type.ToString());
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            Response.Redirect("productmg.aspx?pt=" + ddlProductTypeFilter.SelectedValue + "&name=" + txtNameFilter.Text + "&mn=" + txtModelNumberFilter.Text);
        }

        protected string GetStock(object pmstrs)
        {
            List<KeyValuePair<string, int>> result = new List<KeyValuePair<string, int>>();

            if (!string.IsNullOrEmpty(pmstrs.ToString()))
            {
                List<ProductInfo> plist = Cars.Instance.GetProductList(true);
                foreach (string pmstr in pmstrs.ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (plist.Exists(p => pmstr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0].StartsWith(p.ModelNumber)))
                        result.Add(new KeyValuePair<string, int>(pmstr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0], DataConvert.SafeInt(pmstr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[1])));
                }
            }

            return string.Join("<br />",result.Select(s => s.Key + ":" + s.Value).ToList());
        }
    }
}