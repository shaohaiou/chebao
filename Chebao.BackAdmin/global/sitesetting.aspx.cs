using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;

namespace Chebao.BackAdmin.global
{
    public partial class sitesetting : AdminBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            SitesettingInfo setting = Sitesettings.Instance.GetSitesetting(true);
            if (setting != null)
            {
                txtNotice.Text = setting.Notice;
                txtContact.Text = setting.Contact;
                txtCorpIntroduce.Text = setting.CorpIntroduce;
            }
        }

        private void FillData(SitesettingInfo setting)
        {
            setting.Notice = txtNotice.Text;
            setting.Contact = txtContact.Text;
            setting.CorpIntroduce = txtCorpIntroduce.Text;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SitesettingInfo setting = Sitesettings.Instance.GetSitesetting(true);
            if (setting == null)
            {
                setting = new SitesettingInfo();
                FillData(setting);
                Sitesettings.Instance.AddSitesetting(setting);
            }
            else
            {
                FillData(setting);
                Sitesettings.Instance.UpdateSitesetting(setting);
            }
            WriteSuccessMessage("提示信息","数据提交成功","~/global/sitesetting.aspx");
        }
    }
}