using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using Chebao.Components;
using System.Data;
using Chebao.Tools;

namespace Chebao.Components
{
    public abstract class CommonDataProvider
    {
        private static CommonDataProvider _defaultprovider = null;
        private static object _lock = new object();
        private System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();

        #region 初始化
        /// <summary>
        /// 返回默认的数据提供者类
        /// </summary>
        /// <returns></returns>
        public static CommonDataProvider Instance()
        {
            return Instance("MSSQLCommonDataProvider");
        }

        /// <summary>
        /// 从配置文件加载数据库访问提供者类
        /// </summary>
        /// <param name="providerName">提供者名</param>
        /// <returns>提供者</returns>
        public static CommonDataProvider Instance(string providerName)
        {
            string cachekey = GlobalKey.PROVIDER + "_" + providerName;
            CommonDataProvider objType = MangaCache.GetLocal(cachekey) as CommonDataProvider;//从缓存读取
            if (objType == null)
            {
                CommConfig config = CommConfig.GetConfig();
                Provider dataProvider = (Provider)config.Providers[providerName];
                objType = DataProvider.Instance(dataProvider) as CommonDataProvider;
                string path = null;
                HttpContext context = HttpContext.Current;
                if (context != null)
                    path = context.Server.MapPath("~/config/common.config");
                else
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"config\common.config");
                MangaCache.MaxLocalWithFile(cachekey, objType, path);
            }
            return objType;
        }

        /// <summary>
        ///从配置文件加载默认数据库访问提供者类
        /// </summary>
        private static void LoadDefaultProviders()
        {
            if (_defaultprovider == null)
            {
                lock (_lock)
                {
                    if (_defaultprovider == null)
                    {
                        CommConfig config = CommConfig.GetConfig();
                        Provider dataProvider = (Provider)config.Providers[GlobalKey.DEFAULT_PROVDIER_COMMON];
                        _defaultprovider = DataProvider.Instance(dataProvider) as CommonDataProvider;

                    }
                }
            }
        }

        #endregion

        #region 后台管理员

        /// <summary>
        /// 通过管理员名获取管理员
        /// </summary>
        /// <param name="name">管理员名</param>
        /// <returns></returns>
        public abstract AdminInfo GetAdminByName(string id);

        /// <summary>
        /// 管理员是否已经存在
        /// </summary>
        /// <param name="name">管理员ID</param>
        /// <returns></returns>
        public abstract bool ExistsAdmin(int id);

        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="model">后台用户实体类</param>
        /// <returns>添加成功返回ID</returns>
        public abstract int AddAdmin(AdminInfo model);

        /// <summary>
        /// 更新管理员
        /// </summary>
        /// <param name="model">后台用户实体类</param>
        /// <returns>修改是否成功</returns>
        public abstract bool UpdateAdmin(AdminInfo model);

        public abstract void UpdateBrandPowerSetting(AdminInfo entity);

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="AID"></param>
        /// <returns></returns>
        public abstract bool DeleteAdmin(int AID);

        /// <summary>
        /// 通过ID获取管理员
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>管理员实体信息</returns>
        public abstract AdminInfo GetAdmin(int id);

        /// <summary>
        /// 验证用户登陆
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>用户ID</returns>
        public abstract int ValiAdmin(string userName, string password);

        /// <summary>
        /// 返回所有用户
        /// </summary>
        /// <returns></returns>
        public abstract List<AdminInfo> GetAllAdmins();

        /// <summary>
        /// 获取普通用户
        /// </summary>
        /// <returns></returns>
        public abstract List<AdminInfo> GetUsers();

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userID">管理员ID</param>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="newPassword">新密码</param>
        /// <returns></returns>
        public abstract bool ChangeAdminPw(int userID, string oldPassword, string newPassword);

        /// <summary>
        /// 获取用于加密的值
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public abstract string GetAdminKey(int userID);

        /// <summary>
        /// 填充后台用户实体类
        /// </summary>
        /// <param name="reader">记录集</param>
        /// <returns>实体类</returns>
        protected AdminInfo PopulateAdmin(IDataReader reader)
        {
            AdminInfo admin = new AdminInfo();
            admin.ID = (int)reader["ID"];
            admin.Administrator = DataConvert.SafeBool(reader["Administrator"]);
            admin.LastLoginIP = reader["LastLoginIP"] as string;
            admin.LastLoginTime = reader["LastLoginTime"] as DateTime?;
            admin.Password = reader["Password"] as string;
            admin.UserName = reader["UserName"] as string;
            admin.UserRole = (UserRoleType)(int)reader["UserRole"];

            SerializerData data = new SerializerData();
            data.Keys = reader["PropertyNames"] as string;
            data.Values = reader["PropertyValues"] as string;
            admin.SetSerializerData(data);

            return admin;
        }

