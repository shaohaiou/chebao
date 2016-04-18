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
    public partial class brandmg : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            if (ChebaoContext.Current.AdminUser.UserRole != Components.UserRoleType.管理员 || !CheckModulePower("品牌管理"))
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
            int pageindex = GetInt("page", 1);
            if (pageindex < 1)
            {
                pageindex = 1;
            }
            int pagesize = GetInt("pagesize", 10);
            int total = 0;

            List<BrandInfo> brandlist = Cars.Instance.GetBrandList(true);
            brandlist = brandlist.OrderBy(l => l.NameIndex).ToList();
            total = brandlist.Count();
            brandlist = brandlist.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<BrandInfo>();

            rptBrand.DataSource = brandlist;
            rptBrand.DataBind();


            search_fy.RecordCount = total;
            search_fy.PageSize = pagesize;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string delIds = hdnDelIds.Value;
            if (!string.IsNullOrEmpty(delIds))
            {
                Cars.Instance.DeleteBrands(delIds);
            }

            int addCount = DataConvert.SafeInt(hdnAddCount.Value);

            if (addCount > 0)
            {
                for (int i = 1; i <= addCount; i++)
                {
                    string brandname = Request["txtBrandName" + i];
                    string imgpath = Request["hdnImgpath" + i];
                    if (!string.IsNullOrEmpty(brandname))
                    {
                        BrandInfo entity = new BrandInfo
                        {
                             BrandName = brandname,
                             NameIndex = StrHelper.ConvertE(brandname).Substring(0, 1).ToUpper(),
                             Imgpath = imgpath
                        };
                        Cars.Instance.AddBrand(entity);
                    }
                }
            }

            foreach (RepeaterItem item in rptBrand.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    System.Web.UI.HtmlControls.HtmlInputText txtBrandName = (System.Web.UI.HtmlControls.HtmlInputText)item.FindControl("txtBrandName");
                    System.Web.UI.HtmlControls.HtmlInputText txtNameIndex = (System.Web.UI.HtmlControls.HtmlInputText)item.FindControl("txtNameIndex");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnImgpath = (System.Web.UI.HtmlControls.HtmlInputHidden)item.FindControl("hdnImgpath");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnID = (System.Web.UI.HtmlControls.HtmlInputHidden)item.FindControl("hdnID");
                    if (hdnID != null)
                    {
                        int id = DataConvert.SafeInt(hdnID.Value);
                        if (id > 0)
                        {
                            BrandInfo entity = Cars.Instance.GetBrand(id, true);
                            if (entity != null)
                            {
                                entity.BrandName = txtBrandName.Value;
                                entity.NameIndex = txtNameIndex.Value.ToString().ToUpper();
                                entity.Imgpath = hdnImgpath.Value;
                                Cars.Instance.UpdateBrand(entity);
                            }
                        }
                    }
                }
            }

            Cars.Instance.ReloadBrandListCache();

            WriteSuccessMessage("保存成功！", "数据已经成功保存！", string.IsNullOrEmpty(FromUrl) ? "~/car/brandmg.aspx" : FromUrl);
        }
    }
}