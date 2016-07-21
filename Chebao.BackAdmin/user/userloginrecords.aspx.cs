using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;

namespace Chebao.BackAdmin.user
{
    public partial class userloginrecords : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            if (ChebaoContext.Current.AdminUser.UserRole != Components.UserRoleType.管理员 || !CheckModulePower("用户管理"))
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
                int userid = GetInt("id");
                if (userid > 0)
                {
                    int pageindex = GetInt("page", 1);
                    if (pageindex < 1)
                    {
                        pageindex = 1;
                    }
                    int pagesize = GetInt("pagesize", 10);
                    int total = 0;
                    List<LoginRecordInfo> list = Admins.Instance.GetLoginRecords(userid);
                    list = list.OrderByDescending(l=>l.ID).ToList();

                    total = list.Count();
                    list = list.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<LoginRecordInfo>();
                    rpadmin.DataSource = list;
                    rpadmin.DataBind();
                    search_fy.RecordCount = total;
                    search_fy.PageSize = pagesize;
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(FromUrl);
        }
    }
}