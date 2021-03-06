﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;

namespace Chebao.BackAdmin.user
{
    public partial class subaccountpowersetting : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
        }
        private AdminInfo _currentuser = null;
        private AdminInfo CurrentUser
        {
            get
            {
                int id = GetInt("id");
                if (_currentuser == null && id > 0)
                {
                    _currentuser = Admins.Instance.GetAdmin(id);
                }
                return _currentuser;
            }
        }
        private string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private int CurrentCharIndex = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindControler();
                LoadData();
            }
        }

        private void BindControler()
        {
            rptCarbrand.DataSource = Cars.Instance.GetBrandList(true).OrderBy(b => b.NameIndex).ToList();
            rptCarbrand.DataBind();
        }

        private void LoadData()
        {
            if (CurrentUser != null)
            {
                if (CurrentUser.ParentAccountID != AdminID)
                {
                    WriteErrorMessage("错误提示", "非本帐号子用户", "subaccountlist.aspx");
                }
                else
                {
                    hdnCarbrand.Value = CurrentUser.BrandPowerSetting;
                }
            }
            else
            {
                WriteErrorMessage("错误提示", "非法ID", "subaccountlist.aspx");
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            AdminInfo currentuser = Admins.Instance.GetAdmin(GetInt("id"));
            currentuser.BrandPowerSetting = hdnCarbrand.Value;

            Admins.Instance.UpdateBrandPowerSetting(currentuser);

            WriteSuccessMessage("保存成功！", "数据已经成功保存！", string.IsNullOrEmpty(FromUrl) ? "~/user/subaccountlist.aspx" : FromUrl);
        }

        protected string SetCarbrand(string id)
        {
            string result = string.Empty;

            if (CurrentUser != null)
            {
                string[] brands = CurrentUser.BrandPowerSetting.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (brands.Contains(id) || string.IsNullOrEmpty(CurrentUser.BrandPowerSetting))
                    result = "checked=\"checked\"";
            }

            return result;
        }

        protected string GetNewCharStr(string nameindex)
        {
            string result = string.Empty;

            if (CurrentCharIndex == -1)
            {
                result += string.Format("<ul><li class=\"nh\"><input type=\"checkbox\" class=\"cbxSuball\" checked=\"checked\" />{0}</li>", nameindex);
                CurrentCharIndex = chars.IndexOf(nameindex, StringComparison.OrdinalIgnoreCase);
            }
            else if (CurrentCharIndex > -1 && chars.IndexOf(nameindex, StringComparison.OrdinalIgnoreCase) != CurrentCharIndex)
            {
                result += string.Format("</ul><ul><li class=\"nh\"><input type=\"checkbox\" class=\"cbxSuball\" checked=\"checked\" />{0}</li>", nameindex);
                CurrentCharIndex = chars.IndexOf(nameindex, StringComparison.OrdinalIgnoreCase);
            }


            return result;
        }
    }
}