using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using Chebao.Tools;
using System.Web.UI.HtmlControls;

namespace Chebao.BackAdmin.order
{
    public partial class copyorder : AdminBase
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

        private AdminInfo copyuser = null;
        public AdminInfo CopyUser
        {
            get
            {
                if (copyuser == null)
                {
                    copyuser = Admins.Instance.GetAdminByName(GetString("uname"));
                }

                return copyuser;
            }
        }

        private List<OrderInfo> orderall = null;
        public List<OrderInfo> OrderAll
        {
            get
            {
                if (orderall == null)
                {
                    orderall = Cars.Instance.GetOrderList(true);
                }
                return orderall;
            }
        }

        private System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindControler();
                LoadData();
            }
            else
            {
                SubmitOrder();
            }
        }

        private void BindControler()
        {
            ddlProvince.DataSource = Cars.Instance.GetProvinceList(true);
            ddlProvince.DataTextField = "Name";
            ddlProvince.DataValueField = "ID";
            ddlProvince.DataBind();
            ddlProvince.Items.Insert(0, new ListItem("-省份-", "-1"));

            ddlCity.Items.Insert(0, new ListItem("-城市-", "-1"));
            ddlDistrict.Items.Insert(0, new ListItem("-县区-", "-1"));
        }

        private void LoadData()
        {
            if (CopyUser == null)
            {
                Response.Clear();
                Response.Write("用户名错误！");
                Response.End();
                return;
            }

            int orderid = GetInt("id");
            OrderInfo orderentity = Cars.Instance.GetOrder(orderid, true);
            if (orderentity != null)
            {
                string key = GlobalKey.ORDERPRODUCT_LIST + "_" + AdminID;

                List<OrderInfo> orderall = Cars.Instance.GetOrderList(true);
                List<OrderProductInfo> listOrderProduct = new List<OrderProductInfo>();

                foreach (OrderProductInfo opinfo in orderentity.OrderProducts)
                {
                    ProductInfo pinfo = Cars.Instance.GetProduct(opinfo.ProductID, true);
                    if (pinfo != null)
                    {
                        foreach (ProductMixInfo minfo in opinfo.ProductMixList)
                        {
                            int stock = pinfo.ProductMix.Find(p => p.Key == minfo.Name).Value;
                            if (OrderAll.Exists(o => o.SyncStatus == 0 && o.OrderProducts != null && o.OrderProducts.Exists(p => p.ProductMixList != null && p.ProductMixList.Exists(pm => pm.Name == minfo.Name))))
                            {
                                int amount = 0;
                                List<OrderInfo> orderlist = OrderAll.FindAll(o => o.SyncStatus == 0 && o.OrderProducts != null && o.OrderProducts.Exists(p => p.ProductMixList != null && p.ProductMixList.Exists(pm => pm.Name == minfo.Name)));
                                orderlist.ForEach(delegate(OrderInfo o)
                                {
                                    amount += o.OrderProducts.FindAll(p => p.ProductMixList != null && p.ProductMixList.Exists(pm => pm.Name == minfo.Name)).Sum(p => p.ProductMixList.FindAll(pm => pm.Name == minfo.Name).Sum(pm => pm.Amount));
                                });
                                stock -= amount;
                            }
                            if (stock < 0)
                                stock = 0;
                            if (stock < minfo.Amount)
                                minfo.Amount = stock;
                        }
                    }
                    listOrderProduct.Add(opinfo);
                }

                MangaCache.Add(key, listOrderProduct);

                rptData.DataSource = listOrderProduct;
                rptData.DataBind();

                txtTotalFee.InnerText = Math.Round(listOrderProduct.Sum(p => DataConvert.SafeDecimal(p.Sum)), 2).ToString();

                txtLinkMobile.Value = CopyUser.Mobile;
                txtLinkName.Value = CopyUser.LinkName;
                txtLinkTel.Value = CopyUser.TelPhone;
                txtAddress.Text = CopyUser.Address;
                if (!string.IsNullOrEmpty(CopyUser.Province + CopyUser.City + CopyUser.District))
                {
                    SetSelectedByText(ddlProvince, CopyUser.Province);
                    ddlProvince_SelectedIndexChanged(null, null);
                    SetSelectedByText(ddlCity, CopyUser.City);
                    ddlCity_SelectedIndexChanged(null, null);
                    SetSelectedByText(ddlDistrict, CopyUser.District);
                    ddlDistrict_SelectedIndexChanged(null, null);
                }
                txtPostCode.Value = CopyUser.PostCode;
            }
            else
            {
                Response.Clear();
                Response.Write("订单错误！");
                Response.End();
                return;
            }
        }

        protected void rptData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                OrderProductInfo order = (OrderProductInfo)e.Item.DataItem;
                Repeater rptProductMix = (Repeater)e.Item.FindControl("rptProductMix");
                rptProductMix.DataSource = order.ProductMixList;
                rptProductMix.DataBind();
            }
        }

        protected void ddlProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProvince.SelectedIndex == 0)
            {
                ddlCity.Items.Clear();
                ddlDistrict.Items.Clear();
                ddlCity.Items.Insert(0, new ListItem("-城市-", "-1"));
                ddlDistrict.Items.Insert(0, new ListItem("-县区-", "-1"));
                txtPostCode.Value = string.Empty;
                return;
            }
            int pid = DataConvert.SafeInt(ddlProvince.SelectedValue);
            List<CityInfo> citylist = Cars.Instance.GetCityList(true);
            ddlCity.DataSource = citylist.FindAll(c => c.ProvinceId == pid);
            ddlCity.DataTextField = "Name";
            ddlCity.DataValueField = "ID";
            ddlCity.DataBind();
            ddlCity.Items.Insert(0, new ListItem("-城市-", "-1"));

            ddlDistrict.Items.Clear();
            ddlDistrict.Items.Insert(0, new ListItem("-县区-", "-1"));
        }

        protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCity.SelectedIndex == 0)
            {
                ddlDistrict.Items.Clear();
                ddlDistrict.Items.Insert(0, new ListItem("-县区-", "-1"));
                txtPostCode.Value = string.Empty;
                return;
            }

            int cid = DataConvert.SafeInt(ddlCity.SelectedValue);
            List<DistrictInfo> districtlist = Cars.Instance.GetDistrictList(true);
            ddlDistrict.DataSource = districtlist.FindAll(d => d.CityId == cid);
            ddlDistrict.DataTextField = "Name";
            ddlDistrict.DataValueField = "ID";
            ddlDistrict.DataBind();
            ddlDistrict.Items.Insert(0, new ListItem("-县区-", "-1"));
        }

        protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDistrict.SelectedIndex == 0)
            {
                txtPostCode.Value = string.Empty;
            }
            else
            {
                int did = DataConvert.SafeInt(ddlDistrict.SelectedValue);
                List<DistrictInfo> districtlist = Cars.Instance.GetDistrictList(true);
                txtPostCode.Value = districtlist.Find(d => d.ID == did).PostCode;

                txtAddressShow.InnerText = ddlProvince.SelectedItem.Text + " " + ddlCity.SelectedItem.Text + " " + ddlDistrict.SelectedItem.Text + " " + txtAddress.Text;
            }
        }

        protected void txtAddress_TextChanged(object sender, EventArgs e)
        {
            txtAddressShow.InnerText = ddlProvince.SelectedItem.Text + " " + ddlCity.SelectedItem.Text + " " + ddlDistrict.SelectedItem.Text + " " + txtAddress.Text;
        }

        private void SubmitOrder()
        {
            try
            {
                string checkresult = CheckForm();
                if (!string.IsNullOrEmpty(checkresult))
                {
                    ClientScript.RegisterStartupScript(typeof(string), "aa", "alert(\"" + checkresult + "\");", true);
                    return;
                }
                string key = GlobalKey.ORDERPRODUCT_LIST + "_" + AdminID;
                List<OrderProductInfo> listOrderProduct = MangaCache.Get(key) as List<OrderProductInfo>;

                if (listOrderProduct == null)
                {
                    Response.Clear();
                    Response.Write("订单错误！");
                    Response.End();
                    return;
                }
                else
                {
                    foreach (RepeaterItem item in rptData.Items)
                    {
                        HtmlInputHidden hdnSID = (HtmlInputHidden)item.FindControl("hdnSID");
                        HtmlTextArea txtRemark = (HtmlTextArea)item.FindControl("txtRemark");

                        if (listOrderProduct.Exists(l => l.SID.ToString() == hdnSID.Value))
                        {
                            listOrderProduct.Find(l => l.SID.ToString() == hdnSID.Value).Remark = txtRemark.Value;
                        }
                    }
                    List<OrderInfo> orderlist = Cars.Instance.GetOrderList(true);
                    int ordercount = orderlist.Exists(o => o.UserID == CopyUser.ID) ? orderlist.FindAll(o => o.UserID == CopyUser.ID).Count : 0;
                    OrderInfo entity = new OrderInfo()
                    {
                        OrderNumber = string.Format("{0}-{1}-{2}", CopyUser.UserName, ordercount + 1, DateTime.Now.ToString("yyyyMMdd")),
                        Province = ddlProvince.SelectedItem.Text,
                        City = ddlCity.SelectedItem.Text,
                        District = ddlDistrict.SelectedItem.Text,
                        UserID = CopyUser.ID,
                        UserName = CopyUser.UserName,
                        Address = txtAddress.Text,
                        PostCode = txtPostCode.Value,
                        LinkName = txtLinkName.Value,
                        LinkMobile = txtLinkMobile.Value,
                        LinkTel = txtLinkTel.Value,
                        OrderStatus = OrderStatus.未收款,
                        OrderProducts = listOrderProduct,
                        AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        DeelTime = string.Empty
                    };
                    entity.TotalFee = Math.Round(listOrderProduct.Sum(p => DataConvert.SafeDecimal(p.Sum)), 2).ToString();
                    entity.OrderProductJson = json.Serialize(listOrderProduct);
                    if (entity.OrderProductJson.Length > 65500)
                        entity.OrderProductJson1 = entity.OrderProductJson.Substring(0, 65500);
                    else
                        entity.OrderProductJson1 = entity.OrderProductJson;

                    if (entity.OrderProductJson.Length > 65500 * 2)
                        entity.OrderProductJson2 = entity.OrderProductJson.Substring(65500, 65500);
                    else
                        entity.OrderProductJson2 = entity.OrderProductJson.Length > 65500 ? entity.OrderProductJson.Substring(65500) : string.Empty;

                    if (entity.OrderProductJson.Length > 65500 * 3)
                        entity.OrderProductJson3 = entity.OrderProductJson.Substring(65500 * 2, 65500);
                    else
                        entity.OrderProductJson3 = entity.OrderProductJson.Length > 65500 * 2 ? entity.OrderProductJson.Substring(65500 * 2) : string.Empty;

                    if (entity.OrderProductJson.Length > 65500 * 4)
                        entity.OrderProductJson4 = entity.OrderProductJson.Substring(65500 * 3, 65500);
                    else
                        entity.OrderProductJson4 = entity.OrderProductJson.Length > 65500 * 3 ? entity.OrderProductJson.Substring(65500 * 3) : string.Empty;

                    if (entity.OrderProductJson.Length > 65500 * 5)
                        entity.OrderProductJson5 = entity.OrderProductJson.Substring(65500 * 4, 65500);
                    else
                        entity.OrderProductJson5 = entity.OrderProductJson.Length > 65500 * 4 ? entity.OrderProductJson.Substring(65500 * 4) : string.Empty;

                    if (entity.OrderProductJson.Length > 65500 * 6)
                        entity.OrderProductJson6 = entity.OrderProductJson.Substring(65500 * 5, 65500);
                    else
                        entity.OrderProductJson6 = entity.OrderProductJson.Length > 65500 * 5 ? entity.OrderProductJson.Substring(65500 * 5) : string.Empty;

                    if (entity.OrderProductJson.Length > 65500 * 7)
                        entity.OrderProductJson7 = entity.OrderProductJson.Substring(65500 * 6, 65500);
                    else
                        entity.OrderProductJson7 = entity.OrderProductJson.Length > 65500 * 6 ? entity.OrderProductJson.Substring(65500 * 6) : string.Empty;


                    string addresult = Cars.Instance.AddOrder(entity);
                    if (!string.IsNullOrEmpty(addresult))
                    {
                        ClientScript.RegisterStartupScript(typeof(string), "aa", "alert(\"抱歉!" + addresult + "\");location.href=\"shoppingtrolleymg.aspx\"", true);
                        return;
                    }
                    else
                    {
                        CopyUser.LinkName = txtLinkName.Value;
                        CopyUser.Mobile = txtLinkMobile.Value;
                        CopyUser.TelPhone = txtLinkTel.Value;
                        CopyUser.Province = ddlProvince.SelectedItem.Text;
                        CopyUser.City = ddlCity.SelectedItem.Text;
                        CopyUser.District = ddlDistrict.SelectedItem.Text;
                        CopyUser.Address = txtAddress.Text;
                        CopyUser.PostCode = txtPostCode.Value;

                        Admins.Instance.UpdateAdmin(CopyUser);
                        Response.Redirect("copyordersuccess.aspx");
                        Response.End();
                    }
                }
            }
            catch
            {

            }
        }

        private string CheckForm()
        {
            if (ddlProvince.SelectedIndex == 0 || ddlCity.SelectedIndex == 0 || ddlDistrict.SelectedIndex == 0) return "请完善收货地址";
            if (string.IsNullOrEmpty(txtAddress.Text.Trim())) return "请输入详细地址";
            if (string.IsNullOrEmpty(txtPostCode.Value.Trim())) return "请输入邮政编码";
            if (string.IsNullOrEmpty(txtLinkName.Value.Trim())) return "请输入收货人姓名";
            if (string.IsNullOrEmpty(txtLinkTel.Value.Trim()) && string.IsNullOrEmpty(txtLinkMobile.Value.Trim())) return "手机号码与电话号码至少填一项";
            return string.Empty;
        }
    }
}