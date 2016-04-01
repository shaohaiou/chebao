using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chebao.Tools;
using Newtonsoft.Json;

namespace Chebao.Components
{
    [Serializable]
    public class DiscountStencilInfo : ExtendedAttributes
    {
        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 是否成本折扣
        /// </summary>
        [JsonProperty("iscosts")]
        public int IsCosts { get; set; }

        /// <summary>
        /// 折扣m
        /// </summary>
        [JsonIgnore]
        public decimal DiscountM
        {
            get { return GetDecimal("DiscountM", 10); }
            set { SetExtendedAttribute("DiscountM", value.ToString()); }
        }

        /// <summary>
        /// 折扣y
        /// </summary>
        [JsonIgnore]
        public decimal DiscountY
        {
            get { return GetDecimal("DiscountY", 10); }
            set { SetExtendedAttribute("DiscountY", value.ToString()); }
        }

        /// <summary>
        /// 折扣h
        /// </summary>
        [JsonIgnore]
        public decimal DiscountH
        {
            get { return GetDecimal("DiscountH", 10); }
            set { SetExtendedAttribute("DiscountH", value.ToString()); }
        }

        /// <summary>
        /// 附加项W
        /// </summary>
        [JsonIgnore]
        public decimal AdditemW
        {
            get { return GetDecimal("AdditemW", 10); }
            set { SetExtendedAttribute("AdditemW", value.ToString()); }
        }

        /// <summary>
        /// 附加项F
        /// </summary>
        [JsonIgnore]
        public decimal AdditemF
        {
            get { return GetDecimal("AdditemF", 10); }
            set { SetExtendedAttribute("AdditemF", value.ToString()); }
        }

        /// <summary>
        /// 折扣ls
        /// </summary>
        [JsonIgnore]
        public decimal DiscountLS
        {
            get { return GetDecimal("DiscountLS", 10); }
            set { SetExtendedAttribute("DiscountLS", value.ToString()); }
        }

        /// <summary>
        /// 折扣xsp
        /// </summary>
        [JsonIgnore]
        public decimal DiscountXSP
        {
            get { return GetDecimal("DiscountXSP", 10); }
            set { SetExtendedAttribute("DiscountXSP", value.ToString()); }
        }

        /// <summary>
        /// 折扣mt
        /// </summary>
        [JsonIgnore]
        public decimal DiscountMT
        {
            get { return GetDecimal("DiscountMT", 10); }
            set { SetExtendedAttribute("DiscountMT", value.ToString()); }
        }

        /// <summary>
        /// 折扣b
        /// </summary>
        [JsonIgnore]
        public decimal DiscountB
        {
            get { return GetDecimal("DiscountB", 10); }
            set { SetExtendedAttribute("DiscountB", value.ToString()); }
        }

        /// <summary>
        /// 折扣s
        /// </summary>
        [JsonIgnore]
        public decimal DiscountS
        {
            get { return GetDecimal("DiscountS", 10); }
            set { SetExtendedAttribute("DiscountS", value.ToString()); }
        }

        /// <summary>
        /// 折扣k
        /// </summary>
        [JsonIgnore]
        public decimal DiscountK
        {
            get { return GetDecimal("DiscountK", 10); }
            set { SetExtendedAttribute("DiscountK", value.ToString()); }
        }

        /// <summary>
        /// 折扣p
        /// </summary>
        [JsonIgnore]
        public decimal DiscountP
        {
            get { return GetDecimal("DiscountP", 10); }
            set { SetExtendedAttribute("DiscountP", value.ToString()); }
        }
    }
}
