using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;

namespace Chebao.BackAdmin.useradmin
{
    public partial class myfeedbackview : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
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

            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(FromUrl);
        }
    }
}