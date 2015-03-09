using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chebao.Tools;
using Newtonsoft.Json;

namespace Chebao.Components
{
    [Serializable]
    public class ProductInfo : ExtendedAttributes
    {
        public ProductInfo Clone()
        {
            ProductInfo entity = new ProductInfo() 
            { 
                ID = ID,
                ProductType = ProductType,
                Cabmodels = Cabmodels,
                Introduce = Introduce,
                Stock = Stock,
                Pic = Pic,
                Pics = Pics,
                Name = Name,
                NameIndex = NameIndex,
                Price = Price,
                ModelNumber = ModelNumber,
                OEModelNumber = OEModelNumber,
                Area = Area,
                Material = Material,
                Replacement = Replacement,
                Standard = Standard
            };
            return entity;
        }

        [JsonProperty("id")]
        public int ID { get; set; }

        /// <summary>
        /// 产品类型
        /// </summary>
        [JsonProperty("producttype")]
        public ProductType ProductType { get; set; }

        /// <summary>
        /// 适合的车型
        /// </summary>
        [JsonProperty("cabmodels")]
        public string Cabmodels { get; set; }

        /// <summary>
        /// 产品介绍
        /// </summary>
        [JsonProperty("introduce")]
        public string Introduce { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        [JsonProperty("stock")]
        public int Stock { get; set; }

        /// <summary>
        /// 购物车ID
        /// </summary>
        public int SID { get; set; }

        private string pic;
        /// <summary>
        /// 产品图片（列表）
        /// </summary>
        [JsonProperty("pic")]
        public string Pic
        {
            get
            {
                return string.IsNullOrEmpty(pic) ? "/images/fm.jpg" : pic;
            }
            set
            {
                pic = value;
            }
        }

        /// <summary>
        /// 产品图片（详细页）
        /// </summary>
        [JsonProperty("pics")]
        public string Pics { get; set; }

        public string NameOrder
        {
            get
            {
                return string.IsNullOrEmpty(Name.Trim()) ? "Z" : (string.IsNullOrEmpty(NameIndex) ? "Z" : NameIndex);
            }
        }

        /// <summary>
        /// 产品名称
        /// </summary>
        [JsonIgnore]
        public string Name
        {
            get { return GetString("Name", ""); }
            set { SetExtendedAttribute("Name", value); }
        }

        /// <summary>
        /// 产品名称排序
        /// </summary>
        [JsonIgnore]
        public string NameIndex
        {
            get { return GetString("NameIndex", ""); }
            set { SetExtendedAttribute("NameIndex", value); }
        }

        /// <summary>
        /// 产品价格
        /// </summary>
        [JsonIgnore]
        public string Price
        {
            get { return GetString("Price", ""); }
            set { SetExtendedAttribute("Price", value); }
        }

        /// <summary>
        /// Lamda型号
        /// </summary>
        [JsonIgnore]
        public string ModelNumber
        {
            get { return GetString("ModelNumber", ""); }
            set { SetExtendedAttribute("ModelNumber", value); }
        }

        /// <summary>
        /// OE型号
        /// </summary>
        [JsonIgnore]
        public string OEModelNumber
        {
            get { return GetString("OEModelNumber", ""); }
            set { SetExtendedAttribute("OEModelNumber", value); }
        }

        /// <summary>
        /// 产地
        /// </summary>
        [JsonIgnore]
        public string Area
        {
            get { return GetString("Area", ""); }
            set { SetExtendedAttribute("Area", value); }
        }

        /// <summary>
        /// 材质
        /// </summary>
        [JsonIgnore]
        public string Material
        {
            get { return GetString("Material", ""); }
            set { SetExtendedAttribute("Material", value); }
        }

        /// <summary>
        /// 建议更换周期
        /// </summary>
        [JsonIgnore]
        public string Replacement
        {
            get { return GetString("Replacement", ""); }
            set { SetExtendedAttribute("Replacement", value); }
        }

        /// <summary>
        /// 规格
        /// </summary>
        [JsonIgnore]
        public string Standard
        {
            get { return GetString("Standard", ""); }
            set { SetExtendedAttribute("Standard", value); }
        }
    }
}
