using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chebao.Tools;
using System.Collections.Specialized;
using System.Threading;

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

        public ProductInfo GetProduct(string modelnumber, bool fromCache = false)
        {
            List<ProductInfo> list = GetProductList(fromCache);
            return list.Find(b => b.ModelNumber.ToLower() == modelnumber.ToLower());
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

        private static object locker_refreshproductstock = new object();

        /// <summary>
        /// 更新产品库存
        /// </summary>
        public void RefreshProductStock(bool isall = false)
        {
            lock (locker_refreshproductstock)
            {
                List<ProductInfo> plist = GetProductList(true);
                string url = "http://yd.lamda.us/admin/k1.asp?id=fdskjgbdsfjbg56514zfhg";
                //if (!isall && plist.Exists(p => !string.IsNullOrEmpty(p.ProductMixStr) && !string.IsNullOrEmpty(p.StockLastUpdateTime)))
                //{
                //    DateTime lastupdatetime = plist.FindAll(p => !string.IsNullOrEmpty(p.ProductMixStr) && !string.IsNullOrEmpty(p.StockLastUpdateTime)).Max(p => DataConvert.SafeDate(p.StockLastUpdateTime));
                //    url = "http://yd.lamda.us/admin/k1.asp?id=fdskjgbdsfjbg56514zfhg&t=" + lastupdatetime.ToString("yyyy-MM-dd HH:mm:ss");
                //}
                string strRemoteProducts = Http.GetPage(url, true, "gb2312");
                if (!string.IsNullOrEmpty(strRemoteProducts) && strRemoteProducts.ToLower() != "no")
                {
                    try
                    {
                        List<RemoteProductInfo> rplist = new List<RemoteProductInfo>();
                        foreach (string strp in strRemoteProducts.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            string[] ps = strp.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            try
                            {
                                rplist.Add(new RemoteProductInfo()
                                {
                                    ModelNumber = ps[0],
                                    Stock = DataConvert.SafeInt(ps[1])
                                });
                            }
                            catch { }
                        }

                        List<string> deelModelNumber = new List<string>();
                        foreach (RemoteProductInfo rp in rplist)
                        {
                            if (deelModelNumber.Contains(rp.ModelNumber)) continue;
                            if (plist.Exists(p => rp.ModelNumber.StartsWith(p.ModelNumber)))
                            {
                                ProductInfo pinfo = plist.Find(p => rp.ModelNumber.StartsWith(p.ModelNumber));
                                if (string.IsNullOrEmpty(pinfo.ModelNumber.Trim())) continue;
                                deelModelNumber.AddRange(rplist.FindAll(p => p.ModelNumber.StartsWith(pinfo.ModelNumber)).Select(p => p.ModelNumber));
                                //if (string.IsNullOrEmpty(pinfo.ProductMixStr))
                                pinfo.ProductMixStr = string.Join("|", rplist.FindAll(p => p.ModelNumber.StartsWith(pinfo.ModelNumber)).Select(p => p.ModelNumber + "," + p.Stock));
                                //else
                                //{
                                //    List<KeyValuePair<string, int>> pm = new List<KeyValuePair<string, int>>();
                                //    pm.AddRange(pinfo.ProductMix);
                                //    foreach (RemoteProductInfo rpi in rplist.FindAll(p => p.ModelNumber.StartsWith(pinfo.ModelNumber)))
                                //    {
                                //        if (pm.Exists(p => p.Key == rpi.ModelNumber))
                                //            pm[pm.FindIndex(p => p.Key == rpi.ModelNumber)] = new KeyValuePair<string, int>(rpi.ModelNumber, rpi.Stock);
                                //        else
                                //            pm.Add(new KeyValuePair<string, int>(rpi.ModelNumber, rpi.Stock));
                                //    }

                                //    pinfo.ProductMixStr = string.Join("|", pm.Select(p => p.Key + "," + p.Value));
                                //}
                                pinfo.StockLastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                UpdateProduct(pinfo);
                            }
                        }

                        ReloadProductListCache();
                    }
                    catch (Exception ex)
                    {
                        ExpLog.Write(ex);
                    }
                }
            }
        }

        private class RemoteProductInfo
        {
            public string ModelNumber { get; set; }

            public int Stock { get; set; }
        }

        #region 根据当前用户获取产品分类的价格

        public string GetProductMixPrice(string pricestr, string xsppricestr, string mn, AdminInfo validAdmin)
        {
            decimal price = 0;
            DiscountStencilInfo discountinfo = null;
            if (validAdmin.DiscountStencilID > 0)
                discountinfo = Cars.Instance.GetDiscountStencil(validAdmin.DiscountStencilID, true);
            else if (validAdmin.ParentAccountID > 0)
            {
                AdminInfo parentadmin = Admins.Instance.GetAdmin(validAdmin.ParentAccountID);
                if (parentadmin.DiscountStencilID > 0)
                    discountinfo = Cars.Instance.GetDiscountStencil(parentadmin.DiscountStencilID, true);
                else
                    validAdmin = parentadmin;
            }
            if (discountinfo == null)
            {
                discountinfo = new DiscountStencilInfo(validAdmin);
            }
            price = GetDiscountPrice(pricestr, xsppricestr, mn, discountinfo);

            if (ChebaoContext.Current.AdminUser.ParentAccountID > 0)
                price = price + price * ChebaoContext.Current.AdminUser.SubDiscount / 100;
            return Math.Round(price, 2).ToString();
        }

        public decimal GetDiscountPrice(string pricestr, string xsppricestr, string mn, DiscountStencilInfo discountinfo)
        {
            decimal price = 0;
            decimal price_s = DataConvert.SafeDecimal(pricestr.StartsWith("¥") ? pricestr.Substring(1) : pricestr);
            string mnzk = mn.ToLower().Replace("w", string.Empty).Replace("f", string.Empty);
            if (mnzk.Contains("xsp"))
                price_s = DataConvert.SafeDecimal(xsppricestr.StartsWith("¥") ? xsppricestr.Substring(1) : xsppricestr);
            price = price_s;
            if (mnzk.Contains("mt"))
                price = (price_s + discountinfo.DiscountMTAdd) * discountinfo.DiscountMT / 10;
            else if (mnzk.Contains("m"))
                price = (price_s + discountinfo.DiscountMAdd) * discountinfo.DiscountM / 10;
            else if (mnzk.Contains("py"))
                price = (price_s + discountinfo.DiscountPYAdd) * discountinfo.DiscountPY / 10;
            else if (mnzk.Contains("y"))
                price = (price_s + discountinfo.DiscountYAdd) * discountinfo.DiscountY / 10;
            else if (mnzk.Contains("h"))
                price = (price_s + discountinfo.DiscountHAdd) * discountinfo.DiscountH / 10;
            else if (mnzk.Contains("xsp"))
                price = (price_s + discountinfo.DiscountXSPAdd) * discountinfo.DiscountXSP / 10;
            else if (mnzk.Contains("s") && !mn.ToLower().StartsWith("ls"))
                price = (price_s + discountinfo.DiscountSAdd) * discountinfo.DiscountS / 10;
            else if (mnzk.Contains("k"))
                price = (price_s + discountinfo.DiscountKAdd) * discountinfo.DiscountK / 10;
            else if (mnzk.Contains("p"))
                price = (price_s + discountinfo.DiscountPAdd) * discountinfo.DiscountP / 10;
            else if (mn.ToLower().StartsWith("ls"))
                price = (price_s + discountinfo.DiscountLSAdd) * discountinfo.DiscountLS / 10;
            else if (mn.ToLower().StartsWith("b"))
                price = (price_s + discountinfo.DiscountBAdd) * discountinfo.DiscountB / 10;
            if (mn.ToLower().IndexOf("w") >= 0)
                price = price + price_s * discountinfo.AdditemW / 10;
            if (mn.ToLower().IndexOf("f") >= 0)
                price = price + price_s * discountinfo.AdditemF / 10;

            return price;
        }

        #endregion

        #endregion

        #region 分销管理

        private static object sync_fxhelper = new object();

        /// <summary>
        /// 获取指定用户的产品及库存数据
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<ProductInfo> GetProductListByUser(int userid)
        {
            string key = GlobalKey.PRODUCT_LIST_USER + "_" + userid;
            List<ProductInfo> myproductlist = MangaCache.Get(key) as List<ProductInfo>;
            if (myproductlist == null)
            {
                lock (sync_fxhelper)
                {
                    myproductlist = MangaCache.Get(key) as List<ProductInfo>;
                    if (myproductlist == null)
                    {
                        DateTime timestart = DateTime.Parse("2016-6-1");
                        List<ProductInfo> productall = Cars.Instance.GetProductList(true);
                        List<OrderInfo> orderall = Cars.Instance.GetOrderList(true);
                        List<OrderInfo> myorderlist = orderall.FindAll(l => l.UserID == userid && l.OrderStatus == OrderStatus.已发货);
                        List<OrderInfo> myorderlisteffect = myorderlist.FindAll(l => DateTime.Parse(l.AddTime) > timestart);
                        List<OrderInfo> suborderlist = orderall.FindAll(l => l.OrderStatus != OrderStatus.已取消 && l.ParentID == userid);

                        myproductlist = productall.FindAll(p => myorderlist.Exists(m => m.OrderProducts.Exists(n => n.ProductID == p.ID)));
                        for (int i = 0; i < myproductlist.Count; i++)
                        {
                            List<KeyValuePair<string, int>> productmix = new List<KeyValuePair<string, int>>();
                            for (int j = 0; j < myproductlist[i].ProductMix.Count; j++)
                            {
                                int amountcount = myorderlisteffect.Sum(l => l.OrderProducts.Sum(p => p.ProductMixList.FindAll(m => m.Name == myproductlist[i].ProductMix[j].Key).Sum(m => m.Amount)));
                                int subamountcount = suborderlist.Sum(l => l.OrderProducts.Sum(p => p.ProductMixList.FindAll(m => m.Name == myproductlist[i].ProductMix[j].Key).Sum(m => m.Amount)));

                                productmix.Add(new KeyValuePair<string, int>(myproductlist[i].ProductMix[j].Key, amountcount - subamountcount));
                            }
                            myproductlist[i].UserProductMix = productmix;
                        }
                        MangaCache.Max(key, myproductlist);
                    }
                }
            }            

            return myproductlist;
        }

        public void ReloadProductListUserCache(int userid)
        {
            string key = GlobalKey.PRODUCT_LIST_USER + "_" + userid;
            MangaCache.Remove(key);
            GetProductListByUser(userid);
        }

        public void AddUserStockChange(UserStockChangeInfo entity)
        {
            CommonDataProvider.Instance().AddUserStockChange(entity);

            #region 用户产品库存更新
            
            foreach (OrderProductInfo opinfo in entity.OrderProducts)
            {
                UserProductInfo upinfo = GetUserProductInfo(entity.UserID,opinfo.ProductID,true);
                if (upinfo == null)
                {
                    upinfo = new UserProductInfo()
                    {
                        ProductID = opinfo.ProductID,
                        UserID = entity.UserID,
                        ProductMixStr = string.Join("|", opinfo.ProductMixList.Select(p => p.Name + "," + p.Amount))
                    };
                }
                else 
                {
                    List<KeyValuePair<string, int>> productmix = upinfo.ProductMix;
                    List<KeyValuePair<string, int>> pmlist = new List<KeyValuePair<string, int>>();
                    foreach (ProductMixInfo pm in opinfo.ProductMixList)
                    {
                        if (productmix.Exists(m => m.Key == pm.Name))
                        {
                            int amount = productmix.Find(m => m.Key == pm.Name).Value;
                            pmlist.Add(new KeyValuePair<string, int>(pm.Name, entity.Action == 0 ? (amount - pm.Amount) : (amount + pm.Amount)));
                        }
                        else
                        {
                            pmlist.Add(new KeyValuePair<string, int>(pm.Name, pm.Amount));
                        }
                    }
                    foreach (KeyValuePair<string, int> kvp in productmix)
                    {
                        if (!opinfo.ProductMixList.Exists(pm => pm.Name == kvp.Key))
                        {
                            pmlist.Add(new KeyValuePair<string, int>(kvp.Key, kvp.Value));
                        }
                    }
                    upinfo.ProductMixStr = string.Join("|", pmlist.Select(p => p.Key + "," + p.Value));
                }
                CommonDataProvider.Instance().AddUserProductInfo(upinfo);
            }

            ReloadUserProductListCache(entity.UserID);

            #endregion
        }

        public List<UserProductInfo> GetUserProductInfoList(int userid,bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetUserProductInfoList(userid);

            string key = GlobalKey.USERPRODUCT_LIST;
            List<UserProductInfo> list = MangaCache.Get(key) as List<UserProductInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetUserProductInfoList(userid);
                MangaCache.Max(key, list);
            }
            return list;
        }

        public void ReloadUserProductListCache(int userid)
        {
            string key = GlobalKey.USERPRODUCT_LIST + "_" + userid;
            MangaCache.Remove(key);
            GetUserProductInfoList(userid);
        }

        public UserProductInfo GetUserProductInfo(int userid, int productid, bool fromCache = false)
        {
            List<UserProductInfo> list = GetUserProductInfoList(userid,fromCache);
            return list.Find(l=>l.ProductID == productid);
        }

        /// <summary>
        /// 获取指定用户的盘库记录
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<UserStockChangeInfo> GetUserStockChangeList(int userid)
        { 
            string key = GlobalKey.USERSTOCKCHANGE_LIST + "_" + userid;

            List<UserStockChangeInfo> list = MangaCache.Get(key) as List<UserStockChangeInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetUserStockChangeList(userid);
                MangaCache.Max(key, list);
            }

            return list;
        }

        public List<UserStockChangeInfo> GetUserStockChangeList(int pageindex, int pagesize, UserStockChangeQuery query, out int total)
        {
            return CommonDataProvider.Instance().GetUserStockChangeList(pageindex, pagesize, query, out total);
        }

        public void ReloadUserStockChangeCache(int userid)
        {
            string key = GlobalKey.USERSTOCKCHANGE_LIST + "_" + userid;
            MangaCache.Remove(key);

            GetUserStockChangeList(userid);
        }

        /// <summary>
        /// 初始化用户库存
        /// </summary>
        public void InitUserProductStock()
        {
            if (CommonDataProvider.Instance().IsUserProductStockInit())
                return;

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
                int id = CommonDataProvider.Instance().AddOrder(entity);
                entity.ID = id;
                RefreshOrder(entity);
                if (entity.OrderProducts.Exists(p => p.SID > 0))
                {
                    DeleteShoppingTrolley(string.Join(",", entity.OrderProducts.FindAll(p => p.SID > 0).ToList().Select(p => p.SID.ToString()).ToList()), entity.UserID);
                    ReloadShoppingTrolley(entity.UserID);
                }

                return string.Empty;
            }
        }

        public string UpdateOrderProducts(OrderInfo entity)
        {
            CommonDataProvider.Instance().UpdateOrderProducts(entity);
            entity = GetOrder(entity.ID);
            RefreshOrder(entity);
            return string.Empty;
        }

        public OrderInfo GetOrder(int id, bool fromCache = false)
        {
            if (fromCache)
            {
                List<OrderInfo> list = GetOrderList(fromCache);
                return list.Find(l => l.ID == id);
            }
            else
            {
                return CommonDataProvider.Instance().GetOrderInfo(id);
            }
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

        public void RefreshOrder(OrderInfo order)
        {
            string key = GlobalKey.ORDER_LIST;
            List<OrderInfo> list = MangaCache.Get(key) as List<OrderInfo>;
            if (list == null)
                list = GetOrderList(true);
            if (list == null)
                list = new List<OrderInfo>();
            if (list.Exists(l => l.ID == order.ID))
                list[list.FindIndex(l => l.ID == order.ID)] = order;
            else
                list.Add(order);
            MangaCache.Max(key, list);
        }

        public string UpdateOrderStatus(string ids, OrderStatus status, string username = "系统作业")
        {
            StringBuilder strResult = new StringBuilder();
            OrderInfo order = GetOrder(DataConvert.SafeInt(ids), true);
            if (order.OrderStatus != OrderStatus.未收款 && status == OrderStatus.已收款)
                return string.Empty;
            if (order.OrderStatus != OrderStatus.已收款 && status == OrderStatus.已发货)
                return string.Empty;
            if (order != null)
            {
                if (status == OrderStatus.已发货 || (status == OrderStatus.已取消 && order.OrderStatus == OrderStatus.已发货))
                {
                    //if (order.ParentID == 0)
                    //{
                        AddOrderUpdateQueue(new OrderUpdateQueueInfo()
                        {
                            OrderID = order.ID,
                            OrderStatus = status,
                            DeelStatus = 0
                        });
                    //}
                    strResult.Append(order.UserID.ToString());
                }
            }
            CommonDataProvider.Instance().UpdateOrderStatus(ids, status, username);
            order = GetOrder(DataConvert.SafeInt(ids));
            RefreshOrder(order);
            return strResult.ToString();
        }

        public void UpdateOrderSyncStatus(int id, int status)
        {
            CommonDataProvider.Instance().UpdateOrderSyncStatus(id, status);
        }

        public void UpdateOrderPic(int id, string src, string action)
        {
            CommonDataProvider.Instance().UpdateOrderPic(id, src, action);
            OrderInfo order = GetOrder(id);
            RefreshOrder(order);
        }

        public void AddOrderUpdateQueue(OrderUpdateQueueInfo entity)
        {
            CommonDataProvider.Instance().AddOrderUpdateQueue(entity);
        }

        public void UpdateOrderUpdateQueueStatus(int id, int status)
        {
            CommonDataProvider.Instance().UpdateOrderUpdateQueueStatus(id, status);
        }

        public List<OrderUpdateQueueInfo> GetOrderUpdateQueue()
        {
            return CommonDataProvider.Instance().GetOrderUpdateQueue();
        }

        public void DeelOrderUpdateQueue()
        {
            List<OrderUpdateQueueInfo> list = GetOrderUpdateQueue();
            string url = "http://yd.lamda.us/admin/ck.asp";
            foreach (OrderUpdateQueueInfo entity in list)
            {
                int deelstatus = 1;
                OrderInfo order = GetOrder(DataConvert.SafeInt(entity.OrderID), true);
                if (order != null)
                {
                    string t = "c";
                    if (entity.OrderStatus == OrderStatus.已取消 && order.OrderStatus == OrderStatus.已发货)
                        t = "r";
                    foreach (OrderProductInfo p in order.OrderProducts)
                    {
                        foreach (ProductMixInfo pm in p.ProductMixList)
                        {
                            List<string> query = new List<string>();
                            query.Add("ld=" + pm.Name);
                            query.Add("sl=" + pm.Amount.ToString());
                            query.Add("kf=" + order.UserName);
                            query.Add("t=" + t);
                            query.Add("id=fdskjgbdsfjbg56514zfhg");
                            string syncresult = Http.GetPage(url + "?" + string.Join("&", query), 0);
                            if (syncresult != "ok")
                            {
                                ExpLog.Write(new Exception(syncresult));
                                SyncfailedInfo finfo = new SyncfailedInfo()
                                {
                                    Name = pm.Name,
                                    Amount = pm.Amount.ToString(),
                                    UserName = order.UserName,
                                    AType = t,
                                    AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                    Status = 0
                                };

                                AddSyncfailed(finfo);
                            }
                        }
                    }
                }
                else
                    deelstatus = 2;
                UpdateOrderUpdateQueueStatus(entity.ID, deelstatus);
            }
            if (list != null && list.Count > 0)
            {
                foreach (OrderUpdateQueueInfo entity in list)
                {
                    OrderInfo order = GetOrder(DataConvert.SafeInt(entity.OrderID), true);
                    if (order != null)
                    {
                        UpdateOrderSyncStatus(order.ID, 1);
                        order = GetOrder(DataConvert.SafeInt(entity.OrderID));
                        RefreshOrder(order);
                    }
                }
                RefreshProductStock();
                ReloadProductListCache();
            }
        }

        /// <summary>
        /// 订单过期处理
        /// </summary>
        public void OrderPastDue()
        {
            try
            {
                SitesettingInfo setting = Sitesettings.Instance.GetSitesetting(true);
                if (setting.HourNumber == 0) return;
                List<OrderInfo> orderlist = GetOrderList(true);
                orderlist = orderlist.FindAll(o => o.OrderStatus == OrderStatus.未收款 && DateTime.Parse(o.AddTime).AddHours(setting.HourNumber) < DateTime.Now);
                foreach (OrderInfo entity in orderlist)
                {
                    UpdateOrderStatus(entity.ID.ToString(), OrderStatus.已取消, "付款超时");
                }
            }
            catch { }
        }

        #endregion

        #region 同步失败记录

        public void AddSyncfailed(SyncfailedInfo entity)
        {
            CommonDataProvider.Instance().AddSyncfailed(entity);
        }

        public List<SyncfailedInfo> GetSyncfailedList()
        {
            return CommonDataProvider.Instance().GetSyncfailedList();
        }

        public void UpdateSyncfailedStatus(string ids)
        {
            CommonDataProvider.Instance().UpdateSyncfailedStatus(ids);
        }

        public void DeleteSyncfailed(string ids)
        {
            CommonDataProvider.Instance().UpdateSyncfailedStatus(ids);
        }

        public void Resync(string ids)
        {
            List<SyncfailedInfo> list = GetSyncfailedList();
            string url = "http://yd.lamda.us/admin/ck.asp";

            foreach (string id in ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (list.Exists(l => l.ID.ToString() == id))
                {
                    SyncfailedInfo info = list.Find(l => l.ID.ToString() == id);
                    List<string> query = new List<string>();
                    query.Add("ld=" + info.Name);
                    query.Add("sl=" + info.Amount);
                    query.Add("kf=" + info.UserName);
                    query.Add("t=" + info.AType);
                    query.Add("id=fdskjgbdsfjbg56514zfhg");
                    if (!string.IsNullOrEmpty(Http.GetPage(url + "?" + string.Join("&", query), 0)))
                    {
                        UpdateSyncfailedStatus(id);
                    }
                }
            }
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

        public void ReplyMessageBoard(MessageBoardInfo entity)
        {
            CommonDataProvider.Instance().ReplyMessageBoard(entity);
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

        #region 折扣模版

        public List<DiscountStencilInfo> GetDiscountStencilList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetDiscountStencilList();

            string key = GlobalKey.DISCOUNTSTENCIL_LIST;
            List<DiscountStencilInfo> list = MangaCache.Get(key) as List<DiscountStencilInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetDiscountStencilList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        /// <summary>
        /// 删除品牌
        /// </summary>
        /// <param name="ids"></param>
        public void DeleteDiscountStencils(string ids)
        {
            CommonDataProvider.Instance().DeleteDiscountStencils(ids);
        }

        public int AddDiscountStencil(DiscountStencilInfo entity)
        {
            return CommonDataProvider.Instance().AddDiscountStencil(entity);
        }

        public DiscountStencilInfo GetDiscountStencil(int id, bool fromCache = false)
        {
            List<DiscountStencilInfo> list = GetDiscountStencilList(fromCache);
            return list.Find(b => b.ID == id);
        }

        public DiscountStencilInfo GetCostsDiscount(bool fromCache = false)
        {
            List<DiscountStencilInfo> list = GetDiscountStencilList(fromCache);
            return list.Find(b => b.IsCosts == 1);
        }

        public void UpdateDiscountStencil(DiscountStencilInfo entity)
        {
            CommonDataProvider.Instance().UpdateDiscountStencil(entity);
        }

        public void ReloadDiscountStencilListCache()
        {
            string key = GlobalKey.DISCOUNTSTENCIL_LIST;
            MangaCache.Remove(key);

            GetDiscountStencilList(true);
        }

        #endregion
    }
}
