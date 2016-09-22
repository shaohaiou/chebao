using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chebao.Tools;

namespace Chebao.Components
{
    public class UserStockChangeInfo
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public string UserName { get; set; }

        /// <summary>
        /// 父帐号ID
        /// </summary>
        public int ParentUserID { get; set; }

        /// <summary>
        /// 出入库
        /// <para>0：出库；1：入库</para>
        /// </summary>
        public int Action { get; set; }

        /// <summary>
        /// 审核状态
        /// <para>0：未审核；1审核通过；2：审核不通过</para>
        /// </summary>
        public int CheckStatus { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public string AddTime { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public string CheckUser { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public string CheckTime { get; set; }

        /// <summary>
        /// 出入库说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 系统备注
        /// </summary>
        public string SysRemark { get; set; }

        public string OrderProductJson { get; set; }

        public List<OrderProductInfo> OrderProducts { get; set; }
    }
}
