using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;

namespace Chebao.BackAdmin.message
{
    public partial class messageboardview : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            if (ChebaoContext.Current.AdminUser.UserRole != Components.UserRoleType.管理员 || !CheckModulePower("反馈有奖"))
            {
                Response.Clear();
                Response.Write("您没有权限操作！");
                Response.End();
                return;
            }
        }

        private MessageBoardInfo currentmessage = null;
        protected MessageBoardInfo CurrentMessage
        {
            get
            {
                if (currentmessage == null && GetInt("id") > 0)
                {
                    currentmessage = Cars.Instance.GetMessageBoard(GetInt("id"));
                }
                return currentmessage;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (GetInt("id") == 0 || CurrentMessage == null)
                {
                    WriteMessage("~/message/showmessage.aspx", "错误！", "非法记录！", "", FromUrl);
                    Response.End();
                    return;
                }

                txtReply.Text = CurrentMessage.Reply;
            }
        }

        private void FillData(MessageBoardInfo entity)
        {
            entity.Reply = txtReply.Text;
            entity.ReplyAdmin = AdminName;
            entity.ReplyAdminID = AdminID;
            entity.ReplyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            entity.Status = 1;
        }

        protected void btnReply_Click(object sender, EventArgs e)
        {
            FillData(CurrentMessage);

            Cars.Instance.ReplyMessageBoard(CurrentMessage);

            WriteSuccessMessage("操作完成！", "回复成功", string.IsNullOrEmpty(FromUrl) ? "~/message/messageboardmg.aspx" : FromUrl);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(FromUrl);
        }
    }
}