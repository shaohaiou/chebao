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
using System.IO;
using NPOI.XSSF.UserModel;
using System.Drawing;
using NPOI.HSSF.Util;

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
                if (!string.IsNullOrEmpty(GetString("action")))
                {
                    string action = GetString("action");
                    OrderStatus status = OrderStatus.未收款;
                    switch (action)
                    {
                        case "gather":
                            status = OrderStatus.已收款;
                            break;
                        case "consignment":
                            status = OrderStatus.已发货;
                            break;
                        case "cancel":
                            status = OrderStatus.已取消;
                            break;
                        default:
                            status = OrderStatus.未收款;
                            break;
                    }
                    Cars.Instance.UpdateOrderStatus(GetString("ids"), status);
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
            DateTime timeb = DateTime.Now;
            if (!string.IsNullOrEmpty(GetString("timeb")) && DateTime.TryParse(GetString("timeb"), out timeb))
            {
                list = list.FindAll(l => DateTime.Parse(l.AddTime) >= timeb);
            }
            DateTime timee = DateTime.Now;
            if (!string.IsNullOrEmpty(GetString("timee")) && DateTime.TryParse(GetString("timee"), out timee))
            {
                list = list.FindAll(l => DateTime.Parse(l.AddTime) <= timee);
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
            if (!string.IsNullOrEmpty(GetString("timeb")))
            {
                txtDateB.Text = GetString("timeb");
            }
            if (!string.IsNullOrEmpty(GetString("timee")))
            {
                txtDateE.Text = GetString("timee");
            }
        }

        protected string GetOrderProductsStr(object orderproducts)
        {
            string result = string.Empty;
            List<OrderProductInfo> list = orderproducts as List<OrderProductInfo>;
            if (list != null)
            {
                foreach (OrderProductInfo p in list)
                {
                    result += p.ProductName + "<br />";
                    if (p.ProductMixList != null)
                    {
                        foreach (ProductMixInfo pi in p.ProductMixList)
                        {
                            result += string.Format("<span class=\"pl10\">{0}</span>", pi.Amount + " × " + pi.Name + "<span class=\"gray\">(" + pi.Price + ")</span><br />");
                        }
                    }
                }
            }

            return result;
        }

        protected string GetOrderStatusColor(OrderStatus status)
        {
            string result = string.Empty;

            switch (status)
            {
                case OrderStatus.未收款:
                    result = "red";
                    break;
                case OrderStatus.已收款:
                    result = "orange";
                    break;
                case OrderStatus.已发货:
                    result = "green";
                    break;
                case OrderStatus.已取消:
                    result = "gray";
                    break;
                default:
                    result = "gray";
                    break;
            }

            return result;
        }

        #region 导出Excel

        //static HSSFWorkbook hssfworkbook;

        //public void InitializeWorkbook()
        //{
        //    hssfworkbook = new HSSFWorkbook();
        //    ////create a entry of DocumentSummaryInformation
        //    DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
        //    dsi.Company = "耐磨达";
        //    hssfworkbook.DocumentSummaryInformation = dsi;
        //    ////create a entry of SummaryInformation
        //    SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
        //    si.Subject = "xxx";
        //    hssfworkbook.SummaryInformation = si;
        //}

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            List<OrderInfo> list = Cars.Instance.GetOrderList(true);

            if (list.Exists(l => l.ID.ToString() == hdnIds.Value))
            {
                OrderInfo order = list.Find(l => l.ID.ToString() == hdnIds.Value);
                IWorkbook workbook = null;
                ISheet sheet = null;
                string fileName = Utils.GetMapPath(string.Format(@"\App_Data\订单模板.xls"));
                string newfile = "客户订单.xls";
                using (FileStream file = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    workbook = new HSSFWorkbook(file);
                }
                sheet = workbook.GetSheetAt(0);

                sheet.GetRow(1).Cells[1].SetCellValue(order.OrderNumber);
                sheet.GetRow(2).Cells[1].SetCellValue(order.OrderStatus.ToString());
                sheet.GetRow(2).Cells[4].SetCellValue(order.LinkName);
                sheet.GetRow(2).Cells[6].SetCellValue(order.LinkMobile);
                sheet.GetRow(2).Cells[10].SetCellValue(order.LinkTel);
                sheet.GetRow(3).Cells[1].SetCellValue(order.TotalFee);
                sheet.GetRow(3).Cells[4].SetCellValue(order.Province + order.City + order.District + order.Address);

                int index = 6;
                Regex r = new Regex(@"([mhywf]+)");
                foreach (OrderProductInfo p in order.OrderProducts)
                {
                    bool firstmix = true;
                    if (p.ProductMixList.Count > 0)
                    {
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(index, index + p.ProductMixList.Count - 1, 15, 15));
                    }
                    foreach (ProductMixInfo pi in p.ProductMixList)
                    {
                        string ptname = string.Empty;
                        if (pi.Name.ToLower().IndexOf("m") > 0) ptname = "盘式刹车片";
                        else if (pi.Name.ToLower().IndexOf("y") > 0) ptname = "高端陶瓷刹车片";
                        else if (pi.Name.ToLower().IndexOf("h") > 0) ptname = "高端陶瓷（ＯＥ配置）";
                        string[] cabmodel = p.CabmodelStr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        sheet.GetRow(index).Cells[0].SetCellValue(p.ProductName);
                        sheet.GetRow(index).Cells[1].SetCellValue(ptname);
                        sheet.GetRow(index).Cells[2].SetCellValue(p.ModelNumber);
                        sheet.GetRow(index).Cells[3].SetCellValue(pi.Name);
                        if (cabmodel.Length > 0)
                        {
                            string brand = string.Empty;
                            if (cabmodel.Length == 5)
                                brand = cabmodel[2];
                            else if (cabmodel.Length > 5)
                            {
                                for (int i = 2; i < cabmodel.Length - 2; i++)
                                {
                                    brand = brand + cabmodel[i];
                                }
                            }
                            sheet.GetRow(index).Cells[4].SetCellValue(brand);
                            sheet.GetRow(index).Cells[5].SetCellValue(cabmodel[cabmodel.Length - 2]);
                            sheet.GetRow(index).Cells[6].SetCellValue(cabmodel[cabmodel.Length - 1]);
                            sheet.GetRow(index).Cells[7].SetCellValue(cabmodel[1]);
                        }
                        sheet.GetRow(index).Cells[8].SetCellValue(order.UserName);
                        sheet.GetRow(index).Cells[9].SetCellValue(p.ProductType == ProductType.前刹车片 ? "前" : (p.ProductType == ProductType.后刹车片 ? "后" : string.Empty));
                        try
                        {
                            sheet.GetRow(index).Cells[10].SetCellValue(DateTime.Parse(order.AddTime).ToString("yyyy-M-d"));
                        }
                        catch { }
                        sheet.GetRow(index).Cells[11].SetCellValue(!r.IsMatch(pi.Name.ToLower()) ? string.Empty : r.Match(pi.Name.ToLower()).Groups[1].Value.ToUpper());
                        sheet.GetRow(index).Cells[12].SetCellValue(pi.Price);
                        sheet.GetRow(index).Cells[13].SetCellValue(pi.Amount);
                        sheet.GetRow(index).Cells[14].SetCellValue(pi.Sum);
                        if (firstmix)
                        {
                            sheet.GetRow(index).Cells[15].SetCellValue(p.Remark);
                            firstmix = false;
                        }
                        if (pi.Amount == 0)
                        {
                            ICellStyle cellStyle = workbook.CreateCellStyle();
                            IFont font = workbook.CreateFont();
                            font.Color = HSSFColor.Red.Index;
                            cellStyle.SetFont(font);
                            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                            cellStyle.TopBorderColor = HSSFColor.Black.Index;
                            cellStyle.RightBorderColor = HSSFColor.Black.Index;
                            cellStyle.BottomBorderColor = HSSFColor.Black.Index;
                            cellStyle.LeftBorderColor = HSSFColor.Black.Index;
                            for (int i = 0; i <= 14; i++)
                            {
                                sheet.GetRow(index).Cells[i].CellStyle = cellStyle;
                            }
                        }
                        index++;
                    }
                }

                sheet.ForceFormulaRecalculation = true;
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    workbook.Write(ms);
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(newfile, Encoding.UTF8).ToString() + "");
                    Response.BinaryWrite(ms.ToArray());
                    Response.End();
                    workbook = null;
                }
            }

            #region 已删除

            //List<OrderInfo> list = Cars.Instance.GetOrderList(true);
            //if (!string.IsNullOrEmpty(GetString("ordernumber")))
            //{
            //    string ordernumber = GetString("ordernumber");
            //    list = list.FindAll(l => l.OrderNumber.IndexOf(ordernumber) >= 0);
            //}
            //if (!string.IsNullOrEmpty(GetString("username")))
            //{
            //    string username = GetString("username").ToLower();
            //    list = list.FindAll(l => l.UserName.ToLower().IndexOf(username) >= 0);
            //}
            //if (!string.IsNullOrEmpty(GetString("linkname")))
            //{
            //    string linkname = GetString("linkname").ToLower();
            //    list = list.FindAll(l => l.LinkName.ToLower().IndexOf(linkname) >= 0);
            //}
            //if (GetInt("orderstatus", -1) >= 0)
            //{
            //    OrderStatus orderstatus = (OrderStatus)GetInt("orderstatus", -1);
            //    list = list.FindAll(l => l.OrderStatus == orderstatus);
            //}
            //list = list.OrderByDescending(l => l.ID).ThenByDescending(l => l.OrderStatus).ToList();

            //InitializeWorkbook();
            //HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet1");
            //HSSFRow rowHeader = (HSSFRow)sheet1.CreateRow(0);
            //rowHeader.CreateCell(0).SetCellValue("订单号");
            //rowHeader.CreateCell(1).SetCellValue("订单金额");
            //rowHeader.CreateCell(2).SetCellValue("订单状态");
            //rowHeader.CreateCell(3).SetCellValue("订单产品");
            //rowHeader.CreateCell(4).SetCellValue("收货地址");
            //rowHeader.CreateCell(5).SetCellValue("联系人");
            //rowHeader.CreateCell(6).SetCellValue("手机");
            //rowHeader.CreateCell(7).SetCellValue("电话");
            //sheet1.SetColumnWidth(0, 22 * 256);
            //sheet1.SetColumnWidth(1, 11 * 256);
            //sheet1.SetColumnWidth(2, 8 * 256);
            //sheet1.SetColumnWidth(3, 60 * 256);
            //sheet1.SetColumnWidth(4, 45 * 256);
            //sheet1.SetColumnWidth(5, 8 * 256);
            //sheet1.SetColumnWidth(6, 15 * 256);
            //sheet1.SetColumnWidth(7, 15 * 256);

            //List<ProductInfo> plist = Cars.Instance.GetProductList(true);

            //for (int i = 0; i < list.Count; i++)
            //{
            //    StringBuilder strbProducts = new StringBuilder();
            //    foreach (OrderProductInfo p in list[i].OrderProducts)
            //    {
            //        strbProducts.AppendLine(string.Format("{0} × {1} 单价：{2} Lamda型号:{3} 规格：{4}", p.Amount, p.ProductName, Math.Round(p.Price, 2), p.ModelNumber,p.Standard));
            //    }

            //    HSSFRow row = (HSSFRow)sheet1.CreateRow(i + 1);
            //    row.CreateCell(0).SetCellValue(list[i].OrderNumber);
            //    row.CreateCell(1).SetCellValue(StrHelper.FormatMoney(list[i].TotalFee));
            //    row.CreateCell(2).SetCellValue(list[i].OrderStatus.ToString());
            //    ICell icell = row.CreateCell(3);
            //    ICellStyle cellStyle = hssfworkbook.CreateCellStyle();
            //    cellStyle.WrapText = true;
            //    icell.CellStyle = cellStyle;
            //    icell.SetCellValue(strbProducts.ToString());
            //    row.CreateCell(4).SetCellValue(string.Format("{0} {1} {2} {3}", list[i].Province, list[i].City, list[i].District, list[i].Address));
            //    row.CreateCell(5).SetCellValue(list[i].LinkName);
            //    row.CreateCell(6).SetCellValue(list[i].LinkMobile);
            //    row.CreateCell(7).SetCellValue(list[i].LinkTel);
            //}

            //using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            //{
            //    hssfworkbook.Write(ms);
            //    Response.Clear();
            //    Response.Buffer = true;
            //    Response.ContentType = "application/vnd.ms-excel";
            //    Response.ContentEncoding = System.Text.Encoding.UTF8;
            //    Response.AppendHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode("订单数据", Encoding.UTF8).ToString() + ".xls");
            //    Response.BinaryWrite(ms.ToArray());
            //    Response.End();
            //    hssfworkbook = null;
            //}
            #endregion
        }

        #endregion
    }
}