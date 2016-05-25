using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using Chebao.Tools;

namespace Chebao.BackAdmin.product
{
    public partial class productview : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
        }

        private ProductInfo product = null;
        public ProductInfo Product
        {
            get
            {
                if (product == null)
                {
                    List<ProductInfo> productlist = Cars.Instance.GetProductList(true);
                    int id = GetInt("id");
                    if (id > 0)
                    {
                        product = productlist.Find(p=>p.ID == id);
                    }
                }
                return product;
            }
        }

        public CabmodelInfo SearchCabmodel
        {
            get
            {
                if (Session[GlobalKey.SEARCHCABMODELID] != null)
                {
                    int cabid = DataConvert.SafeInt(Session[GlobalKey.SEARCHCABMODELID]);
                    if (cabid > 0)
                    {
                        if(Product != null)
                        {
                            string[] cabmodels = Product.Cabmodels.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                            if (!cabmodels.Contains(cabid.ToString()))
                                return null;
                        }
                        return Cars.Instance.GetCabmodelList(true).Find(c=>c.ID == cabid);
                    }
                }
                return null;
            }
        }

        private List<CabmodelInfo> cabmodellist = null;
        public List<CabmodelInfo> CabmodelList
        {
            get
            {
                if (cabmodellist == null)
                {
                    cabmodellist = Cars.Instance.GetCabmodelList(true);
                }
                return cabmodellist;
            }
        }

        private bool? isshowintroduce;
        public bool IsShowIntroduce
        {
            get
            {
                //if (Admin.ParentAccountID > 0)
                //    return ParentAdmin.IsShowCabmodel > 0;
                //return Admin.IsShowCabmodel > 0;
                if (!isshowintroduce.HasValue)
                {
                    isshowintroduce = Admin.IsShowIntroduce > 0 && (ParentAdmin == null || ParentAdmin.IsShowIntroduce > 0);
                }
                return isshowintroduce.Value;
            }
        }

        private bool? isshowcabmodel;
        public bool IsShowCabmodel
        {
            get
            {
                //if (Admin.ParentAccountID > 0)
                //    return ParentAdmin.IsShowCabmodel > 0;
                //return Admin.IsShowCabmodel > 0;
                if (!isshowcabmodel.HasValue)
                {
                    isshowcabmodel = Admin.IsShowCabmodel > 0 && (ParentAdmin == null || ParentAdmin.IsShowCabmodel > 0);
                }
                return isshowcabmodel.Value;
            }
        }

        private bool? isshowprice;
        public bool IsShowPrice
        {
            get
            {
                //if (!isshowprice.HasValue)
                //{
                //    if (Admin.ParentAccountID > 0)
                //        isshowprice = ParentAdmin.IsShowPrice > 0;
                //    else
                //        isshowprice = Admin.IsShowPrice > 0;
                //}
                //return isshowprice.Value;
                if (!isshowprice.HasValue)
                {
                    isshowprice = Admin.IsShowPrice > 0 && (ParentAdmin == null || ParentAdmin.IsShowPrice > 0);
                }
                return isshowprice.Value;
            }
        }

        //public int Stock
        //{
        //    get
        //    {
        //        int result = 0;

        //        if(Product != null)
        //        {
        //            result = product.Stock;
        //            List<OrderInfo> orderall = Cars.Instance.GetOrderList(true);
        //            if (orderall.Exists(o => o.OrderStatus == OrderStatus.未处理 && o.OrderProducts != null && o.OrderProducts.Exists(p => p.ProductID == Product.ID)))
        //            {
        //                int amount = 0;
        //                List<OrderInfo> orderlist = orderall.FindAll(o => o.OrderStatus == OrderStatus.未处理 && o.OrderProducts != null && o.OrderProducts.Exists(p => p.ProductID == Product.ID));
        //                orderlist.ForEach(delegate(OrderInfo o) 
        //                {
        //                    amount += o.OrderProducts.FindAll(p => p.ProductID == Product.ID).Sum(p => p.Amount);
        //                });
        //                result -= amount;
        //                if (result < 0)
        //                    result = 0;
        //            }
        //        }

        //        return result;
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            if (Product == null)
            {
                WriteErrorMessage("非法操作", "非法产品ID", "~/product/products.aspx");
                Response.End();
                return;
            }
            if (!string.IsNullOrEmpty(Product.Pics))
            {
                rptPics.DataSource = Product.Pics.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList().Select(s => new KeyValuePair<string, string>(s, s.Replace("_s", "_c"))).ToList();
                rptPics.DataBind();
            }
            string[] cabmodels = Product.Cabmodels.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            rptCabmodels.DataSource = CabmodelList.FindAll(l => cabmodels.Contains(l.ID.ToString()));
            rptCabmodels.DataBind();

            List<ShoppingTrolleyInfo> listShoppingTrolley = Cars.Instance.GetShoppingTrolleyByUserID(AdminID);
            if (listShoppingTrolley.Count > 0)
            {
                List<ProductInfo> listAllProduct = Cars.Instance.GetProductList(true);
                string[] pids = listShoppingTrolley.OrderByDescending(s => s.ID).Select(s => s.ProductID + "|" + s.ID).ToArray();
                List<ProductInfo> listProdcutInShoppingTrolley = new List<ProductInfo>();
                for (int i = 0; i < pids.Length; i++)
                {
                    int pid = DataConvert.SafeInt(pids[i].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[0]);
                    int sid = DataConvert.SafeInt(pids[i].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[1]);
                    ProductInfo entity = listAllProduct.Find(p => p.ID == pid);
                    if (entity != null)
                    {
                        entity.SID = sid;
                        listProdcutInShoppingTrolley.Add(entity);
                    }
                }
                rptShoppingTrolley.DataSource = listProdcutInShoppingTrolley;
                rptShoppingTrolley.DataBind();
            }
        }

        protected string GetFirstPic()
        {
            if (!string.IsNullOrEmpty(Product.Pics))
            {
                if (Product.Pics.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Length > 0)
                {
                    return Product.Pics.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList().Select(s => new KeyValuePair<string, string>(s, s.Replace("_s", "_c"))).ToList()[0].Key;
                }
            }
            return string.Empty;
        }
        #region 购物车


        public string ShoppingTrolleyCount
        {
            get
            {
                string result = string.Empty;

                List<ShoppingTrolleyInfo> list = Cars.Instance.GetShoppingTrolleyByUserID(AdminID);
                List<ProductInfo> productlist = Cars.Instance.GetProductList(true);
                result = list.FindAll(l => productlist.Exists(p => p.ID == l.ProductID)).Count.ToString();

                return result;
            }
        }

        #endregion
    }
}