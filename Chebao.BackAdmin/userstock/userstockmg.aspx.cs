using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using Chebao.Tools;

namespace Chebao.BackAdmin.userstock
{
    public partial class userstockmg : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            if (ChebaoContext.Current.AdminUser.UserRole != Components.UserRoleType.管理员 || !CheckModulePower("库存查询"))
            {
                Response.Clear();
                Response.Write("您没有权限操作！");
                Response.End();
                return;
            }
        }

        protected int ModelNumberAmountCount = 0;

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

            string mn = GetString("mn");
            string n = GetString("n");
            if (!string.IsNullOrEmpty(mn))
            {
                List<AdminInfo> adminlist = Admins.Instance.GetUsers();
                adminlist = adminlist.FindAll(a => a.ParentAccountID == 0 && a.UserRole == UserRoleType.普通用户);
                string key = "USERPRODUCTINFOLIST_LIST";
                List<List<UserProductInfo>> upllist = Session[key] as List<List<UserProductInfo>>;
                if (upllist == null)
                {
                    upllist = new List<List<UserProductInfo>>();
                    adminlist.ForEach(delegate(AdminInfo ainfo)
                    {
                        List<UserProductInfo> upl = Cars.Instance.GetUserProductInfoList(ainfo.ID, true);
                        if (upl != null && upl.Count > 0)
                            upllist.Add(upl);
                    });
                    Session[key] = upllist;
                }
                List<AdminInfo> validadminlist = new List<AdminInfo>();
                List<List<UserProductInfo>> validupllist = new List<List<UserProductInfo>>();
                foreach (List<UserProductInfo> upl in upllist)
                {
                    if (upl.Exists(u => u.ProductMix.Exists(k => k.Key.ToLower() == mn.ToLower())))
                    {
                        validupllist.Add(upl);
                    }
                }
                foreach(AdminInfo ainfo in adminlist)
                {
                    if (validupllist.Exists(l => l[0].UserID == ainfo.ID))
                    {
                        validadminlist.Add(ainfo);
                    }
                };
                //adminlist = adminlist.FindAll(a => upllist.Exists(l => l[0].UserID == a.ID && l.Exists(u => u.ProductMix.Exists(k => k.Key.ToLower() == mn.ToLower()))));
                var data = validadminlist.Select(a => new { UserName = a.UserName, Amount = validupllist.Find(l => l[0].UserID == a.ID).Find(u => u.ProductMix.Exists(k => k.Key.ToLower() == mn.ToLower())).ProductMix.Find(k => k.Key.ToLower() == mn.ToLower()).Value }).ToList();
                rptDataModelNumber.DataSource = data.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                rptDataModelNumber.DataBind();
                ModelNumberAmountCount = data.Sum(t => t.Amount);
                total = data.Count();
            }
            else if (!string.IsNullOrEmpty(n))
            {
                AdminInfo ainfo = Admins.Instance.GetAdminByName(n);
                if (ainfo != null)
                {
                    List<UserProductInfo> upl = Cars.Instance.GetUserProductInfoList(ainfo.ID, true);
                    rptDataUserName.DataSource = upl;
                    rptDataUserName.DataBind();
                    total = upl.Count;
                }
            }

            search_fy.RecordCount = total;
            search_fy.PageSize = pagesize;
            search_fy.CustomInfoHTML = string.Format("<span style=\"line-height: 34px;\">总记录数：{0} 总页数：{1}</span>", total, search_fy.PageCount);

            if (!string.IsNullOrEmpty(GetString("mn")))
                txtModelNumber.Text = GetString("mn");
            if (!string.IsNullOrEmpty(GetString("n")))
                txtUserName.Text = GetString("n");
        }

        protected string GetModelNumber(object o)
        {
            int pid = DataConvert.SafeInt(o);
            return Cars.Instance.GetProduct(pid, true).ModelNumber;
        }

        protected string GetProductMixStr(object o)
        {
            string result = string.Empty;
            List<KeyValuePair<string, int>> list = o as List<KeyValuePair<string, int>>;
            if (list != null)
            {
                foreach (KeyValuePair<string, int> p in list)
                {
                    result += string.Format("<span class=\"pl10\">{0}</span>",p.Key + " × " + p.Value + "<br />");
                }
            }

            return result;
        }
    }
}