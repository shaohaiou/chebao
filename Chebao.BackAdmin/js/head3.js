/*加入收藏夹*/
function bookmark() {
    var title = document.title
    var url = document.location.href
    if (window.sidebar) window.sidebar.addPanel(title, url, "");
    else if (window.opera && window.print) {
        var mbm = document.createElement('a');
        mbm.setAttribute('rel', 'sidebar');
        mbm.setAttribute('href', url);
        mbm.setAttribute('title', title);
        mbm.click();
    }
    else if (document.all) window.external.AddFavorite(url, title);
}
//导航条效果

$(function () {
    var line_url = window.location.href;

    $('.navitems li').removeClass('n_hover');
    if (line_url.indexOf('com/index') > 0 || line_url == "http://www.chebao360.com/") { $('.navitems li').eq(0).addClass('n_hover'); }
    else if (line_url.indexOf('/profession') > 0) { $('.navitems li').eq(1).addClass('n_hover'); }
    else if (line_url.indexOf('/daigou') > 0) { $('.navitems li').eq(2).addClass('n_hover'); }
    else if (line_url.indexOf('/service') > 0) { $('.navitems li').eq(3).addClass('n_hover'); }
    else if (line_url.indexOf('/brand') > 0) { $('.navitems li').eq(4).addClass('n_hover'); }
    else if (line_url.indexOf('/knowledge') > 0) { $('.navitems li').eq(5).addClass('n_hover'); }
});
//回到顶部
$(function () {
    $('#roll_top').click(function () { $('html,body').animate({ scrollTop: '0px' }, 800); });
    $('#fall').click(function () { $('html,body').animate({ scrollTop: $('.c_foot1').offset().top }, 800); });
});
//鼠标经过效果

$(function () {
    $('#store_cate').hover(function () {
        $(this).addClass('page_hover');
    }, function () {
        $(this).removeClass('page_hover');
    });
    $('#store_cate').click(function () {
        $(this).toggleClass('page_hover');
    });
    $('#city_now').click(function () {
        $('.store_h_dq').toggleClass('page_hover');
    });
    //点击
    $('.store_cate a').click(function () {
        var store_cate_val = $(this).text();
        $('#store_cate_val').text(store_cate_val);
        $('#update_service_name').text(store_cate_val);
        initMap();
    })

});

$(function () {
    $(".ts_1 .t1 i").click(function () {
        document.cookie = 'index_huadong=true';
        $(".ts_1").remove();
    });
    $(".ts_2 .t1 i").click(function () {
        document.cookie = 'index_rem_one=true';
        $(".ts_2").remove();
    })
});

$(function () {
    $("#num2").focus(function () {
        $(".ts_2").css('display', 'none')
    })
});

$(function () {
    $(".dd").click(function () {
        $(".ts_1").css('display', 'none')
    });
});

$('.my_order,.d_hover').live({
    mouseenter:
    function () {
        $(this).addClass('t_hover');
        $(this).children('i').addClass('rotate');
        if ($('.order_box ul').hasClass('ajax_user_orders')) {
            $.ajax({
                type: "post",
                data: 'oper=ajax_user_orders',
                url: "/ajax_return.php",
                dataType: "html",
                success: function (data) {
                    $('.my_order .ajax_user_orders').html(data);
                    $('.order_box ul').removeClass('ajax_user_orders')
                }
            });
        }
    },
    mouseleave:
   function () {
       $(this).removeClass('t_hover');
       $(this).children('i').removeClass('rotate');
   }
});

//全部商品分类
$(function () {
    $('#head_01 .p_categorys,#head_01 .pro_list dl').hover(function () {
        $(this).addClass('p_list');
    }, function () {
        $(this).removeClass('p_list');
    })
});
//我的车库
$(function () {
    $('#head_01 .my_garage').hover(function () {
        $(this).addClass('p_list');
        //alert(document.getElementById('user_car_list').innerText);
        var user_car_list_text = $("#user_car_list").text().replace(/\s/gi, '');
        //var user_car_list_text = document.getElementById('user_car_list').innerText.replace(/\s/gi,'');
        //alert(user_car_list_text);
        if (user_car_list_text == null || user_car_list_text == "") {
            $("#user_car_list").html('<li class="g_over"><a href="#" >正在努力为您加载，请稍候···</a></li>');
            //document.getElementById('user_car_list').innerHtml = '<li class="g_over"><a href="#" >正在努力为您加载，请稍候···</a></li>';
            $.ajax({
                type: "post",
                data: 'oper=get_user_car_list',
                url: "/ajax_return.php",
                dataType: "html",
                success: function (data) {
                    //var data_arr = data.split('@@');
                    $("#user_car_list").html(data);
                    //alert(data);
                    //document.getElementById('user_car_list').innerHtml = data;
                    $('#user_car_list li').hover(function () {
                        $(this).addClass('page_hover');
                    }, function () {
                        $(this).removeClass('page_hover');
                    });
                    $('#iclose_btn').click(function () {
                        $('.my_garage').removeClass('p_list');
                    });
                    $('#iclose_bg').hover(function () {
                        $('.my_garage').removeClass('p_list');
                    });
                    //$("#car_login_info").html(data_arr[1]);
                }
            });
        }
        $('select').css('size', "10")
    }, function () {
        //$(this).removeClass('p_list');
    });
    $('#head_01 .n_add a').click(function () {
        $(this).toggle();
        $('.rides').css('display', 'block');
    })
    $('#iclose_btn').click(function () {
        $('.my_garage').removeClass('p_list');
    });
});

