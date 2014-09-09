using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;

namespace Chebao.BackAdmin
{
    public partial class left : AdminBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AdminInfo admin = ChebaoContext.Current.AdminUser;
            if (!admin.Administrator)
            {
                index_page.Visible = false;
                user_main.Visible = false;
                car_main.Visible = false;
                car_main1.Visible = false;
                global_main.Visible = false;
                if (Admin.UserRole == Components.UserRoleType.管理员)
                {
                    user_main.Visible = true;
                }
            }
        }

        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
        }
    }
}