﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;

namespace Chebao.BackAdmin.order
{
    public partial class orderview : AdminBase
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

        private OrderInfo currentorder = null;
        protected OrderInfo CurrentOrder
        {
            get
            {
                if (currentorder == null && GetInt("id") > 0)
                {
                    currentorder = Cars.Instance.GetOrder(GetInt("id"),true);
                }
                return currentorder;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (GetInt("id") == 0 || CurrentOrder == null)
                {
                    WriteMessage("~/message/showmessage.aspx", "错误！", "非法订单！", "", FromUrl);
                    Response.End();
                    return;
                }
                LoadData();
            }
        }

        private void LoadData()
        {
            if (CurrentOrder != null)
            {
                rptData.DataSource = CurrentOrder.OrderProducts;
                rptData.DataBind();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(FromUrl);
        }
    }
}