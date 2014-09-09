using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chebao.Components
{
    public class CabmodelInfo
    {
        public int ID { get; set; }

        public string CabmodelName { get; set; }

        public int BrandID { get; set; }

        public string NameIndex { get; set; }

        public string Pailiang { get; set; }

        public string Nianfen { get; set; }

        public string BrandName { get; set; }

        public string Imgpath { get; set; }

        public string CabmodelNameBind
        {
            get
            {
                return Nianfen.ToString() + " " + BrandName + " " + CabmodelName + " " + Pailiang;
            }
        }
    }
}
