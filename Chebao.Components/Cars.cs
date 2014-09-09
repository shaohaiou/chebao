using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chebao.Components
{
    public class Cars
    {
        #region 单例
        private static object sync_creater = new object();

        private static Cars _instance;
        public static Cars Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync_creater)
                    {
                        if (_instance == null)
                            _instance = new Cars();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region 品牌管理
        
        /// <summary>
        /// 获取品牌列表
        /// </summary>
        /// <param name="fromCache"></param>
        /// <returns></returns>
        public List<BrandInfo> GetBrandList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetBrandList();

            string key = GlobalKey.BRAND_LIST;
            List<BrandInfo> list = MangaCache.Get(key) as List<BrandInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetBrandList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        /// <summary>
        /// 删除品牌
        /// </summary>
        /// <param name="ids"></param>
        public void DeleteBrands(string ids)
        {
            CommonDataProvider.Instance().DeleteBrands(ids);
        }

        public int AddBrand(BrandInfo entity)
        {
            return CommonDataProvider.Instance().AddBrand(entity);
        }

        public BrandInfo GetBrand(int id, bool fromCache = false)
        {
            List<BrandInfo> list = GetBrandList(fromCache);
            return list.Find(b => b.ID == id);
        }

        public void UpdateBrand(BrandInfo entity)
        {
            CommonDataProvider.Instance().UpdateBrand(entity);
        }

        public void ReloadBrandListCache()
        {
            string key = GlobalKey.BRAND_LIST;
            MangaCache.Remove(key);

            GetBrandList(true);
        }

        #endregion

        #region 车型管理

        public List<CabmodelInfo> GetCabmodelList(bool fromCache = false)
        { 
            if(!fromCache)
                return CommonDataProvider.Instance().GetCabmodelList();

            string key = GlobalKey.CABMODEL_LIST;
            List<CabmodelInfo> list = MangaCache.Get(key) as List<CabmodelInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetCabmodelList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public void DeleteCabmodels(string ids)
        {
            CommonDataProvider.Instance().DeleteCabmodels(ids);
        }

        public int AddCabmodel(CabmodelInfo entity)
        {
            return CommonDataProvider.Instance().AddCabmodel(entity);
        }

        public CabmodelInfo GetCabmodel(int id, bool fromCache = false)
        {
            List<CabmodelInfo> list = GetCabmodelList(fromCache);
            return list.Find(b => b.ID == id);
        }

        public void UpdateCabmodel(CabmodelInfo entity)
        {
            CommonDataProvider.Instance().UpdateCabmodel(entity);
        }

        public void ReloadCabmodelListCache()
        {
            string key = GlobalKey.CABMODEL_LIST;
            MangaCache.Remove(key);

            GetCabmodelList(true);
        }

        #endregion

        #region 产品管理

        public List<ProductInfo> GetProductList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetProductList();

            string key = GlobalKey.PRODUCT_LIST;
            List<ProductInfo> list = MangaCache.Get(key) as List<ProductInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetProductList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public void DeletetProducts(string ids)
        {
            CommonDataProvider.Instance().DeleteProducts(ids);
        }

        public int AddProduct(ProductInfo entity)
        {
            return CommonDataProvider.Instance().AddProduct(entity);
        }

        public ProductInfo GetProduct(int id, bool fromCache = false)
        {
            List<ProductInfo> list = GetProductList(fromCache);
            return list.Find(b => b.ID == id);
        }

        public void UpdateProduct(ProductInfo entity)
        {
            CommonDataProvider.Instance().UpdateProduct(entity);
        }

        public void ReloadProductListCache()
        {
            string key = GlobalKey.PRODUCT_LIST;
            MangaCache.Remove(key);

            GetProductList(true);
        }

        #endregion

        #region 系统设置

        public void ExecuteSql(string sql)
        {
            CommonDataProvider.Instance().ExecuteSql(sql);

            ReloadBrandListCache();
            ReloadCabmodelListCache();
            ReloadProductListCache();
        }

        #endregion
    }
}
