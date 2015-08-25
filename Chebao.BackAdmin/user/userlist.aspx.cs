using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Tools;
using Chebao.Components;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using System.Text;

namespace Chebao.BackAdmin.user
{
    public partial class userlist : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            if (ChebaoContext.Current.AdminUser.UserRole != Components.UserRoleType.管理员)
            {
                Response.Clear();
                Response.Write("您没有权限操作！");
                Response.End();
                return;
            }
        }

        private List<AdminInfo> _userlist = null;
        public List<AdminInfo> UserList 
        { 
            get 
            {
                if (_userlist == null)
                    _userlist = Admins.Instance.GetUsers();
                return _userlist;
            }   
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (WebHelper.GetString("action") == "del")
                {
                    Admins.Instance.DeleteAdmin(WebHelper.GetInt("id"));
                    ResponseRedirect(FromUrl);
                }
                else
                {
                    int pageindex = GetInt("page", 1);
                    if (pageindex < 1)
                    {
                        pageindex = 1;
                    }
                    int pagesize = GetInt("pagesize", 10);
                    int total = 0;
                    List<AdminInfo> adminlist = UserList.FindAll(a=>a.ParentAccountID == 0);
                    if (!string.IsNullOrEmpty(GetString("username")))
                        adminlist = adminlist.FindAll(l => l.UserName.ToLower().IndexOf(GetString("username").ToLower()) >= 0);
                    if(!string.IsNullOrEmpty(GetString("linkname")))
                        adminlist = adminlist.FindAll(l => l.LinkName.ToLower().IndexOf(GetString("linkname").ToLower()) >= 0);

                    total = adminlist.Count();
                    adminlist = adminlist.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<AdminInfo>();
                    rpadmin.DataSource = adminlist;
                    rpadmin.DataBind();
                    search_fy.RecordCount = total;
                    search_fy.PageSize = pagesize;

                    if (!string.IsNullOrEmpty(GetString("username")))
                        txtUserName.Text = GetString("username");
                    if (!string.IsNullOrEmpty(GetString("linkname")))
                        txtLinkName.Text = GetString("linkname");
                }
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            Response.Redirect("userlist.aspx?username=" + txtUserName.Text + "&linkname=" + txtLinkName.Text);
        }

        protected string GetSubAccount(object o)
        {
            string result = string.Empty;
            int id = DataConvert.SafeInt(o);

            if (id > 0 && UserList.Exists(l => l.ParentAccountID == id))
                result = "<a class=\"subaccount\" href=\"subaccountmg.aspx?id=" + id + "&from=" + UrlEncode(CurrentUrl) +"\">子用户</a>（" + UserList.FindAll(l => l.ParentAccountID == id).Count + "）";

            return result;
        }

        #region 导出Excel

        static HSSFWorkbook hssfworkbook;

        public void InitializeWorkbook()
        {
            hssfworkbook = new HSSFWorkbook();
            ////create a entry of DocumentSummaryInformation
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "耐磨达";
            hssfworkbook.DocumentSummaryInformation = dsi;
            ////create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "xxx";
            hssfworkbook.SummaryInformation = si;
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            List<AdminInfo> adminlist = Admins.Instance.GetUsers();
            if (!string.IsNullOrEmpty(GetString("username")))
                adminlist = adminlist.FindAll(l => l.UserName.IndexOf(GetString("username")) >= 0);
            if (!string.IsNullOrEmpty(GetString("linkname")))
                adminlist = adminlist.FindAll(l => l.LinkName.IndexOf(GetString("linkname")) >= 0);
            InitializeWorkbook();
            HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet1");
            HSSFRow rowHeader = (HSSFRow)sheet1.CreateRow(0);
            rowHeader.CreateCell(0).SetCellValue("用户名");
            rowHeader.CreateCell(1).SetCellValue("联系人");
            rowHeader.CreateCell(2).SetCellValue("手机");
            rowHeader.CreateCell(3).SetCellValue("电话");
            rowHeader.CreateCell(4).SetCellValue("联系地址");
            rowHeader.CreateCell(5).SetCellValue("邮政编码");
            rowHeader.CreateCell(6).SetCellValue("有效期至");
            sheet1.SetColumnWidth(0, 15 * 256);
            sheet1.SetColumnWidth(1, 9 * 256);
            sheet1.SetColumnWidth(2, 15 * 256);
            sheet1.SetColumnWidth(3, 15 * 256);
            sheet1.SetColumnWidth(4, 40 * 256);
            sheet1.SetColumnWidth(5, 15 * 256);
            sheet1.SetColumnWidth(6, 15 * 256);

            for (int i = 0; i < adminlist.Count; i++)
            {
                HSSFRow row = (HSSFRow)sheet1.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(adminlist[i].UserName);
                row.CreateCell(1).SetCellValue(adminlist[i].LinkName);
                row.CreateCell(2).SetCellValue(adminlist[i].Mobile);
                row.CreateCell(3).SetCellValue(adminlist[i].TelPhone);
                row.CreateCell(4).SetCellValue(string.Format("{0} {1} {2} {3}", adminlist[i].Province, adminlist[i].City, adminlist[i].District, adminlist[i].Address));
                row.CreateCell(5).SetCellValue(adminlist[i].PostCode);
                row.CreateCell(6).SetCellValue(adminlist[i].ValidDate.ToString("yyyyMMdd") == DateTime.MaxValue.ToString("yyyyMMdd") ? "无限制" : (adminlist[i].ValidDate > DateTime.Today ? adminlist[i].ValidDate.ToString("yyyy年MM月dd日") : "已过期"));
            }

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                hssfworkbook.Write(ms);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode("用户数据", Encoding.UTF8).ToString() + ".xls");
                Response.BinaryWrite(ms.ToArray());
                Response.End();
                hssfworkbook = null;
            }
        }

        #endregion
    }
}