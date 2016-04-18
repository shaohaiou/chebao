using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;

namespace Chebao.BackAdmin.order
{
    public partial class syncfailedmg : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            if (ChebaoContext.Current.AdminUser.UserRole != Components.UserRoleType.管理员 || !CheckModulePower("同步失败记录"))
            {
                Response.Clear();
                Response.Write("您没有权限操作！");
                Response.End();
                return;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (GetString("action") == "sync")
            {
                Cars.Instance.Resync(GetString("ids"));

                ResponseRedirect(FromUrl);
                Response.End();
            }
            else if (GetString("action") == "del")
            {
                Cars.Instance.DeleteSyncfailed(GetString("ids"));

                ResponseRedirect(FromUrl);
                Response.End();
            }
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
            int pagesize = search_fy.PageSize;
            int total = 0;

            List<SyncfailedInfo> list = Cars.Instance.GetSyncfailedList();
            total = list.Count();
            list = list.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<SyncfailedInfo>();

            rptData.DataSource = list;
            rptData.DataBind();

            search_fy.RecordCount = total;
            search_fy.PageSize = pagesize;
            search_fy.CustomInfoHTML = string.Format("<span style=\"line-height: 34px;\">总记录数：{0} 总页数：{1}</span>", total, search_fy.PageCount);
        }

        protected void btnSyncfailed_Click(object sender, EventArgs e)
        {
            Cars.Instance.Resync(hdnIds.Value);
            ResponseRedirect(CurrentUrl);
        }
    }
}