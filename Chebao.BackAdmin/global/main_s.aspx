<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main_s.aspx.cs" Inherits="Chebao.BackAdmin.global.main_s" %>
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
     <a href="sitesetting.aspx" target="ztk" id="sitesetting" runat="server" class="current">站点设置</a>
     <a href="sqlexecute.aspx" target="ztk" id="sqlexecute" runat="server">执行sql</a>
     <a href="dealdata.aspx" target="ztk" id="dealdata" runat="server">数据处理</a>
    <%if (CheckModulePower("站点设置"))
          {%>
        <a href="sitesetting.aspx" target="ztk" id="sitesetting" <%if(CheckModulePower("站点设置")){ %>class="current"<%} %>>站点设置</a>
        <%} %>
    <%if (CheckModulePower("执行sql"))
          {%>
        <a href="sqlexecute.aspx" target="ztk" id="sqlexecute" <%if(!CheckModulePower("站点设置") && CheckModulePower("执行sql")){ %>class="current"<%} %>>执行sql</a>
        <%} %>
    <%if (CheckModulePower("数据处理"))
          {%>
        <a href="dealdata.aspx" target="ztk" id="dealdata" <%if(!CheckModulePower("站点设置") && !CheckModulePower("执行sql") && CheckModulePower("数据处理")){ %>class="current"<%} %>>数据处理</a>
        <%} %>
</div>

<div class="r_sy" id="daohan">
	当前位置：系统设置 &gt;&gt; <span id="daohan_sp"><%if (CheckModulePower("站点设置"))
              { %>站点设置<%}
                                           else if (CheckModulePower("执行sql"))
              { %>执行sql<%}
                                           else if (CheckModulePower("数据处理"))
              { %>数据处理<%} %> </span>
</div>
</body>
<script language="javascript" type="text/javascript">
    $(function () {
        $(".right_nav a").each(function () {
            $(this).click(function () {
                $(".r_sy").html("当前位置：系统设置 &gt;&gt; " + $(this).html());
                $(".right_nav a").removeClass("current");
                $(this).addClass("current");
            });
        });
    });
</script>
</html>
