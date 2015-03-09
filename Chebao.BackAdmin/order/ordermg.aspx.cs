using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using Chebao.Tools;
using System.Text.RegularExpressions;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using System.Text;
using NPOI.SS.UserModel;

namespace Chebao.BackAdmin.order
{
    public partial class ordermg : AdminBase
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (GetString("action") == "complete")
                {
                    Cars.Instance.UpdateOrderStatus(GetString("ids"), OrderStatus.已处理);
                    Cars.Instance.ReloadOrder();
                    Response.Redirect(FromUrl);
                    Response.End();
                }
                BindControler();
                LoadData();
            }
        }

        private void BindControler()
        {
            ddlOrderStatus.DataSource = EnumExtensions.ToTable<OrderStatus>();
            ddlOrderStatus.DataTextField = "Name";
            ddlOrderStatus.DataValueField = "Value";
            ddlOrderStatus.DataBind();
            ddlOrderStatus.Items.Insert(0, new ListItem("-订单状态-", "-1"));
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

            List<OrderInfo> list = Cars.Instance.GetOrderList(true);
            if (!string.IsNullOrEmpty(GetString("ordernumber")))
            {
                string ordernumber = GetString("ordernumber");
                list = list.FindAll(l => l.OrderNumber.IndexOf(ordernumber) >= 0);
            }
            if (!string.IsNullOrEmpty(GetString("username")))
            {
                string username = GetString("username").ToLower();
                list = list.FindAll(l => l.UserName.ToLower().IndexOf(username) >= 0);
            }
            if (!string.IsNullOrEmpty(GetString("linkname")))
            {
                string linkname = GetString("linkname").ToLower();
                list = list.FindAll(l => l.LinkName.ToLower().IndexOf(linkname) >= 0);
            }
            if (GetInt("orderstatus", -1) >= 0)
            {
                OrderStatus orderstatus = (OrderStatus)GetInt("orderstatus", -1);
                list = list.FindAll(l => l.OrderStatus == orderstatus);
            }

            list = list.OrderByDescending(l => l.ID).ThenByDescending(l => l.OrderStatus).ToList();
            total = list.Count();
            list = list.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<OrderInfo>();

            rptData.DataSource = list;
            rptData.DataBind();

            search_fy.RecordCount = total;
            search_fy.PageSize = pagesize;
            search_fy.CustomInfoHTML = string.Format("<span style=\"line-height: 34px;\">总记录数：{0} 总页数：{1}</span>", total, search_fy.PageCount);

            if (GetInt("orderstatus", -1) >= 0)
            {
                SetSelectedByValue(ddlOrderStatus, GetString("orderstatus"));
            }
            if (!string.IsNullOrEmpty(GetString("ordernumber")))
            {
                txtOrderNumber.Text = GetString("ordernumber");
            }
            if (!string.IsNullOrEmpty(GetString("username")))
            {
                txtUserName.Text = GetString("username");
            }
            if (!string.IsNullOrEmpty(GetString("linkname")))
            {
                txtLinkName.Text = GetString("linkname");
            }
        }

        protected string GetOrderProductsStr(object orderproducts)
        {
            string result = string.Empty;
            List<OrderProductInfo> list = orderproducts as List<OrderProductInfo>;
            if (list != null)
            {
                result = string.Join("<br />", list.Select(p => p.Amount.ToString() + " × " + string.Format("<a target=\"_blank\" href=\"../product/productview.aspx?id={1}\" style=\"text-decoration: underline;\" title=\"{2}\">{0}</a><span class=\"gray\">(单价：{3})</span>", StrHelper.GetFuzzyChar(p.ProductName, 5), p.ProductID, p.ProductName, StrHelper.FormatMoney(p.Price.ToString()))).ToList());
            }

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
            List<OrderInfo> list = Cars.Instance.GetOrderList(true);
            if (!string.IsNullOrEmpty(GetString("ordernumber")))
            {
                string ordernumber = GetString("ordernumber");
                list = list.FindAll(l => l.OrderNumber.IndexOf(ordernumber) >= 0);
            }
            if (!string.IsNullOrEmpty(GetString("username")))
            {
                string username = GetString("username").ToLower();
                list = list.FindAll(l => l.UserName.ToLower().IndexOf(username) >= 0);
            }
            if (!string.IsNullOrEmpty(GetString("linkname")))
            {
                string linkname = GetString("linkname").ToLower();
                list = list.FindAll(l => l.LinkName.ToLower().IndexOf(linkname) >= 0);
            }
            if (GetInt("orderstatus", -1) >= 0)
            {
                OrderStatus orderstatus = (OrderStatus)GetInt("orderstatus", -1);
                list = list.FindAll(l => l.OrderStatus == orderstatus);
            }
            list = list.OrderByDescending(l => l.ID).ThenByDescending(l => l.OrderStatus).ToList();

            InitializeWorkbook();
            HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet1");
            HSSFRow rowHeader = (HSSFRow)sheet1.CreateRow(0);
            rowHeader.CreateCell(0).SetCellValue("订单号");
            rowHeader.CreateCell(1).SetCellValue("订单金额");
            rowHeader.CreateCell(2).SetCellValue("订单状态");
            rowHeader.CreateCell(3).SetCellValue("订单产品");
            rowHeader.CreateCell(4).SetCellValue("收货地址");
            rowHeader.CreateCell(5).SetCellValue("联系人");
            rowHeader.CreateCell(6).SetCellValue("手机");
            rowHeader.CreateCell(7).SetCellValue("电话");
            sheet1.SetColumnWidth(0, 22 * 256);
            sheet1.SetColumnWidth(1, 11 * 256);
            sheet1.SetColumnWidth(2, 8 * 256);
            sheet1.SetColumnWidth(3, 60 * 256);
            sheet1.SetColumnWidth(4, 45 * 256);
            sheet1.SetColumnWidth(5, 8 * 256);
            sheet1.SetColumnWidth(6, 15 * 256);
            sheet1.SetColumnWidth(7, 15 * 256);

            List<ProductInfo> plist = Cars.Instance.GetProductList(true);

            for (int i = 0; i < list.Count; i++)
            {
                StringBuilder strbProducts = new StringBuilder();
                foreach (OrderProductInfo p in list[i].OrderProducts)
                {
                    strbProducts.AppendLine(string.Format("{0} × {1} 单价：{2} Lamda型号:{3} 规格：{4}", p.Amount, p.ProductName, Math.Round(p.Price, 2), p.ModelNumber,p.Standard));
                }

                HSSFRow row = (HSSFRow)sheet1.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(list[i].OrderNumber);
                row.CreateCell(1).SetCellValue(StrHelper.FormatMoney(list[i].TotalFee));
                row.CreateCell(2).SetCellValue(list[i].OrderStatus.ToString());
                ICell icell = row.CreateCell(3);
                ICellStyle cellStyle = hssfworkbook.CreateCellStyle();
                cellStyle.WrapText = true;
                icell.CellStyle = cellStyle;
                icell.SetCellValue(strbProducts.ToString());
                row.CreateCell(4).SetCellValue(string.Format("{0} {1} {2} {3}", list[i].Province, list[i].City, list[i].District, list[i].Address));
                row.CreateCell(5).SetCellValue(list[i].LinkName);
                row.CreateCell(6).SetCellValue(list[i].LinkMobile);
                row.CreateCell(7).SetCellValue(list[i].LinkTel);
            }

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                hssfworkbook.Write(ms);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode("订单数据", Encoding.UTF8).ToString() + ".xls");
                Response.BinaryWrite(ms.ToArray());
                Response.End();
                hssfworkbook = null;
            }
        }

        #endregion
    }
}