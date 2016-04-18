<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main_s.aspx.cs" Inherits="Chebao.BackAdmin.order.main_s" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
     <script type="text/javascript">
         function setdaohan(t, url) {
             $("#daohan_sp").html("<a href='" + url + "'>" + t + "</a>");
         }
     </script>
</head>
<body style="background:url(../images/xdd.gif) repeat-x;">
<div class="right_nav">
    <%if (CheckModulePower("订单列表"))
          {%>
        <a href="ordermg.aspx" target="ztk" id="ordermg" <%if(CheckModulePower("订单列表")){ %>class="current"<%} %>>订单列表</a>
        <%} %>
    <%if (CheckModulePower("利润查询"))
          {%>
        <a href="profitsmg.aspx" target="ztk" id="profitsmg" <%if(!CheckModulePower("订单列表") && CheckModulePower("利润查询")){ %>class="current"<%} %>>利润查询</a>
        <%} %>
    <%if (CheckModulePower("同步失败记录"))
          {%>
        <a href="syncfailedmg.aspx" target="ztk" id="syncfailedmg" <%if(!CheckModulePower("订单列表") && !CheckModulePower("利润查询") && CheckModulePower("同步失败记录")){ %>class="current"<%} %>>同步失败记录</a>
        <%} %>
</div>

<div class="r_sy" id="daohan">
	当前位置：订单管理 &gt;&gt; <span id="daohan_sp"><%if (CheckModulePower("订单列表"))
              { %>订单列表<%}
                                           else if (CheckModulePower("利润查询"))
              { %>利润查询<%}
                                           else if (CheckModulePower("同步失败记录"))
              { %>同步失败记录<%} %></span>
</div>
</body>
<script language="javascript" type="text/javascript">
    $(function () {
        $(".right_nav a").not("#products").each(function () {
            $(this).click(function () {
                $(".r_sy").html("当前位置：订单管理 &gt;&gt; " + $(this).html());
                $(".right_nav a").removeClass("current");
                $(this).addClass("current");
            });
        });
    });
</script>
</html>
