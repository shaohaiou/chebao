using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using System.Text;
using Chebao.Tools;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace Chebao.BackAdmin.global
{
    public partial class dealdata : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            if (ChebaoContext.Current.AdminUser.UserRole != Components.UserRoleType.管理员 || !CheckModulePower("数据处理"))
            {
                Response.Clear();
                Response.Write("您没有权限操作！");
                Response.End();
                return;
            }
        }
        private string brandindex = "a,b,c,d,f,g,h,j,k,l,m,n,o,p,q,r,s,t,w,x,y,z"; 

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindControler();
            }
        }

        private void BindControler()
        {
            cblBrandindex.DataSource = brandindex.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(c => new KeyValuePair<string, string>(c, c)).ToList();
            cblBrandindex.DataTextField = "Value";
            cblBrandindex.DataValueField = "Key";
            cblBrandindex.DataBind();
            foreach (ListItem item in cblBrandindex.Items)
            {
                item.Selected = true;
            }
        }

        #region 产品名称重置

        public void btnSubmit_Click(object sender, EventArgs e)
        {

            List<CabmodelInfo> cabmodellist = Cars.Instance.GetCabmodelList(true);
            List<ProductInfo> productlist = Cars.Instance.GetProductList(true);

            foreach (ProductInfo p in productlist.FindAll(p => !string.IsNullOrEmpty(p.Cabmodels)))
            {
                string[] cabmodels = p.Cabmodels.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                p.Name = string.Join("、", cabmodellist.FindAll(c => cabmodels.Contains(c.ID.ToString())).Select(c => c.BrandName).ToList().Distinct())
                    + " " + p.ProductType.ToString();
                p.NameIndex = StrHelper.ConvertE(p.Name).Substring(0, 1).ToUpper();

                Cars.Instance.UpdateProduct(p);
            }
            Cars.Instance.ReloadProductListCache();

            StringBuilder sb = new StringBuilder();
            sb.Append("<span class=\"dalv\">执行完成！</span><br />");
            WriteMessage("~/message/showmessage.aspx", "执行完成！", sb.ToString(), "", string.IsNullOrEmpty(FromUrl) ? "~/global/dealdata.aspx" : FromUrl);
        }

        #endregion

        #region 车型图片采集

        public void btnGatherCabmodelimg_Click(object sender, EventArgs e)
        {
            List<CabmodelInfo> cabmodellist = Cars.Instance.GetCabmodelList(true);

            Dictionary<string,string> brandindexs = new Dictionary<string,string>();
            foreach (ListItem item in cblBrandindex.Items)
            {
                if (item.Selected)
                    brandindexs.Add(item.Text,item.Value);
            }
            if (brandindexs.Count == 0) return;
            string urlbrands = "http://www.chebao360.com/goods/index.php?cate=23";
            string regstrbrands = "<span id=\"carbrand\">[\\s\\S]+?</span>";
            Regex regbrands = new Regex(regstrbrands);
            string htmlbrands = GetPage(urlbrands);

            if (regbrands.IsMatch(htmlbrands))
            {
                string strbrands = regbrands.Match(htmlbrands).Groups[0].Value;
                string regstroption = "<option value=\"([\\d]+)\">([\\s\\S]+?)</option>";
                Regex regbrand = new Regex(regstroption);
                if (regbrand.IsMatch(strbrands))
                {
                    foreach (Match mbrand in regbrand.Matches(strbrands))
                    {
                        int brandid = DataConvert.SafeInt(mbrand.Groups[1].Value);
                        string brandname = mbrand.Groups[2].Value.Split(new char[]{' '},StringSplitOptions.RemoveEmptyEntries)[1];
                        string brandindex = mbrand.Groups[2].Value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
                        if (!cabmodellist.Exists(c => c.BrandName == brandname.Trim()))
                            continue;
                        if (!brandindexs.Keys.Contains(brandindex.ToLower())) continue;
                        if (brandid > 0)
                        {
                            string urlmodels = "http://www.chebao360.com/getmodel.php?id=" + brandid;
                            string strmodles = GetPage(urlmodels);
                            if (!string.IsNullOrEmpty(strmodles))
                            {
                                Regex regmodel = new Regex(regstroption);
                                if (regmodel.IsMatch(strmodles))
                                {
                                    foreach (Match mmodel in regmodel.Matches(strmodles))
                                    {
                                        int modelid = DataConvert.SafeInt(mmodel.Groups[1].Value);
                                        string modelname = mmodel.Groups[2].Value;
                                        if (!cabmodellist.Exists(c => c.BrandName == brandname.Trim() && c.CabmodelName == modelname.Trim()))
                                            continue;
                                        if (modelid > 0)
                                        {
                                            string urloutputs = "http://www.chebao360.com/getoutput.php?id=" + modelid;
                                            string stroutputs = GetPage(urloutputs);
                                            if (!string.IsNullOrEmpty(stroutputs))
                                            {
                                                Regex regoutput = new Regex(regstroption);
                                                if (regoutput.IsMatch(stroutputs))
                                                {
                                                    foreach (Match moutput in regoutput.Matches(stroutputs))
                                                    {
                                                        int outputid = DataConvert.SafeInt(moutput.Groups[1].Value);
                                                        string outputname = moutput.Groups[2].Value;
                                                        if (!cabmodellist.Exists(c => c.BrandName == brandname.Trim() && c.CabmodelName == modelname.Trim() && c.Pailiang.ToLower().Replace(" ", string.Empty) == outputname.ToLower().Replace(" ", string.Empty)))
                                                            continue;
                                                        if (outputid > 0)
                                                        {
                                                            string urlyears = "http://www.chebao360.com/getyear.php?id=" + outputid +"&vars=" + outputname;
                                                            string stryears = GetPage(urlyears);
                                                            if (!string.IsNullOrEmpty(stryears))
                                                            {
                                                                Regex regyear = new Regex(regstroption);
                                                                if (regyear.IsMatch(stryears))
                                                                {
                                                                    foreach (Match myear in regyear.Matches(stryears))
                                                                    {
                                                                        int yearid = DataConvert.SafeInt(myear.Groups[1].Value);
                                                                        string yearname = myear.Groups[2].Value;
                                                                        if (!cabmodellist.Exists(c => c.BrandName == brandname.Trim() && c.CabmodelName == modelname.Trim() && c.Pailiang.ToLower().Replace(" ", string.Empty) == outputname.ToLower().Replace(" ", string.Empty) && c.Nianfen == yearname.Trim()))
                                                                            continue;
                                                                        if (yearid > 0)
                                                                        {
                                                                            CabmodelInfo model = cabmodellist.Find(c => c.BrandName == brandname.Trim() && c.CabmodelName == modelname.Trim() && c.Pailiang.ToLower().Replace(" ", string.Empty) == outputname.ToLower().Replace(" ", string.Empty) && c.Nianfen == yearname.Trim());
                                                                            if (!string.IsNullOrEmpty(model.Imgpath)) continue;
                                                                            string urlsearch = "http://www.chebao360.com/goods/index.php?cate=23&brands=" + brandid +"&models=" + modelid +"&outputs=" + outputid +"&isnews=0&years=" + yearid;
                                                                            GetPage(urlsearch);
                                                                            string urlcarphotoinfo = "http://www.chebao360.com/ajax_return.php";
                                                                            string strcarphotoinfo = Post("oper=comments_and_car_detail&id=1446&comb_goods_id=0",urlcarphotoinfo);
                                                                            string regstrcarphotoinfo = "<img src=\"([\\s\\S]+)\"/><span class=\"car_name\">([\\s\\S]+)</span>";
                                                                            Regex regcarphotoinfo = new Regex(regstrcarphotoinfo);
                                                                            if (regcarphotoinfo.IsMatch(strcarphotoinfo))
                                                                            { 
                                                                                Match mcarphotoinfo = regcarphotoinfo.Match(strcarphotoinfo);
                                                                                string imgurl = mcarphotoinfo.Groups[1].Value;
                                                                                string carinfo = mcarphotoinfo.Groups[2].Value;
                                                                                if (!string.IsNullOrEmpty(imgurl) && !string.IsNullOrEmpty(carinfo))
                                                                                {
                                                                                    string hashimgurl = imgurl.GetHashCode().ToString();
                                                                                    string checkimgurl = "_" + hashimgurl + ".jpg";
                                                                                    string imgpath = "/upload/" + new Regex("[^\\d\\w]").Replace(carinfo, string.Empty) + checkimgurl;
                                                                                    if (cabmodellist.Exists(c => !string.IsNullOrEmpty(c.Imgpath) && c.Imgpath.EndsWith(checkimgurl)))
                                                                                        imgpath = cabmodellist.Find(c => !string.IsNullOrEmpty(c.Imgpath) && c.Imgpath.EndsWith(checkimgurl)).Imgpath;
                                                                                    else
                                                                                        Http.DownloadFile(imgurl, Server.MapPath(imgpath),30 * 1000);
                                                                                    //CabmodelInfo model = cabmodellist.Find(c => c.BrandName == brandname.Trim() && c.CabmodelName == modelname.Trim() && c.Pailiang.ToLower().Replace(" ", string.Empty) == outputname.ToLower().Replace(" ", string.Empty) && c.Nianfen == yearname.Trim());
                                                                                    model.Imgpath = imgpath;
                                                                                    Cars.Instance.UpdateCabmodel(model);
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Cars.Instance.ReloadCabmodelListCache();

            StringBuilder sb = new StringBuilder();
            sb.Append("<span class=\"dalv\">执行完成！</span><br />");
            WriteMessage("~/message/showmessage.aspx", "执行完成！", sb.ToString(), "", string.IsNullOrEmpty(FromUrl) ? "~/global/dealdata.aspx" : FromUrl);
        }

        public static string GetPage(string url,int trytimes = 1)
        {
            string content = "";

            try
            {
                HttpWebRequest myHttpWebRequest1 = (HttpWebRequest)WebRequest.Create(url);
                myHttpWebRequest1.KeepAlive = false;
                myHttpWebRequest1.Headers.Set(HttpRequestHeader.Cookie, "PHPSESSID=546roafo66oe7bcie308180lq2; Hm_lvt_c52426cad20f662492dfc1577d7f5afb=1408322249; Hm_lpvt_c52426cad20f662492dfc1577d7f5afb=1408339880; _ga=GA1.2.1435850320.1408332150");
                HttpWebResponse myHttpWebResponse1;

                myHttpWebResponse1 = (HttpWebResponse)myHttpWebRequest1.GetResponse();
                System.Text.Encoding utf8 = System.Text.Encoding.UTF8;
                Stream streamResponse = myHttpWebResponse1.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse, utf8);
                Char[] readBuff = new Char[256];
                int count = streamRead.Read(readBuff, 0, 256);
                while (count > 0)
                {
                    String outputData = new String(readBuff, 0, count);
                    content += outputData;
                    count = streamRead.Read(readBuff, 0, 256);
                }
                myHttpWebResponse1.Close();
            }
            catch
            {
                if (++trytimes <= 3)
                    content = GetPage(url, trytimes);
            }
            return content;

        }

        public string Post(string postData, string xhttpUrl,int trytimes = 1)
        {
            string data = string.Empty;

            try
            {
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(xhttpUrl);
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                httpWReq.Method = "POST";
                httpWReq.AllowAutoRedirect = false;
                httpWReq.Headers.Set(HttpRequestHeader.Cookie, "PHPSESSID=546roafo66oe7bcie308180lq2;");
                Stream reqStream = httpWReq.GetRequestStream();
                StreamWriter reqStreamWrite = new StreamWriter(reqStream);
                byte[] pdata = Encoding.Default.GetBytes(postData);
                //char[] cpara = new ASCIIEncoding().GetChars(pdata);  
                char[] cpara = new char[pdata.Length];
                for (int i = 0; i < pdata.Length; i++)
                { cpara[i] = System.Convert.ToChar(pdata[i]); }
                reqStreamWrite.Write(cpara, 0, cpara.Length);
                reqStreamWrite.Close();
                reqStream.Close();
                HttpWebResponse httpWResp = (HttpWebResponse)httpWReq.GetResponse();
                Stream respStream = httpWResp.GetResponseStream();
                StreamReader respStreamReader = new StreamReader(respStream, System.Text.Encoding.UTF8);
                data = respStreamReader.ReadToEnd();
                respStreamReader.Close();
                respStream.Close();
            }
            catch 
            {
                if (++trytimes <= 3)
                    data = Post(postData, xhttpUrl, trytimes);
            }

            return data;
        }
        
        #endregion

        #region 重命名车型图片

        public void btnRenameImage_Click(object sender, EventArgs e)
        {
            string dirpath = Server.MapPath("/a/");
            if (Directory.Exists(dirpath))
            {
                Regex regfilename = new Regex(@"(\d{4})([\s\S]+)_([\s\S]+)(\.\w+)");
                FileInfo[] files = new DirectoryInfo(dirpath).GetFiles();
                foreach (FileInfo finfo in files)
                {
                    if (regfilename.IsMatch(finfo.Name))
                    {
                        Match m= regfilename.Match(finfo.Name);
                        string year = m.Groups[1].Value;
                        string namestr = m.Groups[2].Value;
                        string ext = m.Groups[4].Value;
                        string newfilename = dirpath+ namestr + "_" + year + ext;
                        finfo.MoveTo(newfilename);
                    }
                }
            }
        }

        #endregion

        #region 产品型号升阶

        public void btnProductModelUpdate_Click(object sender, EventArgs e)
        {
            List<ProductInfo> productlist = Cars.Instance.GetProductList(false);

            foreach (ProductInfo p in productlist)
            {
                if (p.ModelNumber.StartsWith("A") && DataConvert.SafeInt(p.ModelNumber.Replace("A",string.Empty)) > 0)
                {
                    p.ModelNumber = "A" + DataConvert.SafeInt(p.ModelNumber.Replace("A", string.Empty)).ToString("0000");

                    Cars.Instance.UpdateProduct(p);
                }
            }
            Cars.Instance.ReloadProductListCache();
        }

        #endregion

        #region 同步所有产品数据


        protected void btnSyncProduct_Click(object sender, EventArgs e)
        {
            Cars.Instance.RefreshProductStock(true);
        }

        #endregion

        #region 产品价格去符号

        protected void btnProductPriceRecover_Click(object sender, EventArgs e)
        {
            Regex r = new Regex(@"^\d.*$");
            List<ProductInfo> productlist = Cars.Instance.GetProductList(true);

            foreach (ProductInfo p in productlist)
            {
                if (!r.IsMatch(p.Price) && p.Price.Length > 0)
                {
                    p.Price = p.Price.Substring(1);   
                }
                if (!r.IsMatch(p.XSPPrice) && p.XSPPrice.Length > 0)
                {
                    p.XSPPrice = p.XSPPrice.Substring(1);   
                }

                Cars.Instance.UpdateProduct(p);
            }
            Cars.Instance.ReloadProductListCache();

            StringBuilder sb = new StringBuilder();
            sb.Append("<span class=\"dalv\">执行完成！</span><br />");
            WriteMessage("~/message/showmessage.aspx", "执行完成！", sb.ToString(), "", string.IsNullOrEmpty(FromUrl) ? "~/global/dealdata.aspx" : FromUrl);
        }

        #endregion
    }
}