﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using Chebao.Tools;
using System.Text;

namespace Chebao.BackAdmin.user
{
    public partial class useredit : AdminBase
    {
        private AdminInfo admin = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindControler();
                int id = GetInt("id");
                if (id > 0)
                {
                    rePassword.Visible = false;
                    reTruePassword.Visible = false;
                    cvPassword.Visible = false;
                    Header.Title = "编辑用户";
                    admin = Admins.Instance.GetAdmin(id);

                    if (admin != null)
                    {
                        BindData(admin);
                    }
                    else
                    {
                        WriteErrorMessage("操作出错！", "该用户不存在，可能已经被删除！", "~/user/adminlist.aspx");
                    }
                }
                else
                {
                    Header.Title = "添加用户";
                }
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

        /// <summary>
        /// 绑定页面数据
        /// </summary>
        /// <param name="item"></param>
        protected void BindData(AdminInfo admin)
        {
            hdid.Value = admin.ID.ToString();               //管理员ID
            txtUserName.Text = admin.UserName;//账户名
            txtMobile.Text = admin.Mobile;
            txtTelPhone.Text = admin.TelPhone;
            txtAddress.Text = admin.Address;
            txtLinkName.Text = admin.LinkName;
            txtValidDays.Text = admin.ValidDate.ToString("yyyyMMdd") == DateTime.MaxValue.ToString("yyyyMMdd") ? string.Empty : (admin.ValidDate.Subtract(DateTime.Today).Days > 0 ? admin.ValidDate.Subtract(DateTime.Today).Days.ToString() : "0");
            lblValidDays.Text = string.Format("（有效期至：{0}）", admin.ValidDate.ToString("yyyyMMdd") == DateTime.MaxValue.ToString("yyyyMMdd") ? "无限制" : (admin.ValidDate.Subtract(DateTime.Today).Days > 0 ? admin.ValidDate.ToString("yyyy年MM月dd日") : "已过期"));
            if (!string.IsNullOrEmpty(admin.Province + admin.City + admin.District))
            {
                SetSelectedByText(ddlProvince, admin.Province);
                ddlProvince_SelectedIndexChanged(null, null);
                SetSelectedByText(ddlCity, admin.City);
                ddlCity_SelectedIndexChanged(null, null);
                SetSelectedByText(ddlDistrict, admin.District);
            }
            txtPostCode.Value = admin.PostCode;
            txtDiscountM.Text = admin.DiscountM.ToString();
            txtDiscountY.Text = admin.DiscountY.ToString();
            txtDiscountH.Text = admin.DiscountH.ToString();
            txtAdditemW.Text = admin.AdditemW.ToString();
            txtAdditemF.Text = admin.AdditemF.ToString();
            cbxSizeView.Checked = admin.SizeView > 0;
        }

        /// <summary>
        /// 填充实体类属性
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected void FillDate(AdminInfo admin)
        {
            admin.ID = DataConvert.SafeInt(hdid.Value);     //管理员ID
            admin.Administrator = false;        //是否是超级管理员
            if (txtPassword.Text.Trim().Length != 0)
                admin.Password = EncryptString.MD5(txtPassword.Text);              //管理员密码
            admin.UserName = txtUserName.Text;//账户名
            admin.LastLoginIP = string.Empty;
            admin.Mobile = txtMobile.Text;
            admin.UserRole = UserRoleType.普通用户;
            admin.ValidDate = string.IsNullOrEmpty(txtValidDays.Text.Trim()) ? DateTime.MaxValue : DateTime.Today.AddDays(DataConvert.SafeDouble(txtValidDays.Text));
            admin.TelPhone = txtTelPhone.Text;
            admin.PostCode = txtPostCode.Value;
            admin.Address = txtAddress.Text;
            admin.LinkName = txtLinkName.Text;
            if (ddlProvince.SelectedIndex > 0)
                admin.Province = ddlProvince.SelectedItem.Text;
            if (ddlCity.SelectedIndex > 0)
                admin.City = ddlCity.SelectedItem.Text;
            if (ddlDistrict.SelectedIndex > 0)
                ddlDistrict.SelectedItem.Text = admin.District;
            admin.DiscountM = DataConvert.SafeDecimal(txtDiscountM.Text);
            admin.DiscountY = DataConvert.SafeDecimal(txtDiscountY.Text);
            admin.DiscountH = DataConvert.SafeDecimal(txtDiscountH.Text);
            admin.AdditemW = DataConvert.SafeDecimal(txtAdditemW.Text);
            admin.AdditemF = DataConvert.SafeDecimal(txtAdditemF.Text);
            admin.SizeView = cbxSizeView.Checked ? 1 : 0;
        }

        /// <summary>
        /// 保存广告条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btSave_Click(object sender, EventArgs e)
        {
            int id;
            //是否通过页面验证
            if (Page.IsValid)
            {
                id = DataConvert.SafeInt(hdid.Value);

                if (id > 0)
                {
                    admin = Admins.Instance.GetAdmin(id);
                    if (admin == null)
                    {
                        WriteMessage("~/message/showmessage.aspx", "操作出错！", "该用户不存在，可能已经被删除！", "", FromUrl);
                    }
                    else
                    {
                        FillDate(admin);
                        Admins.Instance.UpdateAdmin(admin);
                        if (admin.ID == Admin.ID)
                            Admin = admin;
                    }
                }
                else
                {
                    admin = new AdminInfo();
                    FillDate(admin);
                    id = Admins.Instance.AddAdmin(admin);
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("<span class=\"dalv\">用户保存成功！</span><br />");
                WriteMessage("~/message/showmessage.aspx", "用户保存成功！", sb.ToString(), "", FromUrl);
            }

            return;
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
            }
        }

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
    }
}