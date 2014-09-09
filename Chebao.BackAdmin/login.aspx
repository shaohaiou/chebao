<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Chebao.BackAdmin.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="height: 100%;">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>耐磨达产品查询系统</title>
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var isIE = !!window.ActiveXObject;
        var isIE6 = isIE && !window.XMLHttpRequest;
        if (self.location != top.location) {
            top.location.href = self.location;
        }
        function fGetCode() {
            var gNow = new Date();
            $("#imgcode").attr("src", "checkcode.ashx?x=" + gNow.getSeconds());
        }
        $(function () {
            fGetCode();
//            if (!isIE6)
//                new ZouMa().Start();
//            else {
//                $(".flaydiv img").attr("src","images/login/1.3.png");
//            }
        });
        function ZouMa() {
            this.maxLength = 3; //最低显示数           
            this.Timer = 2000; //计时器间隔时间
            this.Ul = $(".box ul");

            var handId; //计时器id
            var self = this;
            this.Start = function () {
                if (self.Ul.children().length < this.maxLength) {
                    self.Ul.append(self.Ul.children().clone());
                }
                handId = setInterval(self.Play, self.Timer);
            }
            this.Play = function () {
                var img = self.Ul.children().eq(0);
                var left = img.children().eq(0).width();
                img.animate({ "marginLeft": (-1 * left) + "px" }, 600, function () {
                    //appendTo函数是实现走马灯一直不间断播放的秘诀。
                    //目前网上看到的很多走马灯，走到最后一张的时候，会立马闪回第一张，而不是继续从后往前推进，即是没有明白该函数的作用的原因
                    $(this).css("margin-left", "auto").appendTo(self.Ul);
                    $(".flaydiv img").attr("src", $(this).find("img").attr("fimg"));
                });
            }
            $(".flaydiv").hover(function () {
                if (handId) clearInterval(handId);
            }, function () {
                handId = setInterval(self.Play, self.Timer);
            });
        }
    </script>
    <style type="text/css">
        body
        {
            padding: 0;
            font-family: Verdana, Geneva, sans-serif;
            font-size: 12px;
            color:#333;
            margin: 0;
        }
        img
        {
            border: 0;
        }
        a
        {
            color: #09f;
        }
        .main
        {
            width: 100%;
            height:100%;
            display: inline-block; *display: inline; *zoom: 1;
        }
        .logo
        {
            width: 251px;
            float: left;
        }
        .logor
        {
            width: 250px;
           padding: 160px 80px 0 753px;
        }
        .logor ul
        {
            margin: 0;
            padding: 0;
            list-style: none;
        }
        .logor ul li
        {
            padding: 4px 0;
            list-style: none;
            margin: 0;
            text-align: left;
        }
        .hongzi{color:Red;}
        .an
        {
            border: 0px;
            width: 200px;
            font-weight: bold;
            background: #e8e8e8;
            padding: 4px 6px 2px 6px;
            font-size: 18px;
        }
        .ft
        {
            padding-top: 200px;
            text-align: center;
            overflow: hidden;
        }
        
        .srk1
        {
            width: 175px;
            height: 18px;
            border: 1px solid;
            border-color: #707070 #CECECE #CECECE #ABABAB;
            background: #F9F9F9 url(images/login/z1.gif) no-repeat;
            padding-left: 52px;
        }
        .srk1:hover, .srk1:focus
        {
            border-color: #6FB1DF;
            color: #333;
            background-color: #f5f9fd;
        }
        .srk2
        {
            width: 175px;
            height: 18px;
            border: 1px solid;
            border-color: #707070 #CECECE #CECECE #ABABAB;
            background: #F9F9F9 url(images/login/z2.gif) no-repeat;
            padding-left: 52px;
        }
        .srk2:hover, .srk2:focus
        {
            border-color: #6FB1DF;
            color: #333;
            background-color: #f5f9fd;
        }
        .srk3
        {
            width: 78px;
            height: 18px;
            border: 1px solid;
            border-color: #707070 #CECECE #CECECE #ABABAB;
            background: #F9F9F9 url(images/login/z3.gif) no-repeat;
            padding-left: 52px;
        }
        .srk3:hover, .srk3:focus
        {
            border-color: #6FB1DF;
            color: #333;
            background-color: #f5f9fd;
        }
        
         .box
        {
            width: 421px;
            height: 421px;
            margin-top: 34px;
            margin-top: 38px\9;
            *margin-top: 17px;
            margin-left: 649px;
            *margin-left: 639px;
            overflow: hidden;
            /*background:url(images/login/zmbg.png) no-repeat;*/
            position:relative;
        }
        
        .box img
        {
            border-style: none;
        }
        
        .box ul
        {
            margin: 0px;
            padding: 0px;
            list-style-type: none;
            width:2105px;
        }
        
        .box ul li
        {
            float: left;
            width:412px;
            height:421px;
        }
        #boxdiv{width: 421px;z-index:8;}
        .flaydiv{width:421px;height:421px;position:absolute;left:0px;top:0px;z-index:9;}
        .hide{display:none;}
    </style>
</head>
<body style="height: 100%;">
    <div style="margin:0 auto;;width:1080px;height:765px;background:url(images/login/bg.jpg) no-repeat;">
        <form id="form1" runat="server" style="height: 100%; text-align: center;">
        <div class="main">
            <div class="logor">
                <ul>
                    <li>
                        <asp:TextBox ID="tbUserName" runat="server" CssClass="srk1"></asp:TextBox>
                    </li>
                    <li>
                        <asp:TextBox ID="tbUserPwd" runat="server" CssClass="srk2" TextMode="Password"></asp:TextBox>
                    </li>
                    <li style="height: 30px;">
                        <asp:TextBox ID="tbCode" runat="server" CssClass="srk3" Style="float: left;"></asp:TextBox>
                        <span style="display: inline-block; *display: inline; *zoom: 1;"><a href="javascript:fGetCode()">
                            <img src="checkcode.ashx" alt="点击刷新" id="imgcode" width="90" /></a></span>
                    </li>
                    <li>
                        <asp:Button ID="btSave" runat="server" Text="登录" CssClass="an" OnClick="btSave_Click" />
                    </li>
                    <li>
                        <asp:Label ID="lbMsgUser" runat="server" Text="Label" CssClass="hongzi" Visible="false"></asp:Label></li>
                </ul>
            </div>
            <div class="box">
                <div class="flaydiv hide">
                    <img src="images/login/1.2.png" />
                </div>
                <div id="boxdiv" class=" hide">
                    <ul>
                        <li style="display: block;"><a href="#">
                            <img src="images/login/1.1.png" fimg="images/login/2.2.png" /></a></li>
                        <li><a href="#">
                            <img src="images/login/2.1.png" fimg="images/login/3.2.png" /></a></li>
                        <li><a href="#">
                            <img src="images/login/3.1.png" fimg="images/login/4.2.png" /></a></li>
                        <li><a href="#">
                            <img src="images/login/4.1.png" fimg="images/login/5.2.png" /></a></li>
                        <li><a href="#">
                            <img src="images/login/5.1.png" fimg="images/login/1.2.png" /></a></li>
                    </ul>
                </div>
                <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0" width="421" height="421">
                  <param name="movie" value="images/login/lamda.swf">
                  <param name="quality" value="high">
                  <param name="wmode" value="transparent">
                  <embed src="images/login/lamda.swf" width="421" height="421" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" wmode="transparent"></embed>
                </object>
            </div>
        </div>
        </form>
    </div>
</body>
</html>
