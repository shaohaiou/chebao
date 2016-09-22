using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace Chebao.Components.Data
{
    public class CommonPageSql
    {
        /// <summary>
        /// 分页获取数据列表 适用于SQL2000、SQL2005和SQL2008
        /// </summary>
        /// <param name="con">数据库连接字符串</param>
        /// <param name="pageindex">页索引 从0开始</param>
        /// <param name="pagesize">每页记录数</param>
        /// <param name="query">查询接口</param>
        /// <param name="p">输出参数</param>
        /// <returns>DataRead数据集</returns>
        public static IDataReader GetDataReaderByPager(string con, int pageindex, int pagesize, IQuery query, out OleDbParameter p)
        {
            string where = query.BulidQuery();
            string cmd = "";
            if (pageindex == 1)
            {
                cmd = "SELECT TOP " + pagesize + " * FROM " + query.TableName + (string.IsNullOrEmpty(where) ? string.Empty : (" WHERE " + where)) + " ORDER BY" + query.OrderBy;
            }
            else
            {
                cmd = "SELECT TOP " + pagesize + " * FROM " + query.TableName + "WHERE " + (string.IsNullOrEmpty(where) ? string.Empty : (where + " AND ")) + "ID " + (query.OrderBy.ToLower().IndexOf("asc") > 0 ? ">" : "<") + @"
                    (SELECT MIN(ID) 
                    FROM (SELECT TOP " + pagesize * (pageindex - 1) + @" * FROM " + query.TableName + (string.IsNullOrEmpty(where) ? string.Empty : (" WHERE " + where)) + @" ORDER BY" + query.OrderBy + @") AS T1)
                    ORDER BY" + query.OrderBy;
            }
            IDataReader reader = SqlHelper.ExecuteReader(con, CommandType.Text, cmd);
            p = new OleDbParameter("@RecordCount", 0);
            string sql = "SELECT COUNT(0) FROM " + query.TableName + (string.IsNullOrEmpty(where) ? string.Empty : (" WHERE " + where));
            p.Value = SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
            //OleDbParameter[] para = SqlHelperParameterCache.GetSpParameterSet(con, cmd);
            //para[0].Value = query.Column;
            //para[1].Value = query.TableName;
            //para[2].Value = query.BulidQuery();
            //para[3].Value = query.OrderBy;
            //para[4].Value = "ID";
            //para[5].Value = pageindex;
            //para[6].Value = pagesize;
            //para[7].Direction = ParameterDirection.Output;
            //IDataReader reader = SqlHelper.ExecuteReader(con, CommandType.StoredProcedure, cmd, para);
            //p = para[7];
            return reader;
        }

        /// <summary>
        /// 分页获取数据列表 适用于SQL2000、SQL2005和SQL2008
        /// </summary>
        /// <param name="con">数据库连接字符串</param>
        /// <param name="pageindex">页索引 从0开始</param>
        /// <param name="pagesize">每页记录数</param>
        /// <param name="query">查询接口</param>
        /// <param name="p">输出参数</param>
        /// <returns>DataTable数据集</returns>
        public static DataTable GetDataByPager(string con, int pageindex, int pagesize, IQuery query, out OleDbParameter p)
        {
            string cmd = "RecordFromPage";
            OleDbParameter[] para = SqlHelperParameterCache.GetSpParameterSet(con, cmd);
            para[0].Value = query.Column;
            para[1].Value = query.TableName;
            para[2].Value = query.BulidQuery();
            para[3].Value = query.OrderBy;
            para[4].Value = "ID";
            para[5].Value = pageindex;
            para[6].Value = pagesize;
            para[7].Direction = ParameterDirection.Output;
            DataTable datatable = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, cmd, para).Tables[0];
            p = para[7];
            return datatable;
        }
    }
}
