using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using System.Text;
using Chebao.Tools;

namespace Chebao.BackAdmin.user
{
    public partial class adminedit : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            txtMobile.Text = Admin.Mobile;
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            AdminInfo entity = ChebaoContext.Current.AdminUser;
            if (entity != null)
            {
                entity.Mobile = txtMobile.Text;

                Admins.Instance.UpdateAdmin(entity);

                Admin = entity;

                StringBuilder sb = new StringBuilder();
                sb.Append("<span class=\"dalv\">信息保存成功！</span><br />");
                WriteMessage("~/message/showmessage.aspx", "保存成功！", sb.ToString(), "", string.IsNullOrEmpty(FromUrl) ? "~/user/adminedit.aspx" : FromUrl);
            }
        }
    }

}