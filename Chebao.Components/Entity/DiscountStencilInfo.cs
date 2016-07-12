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
        public DiscountStencilInfo() { }

        public DiscountStencilInfo(AdminInfo source)
        {
            DiscountMAdd = source.DiscountMAdd;
            DiscountM = source.DiscountM;
            DiscountYAdd = source.DiscountYAdd;
            DiscountY = source.DiscountY;
            DiscountHAdd = source.DiscountHAdd;
            DiscountH = source.DiscountH;
            DiscountXSPAdd = source.DiscountXSPAdd;
            DiscountXSP = source.DiscountXSP;
            DiscountMTAdd = source.DiscountMTAdd;
            DiscountMT = source.DiscountMT;
            DiscountSAdd = source.DiscountSAdd;
            DiscountS = source.DiscountS;
            DiscountKAdd = source.DiscountKAdd;
            DiscountK = source.DiscountK;
            DiscountPAdd = source.DiscountPAdd;
            DiscountP = source.DiscountP;
            DiscountPYAdd = source.DiscountPYAdd;
            DiscountPY = source.DiscountPY;
            DiscountLSAdd = source.DiscountLSAdd;
            DiscountLS = source.DiscountLS;
            DiscountBAdd = source.DiscountBAdd;
            DiscountB = source.DiscountB;
            AdditemW = source.AdditemW;
            AdditemF = source.AdditemF;
        }

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
        /// 附加m
        /// </summary>
        [JsonIgnore]
        public decimal DiscountMAdd
        {
            get { return GetDecimal("DiscountMAdd", 0); }
            set { SetExtendedAttribute("DiscountMAdd", value.ToString()); }
        }

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
        /// 附加y
        /// </summary>
        [JsonIgnore]
        public decimal DiscountYAdd
        {
            get { return GetDecimal("DiscountYAdd", 0); }
            set { SetExtendedAttribute("DiscountYAdd", value.ToString()); }
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
        /// 附加h
        /// </summary>
        [JsonIgnore]
        public decimal DiscountHAdd
        {
            get { return GetDecimal("DiscountHAdd", 0); }
            set { SetExtendedAttribute("DiscountHAdd", value.ToString()); }
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
        /// 附加ls
        /// </summary>
        [JsonIgnore]
        public decimal DiscountLSAdd
        {
            get { return GetDecimal("DiscountLSAdd", 0); }
            set { SetExtendedAttribute("DiscountLSAdd", value.ToString()); }
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
        /// 附加xsp
        /// </summary>
        [JsonIgnore]
        public decimal DiscountXSPAdd
        {
            get { return GetDecimal("DiscountXSPAdd", 0); }
            set { SetExtendedAttribute("DiscountXSPAdd", value.ToString()); }
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
        /// 附加mt
        /// </summary>
        [JsonIgnore]
        public decimal DiscountMTAdd
        {
            get { return GetDecimal("DiscountMTAdd", 0); }
            set { SetExtendedAttribute("DiscountMTAdd", value.ToString()); }
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
        /// 附加b
        /// </summary>
        [JsonIgnore]
        public decimal DiscountBAdd
        {
            get { return GetDecimal("DiscountBAdd", 0); }
            set { SetExtendedAttribute("DiscountBAdd", value.ToString()); }
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
        /// 附加s
        /// </summary>
        [JsonIgnore]
        public decimal DiscountSAdd
        {
            get { return GetDecimal("DiscountSAdd", 0); }
            set { SetExtendedAttribute("DiscountSAdd", value.ToString()); }
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
        /// 附加k
        /// </summary>
        [JsonIgnore]
        public decimal DiscountKAdd
        {
            get { return GetDecimal("DiscountKAdd", 0); }
            set { SetExtendedAttribute("DiscountKAdd", value.ToString()); }
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
        /// 附加p
        /// </summary>
        [JsonIgnore]
        public decimal DiscountPAdd
        {
            get { return GetDecimal("DiscountPAdd", 0); }
            set { SetExtendedAttribute("DiscountPAdd", value.ToString()); }
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

        /// <summary>
        /// 附加py
        /// </summary>
        [JsonIgnore]
        public decimal DiscountPYAdd
        {
            get { return GetDecimal("DiscountPYAdd", 0); }
            set { SetExtendedAttribute("DiscountPAdd", value.ToString()); }
        }

        /// <summary>
        /// 折扣py
        /// </summary>
        [JsonIgnore]
        public decimal DiscountPY
        {
            get { return GetDecimal("DiscountPY", 10); }
            set { SetExtendedAttribute("DiscountPY", value.ToString()); }
        }
    }
}
