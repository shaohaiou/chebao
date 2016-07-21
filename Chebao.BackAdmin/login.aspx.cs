using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Tools;
using Chebao.Components;

namespace Chebao.BackAdmin
{
    public partial class login : AdminBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ChebaoContext.Current.AdminCheck)
            {
                Response.Clear();
                Response.Write(string.Format("你已经登录，请返回<a href='{0}'>首页</a>", "index.aspx"));
                Response.End();
            }
        }

        /// <summary>
        /// 登录页面不需要验证
        /// </summary>
        protected override void Check()
        {

        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string userName = StrHelper.Trim(tbUserName.Text);
                string password = StrHelper.Trim(tbUserPwd.Text);
                string code = StrHelper.Trim(tbCode.Text).ToLower();

                ///用户名，密码，验证码不允许为空
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(code))
                {
                    if (Session["CheckCode"] != null && Session["CheckCode"].ToString().ToLower() == code)
                    {
                        int id = Admins.Instance.ValiUser(userName, EncryptString.MD5(password));//验证用户

                        if (id > 0)
                        {
                            AdminInfo admin = Admins.Instance.GetAdmin(id);
                            AdminInfo parentadmin = admin.ParentAccountID > 0 ? Admins.Instance.GetAdmin(admin.ParentAccountID) : null;
                            if (admin.ValidDate > DateTime.Today && (parentadmin == null || parentadmin.ValidDate > DateTime.Today))
                            {
                                admin.LastLoginIP = WebHelper.GetClientsIP();
                                admin.LastLoginTime = DateTime.Now;
                                admin.LastLoginPosition = WebHelper.GetClientsPosition();
                                admin.LoginTimes++;
                                Admins.Instance.UpdateAdmin(admin);
                                LoginRecordInfo lrinfo = new LoginRecordInfo() 
                                {
                                    AdminID = admin.ID,
                                    AdminName = admin.UserName,
                                    LoginIP = admin.LastLoginIP,
                                    LoginPosition = admin.LastLoginPosition,
                                    LoginTime = admin.LastLoginTime.Value
                                };
                                Admins.Instance.AddLoginRecord(lrinfo);
                                Session[GlobalKey.SESSION_ADMIN] = admin;
                                ManageCookies.CreateCookie(GlobalKey.SESSION_ADMIN, id.ToString(), true, DateTime.Today.AddDays(1), ChebaoContext.Current.CookieDomain);
                                if (admin.UserRole == Components.UserRoleType.普通用户)
                                    Response.Redirect("product/products.aspx");
                                else
                                    Response.Redirect("index.aspx");
                            }
                            else
                            {
                                lbMsgUser.Text = "您的帐号已过期！";
                                lbMsgUser.Visible = true;
                            }
                        }
                        else
                        {
                            lbMsgUser.Text = "用户名或密码错误！";
                            lbMsgUser.Visible = true;
                        }
                    }
                    else
                    {
                        lbMsgUser.Text = "验证码不正确!";
                        lbMsgUser.Visible = true;
                    }
                    Session[GlobalKey.SESSION_ADMIN] = null;
                }
                else
                {
                    lbMsgUser.Text = "用户名，密码，验证码不能为空！";
                    lbMsgUser.Visible = true;
                }
            }
        }
    }
}