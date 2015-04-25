using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chebao.Components
{
    public class Sitesettings
    {
        #region 单例
        private static object sync_creater = new object();

        private static Sitesettings _instance;
        public static Sitesettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync_creater)
                    {
                        if (_instance == null)
                            _instance = new Sitesettings();
                    }
                }
                return _instance;
            }
        }

        #endregion

        public void AddSitesetting(SitesettingInfo entity)
        {
            CommonDataProvider.Instance().AddSitesetting(entity);
            ReloadSitesetting();
        }

        public void UpdateSitesetting(SitesettingInfo entity)
        {
            CommonDataProvider.Instance().UpdateSitesetting(entity);
            ReloadSitesetting();
        }

        public SitesettingInfo GetSitesetting(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetSitesetting();

            string key = GlobalKey.SITESETTING_KEY;
            SitesettingInfo setting = MangaCache.Get(key) as SitesettingInfo;
            if (setting == null)
            {
                setting = CommonDataProvider.Instance().GetSitesetting();
                MangaCache.Max(key, setting);
            }
            return setting;
        }

        public void ReloadSitesetting()
        {
            string key = GlobalKey.SITESETTING_KEY;
            MangaCache.Remove(key);
            GetSitesetting(true);
        }
    }
}
