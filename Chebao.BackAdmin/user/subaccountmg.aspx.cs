using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;

namespace Chebao.BackAdmin.user
{
    public partial class subaccountmg : AdminBase
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

        private List<AdminInfo> _userlist = null;
        public List<AdminInfo> UserList
        {
            get
            {
                if (_userlist == null)
                    _userlist = Admins.Instance.GetUsers();
                return _userlist;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (GetString("action") == "del")
                {
                    Admins.Instance.DeleteAdmin(GetInt("id"));
                    ResponseRedirect(FromUrl);
                }
                else if(GetInt("id") > 0)
                {
                    int pageindex = GetInt("page", 1);
                    if (pageindex < 1)
                    {
                        pageindex = 1;
                    }
                    int pagesize = GetInt("pagesize", 10);
                    int total = 0;
                    List<AdminInfo> adminlist = UserList.FindAll(a => a.ParentAccountID == GetInt("id"));

                    total = adminlist.Count();
                    adminlist = adminlist.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<AdminInfo>();
                    rpadmin.DataSource = adminlist;
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