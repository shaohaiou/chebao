using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;

namespace Chebao.BackAdmin.user
{
    public partial class subaccountlist : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            if (Admin.ParentAccountID > 0 || Admin.IsAddAccount == 0)
            {
                WriteErrorMessage("操作出错！", "您没有子用户操作权限！", "~/product/products.aspx");
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
                if (GetString("action") == "del" && GetInt("id") > 0)
                {
                    int id = GetInt("id");
                    if (UserList.Exists(l => l.ID == id && l.ParentAccountID == AdminID))
                    {
                        Admins.Instance.DeleteAdmin(id);
                        Response.Redirect("~/user/subaccountlist.aspx");
                        Response.End();
                    }
                    else
                    {
                        WriteMessage("~/message/showclientmessage.aspx", "操作出错！", "该用户不是您的子用户！", "", "/user/subaccountlist.aspx");
                    }
                }
                LoadData();
            }
        }

        private void LoadData()
        {
            rptSubAccount.DataSource = UserList.FindAll(l=>l.ParentAccountID == AdminID);
            rptSubAccount.DataBind();
        }
    }
}