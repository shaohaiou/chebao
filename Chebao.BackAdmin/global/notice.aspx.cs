using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;

namespace Chebao.BackAdmin.global
{
    public partial class notice : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
        }

        private SitesettingInfo sitesetting = null;
        public SitesettingInfo Sitesetting
        {
            get
            {
                if (sitesetting == null)
                    sitesetting = Sitesettings.Instance.GetSitesetting(true);
                return sitesetting;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}