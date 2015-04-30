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
        /// 公告内容
        /// </summary>
        [JsonIgnore]
        public string NoticeDetail
        {
            get { return GetString("NoticeDetail", ""); }
            set { SetExtendedAttribute("NoticeDetail", value); }
        }
    }
}
