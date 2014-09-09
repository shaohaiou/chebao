using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chebao.Components
{
    public class BrandInfo
    {
        public int ID { get; set; }

        public string BrandName { get; set; }

        public string NameIndex { get; set; }

        public string BrandNameBind 
        { 
            get 
            {
                return NameIndex + " " + BrandName;
            } 
        }
    }
}
