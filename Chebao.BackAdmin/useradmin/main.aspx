<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="Chebao.BackAdmin.useradmin.main" %>
<%@ Register src="../uc/header.ascx" tagname="header" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>耐磨达产品查询系统</title>
    <link href='../css/kp.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>'
        rel="stylesheet" type="text/css" />
    <link href="../css/headfoot2.css?t=<%= Chebao.Components.ChebaoContext.Current.Jsversion %>"
        rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="../js/head3.js" type="text/javascript"></script>
    <script src="../js/comm.js" type="text/javascript"></script>
    <script src="../js/jquery.marquee.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(".lmenu ul li").click(function () {
                $(".lmenu ul li").removeClass("lmenucurrent");
                $(this).addClass("lmenucurrent");
            });
        })
    </script>
</head>
<body>
    <uc1:header ID="header1" runat="server" />
    <div id="main" style="background:#eee;">
        <div class="lmenu">
            <ul>
                <li class="lmenucurrent"><a target="ibody" href="stocks.aspx">产品库存</a></li>
                <li><a target="ibody" href="orders.aspx">用户订单</a></li>
            </ul>
        </div>
        <div class="rbody">
            <iframe frameborder="0" marginheight="0" marginwidth="0" src="stocks.aspx" scrolling="no" width="100%" id="ibody" name="ibody" onLoad="iFrameHeight(this);"></iframe>
        </div>
    </div>
    <div class="footer">
        <div class="footmain">
            浙江耐磨达刹车片有限公司 版权所有 2014-2024 Shantui Zhejiang Lamda Brake Pads Co.,Ltd
        </div>
    </div>
</body>
</html>
