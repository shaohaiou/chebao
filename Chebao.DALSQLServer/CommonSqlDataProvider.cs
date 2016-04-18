using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Chebao.Tools;
using System.Data.SqlClient;
using System.Data;
using Chebao.Components;
using Chebao.Components.Data;
using System.Data.OleDb;

namespace Chebao.DALSQLServer
{
    public class CommonSqlDataProvider : CommonDataProvider
    {
        private string _con;
        private string _dbowner;
        private static object sync_helper = new object();

        #region 初始化
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="constr">连接字符串</param>
        /// <param name="owner">数据库所有者</param>
        public CommonSqlDataProvider(string constr, string owner)
        {
            CommConfig config = CommConfig.GetConfig();
            //_con = EncryptString.DESDecode(constr, config.AppSetting["key"]);
            _con = constr;
            _dbowner = owner;
        }
        #endregion

        #region 后台管理员

        /// <summary>
        ///  获取用于加密的值
        /// </summary>
        /// <param name="userID">管理员ID</param>
        /// <returns>用于加密的值</returns>
        public override string GetAdminKey(int userID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 CheckKey from Chebao_AdminUser ");
            strSql.Append(" where [ID]=@ID ");
            object o = SqlHelper.ExecuteScalar(_con, CommandType.Text, strSql.ToString(), new OleDbParameter("@ID", userID));
            return o as string;
        }

        /// <summary>
        /// 管理员是否已经存在
        /// </summary>
        /// <param name="name">管理员ID</param>
        /// <returns>管理员是否存在</returns>
        public override bool ExistsAdmin(int id)
        {
            string sql = "select count(1) from Chebao_AdminUser where [ID]=@ID";
            int i = Convert.ToInt32(SqlHelper.ExecuteScalar(_con, CommandType.Text, sql, new OleDbParameter("@ID", id)));
            if (i > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 通过用户名获得后台管理员信息
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns>管理员实体信息</returns>
        public override AdminInfo GetAdminByName(string UserName)
        {
            string sql = "select * from Chebao_AdminUser where [UserName]=@UserName";
            AdminInfo admin = null;
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql, new OleDbParameter("@UserName", UserName)))
            {
                if (reader.Read())
                {
                    admin = PopulateAdmin(reader);
                }
            }
            return admin;
        }

        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="model">后台用户实体类</param>
        /// <returns>添加成功返回ID</returns>
        public override int AddAdmin(AdminInfo model)
        {
            SerializerData data = model.GetSerializerData();
            string sql = @"
            INSERT INTO Chebao_AdminUser(
                [UserName]
                ,[Password]
                ,[Administrator]
                ,[LastLoginIP]
                ,[LastLoginTime]
                ,[PropertyNames]
                ,[PropertyValues]
                ,[UserRole]
                ) VALUES 
                (@UserName
                ,@Password
                ,@Administrator
                ,@LastLoginIP
                ,@LastLoginTime
                ,@PropertyNames
                ,@PropertyValues
                ,@UserRole)";

            OleDbParameter[] p = 
            {
                new OleDbParameter("@UserName",model.UserName),
                new OleDbParameter("@Password",model.Password),
                new OleDbParameter("@Administrator",model.Administrator),
                new OleDbParameter("@LastLoginIP",model.LastLoginIP),
                new OleDbParameter("@LastLoginTime",model.LastLoginTime.HasValue ? model.LastLoginTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : null),
                new OleDbParameter("@PropertyNames",data.Keys),
                new OleDbParameter("@PropertyValues",data.Values),
                new OleDbParameter("@UserRole",(int)model.UserRole)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
            sql = "SELECT @@IDENTITY";
            model.ID = DataConvert.SafeInt(SqlHelper.ExecuteScalar(_con, CommandType.Text, sql));
            return model.ID;
        }

        /// <summary>
        /// 更新管理员
        /// </summary>
        /// <param name="model">后台用户实体类</param>
        /// <returns>修改是否成功</returns>
        public override bool UpdateAdmin(AdminInfo model)
        {
            SerializerData data = model.GetSerializerData();
            string sql = @"UPDATE Chebao_AdminUser SET 
                [UserName] = @UserName
                ,[Password] = @Password
                ,[Administrator] = @Administrator
                ,[LastLoginIP] = @LastLoginIP
                ,[LastLoginTime] = @LastLoginTime
                ,[PropertyNames] = @PropertyNames
                ,[PropertyValues] = @PropertyValues 
                WHERE [ID] = @ID";
            OleDbParameter[] p = 
            {
                new OleDbParameter("@UserName",model.UserName),
                new OleDbParameter("@Password",model.Password),
                new OleDbParameter("@Administrator",model.Administrator),
                new OleDbParameter("@LastLoginIP",model.LastLoginIP),
                new OleDbParameter("@LastLoginTime",model.LastLoginTime.HasValue ? model.LastLoginTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : null),
                new OleDbParameter("@PropertyNames",data.Keys),
                new OleDbParameter("@PropertyValues",data.Values),
                new OleDbParameter("@ID",model.ID)
            };
            int result = SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
            if (result > 0)
            {
                return true;
            }
            return false;
        }

        public override void UpdateBrandPowerSetting(AdminInfo entity)
        {
            SerializerData data = entity.GetSerializerData();
            string sql = @"
                UPDATE Chebao_AdminUser SET
                    [PropertyNames] = @PropertyNames
                    ,[PropertyValues] = @PropertyValues 
                WHERE [ID] = @ID";
            OleDbParameter[] p = 
            {
                new OleDbParameter("@PropertyNames",data.Keys),
                new OleDbParameter("@PropertyValues",data.Values),
                new OleDbParameter("@ID",entity.ID)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
        }

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="AID">管理员ID</param>
        /// <returns>删除是否成功</returns>
        public override bool DeleteAdmin(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Chebao_AdminUser ");
            strSql.Append(" where [ID]=@ID ");
            int result = SqlHelper.ExecuteNonQuery(_con, CommandType.Text, strSql.ToString(), new OleDbParameter("@ID", id));
            if (result > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 通过ID获取管理员
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>管理员实体信息</returns>
        public override AdminInfo GetAdmin(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from Chebao_AdminUser ");
            strSql.Append(" where [ID]=@ID ");
            AdminInfo admin = null;
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, strSql.ToString(), new OleDbParameter("@ID", id)))
            {
                if (reader.Read())
                {
                    admin = PopulateAdmin(reader);
                }
            }
            return admin;
        }

        /// <summary>
        /// 验证用户登陆
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>用户ID</returns>
        public override int ValiAdmin(string userName, string password)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID from Chebao_AdminUser");
            strSql.Append(" where [UserName]=@UserName and [Password]=@PassWord");

            object obj = SqlHelper.ExecuteScalar(_con, CommandType.Text, strSql.ToString(), new OleDbParameter("@UserName", userName), new OleDbParameter("@PassWord", password));
            if (obj == null)
            {
                return -2;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 返回所有用户
        /// </summary>
        /// <returns>返回所有用户</returns>
        public override List<AdminInfo> GetAllAdmins()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Chebao_AdminUser WHERE [UserRole] = " + (int)UserRoleType.管理员);


            List<AdminInfo> admins = new List<AdminInfo>();
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, strSql.ToString()))
            {
                while (reader.Read())
                {
                    admins.Add(PopulateAdmin(reader));
                }
            }
            return admins;
        }

