﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="Chebao.BackAdmin.user.main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>无标题文档</title>
</head>
<frameset rows="58,*" frameborder="no" border="0" framespacing="0">
  <frame src="main_s.aspx" name="sk" scrolling="No" noresize="noresize" id="sk" title="sk" />
  <%if (CheckModulePower("用户管理"))
    { %>
  <frame src="userlist.aspx" name="ztk" id="ztk" title="ztk"/>
  <%}
    else if (CheckModulePower("管理员管理"))
    {%>
  <frame src="adminlist.aspx" name="ztk" id="ztk" title="ztk"/>
  <%}
    else if (CheckModulePower("折扣模版"))
    {%>
  <frame src="discountstencilmg.aspx" name="ztk" id="ztk" title="ztk"/>
  <%}
    else if (CheckModulePower("成本折扣"))
    {%>
  <frame src="discountstenciledit.aspx?costs=1" name="ztk" id="ztk" title="ztk"/>
  <%} %>
</frameset>
<noframes>
    <body>
    </body>
</noframes>
