using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chebao.Components
{
    public class OrderUpdateQueueInfo
    {
        public int ID { get; set; }

        public int OrderID { get; set; }

        public OrderStatus OrderStatus { get; set; }

        /// <summary>
        /// 处理状态
        /// <para>0：未处理</para>
        /// <para>1：处理完成</para>
        /// <para>2：处理失败</para>
        /// </summary>
        public int DeelStatus { get; set; }
    }
}
