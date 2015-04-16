using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chebao.Tools;

namespace Chebao.Components
{
    [Serializable]
    public class OrderProductInfo
    {
        public int SID { get; set; }

        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public ProductType ProductType { get; set; }

        public string ModelNumber { get; set; }

        public string OEModelNumber { get; set; }

        public string Standard { get; set; }

        public string ProductPic { get; set; }

        public string Introduce { get; set; }

        public decimal Price { get; set; }

        public int Amount { get; set; }

        public string Remark { get; set; }

        public string Sum
        {
            get
            {
                decimal result = 0;

                if (ProductMixList != null && ProductMixList.Count > 0)
                {
                    foreach (ProductMixInfo pm in ProductMixList)
                    {
                        decimal price = DataConvert.SafeDecimal(pm.Price) * pm.Amount;

                        result += price;
                    }
                }

                return Math.Round(result,2).ToString();
            }
        }

        public List<ProductMixInfo> ProductMixList { get; set; }

        public string CabmodelStr { get; set; }

        /// <summary>
        /// 折扣m
        /// </summary>
        public decimal DiscountM
        {
            get;
            set;
        }

        /// <summary>
        /// 折扣y
        /// </summary>
        public decimal DiscountY
        {
            get;
            set;
        }

        /// <summary>
        /// 折扣h
        /// </summary>
        public decimal DiscountH
        {
            get;
            set;
        }

        /// <summary>
        /// 附加项W
        /// </summary>
        public decimal AdditemW
        {
            get;
            set;
        }

        /// <summary>
        /// 附加项F
        /// </summary>
        public decimal AdditemF
        {
            get;
            set;
        }
    }
}
