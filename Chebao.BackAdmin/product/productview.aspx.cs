using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using Chebao.Tools;

namespace Chebao.BackAdmin.product
{
    public partial class productview : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
        }

        private ProductInfo product = null;
        public ProductInfo Product
        {
            get
            {
                if (product == null)
                {
                    List<ProductInfo> productlist = Cars.Instance.GetProductList(true);
                    int id = GetInt("id");
                    if (id > 0)
                    {
                        product = productlist.Find(p=>p.ID == id);
                    }
                }
                return product;
            }
        }

        public CabmodelInfo SearchCabmodel
        {
            get
            {
                if (Session[GlobalKey.SEARCHCABMODELID] != null)
                {
                    int cabid = DataConvert.SafeInt(Session[GlobalKey.SEARCHCABMODELID]);
                    if (cabid > 0)
                    {
                        if(Product != null)
                        {
                            string[] cabmodels = Product.Cabmodels.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                            if (!cabmodels.Contains(cabid.ToString()))
                                return null;
                        }
                        return Cars.Instance.GetCabmodelList(true).Find(c=>c.ID == cabid);
                    }
                }
                return null;
            }
        }

        private List<CabmodelInfo> cabmodellist = null;
        public List<CabmodelInfo> CabmodelList
        {
            get
            {
                if (cabmodellist == null)
                {
                    cabmodellist = Cars.Instance.GetCabmodelList(true);
                }
                return cabmodellist;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            if (Product == null)
            {
                WriteErrorMessage("非法操作", "非法产品ID", "~/product/products.aspx");
                Response.End();
                return;
            }
            if (!string.IsNullOrEmpty(Product.Pics))
            {
                rptPics.DataSource = Product.Pics.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList().Select(s => new KeyValuePair<string, string>(s, s.Replace("_s", "_c"))).ToList();
                rptPics.DataBind();
            }
            string[] cabmodels = Product.Cabmodels.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            rptCabmodels.DataSource = CabmodelList.FindAll(l => cabmodels.Contains(l.ID.ToString()));
            rptCabmodels.DataBind();
        }

        protected string GetFirstPic()
        {
            if (!string.IsNullOrEmpty(Product.Pics))
            {
                if (Product.Pics.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Length > 0)
                {
                    return Product.Pics.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList().Select(s => new KeyValuePair<string, string>(s, s.Replace("_s", "_c"))).ToList()[0].Key;
                }
            }
            return string.Empty;
        }
    }
}