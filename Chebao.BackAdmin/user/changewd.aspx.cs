using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using Chebao.Tools;
using System.Text;

namespace Chebao.BackAdmin.user
{
    public partial class changewd : AdminBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                bool result = Admins.Instance.ChangePassword(ChebaoContext.Current.AdminUserID, EncryptString.MD5(TxtOldPassword.Text), EncryptString.MD5(TxtNewUserPassword.Text));
                if (!result)
                {
                    lerrorMes.Text = "原密码错误";
                    return;
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("<span class=\"dalv\">密码修改成功,请使用新密码登录！</span><br />");
                WriteMessage("~/message/showmessage.aspx", "保存成功！", sb.ToString(), "", "/logout.aspx");
                
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