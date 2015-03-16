using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chebao.Tools;

namespace Chebao.Components
{
    public class Cars
    {
        #region 单例
        private static object sync_creater = new object();

        private static Cars _instance;
        public static Cars Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync_creater)
                    {
                        if (_instance == null)
                            _instance = new Cars();
                    }
                }
                return _instance;
            }
        }

        #endregion

        System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();

        #region 品牌管理

        /// <summary>
        /// 获取品牌列表
        /// </summary>
        /// <param name="fromCache"></param>
        /// <returns></returns>
        public List<BrandInfo> GetBrandList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetBrandList();

            string key = GlobalKey.BRAND_LIST;
            List<BrandInfo> list = MangaCache.Get(key) as List<BrandInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetBrandList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        /// <summary>
        /// 删除品牌
        /// </summary>
        /// <param name="ids"></param>
        public void DeleteBrands(string ids)
        {
            CommonDataProvider.Instance().DeleteBrands(ids);
        }

        public int AddBrand(BrandInfo entity)
        {
            return CommonDataProvider.Instance().AddBrand(entity);
        }

        public BrandInfo GetBrand(int id, bool fromCache = false)
        {
            List<BrandInfo> list = GetBrandList(fromCache);
            return list.Find(b => b.ID == id);
        }

        public void UpdateBrand(BrandInfo entity)
        {
            CommonDataProvider.Instance().UpdateBrand(entity);
        }

        public void ReloadBrandListCache()
        {
            string key = GlobalKey.BRAND_LIST;
            MangaCache.Remove(key);

            GetBrandList(true);
        }

        #endregion

        #region 车型管理

        public List<CabmodelInfo> GetCabmodelList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetCabmodelList();

            string key = GlobalKey.CABMODEL_LIST;
            List<CabmodelInfo> list = MangaCache.Get(key) as List<CabmodelInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetCabmodelList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public void DeleteCabmodels(string ids)
        {
            CommonDataProvider.Instance().DeleteCabmodels(ids);
        }

        public int AddCabmodel(CabmodelInfo entity)
        {
            return CommonDataProvider.Instance().AddCabmodel(entity);
        }

        public CabmodelInfo GetCabmodel(int id, bool fromCache = false)
        {
            List<CabmodelInfo> list = GetCabmodelList(fromCache);
            return list.Find(b => b.ID == id);
        }

        public void UpdateCabmodel(CabmodelInfo entity)
        {
            CommonDataProvider.Instance().UpdateCabmodel(entity);
        }

        public void ReloadCabmodelListCache()
        {
            string key = GlobalKey.CABMODEL_LIST;
            MangaCache.Remove(key);

            GetCabmodelList(true);
        }

        #endregion

        #region 产品管理

        public List<ProductInfo> GetProductList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetProductList();

            string key = GlobalKey.PRODUCT_LIST;
            List<ProductInfo> list = MangaCache.Get(key) as List<ProductInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetProductList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public void DeletetProducts(string ids)
        {
            CommonDataProvider.Instance().DeleteProducts(ids);
        }

        public int AddProduct(ProductInfo entity)
        {
            return CommonDataProvider.Instance().AddProduct(entity);
        }

        public ProductInfo GetProduct(int id, bool fromCache = false)
        {
            List<ProductInfo> list = GetProductList(fromCache);
            return list.Find(b => b.ID == id);
        }

        public void UpdateProduct(ProductInfo entity)
        {
            CommonDataProvider.Instance().UpdateProduct(entity);
        }

        public void ReloadProductListCache()
        {
            string key = GlobalKey.PRODUCT_LIST;
            MangaCache.Remove(key);

            GetProductList(true);
        }

        /// <summary>
        /// 更新产品库存
        /// </summary>
        public void RefreshProductStock()
        {
            List<ProductInfo> plist = GetProductList(true);
            string url = "http://yd.lamda.us/admin/k1.asp?id=fdskjgbdsfjbg56514zfhg";
            if (!plist.Exists(p => p.Stock > 0))
            {
                url = "http://yd.lamda.us/admin/k1.asp?id=fdskjgbdsfjbg56514zfhg&t=all";
            }
            string strRemoteProducts = Http.GetPage(url);
            if (!string.IsNullOrEmpty(strRemoteProducts))
            {
                try
                {
                    List<RemoteProductInfo> rplist = new List<RemoteProductInfo>();
                    foreach (string strp in strRemoteProducts.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string[] ps = strp.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        rplist.Add(new RemoteProductInfo()
                        {
                            ModelNumber = ps[0],
                            Stock = DataConvert.SafeInt(ps[1])
                        });
                    }

                    foreach (RemoteProductInfo rp in rplist)
                    {
                        if (plist.Exists(p => p.ModelNumber == rp.ModelNumber))
                        {
                            ProductInfo pinfo = plist.Find(p => p.ModelNumber == rp.ModelNumber);
                            pinfo.Stock = DataConvert.SafeInt(rp.Stock);
                            UpdateProduct(pinfo);
                        }
                    }
                }
                catch { }
            }
        }

        private class RemoteProductInfo
        {
            public string ModelNumber { get; set; }

            public int Stock { get; set; }
        }

        #endregion

        #region 购物车管理

        private static object sync_shoppingtrolley = new object();

        public List<ShoppingTrolleyInfo> GetShoppingTrolleyByUserID(int userid)
        {
            string key = GlobalKey.SHOPPINGTROLLEY_LIST + "_" + userid;
            List<ShoppingTrolleyInfo> list = MangaCache.Get(key) as List<ShoppingTrolleyInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetShoppingTrolleyByUserID(userid);
                MangaCache.Max(key, list);
            }
            return list;
        }

        public int AddShoppingTrolley(ShoppingTrolleyInfo entity)
        {
            lock (sync_shoppingtrolley)
            {
                List<ShoppingTrolleyInfo> list = GetShoppingTrolleyByUserID(entity.UserID);
                if (list.Exists(s => s.ProductID == entity.ProductID))
                {
                    entity.ID = list.Find(s => s.ProductID == entity.ProductID).ID;
                    entity.Amount += list.Find(s => s.ProductID == entity.ProductID).Amount;
                    return CommonDataProvider.Instance().UpdateShoppingTrolley(entity);
                }
                else
                    return CommonDataProvider.Instance().AddShoppingTrolley(entity);
            }
        }

        public void DeleteShoppingTrolley(string ids, int userid)
        {
            CommonDataProvider.Instance().DeleteShoppingTrolley(ids, userid);
        }

        public void ReloadShoppingTrolley(int userid)
        {
            string key = GlobalKey.SHOPPINGTROLLEY_LIST + "_" + userid;
            MangaCache.Remove(key);
            GetShoppingTrolleyByUserID(userid);
        }

        #endregion

        #region 订单管理

        private static object sync_order = new object();

        public string AddOrder(OrderInfo entity)
        {
            lock (sync_order)
            {
                string checkstock = string.Empty;
                List<OrderInfo> orderall = GetOrderList(true);
                foreach (OrderProductInfo op in entity.OrderProducts)
                {
                    ProductInfo pinfo = GetProduct(op.ProductID, true);
                    int stock = pinfo.Stock;
                    if (orderall.Exists(o => o.OrderStatus == OrderStatus.未处理 && o.OrderProducts != null && o.OrderProducts.Exists(p => p.ProductID == entity.ID)))
                    {
                        int amount = 0;
                        List<OrderInfo> orderlist = orderall.FindAll(o => o.OrderStatus == OrderStatus.未处理 && o.OrderProducts != null && o.OrderProducts.Exists(p => p.ProductID == pinfo.ID));
                        orderlist.ForEach(delegate(OrderInfo o)
                        {
                            amount += o.OrderProducts.FindAll(p => p.ProductID == pinfo.ID).Sum(p => p.Amount);
                        });
                        stock -= amount;
                        if (stock < 0)
                            stock = 0;
                    }
                    if (stock < op.Amount)
                    {
                        checkstock = "宝贝 " + op.ProductName + " 的库存只有" + stock + "，请改数量后在下单。";
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(checkstock))
                {
                    return checkstock;
                }

                CommonDataProvider.Instance().AddOrder(entity);
                ReloadOrder();
                if (entity.OrderProducts.Exists(p => p.SID > 0))
                {
                    DeleteShoppingTrolley(string.Join(",", entity.OrderProducts.FindAll(p => p.SID > 0).ToList().Select(p => p.SID.ToString()).ToList()), entity.UserID);
                    ReloadShoppingTrolley(entity.UserID);
                }

                return string.Empty;
            }
        }

        public OrderInfo GetOrder(int id, bool fromCache = false)
        {
            List<OrderInfo> list = GetOrderList(fromCache);
            return list.Find(l => l.ID == id);
        }

        public List<OrderInfo> GetOrderList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetOrderList();

            string key = GlobalKey.ORDER_LIST;
            List<OrderInfo> list = MangaCache.Get(key) as List<OrderInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetOrderList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public void ReloadOrder()
        {
            string key = GlobalKey.ORDER_LIST;
            MangaCache.Remove(key);
            GetOrderList(true);
        }

        public void UpdateOrderStatus(string ids, OrderStatus status)
        {
            CommonDataProvider.Instance().UpdateOrderStatus(ids, status);
        }

        #endregion

        #region 反馈有奖

        public void AddMessageBoard(MessageBoardInfo entity)
        {
            CommonDataProvider.Instance().AddMessageBoard(entity);
        }

        public List<MessageBoardInfo> GetMessageBoardList()
        {
            return CommonDataProvider.Instance().GetMessageBoardList();
        }

        public MessageBoardInfo GetMessageBoard(int id)
        {
            return CommonDataProvider.Instance().GetMessageBoard(id);
        }

        #endregion

        #region 地区管理

        public List<ProvinceInfo> GetProvinceList(bool fromCache)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetProvinceList();

            string key = GlobalKey.PROVINCE_LIST;
            List<ProvinceInfo> list = MangaCache.Get(key) as List<ProvinceInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetProvinceList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public void ReloadProvince()
        {
            string key = GlobalKey.PROVINCE_LIST;
            MangaCache.Remove(key);
            GetProvinceList(true);
        }

        public List<CityInfo> GetCityList(bool fromCache)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetCityList();

            string key = GlobalKey.CITY_LIST;
            List<CityInfo> list = MangaCache.Get(key) as List<CityInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetCityList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public void ReloadCity()
        {
            string key = GlobalKey.CITY_LIST;
            MangaCache.Remove(key);
            GetCityList(true);
        }

        public List<DistrictInfo> GetDistrictList(bool fromCache)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetDistrictList();

            string key = GlobalKey.DISTRICT_LIST;
            List<DistrictInfo> list = MangaCache.Get(key) as List<DistrictInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetDistrictList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public void ReloadDistrict()
        {
            string key = GlobalKey.DISTRICT_LIST;
            MangaCache.Remove(key);
            GetDistrictList(true);
        }

        #endregion

        #region 系统设置

        public void ExecuteSql(string sql)
        {
            CommonDataProvider.Instance().ExecuteSql(sql);
        }

        #endregion
    }
}
