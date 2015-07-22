using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chebao.Tools;
using Newtonsoft.Json;

namespace Chebao.Components
{
    [Serializable]
    public class AdminInfo : ExtendedAttributes
    {
        #region 属性

        /// <summary>
        /// 用户ID
        /// </summary>
        [JsonProperty("ID")]
        public int ID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [JsonProperty("UserName")]
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [JsonProperty("Password")]
        public string Password { get; set; }

        /// <summary>
        /// 用户是否是超级管理员
        /// </summary>
        [JsonProperty("Administrator")]
        public bool Administrator { get; set; }

        /// <summary>
        /// 最后登录IP
        /// </summary>
        [JsonProperty("LastLoginIP")]
        public string LastLoginIP { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [JsonProperty("LastLoginTime")]
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        [JsonProperty("UserRole")]
        public UserRoleType UserRole { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [JsonIgnore]
        public string Mobile
        {
            get { return GetString("Mobile", ""); }
            set { SetExtendedAttribute("Mobile", value); }
        }

        /// <summary>
        /// 联系电话
        /// </summary>
        [JsonIgnore]
        public string TelPhone
        {
            get { return GetString("TelPhone", ""); }
            set { SetExtendedAttribute("TelPhone", value); }
        }

        /// <summary>
        /// 省份
        /// </summary>
        [JsonIgnore]
        public string Province
        {
            get { return GetString("Province", ""); }
            set { SetExtendedAttribute("Province", value); }
        }

        /// <summary>
        /// 城市
        /// </summary>
        [JsonIgnore]
        public string City
        {
            get { return GetString("City", ""); }
            set { SetExtendedAttribute("City", value); }
        }

        /// <summary>
        /// 县区
        /// </summary>
        [JsonIgnore]
        public string District
        {
            get { return GetString("District", ""); }
            set { SetExtendedAttribute("District", value); }
        }

        /// <summary>
        /// 地址
        /// </summary>
        [JsonIgnore]
        public string Address
        {
            get { return GetString("Address", ""); }
            set { SetExtendedAttribute("Address", value); }
        }

        /// <summary>
        /// 邮政编码
        /// </summary>
        [JsonIgnore]
        public string PostCode
        {
            get { return GetString("PostCode", ""); }
            set { SetExtendedAttribute("PostCode", value); }
        }

        /// <summary>
        /// 联系人姓名
        /// </summary>
        [JsonIgnore]
        public string LinkName
        {
            get { return GetString("LinkName", ""); }
            set { SetExtendedAttribute("LinkName", value); }
        }

        /// <summary>
        /// 到期日期
        /// </summary>
        [JsonIgnore]
        public DateTime ValidDate
        {
            get { return GetDateTime("ValidDate",DateTime.MaxValue); }
            set { SetExtendedAttribute("ValidDate", value.ToShortDateString()); }
        }

        /// <summary>
        /// 登录次数
        /// </summary>
        [JsonIgnore]
        public int LoginTimes
        {
            get { return GetInt("LoginTimes", 0); }
            set { SetExtendedAttribute("LoginTimes", value.ToString()); }
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

        /// <summary>
        /// 尺寸查询权限
        /// </summary>
        [JsonIgnore]
        public int SizeView
        {
            get { return GetInt("SizeView", 0); }
            set { SetExtendedAttribute("SizeView", value.ToString()); }
        }

        /// <summary>
        /// 是否子账户
        /// </summary>
        [JsonIgnore]
        public int IsSubAccount
        {
            get { return GetInt("IsSubAccount", 0); }
            set { SetExtendedAttribute("IsSubAccount", value.ToString()); }
        }

        /// <summary>
        /// 父账户ID
        /// </summary>
        [JsonIgnore]
        public int ParentAccountID
        {
            get { return GetInt("ParentAccountID", 0); }
            set { SetExtendedAttribute("ParentAccountID", value.ToString()); }
        }

        /// <summary>
        /// 溢价比例
        /// </summary>
        public decimal SubDiscount
        {
            get { return GetInt("SubDiscount", 0); }
            set { SetExtendedAttribute("SubDiscount", value.ToString()); }
        }

        /// <summary>
        /// 用户明文密码
        /// </summary>
        [JsonProperty("PasswordText")]
        public string PasswordText
        {
            get { return GetString("PasswordText", ""); }
            set { SetExtendedAttribute("PasswordText", value); }
        }

        /// <summary>
        /// 是否可以添加子账户
        /// </summary>
        [JsonIgnore]
        public int IsAddAccount
        {
            get { return GetInt("IsAddAccount", 0); }
            set { SetExtendedAttribute("IsAddAccount", value.ToString()); }
        }

        #endregion Model
    }
}
