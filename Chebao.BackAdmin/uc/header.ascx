﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="header.ascx.cs" Inherits="Chebao.BackAdmin.uc.header" %>
<script type="text/javascript" language="javascript">
    $(function () {
        if (parseInt($("#noticecontent").width()) > 725) {
            $("#notice marquee").marquee('pointer').mouseover(function () {
                $(this).trigger('stop');
            }).mouseout(function () {
                $(this).trigger('start');
            });
        } else {
            $("#notice marquee").attr("direction", "right").attr("behavior", "alternate").marquee('pointer').trigger('stop');
        }
    })
</script>
<div class="header" id="head_01">
    <div class="header_logo">
        <img src="../images/headlogo.jpg" />
    </div>
    <div class="header_nav">
        <ul>
            <li<%= CurrentTag == "首页" ? " class=\"navcurrent\"" : string.Empty %>><a href="javascript:void(0);">首页</a></li>
            <li<%= CurrentTag == "产品查询" ? " class=\"navcurrent\"" : string.Empty %>><a href="/product/products.aspx">产品查询</a></li>
            <li<%= CurrentTag == "公司简介" ? " class=\"navcurrent\"" : string.Empty %>><a href="/global/corpintroduce.aspx">公司简介</a></li>
            <li<%= CurrentTag == "联系我们" ? " class=\"navcurrent\"" : string.Empty %>><a href="/global/contact.aspx">联系我们</a></li>
            <li<%= CurrentTag == "纠错反馈有奖" ? " class=\"navcurrent\"" : string.Empty %>><a href="/message/messageboard.aspx">纠错反馈有奖</a></li>
        </ul>
        <div class="header_navinfo">
            <span class="navinfo_user">
                <%= Admin.UserName%>，您好！</span> <span class="navinfo_opt"><a href="/logout.aspx">安全退出</a><a
                    class="ml10" href="/user/userchangepw.aspx">修改密码</a><a href="/product/myorders.aspx"
                        class="ml10">我的订单</a><%if (Admin.SizeView > 0)
                                               { %><a href="/product/myorders.aspx" class="cccx">尺寸查询</a><%} %></span>
        </div>
    </div>
    <div id="notice" style="color: White; width: 1000px; margin: 0 auto; height: 30px;
        line-height: 30px;">
        <%if (Sitesetting != null)
          { %>
        <span style="float: left;">公告：</span>
        <div style="margin-left: 36px; width: 725px; overflow: hidden;">
            <marquee style="display: block; height: 30px;" scrollamount="2"><span id="noticecontent"><%= Sitesetting.Notice%></span></marquee>
        </div>
        <%} %>
    </div>
    <!--end-->
</div>