        #endregion

        #region 日志

        public abstract void WriteEventLogEntry(EventLogEntry log);

        public abstract void ClearEventLog(DateTime dt);

        public abstract List<EventLogEntry> GetEventLogs(int pageindex, int pagesize, EventLogQuery query, out int total);


        /// <summary>
        /// 填充日志信息
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected EventLogEntry PopulateEventLogEntry(IDataReader reader)
        {
            EventLogEntry eventlog = new EventLogEntry();
            eventlog.EntryID = DataConvert.SafeInt(reader["ID"]);
            eventlog.EventID = DataConvert.SafeInt(reader["EventID"]);
            eventlog.EventType = (EventType)(byte)(reader["EventType"]);
            eventlog.Message = reader["Message"] as string;
            eventlog.Category = reader["Category"] as string;
            eventlog.MachineName = reader["MachineName"] as string;
            eventlog.ApplicationName = reader["ApplicationName"] as string;
            eventlog.PCount = DataConvert.SafeInt(reader["PCount"]);
            eventlog.AddTime = DataConvert.SafeDate(reader["AddTime"]);
            eventlog.LastUpdateTime = reader["LastUpdateTime"] as DateTime?;
            eventlog.ApplicationType = (ApplicationType)(byte)(reader["AppType"]);
            eventlog.EntryID = DataConvert.SafeInt(reader["EntryID"]);
            return eventlog;
        }


        #endregion

        #region 车辆品牌

        public abstract List<BrandInfo> GetBrandList();

        public abstract void DeleteBrands(string ids);

        public abstract int AddBrand(BrandInfo entity);

        public abstract void UpdateBrand(BrandInfo entity);

        protected BrandInfo PopulateBrand(IDataReader reader)
        {
            BrandInfo entity = new BrandInfo
            {
                ID = DataConvert.SafeInt(reader["ID"]),
                BrandName = reader["BrandName"] as string,
                NameIndex = reader["NameIndex"] as string,
                Imgpath = reader["Imgpath"] as string
            };

            return entity;
        }

        #endregion

        #region 车型

        public abstract List<CabmodelInfo> GetCabmodelList();

        public abstract void DeleteCabmodels(string ids);

        public abstract int AddCabmodel(CabmodelInfo entity);

        public abstract void UpdateCabmodel(CabmodelInfo entity);

        protected CabmodelInfo PopulateCabmodel(IDataReader reader)
        {
            CabmodelInfo entity = new CabmodelInfo
            {
                ID = DataConvert.SafeInt(reader["ID"]),
                CabmodelName = reader["CabmodelName"] as string,
                NameIndex = reader["NameIndex"] as string,
                BrandID = (int)reader["BrandID"],
                Pailiang = reader["Pailiang"] as string,
                Nianfen = reader["Nianfen"] as string,
                BrandName = reader["BrandName"] as string,
                Imgpath = reader["Imgpath"] == null ? string.Empty : reader["Imgpath"].ToString()
            };

            return entity;
        }

        #endregion

        #region 产品

        public abstract List<ProductInfo> GetProductList();

        public abstract void DeleteProducts(string ids);

        public abstract int AddProduct(ProductInfo entity);

        public abstract void UpdateProduct(ProductInfo entity);

        protected ProductInfo PopulateProduct(IDataReader reader)
        {
            ProductInfo entity = new ProductInfo
            {
                ID = DataConvert.SafeInt(reader["ID"]),
                ProductType = (ProductType)(int)reader["ProductType"],
                Cabmodels = reader["Cabmodels"] as string,
                Introduce = reader["Introduce"] as string,
                Pic = reader["Pic"] as string,
                Pics = reader["Pics"] as string,
                Stock = DataConvert.SafeInt(reader["Stock"]),
            };
            SerializerData data = new SerializerData();
            data.Keys = reader["PropertyNames"] as string;
            data.Values = reader["PropertyValues"] as string;
            entity.SetSerializerData(data);

            return entity;
        }

        #endregion

        #region 购物车

        public abstract List<ShoppingTrolleyInfo> GetShoppingTrolleyByUserID(int userid);

