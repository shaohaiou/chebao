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
    public partial class products : AdminBase
    {
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

        public CabmodelInfo SearchCabmodel
        {
            get
            {
                if (Session[GlobalKey.SEARCHCABMODELID] != null)
                {
                    int cabid = DataConvert.SafeInt(Session[GlobalKey.SEARCHCABMODELID]);
                    if (cabid > 0)
                    {
                        return Cars.Instance.GetCabmodelList(true).Find(c => c.ID == cabid);
                    }
                }
                return null;
            }
        }

        protected bool HasProduct = false;

        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
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
            ddlBrand.Items.Insert(0, new ListItem("请选择爱车品牌", "-1"));

            ddlCabmodel.Items.Insert(0, new ListItem("请选择型号", "-1"));
            ddlCabmodel.Enabled = false;
            ddlPailiang.Items.Insert(0, new ListItem("请选择排量", "-1"));
            ddlPailiang.Enabled = false;
            ddlNianfen.Items.Insert(0, new ListItem("请选择出厂年份", "-1"));
            ddlNianfen.Enabled = false;

            rptProductType.DataSource = EnumExtensions.ToTable<ProductType>();
            rptProductType.DataBind();
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
            string number = GetString("n");
            int t = GetInt("t", -1);
            int id = GetInt("id");
            if (id > 0)
            {
                CabmodelInfo cab = CabmodelList.Find(l => l.ID == id);
                if (cab != null)
                {
                    SetSelectedByValue(ddlBrand, cab.BrandID.ToString());

                    ddlCabmodel.DataSource = CabmodelList.FindAll(l => l.BrandID == cab.BrandID).Select(l => l.CabmodelName).Distinct().Select(s => new KeyValuePair<string, string>(s, s)).ToList();
                    ddlCabmodel.DataTextField = "Key";
                    ddlCabmodel.DataValueField = "Value";
                    ddlCabmodel.DataBind();
                    ddlCabmodel.Items.Insert(0, new ListItem("请选择型号", "-1"));
                    ddlCabmodel.Enabled = true;
                    SetSelectedByValue(ddlCabmodel, cab.CabmodelName);

                    ddlPailiang.DataSource = CabmodelList.FindAll(l => l.BrandID == cab.BrandID && l.CabmodelName == cab.CabmodelName).Select(l => l.Pailiang).Distinct().Select(s => new KeyValuePair<string, string>(s, s)).ToList();
                    ddlPailiang.DataTextField = "Key";
                    ddlPailiang.DataValueField = "Value";
                    ddlPailiang.DataBind();
                    ddlPailiang.Items.Insert(0, new ListItem("请选择排量", "-1"));
                    ddlPailiang.Enabled = true;
                    SetSelectedByValue(ddlPailiang, cab.Pailiang);

                    ddlNianfen.DataSource = CabmodelList.FindAll(l => l.BrandID == cab.BrandID && l.CabmodelName == cab.CabmodelName && l.Pailiang == cab.Pailiang).Select(l => l.Nianfen.ToString()).Distinct().OrderBy(l => l).Select(s => new KeyValuePair<string, string>(s, s)).ToList();
                    ddlNianfen.DataTextField = "Key";
                    ddlNianfen.DataValueField = "Value";
                    ddlNianfen.DataBind();
                    ddlNianfen.Items.Insert(0, new ListItem("请选择出厂年份", "-1"));
                    ddlNianfen.Enabled = true;
                    SetSelectedByValue(ddlNianfen, cab.Nianfen.ToString());

                    productlist = productlist.FindAll(p => p.Cabmodels.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Contains(cab.ID.ToString()));
                }
                Session[GlobalKey.SEARCHCABMODELID] = id;
            }
            else
                Session[GlobalKey.SEARCHCABMODELID] = null;

            if (t >= 0)
            {
                productlist = productlist.FindAll(p => p.ProductType == (ProductType)t);
            }
            if (!string.IsNullOrEmpty(number))
            {
                productlist = productlist.FindAll(p => p.ModelNumber.ToLower().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Contains(number.ToLower()));
                txtsearch.Value = number;
                txtsearch.Attributes["style"] = "color:Black;";
            }

            HasProduct = productlist != null && productlist.Count > 0;
            productlist = productlist.OrderBy(p=>(int)p.ProductType).ToList();
            productlist = productlist.Count > 8 ? productlist.GetRange(0, 8) : productlist;
            total = productlist.Count();
            productlist = productlist.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<ProductInfo>();
            rptProduct.DataSource = productlist;
            rptProduct.DataBind();

            search_fy.RecordCount = total;
            search_fy.PageSize = pagesize;

            search_fy1.RecordCount = total;
            search_fy1.PageSize = pagesize;
        }

        #region 车型搜索

        protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPailiang.Items.Clear();
            ddlPailiang.Items.Insert(0, new ListItem("请选择排量", "-1"));
            ddlPailiang.Enabled = false;

            ddlNianfen.Items.Clear();
            ddlNianfen.Items.Insert(0, new ListItem("请选择出厂年份", "-1"));
            ddlNianfen.Enabled = false;

            if (ddlBrand.SelectedIndex == 0)
            {
                ddlCabmodel.Items.Clear();
                ddlCabmodel.Items.Insert(0, new ListItem("请选择型号", "-1"));
                ddlCabmodel.Enabled = false;
            }
            else
            {
                if (!CabmodelList.Exists(l => l.BrandID == DataConvert.SafeInt(ddlBrand.SelectedValue)))
                {
                    ddlCabmodel.Items.Clear();
                    ddlCabmodel.Items.Insert(0, new ListItem("请选择型号", "-1"));
                    ddlCabmodel.Enabled = false;
                }
                else
                {
                    ddlCabmodel.DataSource = CabmodelList.FindAll(l => l.BrandID == DataConvert.SafeInt(ddlBrand.SelectedValue)).Select(l => l.CabmodelName).Distinct().Select(s => new KeyValuePair<string, string>(s, s)).ToList();
                    ddlCabmodel.DataTextField = "Key";
                    ddlCabmodel.DataValueField = "Value";
                    ddlCabmodel.DataBind();
                    ddlCabmodel.Items.Insert(0, new ListItem("请选择型号", "-1"));
                    ddlCabmodel.Enabled = true;
                }
            }
        }

        protected void ddlCabmodel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlNianfen.Items.Clear();
            ddlNianfen.Items.Insert(0, new ListItem("请选择出厂年份", "-1"));
            ddlNianfen.Enabled = false;

            if (ddlCabmodel.SelectedIndex == 0)
            {
                ddlPailiang.Items.Clear();
                ddlPailiang.Items.Insert(0, new ListItem("请选择排量", "-1"));
                ddlPailiang.Enabled = false;
            }
            else
            {
                if (!CabmodelList.Exists(l => l.BrandID == DataConvert.SafeInt(ddlBrand.SelectedValue) && l.CabmodelName == ddlCabmodel.SelectedValue))
                {
                    ddlPailiang.Items.Clear();
                    ddlPailiang.Items.Insert(0, new ListItem("请选择排量", "-1"));
                    ddlPailiang.Enabled = false;
                }
                else
                {
                    ddlPailiang.DataSource = CabmodelList.FindAll(l => l.BrandID == DataConvert.SafeInt(ddlBrand.SelectedValue) && l.CabmodelName == ddlCabmodel.SelectedValue).Select(l => l.Pailiang).Distinct().Select(s => new KeyValuePair<string, string>(s, s)).ToList();
                    ddlPailiang.DataTextField = "Key";
                    ddlPailiang.DataValueField = "Value";
                    ddlPailiang.DataBind();
                    ddlPailiang.Items.Insert(0, new ListItem("请选择排量", "-1"));
                    ddlPailiang.Enabled = true;
                }
            }
        }

        protected void ddlPailiang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPailiang.SelectedIndex == 0)
            {
                ddlNianfen.Items.Clear();
                ddlNianfen.Items.Insert(0, new ListItem("请选择出厂年份", "-1"));
                ddlNianfen.Enabled = false;
            }
            else
            {
                if (!CabmodelList.Exists(l => l.BrandID == DataConvert.SafeInt(ddlBrand.SelectedValue) && l.CabmodelName == ddlCabmodel.SelectedValue && l.Pailiang == ddlPailiang.SelectedValue))
                {
                    ddlNianfen.Items.Clear();
                    ddlNianfen.Items.Insert(0, new ListItem("请选择出厂年份", "-1"));
                    ddlNianfen.Enabled = false;
                }
                else
                {
                    ddlNianfen.DataSource = CabmodelList.FindAll(l => l.BrandID == DataConvert.SafeInt(ddlBrand.SelectedValue) && l.CabmodelName == ddlCabmodel.SelectedValue && l.Pailiang == ddlPailiang.SelectedValue).Select(l => l.Nianfen.ToString()).Distinct().OrderBy(l=>l).Select(s => new KeyValuePair<string, string>(s, s)).ToList();
                    ddlNianfen.DataTextField = "Key";
                    ddlNianfen.DataValueField = "Value";
                    ddlNianfen.DataBind();
                    ddlNianfen.Items.Insert(0, new ListItem("请选择出厂年份", "-1"));
                    ddlNianfen.Enabled = true;
                }
            }
        }

        protected void ddlNianfen_SelectedIndexChanged(object sender, EventArgs e)
        {
            CabmodelInfo cab = CabmodelList.Find(l => l.BrandID == DataConvert.SafeInt(ddlBrand.SelectedValue)
                && l.CabmodelName == ddlCabmodel.SelectedValue
                && l.Pailiang == ddlPailiang.SelectedValue
                && l.Nianfen.ToString() == ddlNianfen.SelectedValue);
            List<string> query = new List<string>();
            query.Add("t=" + GetString("t"));
            if (cab != null)
            {
                query.Add("id=" + cab.ID);
                Session[GlobalKey.SEARCHCABMODELID] = cab.ID;
            }
            else
                Session[GlobalKey.SEARCHCABMODELID] = null;
            ScriptManager.RegisterClientScriptBlock(upnCabmodels, this.GetType(), "redirect", "location='products.aspx?" + string.Join("&", query) + "'", true);
        }

        #endregion

        protected string GetTypeQueryStr()
        {
            return string.Join("&", Request.QueryString.AllKeys.ToList().FindAll(k => k != "t").Select(k => k + "=" + Request.QueryString[k]));
        }
    }
}