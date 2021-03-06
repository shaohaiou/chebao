﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using Chebao.Tools;

namespace Chebao.BackAdmin.user
{
    public partial class adminlist : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            if (ChebaoContext.Current.AdminUser.UserRole != Components.UserRoleType.管理员 || !CheckModulePower("管理员管理"))
            {
                Response.Clear();
                Response.Write("您没有权限操作！");
                Response.End();
                return;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
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
                List<AdminInfo> adminlist = Admins.Instance.GetAllAdmins();
                total = adminlist.Count();
                adminlist = adminlist.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<AdminInfo>();
                rpadmin.DataSource = adminlist;
                rpadmin.DataBind();
                search_fy.RecordCount = total;
                search_fy.PageSize = pagesize;
            }
        }
    }
}