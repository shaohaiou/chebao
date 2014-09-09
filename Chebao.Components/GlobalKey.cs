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
        public static readonly string PRODUCT_LIST = "cache-cabmodel";   //产品缓存键值

        public static readonly string SEARCHCABMODELID = "cache-searchcabmodelid";   //产品搜索id
    }
}