        public abstract int AddShoppingTrolley(ShoppingTrolleyInfo entity);

        public abstract int UpdateShoppingTrolley(ShoppingTrolleyInfo entity);

        public abstract void DeleteShoppingTrolley(string ids, int userid);

        protected ShoppingTrolleyInfo PopulateShoppingTrolley(IDataReader reader)
        {
            ShoppingTrolleyInfo entity = new ShoppingTrolleyInfo
            {
                ID = DataConvert.SafeInt(reader["ID"]),
                ProductID = DataConvert.SafeInt(reader["ProductID"]),
                UserID = DataConvert.SafeInt(reader["UserID"]),
                Amount = DataConvert.SafeInt(reader["Amount"]),
                CabmodelStr = reader["CabmodelStr"] as string
            };

            return entity;
        }

        #endregion

        #region 订单管理

        public abstract void AddOrder(OrderInfo entity);

        public abstract void UpdateOrderProducts(OrderInfo entity);

        public abstract List<OrderInfo> GetOrderList();

        public abstract void UpdateOrderStatus(string ids, OrderStatus status);

        public abstract void UpdateOrderPic(int id, string src, string action);

        public abstract void UpdateOrderSyncStatus(int id, int status);

        protected OrderInfo PopulateOrder(IDataReader reader)
        {
            OrderInfo entity = new OrderInfo
            {
                ID = DataConvert.SafeInt(reader["ID"]),
                OrderNumber = reader["OrderNumber"] as string,
                UserName = reader["UserName"] as string,
                UserID = DataConvert.SafeInt(reader["UserID"]),
                Address = reader["Address"] as string,
                Province = reader["Province"] as string,
                City = reader["City"] as string,
                District = reader["District"] as string,
                PostCode = reader["PostCode"] as string,
                LinkName = reader["LinkName"] as string,
                LinkMobile = reader["LinkMobile"] as string,
                LinkTel = reader["LinkTel"] as string,
                OrderProductJson = reader["OrderProductJson"] as string,
                OrderProductJson1 = reader["OrderProductJson1"] as string,
                OrderProductJson2 = reader["OrderProductJson2"] as string,
                OrderProductJson3 = reader["OrderProductJson3"] as string,
                OrderProductJson4 = reader["OrderProductJson4"] as string,
                OrderProductJson5 = reader["OrderProductJson5"] as string,
                OrderProductJson6 = reader["OrderProductJson6"] as string,
                OrderProductJson7 = reader["OrderProductJson7"] as string,
                OrderStatus = (OrderStatus)(int)reader["OrderStatus"],
                TotalFee = reader["TotalFee"] as string,
                AddTime = reader["AddTime"] as string,
                DeelTime = reader["DeelTime"] as string,
                PicRemittanceAdvice = reader["PicRemittanceAdvice"] as string,
                PicInvoice = reader["PicInvoice"] as string,
                PicListItem = reader["PicListItem"] as string,
                PicBookingnote = reader["PicBookingnote"] as string,
                SyncStatus = DataConvert.SafeInt(reader["SyncStatus"]),
            };
            if (string.IsNullOrEmpty(entity.OrderProductJson))
                entity.OrderProducts = json.Deserialize<List<OrderProductInfo>>(
                    entity.OrderProductJson1
                    + entity.OrderProductJson2
                    + entity.OrderProductJson3
                    + entity.OrderProductJson4
                    + entity.OrderProductJson5
                    + entity.OrderProductJson6
                    + entity.OrderProductJson7);
            else
                entity.OrderProducts = json.Deserialize<List<OrderProductInfo>>(entity.OrderProductJson);

            return entity;
        }

        public abstract void AddOrderUpdateQueue(OrderUpdateQueueInfo entity);

        public abstract void UpdateOrderUpdateQueueStatus(int id,int status);

        public abstract List<OrderUpdateQueueInfo> GetOrderUpdateQueue();

        protected OrderUpdateQueueInfo PopulateOrderUpdateQueue(IDataReader reader)
        {
            return new OrderUpdateQueueInfo() 
            {
                ID = DataConvert.SafeInt(reader["ID"]),
                OrderID = DataConvert.SafeInt(reader["OrderID"]),
                OrderStatus = (OrderStatus)(int)reader["OrderStatus"],
                DeelStatus = DataConvert.SafeInt(reader["DeelStatus"])
            };
        }

        #endregion

        #region 同步失败记录

        public abstract void AddSyncfailed(SyncfailedInfo entity);

