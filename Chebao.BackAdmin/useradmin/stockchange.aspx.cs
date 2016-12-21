using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using Chebao.Tools;

namespace Chebao.BackAdmin.useradmin
{
    public partial class stockchange : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            if (Admin.ParentAccountID > 0)
            {
                Response.Clear();
                Response.Write("您没有权限操作！");
                Response.End();
                return;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string key = GlobalKey.USERSTOCKCHANGEADD_KEY + "_" + GetString("t") + "_" + ChebaoContext.Current.AdminUserID;
            List<KeyValuePair<string, string>> userstockchangelist = MangaCache.Get(key) as List<KeyValuePair<string, string>>;
            if (userstockchangelist == null)
                userstockchangelist = new List<KeyValuePair<string, string>>();
            rptData.DataSource = userstockchangelist.Select(p => new { ModelNumber = p.Key, ProductMix = p.Value });
            rptData.DataBind();
        }

        protected string GetProductMix(string pstr)
        {
            string result = string.Empty;

            string[] plist = pstr.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string pm in plist)
            {
                result += string.Format("{0} × {1} <br />", pm.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0], pm.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[1]);
            }

            return result;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string key = GlobalKey.USERSTOCKCHANGEADD_KEY + "_" + GetString("t") + "_" + ChebaoContext.Current.AdminUserID;
            List<KeyValuePair<string, string>> userstockchangelist = MangaCache.Get(key) as List<KeyValuePair<string, string>>;
            if (userstockchangelist == null)
                userstockchangelist = new List<KeyValuePair<string, string>>();
            List<OrderProductInfo> listOrderProduct = new List<OrderProductInfo>();
            userstockchangelist.ForEach(delegate(KeyValuePair<string, string> kp) 
            {
                ProductInfo p = Cars.Instance.GetProduct(kp.Key, true);
                if (p != null)
                {
                    string[] plist = kp.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    OrderProductInfo oinfo = new OrderProductInfo()
                    {
                        ProductID = p.ID,
                        ProductName = p.Name,
                        ProductType = p.ProductType,
                        ModelNumber = p.ModelNumber
                    };
                    List<ProductMixInfo> ProductMixList = new List<ProductMixInfo>();
                    foreach (string pmstr in plist)
                    {
                        ProductMixInfo pm = new ProductMixInfo();
                        pm.Name = pmstr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];
                        pm.Amount = DataConvert.SafeInt(pmstr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[1]);

                        ProductMixList.Add(pm);
                    }
                    if (ProductMixList.Count > 0)
                    {
                        oinfo.ProductMixList = ProductMixList;
                        listOrderProduct.Add(oinfo);
                    }
                }
            });
            
            System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();

            UserStockChangeInfo entity = new UserStockChangeInfo() 
            {
                UserID = AdminID,
                UserName = AdminName,
                ParentUserID = Admin.ParentAccountID,
                Action = GetInt("t"),
                CheckStatus = 0,
                AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Remark = txtRemark.Value,
                SysRemark = GetInt("t") == 0 ? "盘库-出库" : "盘库-入库",
                OrderProductJson = json.Serialize(listOrderProduct)
            };

            Cars.Instance.AddUserStockChange(entity);
            Cars.Instance.ReloadUserStockChangeCache(entity.UserID);

            WriteSuccessMessage("操作成功", "成功提交", "stockmg.aspx");
        }
    }
}