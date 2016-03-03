using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chebao.Components;

namespace Chebao.Components
{
    public class OrderInfo
    {
        public int ID { get; set; }

        public string OrderNumber { get; set; }

        public string UserName { get; set; }

        public int UserID { get; set; }

        public string Address { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public string PostCode { get; set; }

        public string LinkName { get; set; }

        public string LinkMobile { get; set; }

        public string LinkTel { get; set; }

        public string OrderProductJson { get; set; }

        public List<OrderProductInfo> OrderProducts { get; set; }

        public string OrderProductJson1
        {
            set;
            get;
        }

        public string OrderProductJson2
        {
            set;
            get;
        }

        public string OrderProductJson3
        {
            set;
            get;
        }

        public string OrderProductJson4
        {
            set;
            get;
        }

        public string OrderProductJson5
        {
            set;
            get;
        }

        public string OrderProductJson6
        {
            set;
            get;
        }

        public string OrderProductJson7
        {
            set;
            get;
        }

        public OrderStatus OrderStatus { get; set; }

        public string TotalFee { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public string AddTime { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public string DeelTime { get; set; }

        /// <summary>
        /// 汇款单图片
        /// </summary>
        public string PicRemittanceAdvice { get; set; }

        /// <summary>
        /// 发货单图片
        /// </summary>
        public string PicInvoice { get; set; }

        /// <summary>
        /// 清单图片
        /// </summary>
        public string PicListItem { get; set; }

        /// <summary>
        /// 托运单
        /// </summary>
        public string PicBookingnote { get; set; }

        /// <summary>
        /// 同步状态
        /// <para>0：未处理；1：已处理</para>
        /// </summary>
        public int SyncStatus { get; set; }
    }
}
