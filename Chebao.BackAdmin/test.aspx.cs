using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Chebao.Tools;
using System.Net;
using System.IO;
using System.Text;
using System.IO.Compression;
using Chebao.Components;

namespace Chebao.BackAdmin
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //Response.Write(Server.UrlEncode("http://bj.hongxu.cn/weixin/act.aspx"));
            //string s = string.Empty;
            //s.GetHashCode();
            //Response.Write(FormatNum(""));
            //Cars.Instance.RefreshProductStock();

            Cars.Instance.DeelOrderUpdateQueue();
            //GetPage("http://www.chebao360.com/goods/index.php?cate=23&brands=103&models=1168&outputs=3195&isnews=0&years=2007");
            //Response.Write(Post("oper=comments_and_car_detail&id=1446&comb_goods_id=0", "http://www.chebao360.com/ajax_return.php"));
        }

        public static string GetPage(string url)
        {
            string content = "";
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
            return (content);

        }

        public string Post(string postData, string xhttpUrl)
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
            string data = respStreamReader.ReadToEnd();
            respStreamReader.Close();
            respStream.Close();
            return data;
        }
        
        private string FormatNum(string num)
        {
            string newstr = string.Empty;
            Regex r = new Regex(@"(\d+?)(\d{3})*(\.\d+|$)");
            Match m = r.Match(num); 
            newstr += m.Groups[1].Value;
            for (int i = 0; i < m.Groups[2].Captures.Count; i++)
            {
                newstr += "," + m.Groups[2].Captures[i].Value;
            }
            newstr += m.Groups[3].Value; return newstr;
        }
    }
}