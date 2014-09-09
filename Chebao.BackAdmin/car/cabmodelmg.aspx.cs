using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using Chebao.Tools;

namespace Chebao.BackAdmin.car
{
    public partial class cabmodelmg : AdminBase
    {
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

        private Dictionary<string, string> nianfenlist = null;
        private Dictionary<string, string> NianfenList
        {
            get
            {
                if (nianfenlist == null)
                {
                    nianfenlist = new Dictionary<string, string>();

                    for (int i = DateTime.Now.Year - 15; i < DateTime.Now.Year + 1; i++)
                    {
                        nianfenlist.Add(i.ToString(), i.ToString());
                    }
                }

                return nianfenlist;
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
            int pagesize = GetInt("pagesize", 10);
            int total = 0;

            ddlBrandFilter.DataSource = BrandList;
            ddlBrandFilter.DataTextField = "BrandNameBind";
            ddlBrandFilter.DataValueField = "ID";
            ddlBrandFilter.DataBind();
            ddlBrandFilter.Items.Insert(0, new ListItem("-品牌-", "-1"));

            List<CabmodelInfo> cabmodellist = Cars.Instance.GetCabmodelList(true);
            if (GetInt("id") > 0)
            {
                int id = GetInt("id");
                cabmodellist = cabmodellist.FindAll(l => l.ID == id);

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
                }
            }
            else
            {
                if (GetInt("brand") > 0)
                {
                    int brandid = GetInt("brand");
                    cabmodellist = cabmodellist.FindAll(l => l.BrandID == brandid);
                }
                if (!string.IsNullOrEmpty(GetString("cabmodel")))
                {
                    string cabmodel = GetString("cabmodel");
                    cabmodellist = cabmodellist.FindAll(l => l.CabmodelName == cabmodel);
                }
                if (!string.IsNullOrEmpty(GetString("pailiang")))
                {
                    string pailiang = GetString("pailiang");
                    cabmodellist = cabmodellist.FindAll(l => l.Pailiang.ToLower().IndexOf(pailiang.ToLower()) >= 0);
                }
                if (!string.IsNullOrEmpty(GetString("nianfen")))
                {
                    string nianfen = GetString("nianfen");
                    cabmodellist = cabmodellist.FindAll(l => l.Nianfen.ToLower().IndexOf(nianfen.ToLower()) >= 0);
                }
                if (!string.IsNullOrEmpty(GetString("noimg")))
                    cabmodellist = cabmodellist.FindAll(l => string.IsNullOrEmpty(l.Imgpath));

                ddlCabmodelFilter.Items.Insert(0, new ListItem("-型号-", "-1"));
                ddlCabmodelFilter.Enabled = false;
                ddlPailiangFilter.Items.Insert(0, new ListItem("-排量-", "-1"));
                ddlPailiangFilter.Enabled = false;
                ddlNianfenFilter.Items.Insert(0, new ListItem("-年份-", "-1"));
                ddlNianfenFilter.Enabled = false;

                if (GetInt("brand") > 0)
                {
                    SetSelectedByValue(ddlBrandFilter, GetString("brand"));

                    ddlCabmodelFilter.DataSource = CabmodelList.FindAll(l => l.BrandID == DataConvert.SafeInt(ddlBrandFilter.SelectedValue)).Select(l => l.CabmodelName).Distinct().Select(s => new KeyValuePair<string, string>(s, s)).ToList();
                    ddlCabmodelFilter.DataTextField = "Key";
                    ddlCabmodelFilter.DataValueField = "Value";
                    ddlCabmodelFilter.DataBind();
                    ddlCabmodelFilter.Items.Insert(0, new ListItem("-型号-", "-1"));
                    ddlCabmodelFilter.Enabled = true;
                }
                if (!string.IsNullOrEmpty(GetString("cabmodel")))
                {
                    SetSelectedByValue(ddlCabmodelFilter, GetString("cabmodel"));

                    ddlPailiangFilter.DataSource = CabmodelList.FindAll(l => l.BrandID == DataConvert.SafeInt(ddlBrandFilter.SelectedValue) && l.CabmodelName == ddlCabmodelFilter.SelectedValue).Select(l => l.Pailiang).Distinct().Select(s => new KeyValuePair<string, string>(s, s)).ToList();
                    ddlPailiangFilter.DataTextField = "Key";
                    ddlPailiangFilter.DataValueField = "Value";
                    ddlPailiangFilter.DataBind();
                    ddlPailiangFilter.Items.Insert(0, new ListItem("-排量-", "-1"));
                    ddlPailiangFilter.Enabled = true;
                }
                if (!string.IsNullOrEmpty(GetString("pailiang")))
                {
                    SetSelectedByValue(ddlPailiangFilter, GetString("pailiang"));

                    ddlNianfenFilter.DataSource = CabmodelList.FindAll(l => l.BrandID == DataConvert.SafeInt(ddlBrandFilter.SelectedValue) && l.CabmodelName == ddlCabmodelFilter.SelectedValue && l.Pailiang == ddlPailiangFilter.SelectedValue).Select(l => l.Nianfen.ToString()).Distinct().OrderBy(l => l).Select(s => new KeyValuePair<string, string>(s, s)).ToList();
                    ddlNianfenFilter.DataTextField = "Key";
                    ddlNianfenFilter.DataValueField = "Value";
                    ddlNianfenFilter.DataBind();
                    ddlNianfenFilter.Items.Insert(0, new ListItem("-年份-", "-1"));
                    ddlNianfenFilter.Enabled = true;
                }
                if (!string.IsNullOrEmpty(GetString("nianfen")))
                {
                    SetSelectedByValue(ddlNianfenFilter, GetString("nianfen").Substring(0, 4));
                }
                if (!string.IsNullOrEmpty(GetString("noimg")))
                    chxNoimgFilter.Checked = true;
            }

            cabmodellist = cabmodellist.OrderBy(l => l.BrandID).ThenBy(l => l.NameIndex).ThenBy(l => l.Pailiang).ThenBy(l => l.Nianfen).ToList();
            total = cabmodellist.Count();
            cabmodellist = cabmodellist.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<CabmodelInfo>();

            rptCabmodel.DataSource = cabmodellist;
            rptCabmodel.DataBind();

            search_fy.RecordCount = total;
            search_fy.PageSize = pagesize;
            search_fy.CustomInfoHTML = string.Format("<span style=\"line-height: 34px;\">总记录数：{0} 总页数：{1}</span>", total, search_fy.PageCount);

            ddlBrand.DataSource = BrandList;
            ddlBrand.DataTextField = "BrandNameBind";
            ddlBrand.DataValueField = "ID";
            ddlBrand.DataBind();

            ddlNianfen.DataSource = NianfenList;
            ddlNianfen.DataTextField = "Key";
            ddlNianfen.DataValueField = "Value";
            ddlNianfen.DataBind();
        }

