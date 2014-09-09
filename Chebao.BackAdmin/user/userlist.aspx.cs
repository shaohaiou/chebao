using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Tools;
using Chebao.Components;

namespace Chebao.BackAdmin.user
{
    public partial class userlist : AdminBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (WebHelper.GetString("action") == "del")
                {
                    Admins.Instance.DeleteAdmin(WebHelper.GetInt("id"));
                    ResponseRedirect(FromUrl);
                }
                else
                {
                    int pageindex = GetInt("page", 1);
                    if (pageindex < 1)
                    {
                        pageindex = 1;
                    }
                    int pagesize = GetInt("pagesize", 10);
                    int total = 0;
                    List<AdminInfo> adminlist = Admins.Instance.GetUsers();
                    if (!string.IsNullOrEmpty(GetString("username")))
                        adminlist = adminlist.FindAll(l => l.UserName.IndexOf(GetString("username")) >= 0);

                    total = adminlist.Count();
                    adminlist = adminlist.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<AdminInfo>();
                    rpadmin.DataSource = adminlist;
                    rpadmin.DataBind();
                    search_fy.RecordCount = total;
                    search_fy.PageSize = pagesize;

                    if (!string.IsNullOrEmpty(GetString("username")))
                        txtUserName.Text = GetString("username");
                }
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            Response.Redirect("userlist.aspx?username=" + txtUserName.Text);
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
    }
}