//下拉列表
$(function () {
    $('.categorys > ul > li,.garages > ul > li').hover(function () {
        $(this).addClass('p_over');
    }, function () {
        $(this).removeClass('p_over');
    })
});
//搜索框效果
$(function () {
    $('.n_search input:text').each(function () {
        $(this).focus(function () {
            $(this).animate({ width: "170px" }, 400);
            $(this).css({ background: "#fff", color: "#000" });
        }).blur(function () {
            $(this).animate({ width: "150px" }, 400);
        });
    })
});
//网站内容首页js
$(function () {
    $('.h_goods li').hover(function () {
        $(this).addClass("s_hover");
    }, function () {
        $(this).removeClass("s_hover");

    })

});
$(function () {
    setInterval(function () {
        $('.change_img').prepend($(".change_img li").last().css('height', '0px').animate({ 'height': '145px' }, 1000));
        $('.change_img2').prepend($(".change_img2 li:last-child").css('width', '0px').animate({ 'width': '160px' }, 1000));
    }, 1500)

});

//幻灯片
$(function () {
    var timer_1 = setInterval(autoRun1, 6000);
    var cur = 0;
    function autoRun1() {
        cur++;
        var num_1 = $('#flash img').size();
        cur = (cur == num_1) ? 0 : cur;
        $('#flash a').eq(cur).fadeIn(1200).siblings('a').hide();
        $('#flash ul li').eq(cur).addClass('cur').siblings('li').removeClass('cur');
    }
    $('#flash ul li,#flash > a').hover(function () {
        clearInterval(timer_1);
        var cur = $(this).index();
        $('#flash a').eq(cur).fadeIn(1200).siblings('a').hide();
        $('#flash ul li').eq(cur).addClass('cur').siblings('li').removeClass('cur');
    }, function () {
        timer_1 = setInterval(autoRun1, 5000);
    })
});

$(function () {
    var timer = setInterval(autoRun, 4000);
    var cur = 0;
    function autoRun() {
        cur++;
        var num_2 = $('#flash1 img').size();
        cur = (cur == num_2) ? 0 : cur;
        $('#flash1 a').eq(cur).fadeIn(1000).siblings('a').hide();
        $('#flash1 ul li').eq(cur).addClass('cur').siblings('li').removeClass('cur');
    }
    $('#flash1 ul li,#flash1 > a').hover(function () {
        clearInterval(timer);
        var cur = $(this).index();
        $('#flash1 a').eq(cur).fadeIn(1000).siblings('a').hide();
        $('#flash1 ul li').eq(cur).addClass('cur').siblings('li').removeClass('cur');
    }, function () {
        timer = setInterval(autoRun, 4000);
    })
});

//车型特卖会
$(function () {
    $('#s_main li,.sm_list li,.c_list li,#mbargain').hover(function () {
        $(this).addClass("hover_bb");
    }, function () {
        $(this).removeClass("hover_bb");
    })
});
//鼠标经过显示隐藏
$(function () {
    $('#mbargain').hover(function () {
        $('#bargain').css('display', 'block');
    }, function () {
        $('#bargain').css('display', 'none');
    })
});


//最后一个边框为0
$(function () {
    $('.tm_plan_b dl').last().css('border', '0');
});

//产品列表
$(function () {
    $(".filter_showmore").click(function () {
        $(this).parent().parent().toggleClass("gl_hover_i");
    });

    $(".filter_showmore").toggle(function () {
        $(this).parent().parent().addClass("gl_hover_i");
        $(this).children("i").css("background-Position", "-1px -136px");
    }, function () {
        $(this).children("i").css("background-Position", "-16px -136px");
        $(this).parent().parent().removeClass("gl_hover_i");
    });
    $(".filter_choosemore").click(function () {
        $(this).parent().parent().toggleClass("gl_hover_more");
    });
    $(".filter_cancel,.filter_sure").click(function () {
        $(".g_filter").removeClass("gl_hover_more");
        $(".g_filter").addClass("gl_hover_i");
    });
});

$(function () {
    $(".show_lb").click(function () {
        $(".sm_list").addClass("goods_lb");
        $(".g_sort_r").addClass("show_sort");
    });
    $(".show_gz").click(function () {
        $(".sm_list").removeClass("goods_lb");
        $(".g_sort_r").removeClass("show_sort");
    });

});

$(function () {
    $(".j_FPInput").focus(function () {
        $(".fPb").addClass("fPb_2");
    })
	.blur(function () {
	    //$(".fPb").removeClass("fPb_2");  
	});
});
