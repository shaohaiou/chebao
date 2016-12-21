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
            get
            {
                List<KeyValuePair<string, int>> result = new List<KeyValuePair<string, int>>();

                if (!string.IsNullOrEmpty(ProductMixStr))
                {
                    List<ProductInfo> plist = Cars.Instance.GetProductList(true);
                    foreach (string pmstr in ProductMixStr.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string name = pmstr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];
                        if (plist.Exists(p => name.StartsWith(p.ModelNumber) && name.ToLower().IndexOf("xsp") < 0))
                            result.Add(new KeyValuePair<string, int>(pmstr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0], DataConvert.SafeInt(pmstr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[1])));
                    }
                }

                return result;
            }
        }
    }
}
