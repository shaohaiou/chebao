﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="Chebao.BackAdmin.order.main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>无标题文档</title>
</head>
<frameset rows="58,*" frameborder="no" border="0" framespacing="0">
  <frame src="main_s.aspx" name="sk" scrolling="No" noresize="noresize" id="sk" title="sk" />
  <%if (CheckModulePower("订单列表"))
    { %>
  <frame src="ordermg.aspx" name="ztk" id="ztk" title="ztk" />
  <%}
    else if (CheckModulePower("利润查询"))
    {%>
  <frame src="profitsmg.aspx" name="ztk" id="ztk" title="ztk" />
  <%}
    else if (CheckModulePower("同步失败记录"))
    {%>
  <frame src="syncfailedmg.aspx" name="ztk" id="ztk" title="ztk" />
  <%} %>
</frameset>
<noframes>
    <body>
    </body>
</noframes>
