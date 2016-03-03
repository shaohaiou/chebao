using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chebao.Components
{
    public class GlobalKey
    {
        public static readonly string COMMCONFIG = "cache-config-commconfig";                   //主配置文件缓存键值
        public static readonly string PROVIDER = "cache-provider";                              //数据访问层提供类缓存键值
        public static readonly string DEFAULT_PROVDIER_COMMON = "MSSQLCommonDataProvider";//默认通用数据访问层提供类

        public static readonly string EVENTLOG_KEY = "cache-sign-event";
        public static readonly string SESSION_ADMIN = "Session_Admin";//后台用户Session
        public static readonly string MACHINEKEY_COOKIENAME = "HX_MACHINEKEY";  //客户端唯一标示的cookie键值
        public static readonly string CONTEXT_KEY = "ChebaoContext";               //当前上下文键值
        public static readonly string REWRITER_KEY = "cache-config-rewriter";

        public static readonly string BRAND_LIST = "cache-brand";   //品牌缓存键值
        public static readonly string CABMODEL_LIST = "cache-cabmodel";   //车型缓存键值
        public static readonly string PRODUCT_LIST = "cache-product";   //产品缓存键值
        public static readonly string DISCOUNTSTENCIL_LIST = "cache-discountstencil";   //折扣模版缓存键值

        public static readonly string SHOPPINGTROLLEY_LIST = "cache-shoppingtrolley";   //购物车缓存键值
        public static readonly string SHOPPINGTROLLEYADD_KEY = "cache-shoppingtrolleyadd";   //添加到购物车缓存键值
        public static readonly string SELECTEDSIDNUMBER_COOKIENAME = "cookie-selectedsidnumber";   //购物车选择状态cookie数量
        public static readonly string SELECTEDSID_COOKIENAME = "cookie-selectedsid";   //购物车选择状态

        public static readonly string ORDER_LIST = "cache-order";   //订单缓存键值
        public static readonly string ORDERPRODUCT_LIST = "cache-orderproduct";   //订单产品缓存键值

        public static readonly string PROVINCE_LIST = "cache-province";   //省份缓存键值
        public static readonly string CITY_LIST = "cache-city";   //城市缓存键值
        public static readonly string DISTRICT_LIST = "cache-district";   //地区缓存键值

        public static readonly string SEARCHCABMODELID = "cache-searchcabmodelid";   //产品搜索id

        public static readonly string SITESETTING_KEY = "cache-sitesetting";   //站点设置缓存键值
    }
}
