using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chebao.Tools;
using Newtonsoft.Json;

namespace Chebao.Components
{
    public class UserProductInfo
    {
        public int ID { get; set; }

        public int ProductID { get; set; }

        public int UserID { get; set; }

        /// <summary>
        /// 产品组合及库存承载字符串
        /// </summary>
        public string ProductMixStr { get; set; }

        /// <summary>
        /// 产品组合及库存
        /// </summary>
        public List<KeyValuePair<string, int>> ProductMix
        {
            get;
            set;
        }
    }
}
