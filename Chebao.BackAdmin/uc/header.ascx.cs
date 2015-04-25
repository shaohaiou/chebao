using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;

namespace Chebao.BackAdmin.uc
{
    public partial class header : System.Web.UI.UserControl
    {
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

        public AdminInfo Admin
        {
            get
            {
                return ChebaoContext.Current.AdminUser;
            }
        }

        public string CurrentTag { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}