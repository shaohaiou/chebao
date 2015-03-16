using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Chebao.Components;
using Chebao.Tools;
using System.Web.SessionState;

namespace Chebao.BackAdmin
{
    /// <summary>
    /// checkadmin 的摘要说明
    /// </summary>
    public class remoteaction : IHttpHandler, IRequiresSessionState
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

            //如果通过验证
            string methodName = WebHelper.GetString("action");//获取请求类型
            switch (methodName)
            {
                case "deleteshoppingtrolley":
                    DeleteShoppingTrolley();
                    break;
                case "addshoppingtrolley":
                    AddShoppingTrolley();
                    break;
                case "buyproduct":
                    BuyProduct();
                    break;
                default:
                    result = string.Format(result, "fail", "非法操作");
                    break;
            }
            Response.Clear();
            Response.Write(result);
            Response.End();
        }

        private void DeleteShoppingTrolley()
        {
            if (ChebaoContext.Current.AdminUser == null)
            {
                result = string.Format(result, "fail", "非法用户");
                return;
            }
            string ids = WebHelper.GetString("ids");
            Cars.Instance.DeleteShoppingTrolley(ids, ChebaoContext.Current.AdminUserID);
            Cars.Instance.ReloadShoppingTrolley(ChebaoContext.Current.AdminUserID);
            result = string.Format(result, "success", "");
        }

        private void AddShoppingTrolley()
        {
            if (ChebaoContext.Current.AdminUser == null)
            {
                result = string.Format(result, "fail", "非法用户");
                return;
            }
            int pid = WebHelper.GetInt("pid");
            int amount = WebHelper.GetInt("amount");
            ShoppingTrolleyInfo entity = new ShoppingTrolleyInfo()
            {
                Amount = amount,
                ProductID = pid,
                UserID = ChebaoContext.Current.AdminUserID
            };
            if (Cars.Instance.AddShoppingTrolley(entity) > 0)
            {
                Cars.Instance.ReloadShoppingTrolley(entity.UserID);
                string key = GlobalKey.SHOPPINGTROLLEYADD_KEY + "_" + ChebaoContext.Current.AdminUserID;
                MangaCache.Add(key, entity, 5 * 60);
                result = string.Format(result, "success", "");
            }
            else
            {
                result = string.Format(result, "fail", "执行失败，请刷新页面重新提交");
            }
        }

        private void BuyProduct()
        {
            if (ChebaoContext.Current.AdminUser == null)
            {
                result = string.Format(result, "fail", "非法用户");
                return;
            }
            int pid = WebHelper.GetInt("pid");
            int amount = WebHelper.GetInt("amount");
            List<OrderProductInfo> listOrderProduct = new List<OrderProductInfo>();
            ProductInfo pinfo = Cars.Instance.GetProduct(pid, true);
            if(pinfo != null)
            {
                try
                {
                    OrderProductInfo oinfo = new OrderProductInfo()
                    {
                        SID = 0,
                        ProductID = pid,
                        ProductName = pinfo.Name,
                        ModelNumber = pinfo.ModelNumber,
                        OEModelNumber = pinfo.OEModelNumber,
                        Standard = pinfo.Standard,
                        ProductPic = pinfo.Pic,
                        Introduce = string.Format("Lamda型号：{0} 规格：{1}", pinfo.ModelNumber, pinfo.Standard),
                        Price = DataConvert.SafeDecimal(pinfo.Price.StartsWith("¥") ? pinfo.Price.Substring(1) : pinfo.Price),
                        Amount = amount,
                        Remark = string.Empty
                    };

                    listOrderProduct.Add(oinfo);

                    string key = GlobalKey.ORDERPRODUCT_LIST + "_" + ChebaoContext.Current.AdminUserID;
                    MangaCache.Add(key, listOrderProduct);

                    result = string.Format(result, "success", "");
                }
                catch
                {
                    result = string.Format(result, "fail", "发生错误，请刷新页面重新提交");
                }
            }
            else
            {
                result = string.Format(result, "fail", "执行失败，请刷新页面重新提交");
            }
        }
    }
}