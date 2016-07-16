using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using Chebao.Tools;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using System.Text;

namespace Chebao.BackAdmin.order
{
    public partial class profitsmg : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            if (ChebaoContext.Current.AdminUser.UserRole != Components.UserRoleType.管理员 || !CheckModulePower("利润查询"))
            {
                Response.Clear();
                Response.Write("您没有权限操作！");
                Response.End();
                return;
            }
        }

        protected decimal TotalFee { get; set; }

        protected decimal CostsTotal { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            int pageindex = GetInt("page", 1);
            if (pageindex < 1)
            {
                pageindex = 1;
            }
            int pagesize = search_fy.PageSize;
            int total = 0;

            List<OrderInfo> list = new List<OrderInfo>();
            if (!string.IsNullOrEmpty(GetString("timeb")) || !string.IsNullOrEmpty(GetString("timee")))
            {
                list = Cars.Instance.GetOrderList(true);
                list = list.FindAll(o => o.OrderStatus == OrderStatus.已发货).ToList();
                DateTime timeb = DateTime.Now;
                if (!string.IsNullOrEmpty(GetString("timeb")) && DateTime.TryParse(GetString("timeb"), out timeb))
                {
                    list = list.FindAll(l => DateTime.Parse(l.DeelTime) >= timeb);
                }
                DateTime timee = DateTime.Now;
                if (!string.IsNullOrEmpty(GetString("timee")) && DateTime.TryParse(GetString("timee"), out timee))
                {
                    list = list.FindAll(l => DateTime.Parse(l.DeelTime) < timee.AddDays(1));
                }
            }
            if (!string.IsNullOrEmpty(GetString("username")))
            {
                list = list.FindAll(l => l.UserName.ToLower().Contains(GetString("username").ToLower()));
            }

            foreach (OrderInfo order in list)
            {
                TotalFee += DataConvert.SafeDecimal(order.TotalFee);
                foreach (OrderProductInfo p in order.OrderProducts)
                {
                    if (p.ProductMixList != null)
                    {
                        foreach (ProductMixInfo m in p.ProductMixList)
                        {
                            CostsTotal += DataConvert.SafeDecimal(m.CostsSum);
                        }
                    }
                }
            }

            list = list.OrderByDescending(l => l.ID).ToList();
            total = list.Count();
            list = list.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<OrderInfo>();

            rptData.DataSource = list;
            rptData.DataBind();

            search_fy.RecordCount = total;
            search_fy.PageSize = pagesize;
            search_fy.CustomInfoHTML = string.Format("<span style=\"line-height: 34px;\">总记录数：{0} 总页数：{1}</span>", total, search_fy.PageCount);

            if (!string.IsNullOrEmpty(GetString("timeb")))
            {
                txtDateB.Text = GetString("timeb");
            }
            if (!string.IsNullOrEmpty(GetString("timee")))
            {
                txtDateE.Text = GetString("timee");
            }
            if (!string.IsNullOrEmpty(GetString("username")))
            {
                txtUserName.Text = GetString("username");
            }
        }

        protected void rptData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                decimal totalcosts = 0;
                OrderInfo order = (OrderInfo)e.Item.DataItem;

                foreach (OrderProductInfo p in order.OrderProducts)
                {
                    if (p.ProductMixList != null)
                    {
                        foreach (ProductMixInfo m in p.ProductMixList)
                        {
                            totalcosts += DataConvert.SafeDecimal(m.CostsSum);
                        }
                    }
                }

                Label lblProfits = (Label)e.Item.FindControl("lblProfits");
                lblProfits.Text = StrHelper.FormatMoney((DataConvert.SafeDecimal(order.TotalFee) - totalcosts).ToString());
            }
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
            List<OrderInfo> list = new List<OrderInfo>();
            if (!string.IsNullOrEmpty(GetString("timeb")) || !string.IsNullOrEmpty(GetString("timee")))
            {
                list = Cars.Instance.GetOrderList(true);
                list = list.FindAll(o => o.OrderStatus == OrderStatus.已发货).ToList();
                DateTime timeb = DateTime.Now;
                if (!string.IsNullOrEmpty(GetString("timeb")) && DateTime.TryParse(GetString("timeb"), out timeb))
                {
                    list = list.FindAll(l => DateTime.Parse(l.DeelTime) >= timeb);
                }
                DateTime timee = DateTime.Now;
                if (!string.IsNullOrEmpty(GetString("timee")) && DateTime.TryParse(GetString("timee"), out timee))
                {
                    list = list.FindAll(l => DateTime.Parse(l.DeelTime) < timee.AddDays(1));
                }
            }
            if (!string.IsNullOrEmpty(GetString("username")))
            {
                list = list.FindAll(l => l.UserName.ToLower().Contains(GetString("username").ToLower()));
            }
            foreach (OrderInfo order in list)
            {
                TotalFee += DataConvert.SafeDecimal(order.TotalFee);
                foreach (OrderProductInfo p in order.OrderProducts)
                {
                    if (p.ProductMixList != null)
                    {
                        foreach (ProductMixInfo m in p.ProductMixList)
                        {
                            CostsTotal += DataConvert.SafeDecimal(m.CostsSum);
                        }
                    }
                }
            }

            list = list.OrderByDescending(l => l.ID).ToList();

            InitializeWorkbook();
            HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet1");
            HSSFRow rowTop = (HSSFRow)sheet1.CreateRow(0);
            rowTop.CreateCell(0).SetCellValue("利润合计：");
            rowTop.CreateCell(1).SetCellValue(StrHelper.FormatMoney((TotalFee - CostsTotal).ToString()));
            HSSFRow rowHeader = (HSSFRow)sheet1.CreateRow(1);
            rowHeader.CreateCell(0).SetCellValue("订单号");
            rowHeader.CreateCell(1).SetCellValue("发货时间");
            rowHeader.CreateCell(2).SetCellValue("用户名");
            rowHeader.CreateCell(3).SetCellValue("订单总额");
            rowHeader.CreateCell(4).SetCellValue("利润");
            sheet1.SetColumnWidth(0, 22 * 256);
            sheet1.SetColumnWidth(1, 18 * 256);
            sheet1.SetColumnWidth(2, 12 * 256);
            sheet1.SetColumnWidth(3, 14 * 256);
            sheet1.SetColumnWidth(4, 14 * 256);

            for (int i = 0; i < list.Count; i++)
            {
                HSSFRow row = (HSSFRow)sheet1.CreateRow(i + 2);
                row.CreateCell(0).SetCellValue(list[i].OrderNumber);
                row.CreateCell(1).SetCellValue(list[i].DeelTime);
                row.CreateCell(2).SetCellValue(list[i].UserName);
                row.CreateCell(3).SetCellValue(StrHelper.FormatMoney(list[i].TotalFee).ToString());
                decimal totalcosts = 0;
                foreach (OrderProductInfo p in list[i].OrderProducts)
                {
                    if (p.ProductMixList != null)
                    {
                        foreach (ProductMixInfo m in p.ProductMixList)
                        {
                            totalcosts += DataConvert.SafeDecimal(m.CostsSum);
                        }
                    }
                }
                row.CreateCell(4).SetCellValue(StrHelper.FormatMoney((DataConvert.SafeDecimal(list[i].TotalFee) - totalcosts).ToString()));
            }

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                hssfworkbook.Write(ms);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode("利润查询", Encoding.UTF8).ToString() + ".xls");
                Response.BinaryWrite(ms.ToArray());
                Response.End();
                hssfworkbook = null;
            }
        }

        #endregion
    }
}