        public override List<AdminInfo> GetUsers()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Chebao_AdminUser WHERE [UserRole] <> " + (int)UserRoleType.管理员);


            List<AdminInfo> admins = new List<AdminInfo>();
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, strSql.ToString()))
            {
                while (reader.Read())
                {
                    admins.Add(PopulateAdmin(reader));
                }
            }
            return admins;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userID">管理员ID</param>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="newPassword">新密码</param>
        /// <returns>修改密码是否成功</returns>
        public override bool ChangeAdminPw(int userID, string oldPassword, string newPassword)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Chebao_AdminUser set ");
            strSql.Append("[Password]=@NewPassword");
            strSql.Append(" where [ID]=@ID and [Password]=@Password ");
            int result = SqlHelper.ExecuteNonQuery(_con, CommandType.Text, strSql.ToString(), new OleDbParameter("@NewPassword", newPassword), new OleDbParameter("@ID", userID), new OleDbParameter("@Password", oldPassword));
            if (result < 1)
                return false;
            return true;
        }

        #endregion

        #region 日志

        /// <summary>
        /// 写入日志信息
        /// </summary>
        /// <param name="log"></param>
        public override void WriteEventLogEntry(EventLogEntry log)
        {
            try
            {
                string sql = "Chebao_AddEvent";
                OleDbParameter[] parameters = 
                    {
                        new OleDbParameter("@Uniquekey", log.Uniquekey),
                        new OleDbParameter("@EventType", log.EventType),
                        new OleDbParameter("@EventID",log.EventID),
                        new OleDbParameter("@Message",log.Message),
                        new OleDbParameter("@Category",log.Category),
                        new OleDbParameter("@MachineName",log.MachineName),
                        new OleDbParameter("@ApplicationName",log.ApplicationName),
                        new OleDbParameter("@ApplicationID",log.ApplicationID),
                        new OleDbParameter("@AppType",log.ApplicationType),
                        new OleDbParameter("@EntryID",log.EntryID),
                        new OleDbParameter("@PCount",log.PCount),
                        new OleDbParameter("@LastUpdateTime",log.LastUpdateTime)
                    };
                SqlHelper.ExecuteNonQuery(_con, CommandType.StoredProcedure, sql, parameters);
            }
            catch { }
        }

        /// <summary>
        /// 根据时间清除日志
        /// </summary>
        /// <param name="dt"></param>
        public override void ClearEventLog(DateTime dt)
        {
            throw new NotImplementedException();
        }

        public override List<EventLogEntry> GetEventLogs(int pageindex, int pagesize, EventLogQuery query, out int total)
        {
            List<EventLogEntry> eventlist = new List<EventLogEntry>();
            OleDbParameter p;
            if (pageindex != -1)
            {
                using (IDataReader reader = CommonPageSql.GetDataReaderByPager(_con, pageindex, pagesize, query, out p))
                {
                    while (reader.Read())
                    {
                        eventlist.Add(PopulateEventLogEntry(reader));
                    }
                }
                total = int.Parse(p.Value.ToString());
            }

            else
            {
                using (IDataReader reader = CommonSelectSql.SelectGetReader(_con, pagesize, query))
                {
                    while (reader.Read())
                    {
                        eventlist.Add(PopulateEventLogEntry(reader));
                    }
                }
                total = eventlist.Count();
            }
            return eventlist;
        }
        #endregion

        #region 车辆品牌

        public override List<BrandInfo> GetBrandList()
        {
            List<BrandInfo> list = new List<BrandInfo>();
            string sql = "SELECT * FROM Chebao_Brand";
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateBrand(reader));
                }
            }

            return list;
        }

        public override void DeleteBrands(string ids)
        {
            string sql = string.Format("DELETE FROM Chebao_Brand WHERE [ID] IN ({0})", ids);
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql);
        }

        public override int AddBrand(BrandInfo entity)
        {
            int result = 0;
            string sql = @"SELECT COUNT(0) FROM Chebao_Brand WHERE [BrandName] = @BrandName";
            OleDbParameter[] p = 
            { 
                new OleDbParameter("@BrandName",entity.BrandName)
            };
            if (DataConvert.SafeInt(SqlHelper.ExecuteScalar(_con, CommandType.Text, sql, p)) == 0)
            {
                sql = @"
                INSERT INTO Chebao_Brand(
                    [BrandName]
                    ,[NameIndex]
                    ,[Imgpath]
                )VALUES(
                    @BrandName
                    ,@NameIndex
                    ,@Imgpath
                )";

                p = new OleDbParameter[]
                { 
                    new OleDbParameter("@BrandName",entity.BrandName),
                    new OleDbParameter("@NameIndex",entity.NameIndex),
                    new OleDbParameter("@Imgpath",entity.Imgpath)
                };
                SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
                sql = "SELECT MAX([ID]) FROM Chebao_Brand";
                result = DataConvert.SafeInt(SqlHelper.ExecuteScalar(_con, CommandType.Text, sql));
            }
            return result;
        }

        public override void UpdateBrand(BrandInfo entity)
        {
            string sql = "SELECT COUNT(0) FROM Chebao_Brand WHERE [BrandName] = @BrandName AND [ID] <> @ID";
            OleDbParameter[] p = 
            { 
                new OleDbParameter("@BrandName",entity.BrandName),
                new OleDbParameter("@ID",entity.ID)
            };
            if (DataConvert.SafeInt(SqlHelper.ExecuteScalar(_con, CommandType.Text, sql, p)) == 0)
            {
                sql = @"
                UPDATE Chebao_Brand SET
                    [BrandName] = @BrandName
                    ,[NameIndex] = @NameIndex
                    ,[Imgpath] = @Imgpath
                WHERE [ID] = @ID";
                p = new OleDbParameter[]
                { 
                    new OleDbParameter("@BrandName",entity.BrandName),
                    new OleDbParameter("@NameIndex",entity.NameIndex),
                    new OleDbParameter("@Imgpath",entity.Imgpath),
                    new OleDbParameter("@ID",entity.ID)
                };
                SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
            }
        }

        #endregion

        #region 车型

        public override List<CabmodelInfo> GetCabmodelList()
        {
            List<CabmodelInfo> list = new List<CabmodelInfo>();
            string sql = "SELECT t1.*,t2.BrandName FROM Chebao_Cabmodel T1 LEFT OUTER JOIN Chebao_Brand T2 ON T1.BrandID = T2.ID";
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateCabmodel(reader));
                }
            }

            return list;
        }

        public override void DeleteCabmodels(string ids)
        {
            string sql = string.Format("DELETE FROM Chebao_Cabmodel WHERE [ID] IN ({0})", ids);
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql);
        }

        public override int AddCabmodel(CabmodelInfo entity)
        {
            int result = 0;
            string sql = "SELECT COUNT(0) FROM Chebao_Cabmodel WHERE [CabmodelName] = @CabmodelName AND [BrandID] = @BrandID AND [Pailiang] = @Pailiang AND [Nianfen] = @Nianfen";
            OleDbParameter[] p = 
            { 
                new OleDbParameter("@CabmodelName",entity.CabmodelName),
                new OleDbParameter("@BrandID",entity.BrandID),
                new OleDbParameter("@Pailiang",entity.Pailiang),
                new OleDbParameter("@Nianfen",entity.Nianfen)
            };
            if (DataConvert.SafeInt(SqlHelper.ExecuteScalar(_con, CommandType.Text, sql, p)) == 0)
            {
                sql = @"
                INSERT INTO Chebao_Cabmodel(
                    [CabmodelName]
                    ,[BrandID]
                    ,[Pailiang]
                    ,[Nianfen]
                    ,[NameIndex]
                    ,[Imgpath]
                )VALUES(
                    @CabmodelName
                    ,@BrandID
                    ,@Pailiang
                    ,@Nianfen
                    ,@NameIndex
                    ,@Imgpath
                )";

                p = new OleDbParameter[]
                { 
                    new OleDbParameter("@CabmodelName",entity.CabmodelName),
                    new OleDbParameter("@BrandID",entity.BrandID),
                    new OleDbParameter("@Pailiang",entity.Pailiang),
                    new OleDbParameter("@Nianfen",entity.Nianfen),
                    new OleDbParameter("@NameIndex",entity.NameIndex),
                    new OleDbParameter("@Imgpath",entity.Imgpath)
                };
                SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
                sql = "SELECT MAX([ID]) FROM Chebao_Cabmodel";
                result = DataConvert.SafeInt(SqlHelper.ExecuteScalar(_con, CommandType.Text, sql));
            }
            return result;
        }

        public override void UpdateCabmodel(CabmodelInfo entity)
        {
            string sql = "SELECT COUNT(0) FROM Chebao_Cabmodel WHERE [CabmodelName] = @CabmodelName AND [BrandID] = @BrandID AND [Pailiang] = @Pailiang AND [Nianfen] = @Nianfen AND [ID] <> @ID";
            OleDbParameter[] p = 
            { 
                new OleDbParameter("@CabmodelName",entity.CabmodelName),
                new OleDbParameter("@BrandID",entity.BrandID),
                new OleDbParameter("@Pailiang",entity.Pailiang),
                new OleDbParameter("@Nianfen",entity.Nianfen),
                new OleDbParameter("@ID",entity.ID),
            };
            if (DataConvert.SafeInt(SqlHelper.ExecuteScalar(_con, CommandType.Text, sql, p)) == 0)
            {
                sql = @"
                UPDATE Chebao_Cabmodel SET
                    [CabmodelName] = @CabmodelName
                    ,[BrandID] = @BrandID
                    ,[Pailiang] = @Pailiang
                    ,[Nianfen] = @Nianfen
                    ,[NameIndex] = @NameIndex
                    ,[Imgpath] = @Imgpath
                WHERE [ID] = @ID";
                p = new OleDbParameter[]
                { 
                    new OleDbParameter("@CabmodelName",entity.CabmodelName),
                    new OleDbParameter("@BrandID",entity.BrandID),
                    new OleDbParameter("@Pailiang",entity.Pailiang),
                    new OleDbParameter("@Nianfen",entity.Nianfen),
                    new OleDbParameter("@NameIndex",entity.NameIndex),
                    new OleDbParameter("@Imgpath",entity.Imgpath),
                    new OleDbParameter("@ID",entity.ID)
                };
                SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
            }
        }

        #endregion

        #region 产品

        public override List<ProductInfo> GetProductList()
        {
            List<ProductInfo> list = new List<ProductInfo>();
            string sql = "SELECT * FROM Chebao_Product";
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateProduct(reader));
                }
            }

            return list;
        }

        public override void DeleteProducts(string ids)
        {
            string sql = string.Format("DELETE FROM Chebao_Product WHERE [ID] IN ({0})", ids);
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql);
        }

        public override int AddProduct(ProductInfo entity)
        {
            int result = 0;
            string sql = @"
                INSERT INTO Chebao_Product(
                    [ProductType]
                    ,[Cabmodels]
                    ,[Introduce]
                    ,[Pic]
                    ,[Pics]
                    ,[PropertyNames]
                    ,[PropertyValues]
                )VALUES(
                    @ProductType
                    ,@Cabmodels
                    ,@Introduce
                    ,@Pic
                    ,@Pics
                    ,@PropertyNames
                    ,@PropertyValues
                )";

            SerializerData data = entity.GetSerializerData();
            OleDbParameter[] p = 
            { 
                new OleDbParameter("@ProductType",(int)entity.ProductType),
                new OleDbParameter("@Cabmodels",entity.Cabmodels),
                new OleDbParameter("@Introduce",entity.Introduce),
                new OleDbParameter("@Pic",entity.Pic),
                new OleDbParameter("@Pics",entity.Pics),
                new OleDbParameter("@PropertyNames",data.Keys),
                new OleDbParameter("@PropertyValues",data.Values)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
            sql = "SELECT MAX([ID]) FROM Chebao_Product";
            result = DataConvert.SafeInt(SqlHelper.ExecuteScalar(_con, CommandType.Text, sql));
            return result;
        }

        public override void UpdateProduct(ProductInfo entity)
        {
            string sql = @"
                UPDATE Chebao_Product SET
                    [ProductType] = @ProductType
                    ,[Cabmodels] = @Cabmodels
                    ,[Introduce] = @Introduce
                    ,[Pic] = @Pic
                    ,[Pics] = @Pics
                    ,[PropertyNames] = @PropertyNames
                    ,[PropertyValues] = @PropertyValues
                WHERE [ID] = @ID";

            SerializerData data = entity.GetSerializerData();
            OleDbParameter[] p = 
            { 
                new OleDbParameter("@ProductType",(int)entity.ProductType),
                new OleDbParameter("@Cabmodels",entity.Cabmodels),
                new OleDbParameter("@Introduce",entity.Introduce),
                new OleDbParameter("@Pic",entity.Pic),
                new OleDbParameter("@Pics",entity.Pics),
                new OleDbParameter("@PropertyNames",data.Keys),
                new OleDbParameter("@PropertyValues",data.Values),
                new OleDbParameter("@ID",entity.ID)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);

        }

        #endregion

        #region 购物车

        public override List<ShoppingTrolleyInfo> GetShoppingTrolleyByUserID(int userid)
        {
            List<ShoppingTrolleyInfo> list = new List<ShoppingTrolleyInfo>();
            string sql = "SELECT * FROM Chebao_ShoppingTrolley WHERE [UserID] = " + userid;
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateShoppingTrolley(reader));
                }
            }

            return list;
        }

        public override int AddShoppingTrolley(ShoppingTrolleyInfo entity)
        {
            int result = 0;
            string sql = @"
                INSERT INTO Chebao_ShoppingTrolley(
                    [ProductID]
                    ,[UserID]
                    ,[Amount]
                    ,[CabmodelStr]
                )VALUES(
                    @ProductID
                    ,@UserID
                    ,@Amount
                    ,@CabmodelStr
                )";

            OleDbParameter[] p = 
            { 
                new OleDbParameter("@ProductID",entity.ProductID),
                new OleDbParameter("@UserID",entity.UserID),
                new OleDbParameter("@Amount",entity.Amount),
                new OleDbParameter("@CabmodelStr",entity.CabmodelStr)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
            sql = "SELECT MAX([ID]) FROM Chebao_ShoppingTrolley";
            result = DataConvert.SafeInt(SqlHelper.ExecuteScalar(_con, CommandType.Text, sql));
            return result;
        }

        public override int UpdateShoppingTrolley(ShoppingTrolleyInfo entity)
        {
            string sql = @"
                UPDATE Chebao_ShoppingTrolley SET
                    [Amount]=@Amount
                WHERE [ID]=@ID";

            OleDbParameter[] p = 
            { 
                new OleDbParameter("@Amount",entity.Amount),
                new OleDbParameter("@ID",entity.ID)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
            return entity.ID;
        }

        public override void DeleteShoppingTrolley(string ids,int userid)
        {
            string sql = string.Format("DELETE FROM Chebao_ShoppingTrolley WHERE [UserID] = {0} AND [ID] IN ({1})", userid, ids);
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql);
        }

        #endregion

        #region 订单管理

        public override int AddOrder(OrderInfo entity)
        {
            string sql = @"
                INSERT INTO Chebao_Order(
                    [OrderNumber]
                    ,[UserName]
                    ,[UserID]
                    ,[Province]
                    ,[City]
                    ,[District]
                    ,[PostCode]
                    ,[LinkName]
                    ,[Address]
                    ,[LinkMobile]
                    ,[LinkTel]
                    ,[OrderProductJson1]
                    ,[OrderProductJson2]
                    ,[OrderProductJson3]
                    ,[OrderProductJson4]
                    ,[OrderProductJson5]
                    ,[OrderProductJson6]
                    ,[OrderProductJson7]
                    ,[OrderStatus]
                    ,[TotalFee]
                    ,[AddTime]
                    ,[DeelTime]
                    ,[PicRemittanceAdvice]
                    ,[PicInvoice]
                    ,[PicListItem]
                    ,[PicBookingnote]
                )VALUES(
                    @OrderNumber
                    ,@UserName
                    ,@UserID
                    ,@Province
                    ,@City
                    ,@District
                    ,@PostCode
                    ,@LinkName
                    ,@Address
                    ,@LinkMobile
                    ,@LinkTel
                    ,@OrderProductJson1
                    ,@OrderProductJson2
                    ,@OrderProductJson3
                    ,@OrderProductJson4
                    ,@OrderProductJson5
                    ,@OrderProductJson6
                    ,@OrderProductJson7
                    ,@OrderStatus
                    ,@TotalFee
                    ,@AddTime
                    ,@DeelTime
                    ,@PicRemittanceAdvice
                    ,@PicInvoice
                    ,@PicListItem
                    ,@PicBookingnote
                )";

            OleDbParameter[] p = 
            { 
                new OleDbParameter("@OrderNumber",entity.OrderNumber),
                new OleDbParameter("@UserName",entity.UserName),
                new OleDbParameter("@UserID",entity.UserID),
                new OleDbParameter("@Province",entity.Province),
                new OleDbParameter("@City",entity.City),
                new OleDbParameter("@District",entity.District),
                new OleDbParameter("@PostCode",entity.PostCode),
                new OleDbParameter("@LinkName",entity.LinkName),
                new OleDbParameter("@Address",entity.Address),
                new OleDbParameter("@LinkMobile",entity.LinkMobile),
                new OleDbParameter("@LinkTel",entity.LinkTel),
                new OleDbParameter("@OrderProductJson1",entity.OrderProductJson1),
                new OleDbParameter("@OrderProductJson2",entity.OrderProductJson2),
                new OleDbParameter("@OrderProductJson3",entity.OrderProductJson3),
                new OleDbParameter("@OrderProductJson4",entity.OrderProductJson4),
                new OleDbParameter("@OrderProductJson5",entity.OrderProductJson5),
                new OleDbParameter("@OrderProductJson6",entity.OrderProductJson6),
                new OleDbParameter("@OrderProductJson7",entity.OrderProductJson7),
                new OleDbParameter("@OrderStatus",(int)entity.OrderStatus),
                new OleDbParameter("@TotalFee",entity.TotalFee),
                new OleDbParameter("@AddTime",entity.AddTime),
                new OleDbParameter("@DeelTime",entity.DeelTime),
                new OleDbParameter("@PicRemittanceAdvice",entity.PicRemittanceAdvice),
                new OleDbParameter("@PicInvoice",entity.PicInvoice),
                new OleDbParameter("@PicListItem",entity.PicListItem),
                new OleDbParameter("@PicBookingnote",entity.PicBookingnote)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
            sql = "SELECT MAX([ID]) FROM Chebao_Order";
            return DataConvert.SafeInt(SqlHelper.ExecuteScalar(_con, CommandType.Text, sql));
        }

        public override void UpdateOrderProducts(OrderInfo entity)
        {
            string sql = @"
                UPDATE Chebao_Order SET 
                    [TotalFee] = @TotalFee
                    ,[OrderProductJson1] = @OrderProductJson1
                    ,[OrderProductJson2] = @OrderProductJson2
                    ,[OrderProductJson3] = @OrderProductJson3
                    ,[OrderProductJson4] = @OrderProductJson4
                    ,[OrderProductJson5] = @OrderProductJson5
                    ,[OrderProductJson6] = @OrderProductJson6
                    ,[OrderProductJson7] = @OrderProductJson7
                WHERE [ID] = @ID
            ";
            OleDbParameter[] p = 
            { 
                new OleDbParameter("@TotalFee",entity.TotalFee),
                new OleDbParameter("@OrderProductJson1",entity.OrderProductJson1),
                new OleDbParameter("@OrderProductJson2",entity.OrderProductJson2),
                new OleDbParameter("@OrderProductJson3",entity.OrderProductJson3),
                new OleDbParameter("@OrderProductJson4",entity.OrderProductJson4),
                new OleDbParameter("@OrderProductJson5",entity.OrderProductJson5),
                new OleDbParameter("@OrderProductJson6",entity.OrderProductJson6),
                new OleDbParameter("@OrderProductJson7",entity.OrderProductJson7),
                new OleDbParameter("@ID",entity.ID),
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
        }

        public override List<OrderInfo> GetOrderList()
        {
            List<OrderInfo> list = new List<OrderInfo>();
            string sql = "SELECT * FROM Chebao_Order";
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateOrder(reader));
                }
            }

            return list;
        }

        public override OrderInfo GetOrderInfo(int id)
        {
            OrderInfo entity = new OrderInfo();
            string sql = "SELECT * FROM Chebao_Order WHERE [ID] = @ID";
            OleDbParameter[] p = 
            {
                new OleDbParameter("@ID",id)
            };
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql,p))
            {
                if (reader.Read())
                {
                    entity = PopulateOrder(reader);
                }
            }
            return entity;
        }

        public override void UpdateOrderStatus(string ids, OrderStatus status,string username)
        {
            string sql = "UPDATE Chebao_Order SET [OrderStatus] = @OrderStatus,[DeelTime]=@DeelTime,[StatusUpdateUser]=@StatusUpdateUser WHERE ID IN(" + ids + ")";
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, new OleDbParameter[] { new OleDbParameter("@OrderStatus", (int)status), new OleDbParameter("@DeelTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), new OleDbParameter("@StatusUpdateUser",username) });
        }

        public override void UpdateOrderPic(int id, string src,string action)
        {
            string column = string.Empty;
            switch (action)
            {
                case "remittanceadvice":
                    column = "[PicRemittanceAdvice]";
                    break;
                case "invoice":
                    column = "[PicInvoice]";
                    break;
                case "listitem":
                    column = "[PicListItem]";
                    break;
                case "bookingnote":
                    column = "[PicBookingnote]";
                    break;
                default:
                    break;
            }
            string sql = "UPDATE Chebao_Order SET " + column + " = @Src WHERE ID =" + id;
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, new OleDbParameter("@Src", src));
        }

        public override void UpdateOrderSyncStatus(int id, int status)
        {
            string sql = "UPDATE Chebao_Order SET [SyncStatus] = @SyncStatus WHERE [ID] = @ID";
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, new OleDbParameter[] { new OleDbParameter("@SyncStatus", status), new OleDbParameter("@ID", id) });
        }

        public override void AddOrderUpdateQueue(OrderUpdateQueueInfo entity)
        {
            string sql = @"
                INSERT INTO Chebao_OrderUpdateQueue(
                    [OrderID]
                    ,[OrderStatus]
                    ,[DeelStatus]
                )VALUES(
                    @OrderID
                    ,@OrderStatus
                    ,@DeelStatus
                )";

            OleDbParameter[] p = 
            { 
                new OleDbParameter("@OrderID",entity.OrderID),
                new OleDbParameter("@OrderStatus",entity.OrderStatus),
                new OleDbParameter("@DeelStatus",entity.DeelStatus)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
        }

        public override void UpdateOrderUpdateQueueStatus(int id, int status)
        {
            string sql = "UPDATE Chebao_OrderUpdateQueue SET [DeelStatus] = @DeelStatus WHERE [ID] = @ID";
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, new OleDbParameter[] { new OleDbParameter("@DeelStatus", status), new OleDbParameter("@ID", id) });
        }

        public override List<OrderUpdateQueueInfo> GetOrderUpdateQueue()
        {
            List<OrderUpdateQueueInfo> list = new List<OrderUpdateQueueInfo>();
            string sql = "SELECT * FROM Chebao_OrderUpdateQueue WHERE [DeelStatus] = 0";
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateOrderUpdateQueue(reader));
                }
            }

            return list;
        }

        #endregion

        #region 同步失败记录

        public override void AddSyncfailed(SyncfailedInfo entity)
        {
            string sql = @"
                INSERT INTO Chebao_Syncfailed(
                [Name]
                ,[Amount]
                ,[UserName]
                ,[AType]
                ,[AddTime]
                ,[Status]
                )VALUES(
                @Name
                ,@Amount
                ,@UserName
                ,@AType
                ,@AddTime
                ,@Status
                )
            ";
            OleDbParameter[] p = 
            { 
                new OleDbParameter("@OrderNumber",entity.Name),
                new OleDbParameter("@OrderNumber",entity.Amount),
                new OleDbParameter("@UserName",entity.UserName),
                new OleDbParameter("@UserName",entity.AType),
                new OleDbParameter("@AddTime",entity.AddTime),
                new OleDbParameter("@AddTime",entity.Status)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
        }

        public override List<SyncfailedInfo> GetSyncfailedList()
        {
            List<SyncfailedInfo> list = new List<SyncfailedInfo>();
            string sql = "SELECT * FROM Chebao_Syncfailed WHERE [Status] = 0 ORDER BY [ID] DESC";
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateSyncfailed(reader));
                }
            }

            return list;
        }

        public override void UpdateSyncfailedStatus(string ids)
        {
            string sql = "UPDATE Chebao_Syncfailed SET [Status] = 1 WHERE [ID] IN(" + ids + ")";
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql);
        }

        #endregion

        #region 反馈有奖

        public override void AddMessageBoard(MessageBoardInfo entity)
        {
            string sql = @"
                INSERT INTO Chebao_MessageBoard(
                    [UserID]
                    ,[UserName]
                    ,[AddTime]
                    ,[Title]
                    ,[Content]
                )VALUES(
                    @UserID
                    ,@UserName
                    ,@AddTime
                    ,@Title
                    ,@Content
                )";

            OleDbParameter[] p = 
            { 
                new OleDbParameter("@UserID",entity.UserID),
                new OleDbParameter("@UserName",entity.UserName),
                new OleDbParameter("@AddTime",entity.AddTime),
                new OleDbParameter("@Title",entity.Title),
                new OleDbParameter("@Content",entity.Content)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
        }

        public override List<MessageBoardInfo> GetMessageBoardList()
        {
            List<MessageBoardInfo> list = new List<MessageBoardInfo>();
            string sql = "SELECT * FROM Chebao_MessageBoard";
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateMessageBoard(reader));
                }
            }

            return list;
        }

        public override MessageBoardInfo GetMessageBoard(int id)
        {
            string sql = "select * from Chebao_MessageBoard where [ID]=@ID";
            MessageBoardInfo entity = null;
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql, new OleDbParameter("@ID", id)))
            {
                if (reader.Read())
                {
                    entity = PopulateMessageBoard(reader);
                }
            }
            return entity;
        }

        #endregion

        #region 地区管理

        public override List<ProvinceInfo> GetProvinceList()
        {
            List<ProvinceInfo> list = new List<ProvinceInfo>();
            string sql = "SELECT * FROM Province";
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateProvince(reader));
                }
            }

            return list;
        }

        public override List<CityInfo> GetCityList()
        {
            List<CityInfo> list = new List<CityInfo>();
            string sql = "SELECT * FROM City";
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateCity(reader));
                }
            }

            return list;
        }

        public override List<DistrictInfo> GetDistrictList()
        {
            List<DistrictInfo> list = new List<DistrictInfo>();
            string sql = "SELECT * FROM District";
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateDistrict(reader));
                }
            }

            return list;
        }

        #endregion

        #region 系统设置

        public override void ExecuteSql(string sql)
        {
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql);
        }

        #endregion

        #region 站点设置

        public override void AddSitesetting(SitesettingInfo entity)
        {
            string sql = @"
                INSERT INTO Chebao_SiteSetting(
                    [Notice]
                    ,[CorpIntroduce]
                    ,[Contact]
                    ,[PropertyNames]
                    ,[PropertyValues]
                )VALUES(
                    @Notice
                    ,@CorpIntroduce
                    ,@Contact
                    ,@PropertyNames
                    ,@PropertyValues
                )";

            SerializerData data = entity.GetSerializerData();
            OleDbParameter[] p = 
            { 
                new OleDbParameter("@Notice",entity.Notice),
                new OleDbParameter("@CorpIntroduce",entity.CorpIntroduce),
                new OleDbParameter("@Contact",entity.Contact),
                new OleDbParameter("@PropertyNames",data.Keys),
                new OleDbParameter("@PropertyValues",data.Values)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
        }

        public override void UpdateSitesetting(SitesettingInfo entity)
        {
            string sql = @"
                UPDATE Chebao_SiteSetting SET
                    [Notice] = @Notice
                    ,[CorpIntroduce] = @CorpIntroduce
                    ,[Contact] = @Contact
                    ,[PropertyNames] = @PropertyNames
                    ,[PropertyValues] = @PropertyValues";

            SerializerData data = entity.GetSerializerData();
            OleDbParameter[] p = 
            { 
                new OleDbParameter("@Notice",entity.Notice),
                new OleDbParameter("@CorpIntroduce",entity.CorpIntroduce),
                new OleDbParameter("@Contact",entity.Contact),
                new OleDbParameter("@PropertyNames",data.Keys),
                new OleDbParameter("@PropertyValues",data.Values)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
        }

        public override SitesettingInfo GetSitesetting()
        {
            string sql = "select * from Chebao_SiteSetting";
            SitesettingInfo setting = null;
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                if (reader.Read())
                {
                    setting = PopulateSitesetting(reader);
                }
            }
            return setting;
        }

        #endregion

        #region 折扣模版

        public override List<DiscountStencilInfo> GetDiscountStencilList()
        {

            List<DiscountStencilInfo> list = new List<DiscountStencilInfo>();
            string sql = "SELECT * FROM Chebao_DiscountStencil";
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateDiscountStencil(reader));
                }
            }

            return list;
        }

        public override void DeleteDiscountStencils(string ids)
        {
            string sql = string.Format("DELETE FROM Chebao_DiscountStencil WHERE [ID] IN ({0})", ids);
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql);
        }

        public override int AddDiscountStencil(DiscountStencilInfo entity)
        {
            int result = 0;
            string sql = @"SELECT COUNT(0) FROM Chebao_DiscountStencil WHERE [Name] = @Name";
            OleDbParameter[] p = 
            { 
                new OleDbParameter("@Name",entity.Name)
            };
            if (DataConvert.SafeInt(SqlHelper.ExecuteScalar(_con, CommandType.Text, sql, p)) == 0)
            {
                SerializerData data = entity.GetSerializerData();
                sql = @"
                INSERT INTO Chebao_DiscountStencil(
                    [Name]
                    ,[IsCosts]
                    ,[PropertyNames]
                    ,[PropertyValues]
                )VALUES(
                    @Name
                    ,@IsCosts
                    ,@PropertyNames
                    ,@PropertyValues
                )";

                p = new OleDbParameter[]
                { 
                    new OleDbParameter("@Name",entity.Name),
                    new OleDbParameter("@IsCosts",entity.IsCosts),
                    new OleDbParameter("@PropertyNames",data.Keys),
                    new OleDbParameter("@PropertyValues",data.Values)
                };
                SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
                sql = "SELECT MAX([ID]) FROM Chebao_DiscountStencil";
                result = DataConvert.SafeInt(SqlHelper.ExecuteScalar(_con, CommandType.Text, sql));
            }
            return result;
        }

        public override void UpdateDiscountStencil(DiscountStencilInfo entity)
        {
            string sql = "SELECT COUNT(0) FROM Chebao_DiscountStencil WHERE [Name] = @Name AND [ID] <> @ID";
            OleDbParameter[] p = 
            { 
                new OleDbParameter("@Name",entity.Name),
                new OleDbParameter("@ID",entity.ID)
            };
            if (DataConvert.SafeInt(SqlHelper.ExecuteScalar(_con, CommandType.Text, sql, p)) == 0)
            {
                SerializerData data = entity.GetSerializerData();
                sql = @"
                UPDATE Chebao_DiscountStencil SET
                    [Name] = @Name
                    ,[PropertyNames] = @PropertyNames
                    ,[PropertyValues] = @PropertyValues
                WHERE [ID] = @ID";
                p = new OleDbParameter[]
                { 
                    new OleDbParameter("@Name",entity.Name),
                    new OleDbParameter("@PropertyNames",data.Keys),
                    new OleDbParameter("@PropertyValues",data.Values),
                    new OleDbParameter("@ID",entity.ID)
                };
                SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
            }
        }

        #endregion
    }
}
