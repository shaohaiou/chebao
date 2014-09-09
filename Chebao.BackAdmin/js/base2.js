
//产品详情页
function tabs(tabId, tabNum) {
    //设置点击后的切换样式

    $(tabId + " .tab li").removeClass("curr");
    $(tabId + " .tab li").eq(tabNum).addClass("curr");
    //根据参数决定显示内容

    $(tabId + " .tabcon").hide();
    if (tabNum != 2) {
        $(tabId + " .tabcon").eq(tabNum).show();
        $('#con_one_1').css('display', 'block');
    }
    else {
        $(tabId + " .tabcon").eq(0).show();
        $('#con_one_1').css('display', 'none');
    }


};
//==================图片详细页函数=====================
//鼠标经过预览图片函数
function preview(img) {
    $("#preview .jqzoom img").attr("src", $(img).attr("src"));
    $("#preview .jqzoom img").attr("jqimg", $(img).attr("bimg"));
};

//图片放大镜效果
$(function () {

    $(".jqzoom").jqueryzoom({ xzoom: 400, yzoom: 300 });

});

//图片预览小图移动效果,页面加载时触发
$(function () {
    var tempLength = 0; //临时变量,当前移动的长度
    var viewNum = 5; //设置每次显示图片的个数量
    var moveNum = 2; //每次移动的数量
    var moveTime = 300; //移动速度,毫秒
    var scrollDiv = $(".spec-scroll .items ul"); //进行移动动画的容器
    var scrollItems = $(".spec-scroll .items ul li"); //移动容器里的集合
    var moveLength = scrollItems.eq(0).width() * moveNum; //计算每次移动的长度
    var countLength = (scrollItems.length - viewNum) * scrollItems.eq(0).width(); //计算总长度,总个数*单个长度

    //下一张

    $(".spec-scroll .next").bind("click", function () {
        if (tempLength < countLength) {
            if ((countLength - tempLength) > moveLength) {
                scrollDiv.animate({ left: "-=" + moveLength + "px" }, moveTime);
                tempLength += moveLength;
            } else {
                scrollDiv.animate({ left: "-=" + (countLength - tempLength) + "px" }, moveTime);
                tempLength += (countLength - tempLength);
            }
        }
    });
    //上一张

    $(".spec-scroll .prev").bind("click", function () {

        if (tempLength > 0) {
            //$(".prev").css("backgroundPosition","-24px -204px");
            if (tempLength > moveLength) {

                scrollDiv.animate({ left: "+=" + moveLength + "px" }, moveTime);
                tempLength -= moveLength;
            } else {

                scrollDiv.animate({ left: "+=" + tempLength + "px" }, moveTime);
                tempLength = 0;
            }
        }
    });
});

//产品详情推荐
$(function () {
    $('.recommend_box #addresser').blur(function () {
        var addresser = $(this).val().trim();
        if (!addresser) {
            $('.addresser_wrong').text('请填姓名');
        } else {
            $('.addresser_wrong').html('<img src="/pro_images/agency/task/right-icon.jpg">');
        }
    });
    $('.recommend_box #recipient').blur(function () {
        var recipient = $(this).val().trim();
        if (!recipient) {
            $('.email_wrong').text('请填邮箱');
        } else if (recipient.search(/^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/) == -1) {
            $('.email_wrong').text('格式有误');
        } else {
            $('.email_wrong').html('<img src="/pro_images/agency/task/right-icon.jpg">');
        }
    });

    $('.recommend_box .s_email').click(function () {
        var addresser = $('.recommend_box #addresser').val().trim(); //姓名
        var recipient = $('.recommend_box #recipient').val().trim(); //邮箱

        if (addresser == '您的姓名' || !addresser) {
            $('.addresser_wrong').text('请填姓名');
            return false;
        }
        if (recipient == '对方邮箱' || !recipient) {
            $('.email_wrong').text('请填邮箱');
            return false;
        } else if (recipient.search(/^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/) == -1) {
            $('.email_wrong').text('格式有误');
            return false;
        }

        var goods_id = $('.recommend_box #recom_goods_id').val().trim();
        var recommend_reason = $('.recommend_box #recommend_reason').val().trim();
        if (!recommend_reason) {
            recommend_reason = $('.recommend_box #recommend_reason').attr('placeholder');
        }
        $.ajax({
            url: '/ajax_return.php',
            type: 'POST',
            dataType: 'html',
            data: 'oper=ajax_pro_recommend' + '&user_name=' + addresser + '&email=' + recipient + '&goods_id=' + goods_id + '&recommend_reason=' + recommend_reason,
            async: false,
            cache: false,
            success: function (data) {
                var data = eval('(' + data + ')'); //将php提交过来的json数据，但在js里面是用html格式也就是字符串，只要转换成json格式就可用了
                alert(data.desc);
                /*var str = data.str;
                if(str!=''){
                //$('.commentlist').html('');
                $('.commentlist').load("/all_goods_comment.php");
                if(page==undefined && $('.comment_type li').hasClass('comment_all')){
                $('.comment_type li').removeClass('comment_all');
                $('#'+e).addClass('comment_all');
                }
                }*/
            }
        });
    });

    $('.gprice ul li:eq(2)').hide(); //将详情页面的优惠暂时隐藏

});

//详情评论分页
function all_goods_comment(id, e, page) {
    //判断是否已经添加，若是则返回无
    $('.commentlist').html("<div class='hold_on'>数据加载中，请稍候···</div>");
    $.ajax({
        url: ajax_url,
        type: 'POST',
        dataType: 'html',
        data: 'oper=all_goods_comment' + '&sub_oper=' + e + '&id=' + id + '&page=' + page,
        async: false,
        cache: false,
        success: function (data) {
            var data = eval('(' + data + ')'); //将php提交过来的json数据，但在js里面是用html格式也就是字符串，只要转换成json格式就可用了
            var str = data.str;
            if (str != '') {
                $('.commentlist').load("/all_goods_comment.php");
                if (page == undefined && $('.comment_type li').hasClass('comment_all')) {
                    $('.comment_type li').removeClass('comment_all');
                    $('#' + e).addClass('comment_all');
                }
            }
        }
    });
}