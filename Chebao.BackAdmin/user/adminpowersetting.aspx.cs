using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;

namespace Chebao.BackAdmin.user
{
    public partial class adminpowersetting : AdminBase
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
        protected AdminInfo CurrentUser
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdnModulepower.Value = CurrentUser.ModulePowerSetting;
            }
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            AdminInfo currentuser = Admins.Instance.GetAdmin(GetInt("id"));
            currentuser.ModulePowerSetting = hdnModulepower.Value;

            Admins.Instance.UpdateBrandPowerSetting(currentuser);

            WriteSuccessMessage("保存成功！", "数据已经成功保存！", string.IsNullOrEmpty(FromUrl) ? "~/user/adminlist.aspx" : FromUrl);
        }

        protected string SetModulepower(string name)
        {
            string result = string.Empty;

            if (CurrentUser != null)
            {
                string[] modules = CurrentUser.ModulePowerSetting.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (modules.Contains(name))
                    result = "checked=\"checked\"";
            }

            return result;
        }
    }
}