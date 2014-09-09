using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using System.Text;

namespace Chebao.BackAdmin.global
{
    public partial class sqlexecute : AdminBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void btnSubmit_Click(object sender, EventArgs e)
        {
            Cars.Instance.ExecuteSql(txtsql.Value);

            StringBuilder sb = new StringBuilder();
            sb.Append("<span class=\"dalv\">执行完成！</span><br />");
            WriteMessage("~/message/showmessage.aspx", "执行完成！", sb.ToString(), "", string.IsNullOrEmpty(FromUrl) ? "~/global/sqlexecute.aspx" : FromUrl);
        }
    }
}