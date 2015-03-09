using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;

namespace Chebao.BackAdmin.product
{
    public partial class addproductsucceed : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
        }

        private ShoppingTrolleyInfo _currentshoppingtrolley = null;
        public ShoppingTrolleyInfo CurrentShoppingTrolley
        {
            get
            {
                if (_currentshoppingtrolley == null)
                {
                    string key = GlobalKey.SHOPPINGTROLLEYADD_KEY + "_" + AdminID;
                    _currentshoppingtrolley = MangaCache.Get(key) as ShoppingTrolleyInfo;
                }
                return _currentshoppingtrolley;
            }
        }

        private ProductInfo _currentproduct = null;
        public ProductInfo CurrentProduct
        {
            get
            {
                if (_currentproduct == null && CurrentShoppingTrolley != null)
                {
                    List<ProductInfo> listproduct = Cars.Instance.GetProductList(true);
                    _currentproduct = listproduct.Find(p => p.ID == CurrentShoppingTrolley.ProductID);
                }

                return _currentproduct;
            }
        }

        public string ShoppingTrolleyCount
        {
            get
            {
                string result = string.Empty;

                List<ShoppingTrolleyInfo> list = Cars.Instance.GetShoppingTrolleyByUserID(AdminID);
                if (list.Count > 0)
                    result = list.Count.ToString();

                return result;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}