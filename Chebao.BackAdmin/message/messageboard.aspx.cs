using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;

namespace Chebao.BackAdmin.message
{
    public partial class messageboard : AdminBase
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

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMessageTitle.Text.Trim()))
            {
                ClientScript.RegisterStartupScript(typeof(string), "aa", "alert(\"请填写简述信息\");", true);
                return;
            }
            if (string.IsNullOrEmpty(hdnMessageContent.Value.Trim()))
            {
                ClientScript.RegisterStartupScript(typeof(string), "aa", "alert(\"请填写详细描述信息\");", true);
                return;
            }

            MessageBoardInfo entity = new MessageBoardInfo() 
            { 
                UserID = AdminID,
                UserName = AdminName,
                LinkName = Admin.LinkName,
                Title = txtMessageTitle.Text,
                Content = hdnMessageContent.Value,
                AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            Cars.Instance.AddMessageBoard(entity);
            txtMessageTitle.Text = string.Empty;
            hdnMessageContent.Value = string.Empty;
            ClientScript.RegisterStartupScript(typeof(string), "aa", "alert(\"提交成功，感谢您的反馈！\");", true);
        }
    }
}