<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="Chebao.BackAdmin.userstock.main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>无标题文档</title>
</head>
<frameset rows="58,*" frameborder="no" border="0" framespacing="0">
  <frame src="main_s.aspx" name="sk" scrolling="No" noresize="noresize" id="sk" title="sk" />
  <%if (CheckModulePower("盘库审核"))
    { %>
  <frame src="userstockcheck.aspx" name="ztk" id="ztk" title="ztk" />
  <%}
    else if (CheckModulePower("库存查询"))
    {%>
  <frame src="userstockmg.aspx" name="ztk" id="ztk" title="ztk" />
  <%}
    else if (CheckModulePower("出入库记录"))
    {%>
  <frame src="userstockrecordmg.aspx" name="ztk" id="ztk" title="ztk" />
  <%} %>
</frameset>
<noframes>
    <body>
    </body>
</noframes>
