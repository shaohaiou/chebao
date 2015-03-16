﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using Chebao.Tools;
using System.Web.UI.HtmlControls;

namespace Chebao.BackAdmin.product
{
    public partial class placeorder : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
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
            string key = GlobalKey.ORDERPRODUCT_LIST + "_" + AdminID;

            List<OrderProductInfo> listOrderProduct = MangaCache.Get(key) as List<OrderProductInfo>;
            if (listOrderProduct != null)
            {
                rptData.DataSource = listOrderProduct;
                rptData.DataBind();

                txtTotalFee.InnerText = Math.Round(listOrderProduct.Sum(p => DataConvert.SafeDecimal(p.Sum)), 2).ToString();
            }

            txtLinkMobile.Value = Admin.Mobile;
            txtLinkName.Value = Admin.LinkName;
            txtLinkTel.Value = Admin.TelPhone;
            txtAddress.Text = Admin.Address;
            if (!string.IsNullOrEmpty(Admin.Province + Admin.City + Admin.District))
            {
                SetSelectedByText(ddlProvince, Admin.Province);
                ddlProvince_SelectedIndexChanged(null, null);
                SetSelectedByText(ddlCity, Admin.City);
                ddlCity_SelectedIndexChanged(null, null);
                SetSelectedByText(ddlDistrict, Admin.District);
                ddlDistrict_SelectedIndexChanged(null, null);
            }
            txtPostCode.Value = Admin.PostCode;
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
                ClientScript.RegisterStartupScript(typeof(string), "aa", "alert(\"订单产品错误\");location.href=\"shoppingtrolleymg.aspx\"", true);
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

                OrderInfo entity = new OrderInfo()
                {
                    OrderNumber = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString("000") + AdminID.ToString("0000"),
                    Province = ddlProvince.SelectedItem.Text,
                    City = ddlCity.SelectedItem.Text,
                    District = ddlDistrict.SelectedItem.Text,
                    UserID = AdminID,
                    UserName = AdminName,
                    Address = txtAddress.Text,
                    PostCode = txtPostCode.Value,
                    LinkName = txtLinkName.Value,
                    LinkMobile = txtLinkMobile.Value,
                    LinkTel = txtLinkTel.Value,
                    OrderStatus = OrderStatus.未处理,
                    OrderProducts = listOrderProduct,
                    AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    DeelTime = string.Empty
                };
                entity.TotalFee = Math.Round(listOrderProduct.Sum(p => DataConvert.SafeDecimal(p.Sum)), 2).ToString();
                entity.OrderProductJson = json.Serialize(listOrderProduct);

                string addresult = Cars.Instance.AddOrder(entity);
                if (!string.IsNullOrEmpty(addresult))
                {
                    ClientScript.RegisterStartupScript(typeof(string), "aa", "alert(\"抱歉!" + addresult + "\");location.href=\"shoppingtrolleymg.aspx\"", true);
                    return;
                }
                else
                {
                    Admin.LinkName = txtLinkName.Value;
                    Admin.Mobile = txtLinkMobile.Value;
                    Admin.TelPhone = txtLinkTel.Value;
                    Admin.Province = ddlProvince.SelectedItem.Text;
                    Admin.City = ddlCity.SelectedItem.Text;
                    Admin.District = ddlDistrict.SelectedItem.Text;
                    Admin.Address = txtAddress.Text;
                    Admin.PostCode = txtPostCode.Value;

                    Admins.Instance.UpdateAdmin(Admin);
                    Response.Redirect("placeordersuccess.aspx");
                    Response.End();
                }
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