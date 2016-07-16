using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using Chebao.Tools;
using System.Web.UI.HtmlControls;
using System.Text;

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

        private bool? isshowprice;
        public bool IsShowPrice
        {
            get
            {
                //if (!isshowprice.HasValue)
                //{
                //    if (Admin.ParentAccountID > 0)
                //        isshowprice = ParentAdmin.IsShowPrice > 0;
                //    else
                //        isshowprice = Admin.IsShowPrice > 0;
                //}
                if (!isshowprice.HasValue)
                {
                    isshowprice = Admin.IsShowPrice > 0 && (ParentAdmin == null || ParentAdmin.IsShowPrice > 0);
                }
                return isshowprice.Value;
            }
        }

        private bool? isshowcabmodel;
        public bool IsShowCabmodel
        {
            get
            {
                if (!isshowcabmodel.HasValue)
                {
                    isshowcabmodel = Admin.IsShowCabmodel > 0 && (ParentAdmin == null || ParentAdmin.IsShowCabmodel > 0);
                }
                return isshowcabmodel.Value;

                //if (Admin.ParentAccountID > 0)
                //    return ParentAdmin.IsShowCabmodel > 0;
                //return Admin.IsShowCabmodel > 0;
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

            //品牌图片
            List<BrandInfo> brandlist = Cars.Instance.GetBrandList(true);
            foreach (ListItem item in ddlBrand.Items)
            {
                if (item.Value != "-1")
                {
                    string imgpath = brandlist.Find(b => b.ID.ToString() == item.Value).Imgpath;
                    if (!string.IsNullOrEmpty(imgpath))
                    {
                        item.Attributes.Add("data-image", imgpath);
                    }
                }
            }
        }

        private void BindControler()
        {
            List<BrandInfo> brandlist = Cars.Instance.GetBrandList(true);
            if (!string.IsNullOrEmpty(Admin.BrandPowerSetting))
            {
                string[] brands = Admin.BrandPowerSetting.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                brandlist = brandlist.FindAll(b => brands.Contains(b.ID.ToString()));
            }
            brandlist = brandlist.OrderBy(l => l.NameIndex).ToList();
            ddlBrand.DataSource = brandlist;
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

            List<ProductInfo> productlist;
            //if (Admin.ParentAccountID == 0)
                productlist = Cars.Instance.GetProductList(true);
            //else
            //    productlist = Cars.Instance.GetProductListByUser(Admin.ParentAccountID);
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
            productlist = productlist.OrderBy(p => (int)p.ProductType).ToList();
            productlist = productlist.Count > 8 ? productlist.GetRange(0, 8) : productlist;
            total = productlist.Count();
            productlist = productlist.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<ProductInfo>();
            rptProduct.DataSource = productlist;
            rptProduct.DataBind();

            search_fy.RecordCount = total;
            search_fy.PageSize = pagesize;

            search_fy1.RecordCount = total;
            search_fy1.PageSize = pagesize;

            List<ShoppingTrolleyInfo> listShoppingTrolley = Cars.Instance.GetShoppingTrolleyByUserID(AdminID);
            List<ProductInfo> listAllProduct = Cars.Instance.GetProductList(true);
            listShoppingTrolley = listShoppingTrolley.FindAll(l => listAllProduct.Exists(p => p.ID == l.ProductID));
            if (listShoppingTrolley.Count > 0)
            {
                string[] pids = listShoppingTrolley.OrderByDescending(s => s.ID).Select(s => s.ProductID + "|" + s.ID).ToArray();
                List<ProductInfo> listProdcutInShoppingTrolley = new List<ProductInfo>();
                for (int i = 0; i < pids.Length; i++)
                {
                    int pid = DataConvert.SafeInt(pids[i].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[0]);
                    int sid = DataConvert.SafeInt(pids[i].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[1]);
                    ProductInfo entity = listAllProduct.Find(p => p.ID == pid);
                    if (entity != null)
                    {
                        entity.SID = sid;
                        listProdcutInShoppingTrolley.Add(entity);
                    }
                }
                rptShoppingTrolley.DataSource = listProdcutInShoppingTrolley;
                rptShoppingTrolley.DataBind();
            }
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
            ScriptManager.RegisterStartupScript(upnCabmodels, upnCabmodels.GetType(), "dd", "MSDropDown();", true);
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
            ScriptManager.RegisterStartupScript(upnCabmodels, upnCabmodels.GetType(), "dd", "MSDropDown();", true);
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
                    ddlNianfen.DataSource = CabmodelList.FindAll(l => l.BrandID == DataConvert.SafeInt(ddlBrand.SelectedValue) && l.CabmodelName == ddlCabmodel.SelectedValue && l.Pailiang == ddlPailiang.SelectedValue).Select(l => l.Nianfen.ToString()).Distinct().OrderBy(l => l).Select(s => new KeyValuePair<string, string>(s, s)).ToList();
                    ddlNianfen.DataTextField = "Key";
                    ddlNianfen.DataValueField = "Value";
                    ddlNianfen.DataBind();
                    ddlNianfen.Items.Insert(0, new ListItem("请选择出厂年份", "-1"));
                    ddlNianfen.Enabled = true;
                }
            }
            ScriptManager.RegisterStartupScript(upnCabmodels, upnCabmodels.GetType(), "dd", "MSDropDown();", true);
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

        #region 购物车


        public string ShoppingTrolleyCount
        {
            get
            {
                string result = string.Empty;

                List<ShoppingTrolleyInfo> list = Cars.Instance.GetShoppingTrolleyByUserID(AdminID);
                List<ProductInfo> productlist = Cars.Instance.GetProductList(true);
                result = list.FindAll(l => productlist.Exists(p => p.ID == l.ProductID)).Count.ToString();

                return result;
            }
        }

        #endregion

        protected string GetTypeQueryStr()
        {
            return string.Join("&", Request.QueryString.AllKeys.ToList().FindAll(k => k != "t").Select(k => k + "=" + Request.QueryString[k]));
        }

        protected string GetBrandSelHtml()
        {
            StringBuilder strB = new StringBuilder();
            string letters = "abcdefghijklmnopqrstuvwxyz";
            List<BrandInfo> brandlist = Cars.Instance.GetBrandList(true);
            for (int i = 0; i < letters.Length; i++)
            {
                string letter = letters[i].ToString();
                if (brandlist.Exists(b => b.NameIndex.ToLower() == letter))
                {
                    List<BrandInfo> list = brandlist.FindAll(b => b.NameIndex.ToLower() == letter);
                    int rows = list.Count / 4 + (list.Count % 4 > 0 ? 1 : 0);
                    for (int j = 0; j < rows; j++)
                    { 
                        strB.AppendLine("<tr>");
                        strB.AppendFormat("<td class=\"cityBlue\">{0}</td>", j == 0 ? letter.ToUpper() : string.Empty);
                        for (int k = 0; k < 4; k++)
                        {
                            if (list.Count > (j * 4 + k))
                            {
                                BrandInfo brand = list[j * 4 + k];
                                strB.AppendLine("<td class=\"zz_51Lower brandsel\" style=\"cursor: pointer; width: 185px;padding-left: 1px;\" val=\"" + brand.ID + "\">" + (string.IsNullOrEmpty(brand.Imgpath) ? string.Empty : ("<img src=\"" + brand.Imgpath +"\" style=\"width: 22px; height: 22px;float:left;margin-right:2px\">")) + brand.BrandName + "</td>");
                            }
                            else
                                strB.AppendLine("<td></td>");

                        }

                        strB.AppendLine("<tr>");
                    }
                }
            }

            return strB.ToString();
        }
    }
}