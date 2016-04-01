using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using Chebao.Tools;

namespace Chebao.BackAdmin.product
{
    public partial class import : AdminBase
    {
        private static object syncHelper = new object();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(fileImport.Value))
            {
                lblMsg.Text = "请选择文件";
                return;
            }
            DataTable t = ImportDataTableFromExcel(fileImport.PostedFile.InputStream, 0, 0);
            lock (syncHelper)
            {
                List<BrandInfo> brandlist = Cars.Instance.GetBrandList(true);
                List<CabmodelInfo> cabmodellist = Cars.Instance.GetCabmodelList(true);
                List<ProductInfo> productlist = Cars.Instance.GetProductList(true);
                foreach (DataRow row in t.Rows)
                {
                    string brandname = row[0].ToString();
                    string cabmodelname = row[1].ToString();
                    string pailiang = row[2].ToString();
                    string nianfen = row[3].ToString();
                    string[] modelnumber = new string[4] { row[4].ToString().Trim(), row[5].ToString().Trim(), row[6].ToString().Trim(), row[7].ToString().Trim() };
                    int brandid = 0;
                    int cabmodelid = 0;
                    int productid = 0;
                    if (brandlist.Exists(b => b.BrandName == brandname))
                    {
                        brandid = brandlist.Find(b => b.BrandName == brandname).ID;
                    }
                    else
                    {
                        BrandInfo entity = new BrandInfo
                        {
                            BrandName = brandname,
                            NameIndex = StrHelper.ConvertE(brandname).Substring(0, 1).ToUpper()
                        };
                        brandid = Cars.Instance.AddBrand(entity);
                        entity.ID = brandid;
                        brandlist.Add(entity);
                    }
                    if (brandid > 0)
                    {
                        if (!cabmodellist.Exists(c => c.BrandID == brandid && c.CabmodelName == cabmodelname && c.Pailiang == pailiang && c.Nianfen == nianfen))
                        {
                            if (cabmodelname == "车型 (中文)") continue;
                            CabmodelInfo cabmodel = new CabmodelInfo();
                            cabmodel.BrandID = brandid;
                            cabmodel.CabmodelName = cabmodelname;
                            cabmodel.Pailiang = pailiang;
                            cabmodel.Nianfen = nianfen;
                            cabmodel.NameIndex = StrHelper.ConvertE(cabmodel.CabmodelName).Substring(0, 1).ToUpper();

                            cabmodelid = Cars.Instance.AddCabmodel(cabmodel);
                            cabmodel.ID = cabmodelid;
                            cabmodellist.Add(cabmodel);
                        }
                        else
                            cabmodelid = cabmodellist.Find(c => c.BrandID == brandid && c.CabmodelName == cabmodelname && c.Pailiang == pailiang && c.Nianfen == nianfen).ID;
                    }
                    if (cabmodelid > 0)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            if (modelnumber[i] == "对不起") continue;
                            if (string.IsNullOrEmpty(modelnumber[i] + modelnumber[i + 2])) continue;
                            if (!productlist.Exists(p => p.ModelNumber == modelnumber[i] && p.OEModelNumber == modelnumber[i + 2]))
                            {
                                ProductInfo product = new ProductInfo();
                                product.ModelNumber = modelnumber[i];
                                product.OEModelNumber = modelnumber[i + 2];
                                product.ProductType = i == 0 ? ProductType.前刹车片 : ProductType.后刹车片;
                                product.Pic = "/upload/" + product.ModelNumber + ".jpg";
                                product.Pics = "/upload/"+ product.ModelNumber + ".jpg||||";
                                productid = Cars.Instance.AddProduct(product);

                                product.ID = productid;
                                productlist.Add(product);
                            }
                            else
                                productid = productlist.Find(p => p.ModelNumber == modelnumber[i]).ID;

                            if (productid > 0)
                            {
                                ProductInfo product = productlist.Find(p => p.ID == productid);
                                if (product != null)
                                {
                                    if (!string.IsNullOrEmpty(product.Cabmodels))
                                    {
                                        List<string> cabmodels = product.Cabmodels.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                        if (!cabmodels.Exists(s => s == cabmodelid.ToString()))
                                        {
                                            cabmodels.Add(cabmodelid.ToString());
                                            product.Cabmodels = string.Join("|", cabmodels);
                                            Cars.Instance.UpdateProduct(product);
                                        }
                                    }
                                    else
                                    {
                                        product.Cabmodels = cabmodelid.ToString();
                                        Cars.Instance.UpdateProduct(product);
                                    }
                                }
                            }
                        }

                    }
                }
            }

            Cars.Instance.ReloadBrandListCache();
            Cars.Instance.ReloadCabmodelListCache();
            Cars.Instance.ReloadProductListCache();
            WriteSuccessMessage("操作完成", "导入数据成功！", "~/product/import.aspx");
        }

        protected void btnSubmitInfo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(fileImportInfo.Value))
            {
                lblMsg.Text = "请选择文件";
                return;
            }
            DataTable t = ImportDataTableFromExcel(fileImportInfo.PostedFile.InputStream, 0, 0,2);
            lock (syncHelper)
            {
                List<CabmodelInfo> cabmodellist = Cars.Instance.GetCabmodelList(true);
                List<ProductInfo> productlist = Cars.Instance.GetProductList(true);
                foreach (DataRow row in t.Rows)
                {
                    string modelnumber = row[0].ToString();
                    string price = row[1].ToString().Replace("￥",string.Empty);
                    string area = row[2].ToString();
                    string material = row[3].ToString();
                    string replacement = row[4].ToString();

                    if(productlist.Exists(p=>p.ModelNumber == modelnumber.Trim()))
                    {
                        foreach (ProductInfo p in productlist.FindAll(p => p.ModelNumber == modelnumber.Trim()))
                        {
                            if (!string.IsNullOrEmpty(p.Cabmodels))
                            {
                                string[] cabmodels = p.Cabmodels.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                                p.Name = string.Join("、", cabmodellist.FindAll(c => cabmodels.Contains(c.ID.ToString())).Select(c => c.BrandName).ToList().Distinct()) 
                                    + " " + p.ProductType.ToString();
                            }
                            
                            p.Price = price;
                            //p.XSPPrice = price;
                            p.Area = area;
                            p.Material = material;
                            p.Replacement = replacement;
                            Cars.Instance.UpdateProduct(p);
                        }
                    }
                }
            }

            Cars.Instance.ReloadCabmodelListCache();
            WriteSuccessMessage("操作完成", "导入数据成功！", "~/product/import.aspx");
        }

        /// <summary>
        /// 由Excel导入DataTable
        /// </summary>
        /// <param name="excelFileStream">Excel文件流</param>
        /// <param name="sheetName">Excel工作表索引</param>
        /// <param name="headerRowIndex">Excel表头行索引</param>
        /// <returns>DataTable</returns>
        public static DataTable ImportDataTableFromExcel(Stream excelFileStream, int sheetIndex, int headerRowIndex,int headerColIndex = 0)
        {
            HSSFWorkbook workbook = new HSSFWorkbook(excelFileStream);
            HSSFSheet sheet = (HSSFSheet)workbook.GetSheetAt(sheetIndex);
            DataTable table = new DataTable();
            HSSFRow headerRow = (HSSFRow)sheet.GetRow(headerRowIndex);
            int cellCount = headerRow.LastCellNum;
            for (int i = headerColIndex; i < cellCount; i++)
            {
                if (headerRow.GetCell(i) == null || headerRow.GetCell(i).StringCellValue.Trim() == "")
                {
                    // 如果遇到第一个空列，则不再继续向后读取
                    cellCount = i + 1;
                    break;
                }
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                HSSFRow row = (HSSFRow)sheet.GetRow(i);
                if (row == null || row.GetCell(0) == null)
                {
                    // 如果遇到第一个空行，则不再继续向后读取
                    break;
                }
                DataRow dataRow = table.NewRow();
                for (int j = headerColIndex; j < cellCount; j++)
                {
                    dataRow[j - headerColIndex] = row.GetCell(j) == null ? string.Empty : (string.IsNullOrEmpty(row.GetCell(j).ToString()) ? table.Rows[table.Rows.Count - 1][j] : row.GetCell(j));
                }
                table.Rows.Add(dataRow);
            }
            excelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }
    }
}