using System;
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
    public partial class discountstenciledit : AdminBase
    {
        private DiscountStencilInfo discountstencil = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = GetInt("id");
                int costs = GetInt("costs");
                if (id > 0)
                {
                    Header.Title = "编辑折扣模版";
                    discountstencil = Cars.Instance.GetDiscountStencil(id);

                    if (discountstencil != null)
                    {
                        BindData(discountstencil);
                    }
                    else
                    {
                        WriteErrorMessage("操作出错！", "该模版不存在，可能已经被删除！", "~/global/discountstencilmg.aspx");
                    }
                }
                else if (costs > 0)
                {
                    Header.Title = "成本折扣设置";
                    discountstencil = Cars.Instance.GetCostsDiscount();
                    if (discountstencil != null)
                    {
                        BindData(discountstencil);
                    }
                }
                else
                {
                    Header.Title = "添加折扣模版";
                }
            }
        }

        /// <summary>
        /// 绑定页面数据
        /// </summary>
        /// <param name="item"></param>
        protected void BindData(DiscountStencilInfo discountstencil)
        {
            hdid.Value = discountstencil.ID.ToString();               //ID
            txtName.Text = discountstencil.Name;
            txtDiscountM.Text = discountstencil.DiscountM.ToString();
            txtDiscountY.Text = discountstencil.DiscountY.ToString();
            txtDiscountH.Text = discountstencil.DiscountH.ToString();
            txtAdditemW.Text = discountstencil.AdditemW.ToString();
            txtAdditemF.Text = discountstencil.AdditemF.ToString();
            txtDiscountLS.Text = discountstencil.DiscountLS.ToString();
            txtDiscountXSP.Text = discountstencil.DiscountXSP.ToString();
            txtDiscountMT.Text = discountstencil.DiscountMT.ToString();
            txtDiscountB.Text = discountstencil.DiscountB.ToString();
            txtDiscountS.Text = discountstencil.DiscountS.ToString();
            txtDiscountK.Text = discountstencil.DiscountK.ToString();
            txtDiscountP.Text = discountstencil.DiscountP.ToString();
        }

        /// <summary>
        /// 填充实体类属性
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected void FillDate(DiscountStencilInfo discountstencil)
        {
            discountstencil.ID = DataConvert.SafeInt(hdid.Value);     //ID
            discountstencil.Name = txtName.Text;
            discountstencil.DiscountM = DataConvert.SafeDecimal(txtDiscountM.Text);
            discountstencil.DiscountY = DataConvert.SafeDecimal(txtDiscountY.Text);
            discountstencil.DiscountH = DataConvert.SafeDecimal(txtDiscountH.Text);
            discountstencil.AdditemW = DataConvert.SafeDecimal(txtAdditemW.Text);
            discountstencil.AdditemF = DataConvert.SafeDecimal(txtAdditemF.Text);
            discountstencil.DiscountLS = DataConvert.SafeDecimal(txtDiscountLS.Text);
            discountstencil.DiscountXSP = DataConvert.SafeDecimal(txtDiscountXSP.Text);
            discountstencil.DiscountMT = DataConvert.SafeDecimal(txtDiscountMT.Text);
            discountstencil.DiscountB = DataConvert.SafeDecimal(txtDiscountB.Text);
            discountstencil.DiscountS = DataConvert.SafeDecimal(txtDiscountS.Text);
            discountstencil.DiscountK = DataConvert.SafeDecimal(txtDiscountK.Text);
            discountstencil.DiscountP = DataConvert.SafeDecimal(txtDiscountP.Text);
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
                    discountstencil = Cars.Instance.GetDiscountStencil(id);
                    if (discountstencil == null)
                    {
                        WriteMessage("~/message/showmessage.aspx", "操作出错！", "该模版不存在，可能已经被删除！", "", FromUrl);
                    }
                    else
                    {
                        FillDate(discountstencil);
                        Cars.Instance.UpdateDiscountStencil(discountstencil);
                    }
                }
                else if (GetInt("costs") > 0)
                {
                    discountstencil = Cars.Instance.GetCostsDiscount();
                    if (discountstencil == null)
                    {
                        discountstencil = new DiscountStencilInfo()
                        {
                            IsCosts = 1,
                            Name = string.Empty
                        };
                        FillDate(discountstencil);
                        Cars.Instance.AddDiscountStencil(discountstencil);
                    }
                    else
                    {
                        FillDate(discountstencil);
                        Cars.Instance.UpdateDiscountStencil(discountstencil);
                    }
                }
                else
                {
                    discountstencil = new DiscountStencilInfo();
                    FillDate(discountstencil);
                    id = Cars.Instance.AddDiscountStencil(discountstencil);
                }

                Cars.Instance.ReloadDiscountStencilListCache();

                StringBuilder sb = new StringBuilder();
                sb.Append("<span class=\"dalv\">操作成功！</span><br />");
                WriteMessage("~/message/showmessage.aspx", "数据保存成功！", sb.ToString(), "", FromUrl);
            }

            return;
        }

        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            int id = GetInt("id");
            if (id != 1)
            {
                if (ChebaoContext.Current.AdminUser.UserRole != Components.UserRoleType.管理员 || !CheckModulePower("折扣模版"))
                {
                    Response.Clear();
                    Response.Write("您没有权限操作！");
                    Response.End();
                    return;
                }
            }
            else
            {
                if (ChebaoContext.Current.AdminUser.UserRole != Components.UserRoleType.管理员 || !CheckModulePower("成本折扣"))
                {
                    Response.Clear();
                    Response.Write("您没有权限操作！");
                    Response.End();
                    return;
                }
            }
        }
    }
}