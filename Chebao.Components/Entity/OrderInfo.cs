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
    }
}
