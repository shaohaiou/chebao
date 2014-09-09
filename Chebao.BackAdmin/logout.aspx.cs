using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using Chebao.Tools;

namespace Chebao.BackAdmin
{
    public partial class logout : AdminBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session[GlobalKey.SESSION_ADMIN] = null;
            ManageCookies.RemoveCookie(GlobalKey.SESSION_ADMIN);
            ResponseRedirect("index.aspx");
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