using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chebao.Components
{
    [Serializable]
    public class OrderProductInfo
    {
        public int SID { get; set; }

        public int ProductID { get; set; }

        public string ProductName { get; set; }

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
                return Math.Round(Price * Amount, 2).ToString();
            }
        }
    }
}
