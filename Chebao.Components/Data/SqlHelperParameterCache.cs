using System;
using System.Data;
using System.Xml;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;

namespace Chebao.Components.Data
{
    public class SqlHelperParameterCache
    {
        #region 私有方法，变量，跟构造函数

        private SqlHelperParameterCache() { }

        private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 返回存储过程中的参数信息
        /// </summary>
        /// <param name="connection">数据库连接对象</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="includeReturnValueParameter">是否返回需要返回的参数@RETURN_VALUE</param>
        /// <returns>返回的参数数组.</returns>
        private static OleDbParameter[] DiscoverSpParameterSet(OleDbConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null) throw new ArgumentNullException("没有提供数据库连接对象");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("存储过程名");

            OleDbCommand cmd = new OleDbCommand(spName, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            connection.Open();
            OleDbCommandBuilder.DeriveParameters(cmd);
            connection.Close();

            if (!includeReturnValueParameter)
            {
                cmd.Parameters.RemoveAt(0);
            }

            OleDbParameter[] discoveredParameters = new OleDbParameter[cmd.Parameters.Count];

            cmd.Parameters.CopyTo(discoveredParameters, 0);

            foreach (OleDbParameter discoveredParameter in discoveredParameters)
            {
                discoveredParameter.Value = DBNull.Value;
            }
            return discoveredParameters;
        }

        /// <summary>
        /// 复制一份参数数组
        /// </summary>
        /// <param name="originalParameters">参数数组</param>
        /// <returns>参数数组</returns>
        private static OleDbParameter[] CloneParameters(OleDbParameter[] originalParameters)
        {
            OleDbParameter[] clonedParameters = new OleDbParameter[originalParameters.Length];

            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (OleDbParameter)((ICloneable)originalParameters[i]).Clone();
            }

            return clonedParameters;
        }

        #endregion

        #region 缓存方法

        /// <summary>
        /// 将参数数组添加到缓存
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供OleDbCommand命令中用到的参数列表</param>
        public static void CacheParameterSet(string connectionString, string commandText, params OleDbParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            string hashKey = connectionString + ":" + commandText;

            paramCache[hashKey] = commandParameters;
        }

        /// <summary>
        /// 从缓存里获取参数数组
        /// </summary>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <returns>参数数组</returns>
        public static OleDbParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("没有提供数据库连接字符串");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("没有提供存储过程的名字或者 T-SQL 语句");

            string hashKey = connectionString + ":" + commandText;

            OleDbParameter[] cachedParameters = paramCache[hashKey] as OleDbParameter[];
            if (cachedParameters == null)
            {
                return null;
            }
            else
            {
                return CloneParameters(cachedParameters);
            }
        }

        #endregion caching functions

        #region 获取参数

        /// <summary>
        /// 通过存储过程名跟连接字符串获取缓存中的参数数组
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="spName">存储过程名</param>
        /// <returns>参数数组</returns>
        public static OleDbParameter[] GetSpParameterSet(string connectionString, string spName)
        {
            return GetSpParameterSet(connectionString, spName, false);
        }

        /// <summary>
        /// 通过存储过程名跟连接字符串获取缓存中的参数数组
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="includeReturnValueParameter">是否返回输出参数</param>
        /// <returns>参数数组</returns>
        public static OleDbParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("没有提供数据库连接字符串");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("没有提供存储过程名");

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                return GetSpParameterSetInternal(connection, spName, includeReturnValueParameter);
            }
        }

        /// <summary>
        /// 通过数据库连接对象获取缓存中的参数数组
        /// </summary>
        /// <param name="connection">数据库连接对象</param>
        /// <param name="spName">存储过程名</param>
        /// <returns>参数数组</returns>
        internal static OleDbParameter[] GetSpParameterSet(OleDbConnection connection, string spName)
        {
            return GetSpParameterSet(connection, spName, false);
        }

        /// <summary>
        /// 通过数据库连接对象获取缓存中的参数数组
        /// </summary>
        /// <param name="connection">数据库连接对象</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="includeReturnValueParameter">是否返回输出参数</param>
        /// <returns>参数数组</returns>
        internal static OleDbParameter[] GetSpParameterSet(OleDbConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null) throw new ArgumentNullException("没有提供数据库连接对象");
            using (OleDbConnection clonedConnection = (OleDbConnection)((ICloneable)connection).Clone())
            {
                return GetSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter);
            }
        }

        /// <summary>
        /// 通过数据库连接对象获取缓存中的参数数组
        /// </summary>
        /// <param name="connection">数据库连接对象</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="includeReturnValueParameter">是否返回输出参数</param>
        /// <returns>参数数组</returns>
        private static OleDbParameter[] GetSpParameterSetInternal(OleDbConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null) throw new ArgumentNullException("没有提供数据库连接对象");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("没有提供存储过程名");

            string hashKey = connection.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");

            OleDbParameter[] cachedParameters;

            cachedParameters = paramCache[hashKey] as OleDbParameter[];
            if (cachedParameters == null)
            {
                OleDbParameter[] spParameters = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);
                paramCache[hashKey] = spParameters;
                cachedParameters = spParameters;
            }

            return CloneParameters(cachedParameters);
        }

        #endregion
    }
}
