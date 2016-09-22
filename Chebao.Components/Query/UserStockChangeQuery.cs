using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chebao.Components;

namespace Chebao.Components
{
    public class UserStockChangeQuery : IQuery
    {
        private string _column = "*";
        private string _tableName = "Chebao_UserStockChange";
        private string _orderby = " ID desc";

        #region IQuery 成员

        /// <summary>
        /// 需要返回的列
        /// </summary>
        public string Column
        {
            get
            {
                return _column;
            }
            set
            {
                _column = value;
            }
        }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            get
            {
                return _tableName;
            }
            set
            {
                _tableName = value;
            }
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderBy
        {
            get
            {
                return _orderby;
            }
            set
            {
                _orderby = value;
            }
        }
        public string UserName { get; set; }
        public int? CheckStatus { get; set; }
        /// <summary>
        /// 生成where
        /// </summary>
        /// <returns></returns>
        public string BulidQuery()
        {
            List<string> query = new List<string>();
            if (!string.IsNullOrEmpty(UserName))
            {
                query.Add(string.Format("[UserName]='{0}'", UserName));
            }
            if (CheckStatus.HasValue)
            {
                query.Add(string.Format("[CheckStatus]={0}", CheckStatus.Value));
            }
            return string.Join(" AND ", query);

        }

        /// <summary>
        /// 生成sql
        /// </summary>
        /// <returns></returns>
        public string BulidSelect(string where, string tableName = "")
        {
            return string.Empty;
        }
        #endregion
    }
}
