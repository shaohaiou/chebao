using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chebao.Components;
using Chebao.Tools;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.Drawing;
using System.Data;

namespace Chebao.BackAdmin.useradmin
{
    public partial class orders : AdminBase
    {
        protected override void Check()
        {
            if (!ChebaoContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            if (Admin.ParentAccountID > 0)
            {
                Response.Clear();
                Response.Write("您没有权限操作！");
                Response.End();
                return;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(GetString("action")))
                {
                    string action = GetString("action");
                    OrderStatus status = OrderStatus.未收款;
                    switch (action)
                    {
                        case "gather":
                            status = OrderStatus.已收款;
                            break;
                        case "consignment":
                            status = OrderStatus.已发货;
                            break;
                        case "cancel":
                            status = OrderStatus.已取消;
                            break;
                        default:
                            status = OrderStatus.未收款;
                            break;
                    }
                    List<string> syncResultlist = new List<string>();
                    string[] ids = GetString("ids").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string id in ids)
                    {
                        syncResultlist.Add(Cars.Instance.UpdateOrderStatus(id, status, AdminName));
                    }
                    if (syncResultlist.Count > 0 && status == OrderStatus.已取消)
                        Session["syncResult"] = string.Join(",",syncResultlist.Distinct());
                    Response.Redirect(FromUrl);
                    Response.End();
                }
                BindControler();
                LoadData();
            }
            else
            {
                if (!string.IsNullOrEmpty(hdnAction.Value) && !string.IsNullOrEmpty(hdnId.Value))
                {
                    OrderStatus status = OrderStatus.未收款;
                    switch (hdnAction.Value)
                    {
                        case "gather":
                            status = OrderStatus.已收款;
                            break;
                        case "consignment":
                            status = OrderStatus.已发货;
                            break;
                        case "cancel":
                            status = OrderStatus.已取消;
                            break;
                        default:
                            status = OrderStatus.未收款;
                            break;
                    }
                    string syncResult = Cars.Instance.UpdateOrderStatus(hdnId.Value, status, AdminName);
                    if (!string.IsNullOrEmpty(syncResult) && status == OrderStatus.已取消)
                        Session["syncResult"] = syncResult;
                    Response.Redirect(UrlDecode(hdnFrom.Value));
                    Response.End();
                }
            }
        }

        private void BindControler()
        {
            DataTable dtOrderStatus = EnumExtensions.ToTable<OrderStatus>();
            ddlOrderStatusFilter.DataSource = dtOrderStatus.DefaultView;
            ddlOrderStatusFilter.DataTextField = "Name";
            ddlOrderStatusFilter.DataValueField = "Value";
            ddlOrderStatusFilter.DataBind();
            ddlOrderStatusFilter.Items.Insert(0, new ListItem("-订单状态-", "-1"));
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

            List<OrderInfo> list = Cars.Instance.GetOrderList(true);
            list = list.FindAll(l => l.ParentID == AdminID);
            if (!string.IsNullOrEmpty(GetString("n")))
            {
                string ordernumber = GetString("n");
                list = list.FindAll(l => l.OrderNumber.IndexOf(ordernumber) >= 0);
            }
            if (!string.IsNullOrEmpty(GetString("u")))
            {
                string username = GetString("u").ToLower();
                list = list.FindAll(l => l.UserName.ToLower().IndexOf(username) >= 0);
            }
            if (GetInt("s", -1) >= 0)
            {
                OrderStatus orderstatus = (OrderStatus)GetInt("s", -1);
                list = list.FindAll(l => l.OrderStatus == orderstatus);
            }

            list = list.OrderByDescending(l => l.ID).ThenByDescending(l => l.OrderStatus).ToList();
            total = list.Count();
            list = list.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<OrderInfo>();

            rptData.DataSource = list;
            rptData.DataBind();

            search_fy.RecordCount = total;
            search_fy.PageSize = pagesize;
            search_fy.CustomInfoHTML = string.Format("<span style=\"line-height: 34px;\">总记录数：{0} 总页数：{1}</span>", total, search_fy.PageCount);

            if (GetInt("s", -1) >= 0)
            {
                SetSelectedByValue(ddlOrderStatusFilter, GetString("s"));
            }
            if (!string.IsNullOrEmpty(GetString("n")))
            {
                txtOrderNumberFilter.Value = GetString("n");
            }
            if (!string.IsNullOrEmpty(GetString("u")))
            {
                txtUserNameFilter.Value = GetString("u");
            }

            hdnFrom.Value = UrlEncode(CurrentUrl);

            if (Session["syncResult"] != null && !string.IsNullOrEmpty(Session["syncResult"].ToString()))
            {
                hdnSyncresult.Value = Session["syncResult"].ToString();
                Session.Remove("syncResult");
            }
        }

        protected string GetOrderProductsStr(object orderproducts)
        {
            string result = string.Empty;
            List<OrderProductInfo> list = orderproducts as List<OrderProductInfo>;
            if (list != null)
            {
                foreach (OrderProductInfo p in list)
                {
                    result += p.ProductName + "<br />";
                    if (p.ProductMixList != null)
                    {
                        foreach (ProductMixInfo pi in p.ProductMixList)
                        {
                            result += string.Format("<span class=\"pl10\">{0}</span>", pi.Amount + " × " + pi.Name + "<span class=\"gray\">(" + pi.Price + ")</span><br />");
                        }
                    }
                }
            }

            return result;
        }

        protected string GetOrderStatusColor(OrderStatus status)
        {
            string result = string.Empty;

            switch (status)
            {
                case OrderStatus.未收款:
                    result = "red";
                    break;
                case OrderStatus.已收款:
                    result = "orange";
                    break;
                case OrderStatus.已发货:
                    result = "green";
                    break;
                case OrderStatus.已取消:
                    result = "gray";
                    break;
                default:
                    result = "gray";
                    break;
            }

            return result;
        }
    }
}