<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="import.aspx.cs" Inherits="Chebao.BackAdmin.product.import" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>数据导入</title>
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../js/comm.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            if ($("#lblMsg").text() != "") {
                setTimeout(function () {
                    $("#lblMsg").text("");
                }, 1000);
            }

            $("#btnSubmit").click(function () {
                showLoading("正在导入数据，请稍候...");
            });

            $("#btnSubmitInfo").click(function () {
                showLoading("正在导入数据，请稍候...");
            });

            $("#btnSubmitIntroduce").click(function () {
                showLoading("正在导入数据，请稍候...");
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
        <ul>
            <li class="w100 fll tr bold">产品介绍导入：</li>
            <li>
                <input type="file" runat="server" id="fileImportIntroduce" />
                <asp:Button runat="server" ID="btnSubmitIntroduce" OnClick="btnSubmitIntroduce_Click" Text=" 提交 " /></li>
        </ul>
        <asp:Label runat="server" ID="lblMsg" Text="" CssClass="red"></asp:Label>
    </div>
    </form>
</body>
</html>
