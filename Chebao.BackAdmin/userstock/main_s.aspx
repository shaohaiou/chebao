<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main_s.aspx.cs" Inherits="Chebao.BackAdmin.userstock.main_s" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>无标题文档</title>
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
    <%if (CheckModulePower("库存查询"))
          {%>
        <a href="userstockmg.aspx" target="ztk" id="userstockmg" <%if(CheckModulePower("库存查询")){ %>class="current"<%} %>>库存查询</a>
        <%} %>
    <%if (CheckModulePower("出入库记录"))
          {%>
        <a href="userstockrecordmg.aspx" target="ztk" id="userstockrecordmg" <%if(!CheckModulePower("库存查询") && CheckModulePower("出入库记录")){ %>class="current"<%} %>>出入库记录</a>
        <%} %>
</div>

<div class="r_sy" id="daohan">
	当前位置：系统设置 &gt;&gt; <span id="daohan_sp"><%if (CheckModulePower("库存查询"))
              { %>库存查询<%}
                                           else if (CheckModulePower("出入库记录"))
              { %>出入库记录<%} %></span>
</div>
</body>
<script language="javascript" type="text/javascript">
    $(function () {
        $(".right_nav a").not("#products").each(function () {
            $(this).click(function () {
                $(".r_sy").html("当前位置：用户库存 &gt;&gt; " + $(this).html());
                $(".right_nav a").removeClass("current");
                $(this).addClass("current");
            });
        });
    });
</script>
</html>
