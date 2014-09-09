using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;

namespace Chebao.BackAdmin.message
{
    public partial class showmessage : AdminBase
    {
        public string ReUrl = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ReUrl = ChebaoContext.Current.ReturnUrl;
                lTitle.Text = ChebaoContext.Current.MessageTitle;
                lContent.Text = ChebaoContext.Current.Message;
                hyreturn.NavigateUrl = ChebaoContext.Current.ReturnUrl;
            }
        }

        protected override void Check()
        {
            //if (!ChebaoContext.Current.AdminCheck)
            //{
            //    Response.Redirect("~/Login.aspx");
            //    return;
            //}
        }
    }
}