        protected void rptCabmodel_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CabmodelInfo info = (CabmodelInfo)e.Item.DataItem;
                System.Web.UI.WebControls.DropDownList ddlBrand = (System.Web.UI.WebControls.DropDownList)e.Item.FindControl("ddlBrand");
                System.Web.UI.WebControls.DropDownList ddlNianfen = (System.Web.UI.WebControls.DropDownList)e.Item.FindControl("ddlNianfen");

                if (ddlBrand != null)
                {
                    ddlBrand.DataSource = BrandList;
                    ddlBrand.DataTextField = "BrandNameBind";
                    ddlBrand.DataValueField = "ID";
                    ddlBrand.DataBind();

                    SetSelectedByValue(ddlBrand, info.BrandID.ToString());
                }
                if (ddlNianfen != null)
                {
                    ddlNianfen.DataSource = NianfenList;
                    ddlNianfen.DataTextField = "Key";
                    ddlNianfen.DataValueField = "Value";
                    ddlNianfen.DataBind();

                    SetSelectedByValue(ddlNianfen, info.Nianfen.ToString());
                }
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
                    ddlNianfenFilter.DataSource = CabmodelList.FindAll(l => l.BrandID == DataConvert.SafeInt(ddlBrandFilter.SelectedValue) && l.CabmodelName == ddlCabmodelFilter.SelectedValue && l.Pailiang == ddlPailiangFilter.SelectedValue).Select(l => l.Nianfen.ToString()).Distinct().OrderBy(l=>l).Select(s => new KeyValuePair<string, string>(s, s)).ToList();
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
            {
                query.Add("id=" + cab.ID);
            }
            ScriptManager.RegisterClientScriptBlock(upnCabmodels, this.GetType(), "redirect", "location='cabmodelmg.aspx?" + string.Join("&", query) + "'", true);
        }

