using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Chebao.Components;
using Chebao.Tools;

namespace Chebao.BackAdmin
{
    public class BackAdminActionHandler : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private HttpResponse Response;
        private HttpRequest Request;
        private string result = "{{\"Value\":\"{0}\",\"Msg\":\"{1}\"}}";

        public void ProcessRequest(HttpContext context)
        {
            Response = context.Response;
            Request = context.Request;

            if (!ChebaoContext.Current.AdminCheck)
            {
                result = string.Format(result, "fail", "您没有权限操作");
            }
            else
            {
                string action = WebHelper.GetString("action");

                if (action == "refreshbackadmincache")
                {
                    RefreshBackadminCache();
                }
                else
                {
                    result = string.Format(result, "fail", "非法操作");
                }
            }

            Response.Clear();
            Response.Write(result);
            Response.End();
        }

        private void RefreshBackadminCache()
        {
            try
            {

                result = string.Format(result, "success", "");
            }
            catch { result = string.Format(result, "fail", "执行失败"); }
        }
    }

}