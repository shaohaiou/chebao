using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;

namespace Chebao.BackAdmin
{
    public partial class index : AdminBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            if (Admin.UserRole == Components.UserRoleType.普通用户)
            {
                Response.Redirect("~/product/products.aspx");
            }
        }
    }
}