        #endregion

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string delIds = hdnDelIds.Value;
            if (!string.IsNullOrEmpty(delIds))
            {
                Cars.Instance.DeleteCabmodels(delIds);
            }

            int addCount = DataConvert.SafeInt(hdnAddCount.Value);

            if (addCount > 0)
            {
                for (int i = 1; i <= addCount; i++)
                {
                    string cabmodelname = Request["txtCabmodelName" + i];
                    int brandid = DataConvert.SafeInt(Request["ddlBrand" + i]);
                    string pailiang = Request["txtPailiang" + i];
                    string nianfen = Request["txtNianfen" + i];
                    string imgpath = Request["hdnImgpath" + i];

                    if (!string.IsNullOrEmpty(cabmodelname) && brandid > 0)
                    {
                        CabmodelInfo entity = new CabmodelInfo
                        {
                            BrandID = brandid,
                            CabmodelName = cabmodelname,
                            NameIndex = StrHelper.ConvertE(cabmodelname).Substring(0, 1).ToUpper(),
                            Pailiang = pailiang,
                            Nianfen = nianfen,
                            Imgpath = imgpath
                        };
                        Cars.Instance.AddCabmodel(entity);
                    }
                }
            }

            foreach (RepeaterItem item in rptCabmodel.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    System.Web.UI.HtmlControls.HtmlInputText txtCabmodelName = (System.Web.UI.HtmlControls.HtmlInputText)item.FindControl("txtCabmodelName");
                    System.Web.UI.WebControls.DropDownList ddlBrand = (System.Web.UI.WebControls.DropDownList)item.FindControl("ddlBrand");
                    System.Web.UI.HtmlControls.HtmlInputText txtPailiang = (System.Web.UI.HtmlControls.HtmlInputText)item.FindControl("txtPailiang");
                    System.Web.UI.HtmlControls.HtmlInputText txtNianfen = (System.Web.UI.HtmlControls.HtmlInputText)item.FindControl("txtNianfen");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnID = (System.Web.UI.HtmlControls.HtmlInputHidden)item.FindControl("hdnID");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnImgpath = (System.Web.UI.HtmlControls.HtmlInputHidden)item.FindControl("hdnImgpath");
                    if (hdnID != null)
                    {
                        int id = DataConvert.SafeInt(hdnID.Value);
                        if (id > 0)
                        {
                            CabmodelInfo entity = Cars.Instance.GetCabmodel(id, true);
                            if (entity != null)
                            {
                                entity.CabmodelName = txtCabmodelName.Value;
                                entity.BrandID = DataConvert.SafeInt(ddlBrand.SelectedValue);
                                entity.NameIndex = StrHelper.ConvertE(entity.CabmodelName).Substring(0, 1).ToUpper();
                                entity.Pailiang = txtPailiang.Value;
                                entity.Nianfen = txtNianfen.Value;
                                entity.Imgpath = hdnImgpath.Value;
                                Cars.Instance.UpdateCabmodel(entity);
                            }
                        }
                    }
                }
            }

            Cars.Instance.ReloadCabmodelListCache();

            WriteSuccessMessage("保存成功！", "数据已经成功保存！", string.IsNullOrEmpty(FromUrl) ? "~/car/cabmodelmg.aspx" : FromUrl);
        }
    }
}