        public abstract List<SyncfailedInfo> GetSyncfailedList();

        public abstract void UpdateSyncfailedStatus(string ids);

        protected SyncfailedInfo PopulateSyncfailed(IDataReader reader)
        {
            SyncfailedInfo entity = new SyncfailedInfo
            {
                ID = DataConvert.SafeInt(reader["ID"]),
                Name = reader["Name"] as string,
                Amount = reader["Amount"] as string,
                UserName = reader["UserName"] as string,
                AType = reader["AType"] as string,
                AddTime = reader["AddTime"] as string,
                Status = DataConvert.SafeInt(reader["Status"]),
            };

            return entity;
        }

        #endregion

        #region 反馈有奖

        public abstract void AddMessageBoard(MessageBoardInfo entity);

        public abstract List<MessageBoardInfo> GetMessageBoardList();

        public abstract MessageBoardInfo GetMessageBoard(int id);

        protected MessageBoardInfo PopulateMessageBoard(IDataReader reader)
        {
            MessageBoardInfo entity = new MessageBoardInfo
            {
                ID = DataConvert.SafeInt(reader["ID"]),
                UserName = reader["UserName"] as string,
                UserID = DataConvert.SafeInt(reader["UserID"]),
                AddTime = reader["AddTime"] as string,
                Title = reader["Title"] as string,
                Content = reader["Content"] as string
            };

            return entity;
        }

        #endregion

        #region 地区

        public abstract List<ProvinceInfo> GetProvinceList();

        public abstract List<CityInfo> GetCityList();

        public abstract List<DistrictInfo> GetDistrictList();

        protected ProvinceInfo PopulateProvince(IDataReader reader)
        {
            ProvinceInfo entity = new ProvinceInfo
            {
                ID = DataConvert.SafeInt(reader["ID"]),
                Name = reader["Name"] as string,
                Orderid = DataConvert.SafeInt(reader["orderid"])
            };

            return entity;
        }

        protected CityInfo PopulateCity(IDataReader reader)
        {
            CityInfo entity = new CityInfo
            {
                ID = DataConvert.SafeInt(reader["ID"]),
                Name = reader["Name"] as string,
                ProvinceId = DataConvert.SafeInt(reader["ProvinceId"]),
                AreaCode = reader["AreaCode"] as string
            };

            return entity;
        }

        protected DistrictInfo PopulateDistrict(IDataReader reader)
        {
            DistrictInfo entity = new DistrictInfo
            {
                ID = DataConvert.SafeInt(reader["ID"]),
                CityId = DataConvert.SafeInt(reader["CityId"]),
                Name = reader["Name"] as string,
                PostCode = reader["PostCode"] as string
            };

            return entity;
        }

        #endregion

        #region 系统设置

        public abstract void ExecuteSql(string sql);

        #endregion

        #region 站点设置


        public abstract void AddSitesetting(SitesettingInfo entity);

        public abstract void UpdateSitesetting(SitesettingInfo entity);

        public abstract SitesettingInfo GetSitesetting();

        protected SitesettingInfo PopulateSitesetting(IDataReader reader)
        {
            SitesettingInfo entity = new SitesettingInfo
            {
                Notice = reader["Notice"] as string,
                CorpIntroduce = reader["CorpIntroduce"] as string,
                Contact = reader["Contact"] as string
            };

            SerializerData data = new SerializerData();
            data.Keys = reader["PropertyNames"] as string;
            data.Values = reader["PropertyValues"] as string;
            entity.SetSerializerData(data);

            return entity;
        }

        #endregion

        #region 折扣模版

        public abstract List<DiscountStencilInfo> GetDiscountStencilList();

        public abstract void DeleteDiscountStencils(string ids);

        public abstract int AddDiscountStencil(DiscountStencilInfo entity);

        public abstract void UpdateDiscountStencil(DiscountStencilInfo entity);

        protected DiscountStencilInfo PopulateDiscountStencil(IDataReader reader)
        {
            DiscountStencilInfo entity = new DiscountStencilInfo
            {
                ID = DataConvert.SafeInt(reader["ID"]),
                Name = reader["Name"] as string,
                IsCosts = DataConvert.SafeInt(reader["IsCosts"])
            };

            SerializerData data = new SerializerData();
            data.Keys = reader["PropertyNames"] as string;
            data.Values = reader["PropertyValues"] as string;
            entity.SetSerializerData(data);

            return entity;
        }

        #endregion
    }
}
