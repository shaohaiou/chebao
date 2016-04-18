<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main_s.aspx.cs" Inherits="Chebao.BackAdmin.product.main_s" %>
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
    <%if (CheckModulePower("产品列表"))
          {%>
        <a href="productmg.aspx" target="ztk" id="productmg" <%if(CheckModulePower("产品列表")){ %>class="current"<%} %>>产品列表</a>
        <%} %>
    <%if (CheckModulePower("新增产品"))
          {%>
        <a href="productedit.aspx" target="ztk" id="productadd" <%if(!CheckModulePower("产品列表") && CheckModulePower("新增产品")){ %>class="current"<%} %>>新增产品</a>
        <%} %>
    <%if (CheckModulePower("数据导入"))
          {%>
        <a href="import.aspx" target="ztk" id="import" <%if(!CheckModulePower("产品列表") && !CheckModulePower("新增产品") && CheckModulePower("数据导入")){ %>class="current"<%} %>>数据导入</a>
        <%} %>
    <%if (CheckModulePower("用户界面"))
          {%>
        <a href="products.aspx" target="ztk" id="products" <%if(!CheckModulePower("产品列表") && !CheckModulePower("新增产品") && !CheckModulePower("数据导入") && CheckModulePower("用户界面")){ %>class="current"<%} %>>用户界面</a>
        <%} %>
</div>

<div class="r_sy" id="daohan">
	当前位置：系统设置 &gt;&gt; <span id="daohan_sp"><%if (CheckModulePower("产品列表"))
              { %>产品列表<%}
                                           else if (CheckModulePower("新增产品"))
              { %>新增产品<%}
                                           else if (CheckModulePower("数据导入"))
              { %>数据导入<%}
                                           else if (CheckModulePower("用户界面"))
              {%>用户界面<%} %></span>
</div>
</body>
<script language="javascript" type="text/javascript">
    $(function () {
        $(".right_nav a").not("#products").each(function () {
            $(this).click(function () {
                $(".r_sy").html("当前位置：产品列表 &gt;&gt; " + $(this).html());
                $(".right_nav a").removeClass("current");
                $(this).addClass("current");
            });
        });
    });
</script>
</html>
