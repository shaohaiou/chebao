using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chebao.Tools;
using Newtonsoft.Json;

namespace Chebao.Components
{
    [Serializable]
    public class SitesettingInfo : ExtendedAttributes
    {
        public string Notice { get; set; }

        public string CorpIntroduce { get; set; }

        public string Contact { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [JsonIgnore]
        public string Ext
        {
            get { return GetString("Ext", ""); }
            set { SetExtendedAttribute("Ext", value); }
        }
    }
}
