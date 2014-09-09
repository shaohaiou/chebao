<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dealdata.aspx.cs" Inherits="Chebao.BackAdmin.global.dealdata" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>数据处理</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(".btn").click(function () {
                if (confirm($(this).attr("msg"))) {
                    $(".overlay").css({ 'display': 'block', 'opacity': '0.8' });
                    $(".showbox").stop(true).animate({ 'margin-top': '200px', 'opacity': '1' }, 200);
                } else
                    return false;
            });
        });
    </script>
    <style type="text/css">
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
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="biaoge3">
        <caption class="bt2">
            数据处理</caption>
        <tbody>
            <tr>
                <td class="bg1">
                    产品名称重置：
                </td>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" msg="确定要重置产品名称吗？" Text="执行" CssClass="an1 btn" OnClick="btnSubmit_Click" />
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    车型图片采集：
                </td>
                <td>
                    <asp:CheckBoxList runat="server" ID="cblBrandindex" RepeatDirection="Horizontal">
                    </asp:CheckBoxList>
                    <br />
                    <asp:Button ID="btnGatherCabmodelimg" runat="server" msg="确定要采集车型图片吗？" Text="执行" CssClass="an1 btn"
                        OnClick="btnGatherCabmodelimg_Click" />
                </td>
            </tr>
            <tr>
                <td class="bg1">
                    车型图片重命名：
                </td>
                <td>
                    <asp:Button ID="btnRenameImage" runat="server" msg="确定要对车型图片重命名吗？" Text="执行" CssClass="an1 btn" OnClick="btnRenameImage_Click" />
                </td>
            </tr>
        </tbody>
    </table>
    </form>
    <div class="overlay">
    </div>
    <div id="AjaxLoading" class="showbox">
        <div class="loadingWord">
            <img src="/images/waiting.gif">正在处理，请稍候...</div>
    </div>
    </form>
</body>
</html>
