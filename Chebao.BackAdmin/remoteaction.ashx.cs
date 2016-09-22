using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Chebao.Components;
using Chebao.Tools;
using System.Web.SessionState;
using System.Threading;

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
        private HttpSessionState Session;
        private string result = "{{\"Value\":\"{0}\",\"Msg\":\"{1}\"}}";
        public CabmodelInfo SearchCabmodel
        {
            get
            {
                if (Session[GlobalKey.SEARCHCABMODELID] != null)
                {
                    int cabid = DataConvert.SafeInt(Session[GlobalKey.SEARCHCABMODELID]);
                    if (cabid > 0)
                    {
                        return Cars.Instance.GetCabmodelList(true).Find(c => c.ID == cabid);
                    }
                }
                return null;
            }
        }
        public void ProcessRequest(HttpContext context)
        {
            Response = context.Response;
            Request = context.Request;
            Session = context.Session;
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
                case "adduserstockchange":
                    AddUserStockChange();
                    break;
                case "buyproduct":
                    BuyProduct();
                    break;
                case "updateorderpic":
                    UpdateOrderPic();
                    break;
                case "reloaduserproductcache":
                    ReloadUserProductListCache();
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
            string productmix = WebHelper.GetString("productmix");
            ShoppingTrolleyInfo entity = new ShoppingTrolleyInfo()
            {
                Amount = amount,
                ProductID = pid,
                UserID = ChebaoContext.Current.AdminUserID,
                CabmodelStr = SearchCabmodel == null ? string.Empty : SearchCabmodel.CabmodelNameBind,
                ProductMix = productmix
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

        private void AddUserStockChange()
        {
            if (ChebaoContext.Current.AdminUser == null)
            {
                result = string.Format(result, "fail", "非法用户");
                return;
            }
            string mnumber = WebHelper.GetString("mnumber");
            string productmix = WebHelper.GetString("productmix");
            int t = WebHelper.GetInt("t");
            string key = GlobalKey.USERSTOCKCHANGEADD_KEY + "_" + t + "_" + ChebaoContext.Current.AdminUserID;
            List<KeyValuePair<string, string>> userstockchangelist = MangaCache.Get(key) as List<KeyValuePair<string, string>>;
            if (userstockchangelist == null) 
                userstockchangelist = new List<KeyValuePair<string, string>>();
            if (userstockchangelist.Exists(p => p.Key.ToLower() == mnumber.ToLower()))
                result = string.Format(result, "fail", "已添加了该产品，请勿重复添加");
            else if (userstockchangelist.Count >= 20)
                result = string.Format(result, "fail", "一次最多只能入库20个产品");
            else
            {
                userstockchangelist.Add(new KeyValuePair<string,string>(mnumber, productmix));
                MangaCache.Max(key, userstockchangelist);
                result = string.Format(result, "success", "");
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
                        Price = DataConvert.SafeDecimal(pinfo.Name.IndexOf("xsp") > 0 ? (pinfo.XSPPrice.StartsWith("¥") ? pinfo.XSPPrice.Substring(1) : pinfo.XSPPrice) : (pinfo.Price.StartsWith("¥") ? pinfo.Price.Substring(1) : pinfo.Price)),
                        //Price = DataConvert.SafeDecimal(pinfo.Price.StartsWith("¥") ? pinfo.Price.Substring(1) : pinfo.Price),
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

        private void UpdateOrderPic()
        {
            if (ChebaoContext.Current.AdminUser == null)
            {
                result = string.Format(result, "fail", "非法用户");
                return;
            }
            int id = WebHelper.GetInt("id");
            string src= WebHelper.GetString("src");
            string col = WebHelper.GetString("col");
            if (id > 0 && !string.IsNullOrEmpty(src) && !string.IsNullOrEmpty(col))
            {
                Cars.Instance.UpdateOrderPic(id, src, col);
                result = string.Format(result, "success", "");
            }
            else
            {
                result = string.Format(result, "fail", "参数错误");
            }
        }

        private void ReloadUserProductListCache()
        {
            if (ChebaoContext.Current.AdminUser == null)
            {
                result = string.Format(result, "fail", "非法用户");
                return;
            }
            result = string.Format(result, "success", "");
            return;
            try
            {
                string ids = WebHelper.GetString("userid");
                if (!string.IsNullOrEmpty(ids))
                {
                    foreach (string id in ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        AdminInfo user = Admins.Instance.GetAdmin(DataConvert.SafeInt(id));
                        if (user.ParentAccountID == 0)
                            Cars.Instance.ReloadUserProductListCache(user.ID);
                        else
                            Cars.Instance.ReloadUserProductListCache(user.ParentAccountID);
                    }
                    result = string.Format(result, "success", "");
                }
                else
                {
                    result = string.Format(result, "fail", "参数错误");
                }
            }
            catch (Exception ex)
            {
                ExpLog.Write(ex);
                result = string.Format(result, "fail", ex.Message);
            }
        }
    }
}