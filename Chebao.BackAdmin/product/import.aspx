<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="import.aspx.cs" Inherits="Chebao.BackAdmin.product.import" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>数据导入</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            if ($("#lblMsg").text() != "") {
                setTimeout(function () {
                    $("#lblMsg").text("");
                }, 1000);
            }

            $("#btnSubmit").click(function () {
                $(".overlay").css({ 'display': 'block', 'opacity': '0.8' });
                $(".showbox").stop(true).animate({ 'margin-top': '200px', 'opacity': '1' }, 200);
            });

            $("#btnSubmitInfo").click(function () {
                $(".overlay").css({ 'display': 'block', 'opacity': '0.8' });
                $(".showbox").stop(true).animate({ 'margin-top': '200px', 'opacity': '1' }, 200);
            });
        });
    </script>
    <style type="text/css">
        *
        {
            margin: 0;
            padding: 0;
            list-style-type: none;
        }
        a, img
        {
            border: 0;
        }
        .demo
        {
            margin: 100px auto 0 auto;
            width: 400px;
            text-align: center;
            font-size: 18px;
        }
        .demo .action
        {
            color: #3366cc;
            text-decoration: none;
            font-family: "微软雅黑" , "宋体";
        }
        
        .overlay
        {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            z-index: 998;
            width: 100%;
            height: 100%;
            _padding: 0 20px 0 0;
            background: #f6f4f5;
            display: none;
        }
        .showbox
        {
            position: fixed;
            top: 0;
            left: 50%;
            z-index: 9999;
            opacity: 0;
            filter: alpha(opacity=0);
            margin-left: -80px;
        }
        *html, *html body
        {
            background-image: url(about:blank);
            background-attachment: fixed;
        }
        *html .showbox, *html .overlay
        {
            position: absolute;
            top: expression(eval(document.documentElement.scrollTop));
        }
        #AjaxLoading
        {
            border: 1px solid #8CBEDA;
            color: #37a;
            font-size: 12px;
            font-weight: bold;
        }
        #AjaxLoading div.loadingWord
        {
            width: 240px;
            height: 50px;
            line-height: 50px;
            border: 2px solid #D6E7F2;
            background: #fff;
        }
        #AjaxLoading img
        {
            margin: 10px 15px;
            float: left;
            display: inline;
        }
        li
        {
            line-height: 32px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="pl10 pt10">
        <ul>
            <li class="w100 fll tr bold">新产品导入：</li>
            <li>
                <input type="file" runat="server" id="fileImport" />
                <asp:Button runat="server" ID="btnSubmit" OnClick="btnSubmit_Click" Text=" 提交 " /></li>
        </ul>
        <ul>
            <li class="w100 fll tr bold">扩展信息导入：</li>
            <li>
                <input type="file" runat="server" id="fileImportInfo" />
                <asp:Button runat="server" ID="btnSubmitInfo" OnClick="btnSubmitInfo_Click" Text=" 提交 " /></li>
        </ul>
        <asp:Label runat="server" ID="lblMsg" Text="" CssClass="red"></asp:Label>
    </div>
    </form>
    <div class="overlay">
    </div>
    <div id="AjaxLoading" class="showbox">
        <div class="loadingWord">
            <img src="/images/waiting.gif">正在导入数据，请稍候...</div>
    </div>
</body>
</html>
