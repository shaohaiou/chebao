using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chebao.Tools;

namespace Chebao.Components
{
    [Serializable]
    public class ProductMixInfo
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public int SID { get; set; }
        public string Price { get; set; }
        public int Amount { get; set; }
        public string Sum
        {
            get
            {
                return Math.Round(Amount * DataConvert.SafeDecimal(Price),2).ToString();
            }
        }
    }
}
