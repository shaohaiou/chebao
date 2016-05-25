/// <reference path="jquery-1.3.2.min.js" />

var timer_prompts;
function prompts(t, msg) {
    if ($(".msgflay").length == 0) {
        var html = "<div class=\"msgflay\">"
            + "<div class=\"msgflayl\"></div>"
            + "<div class=\"msgflayb\"><span></span></div>"
            + "<div class=\"msgflayr\"></div></div>";
        $("body").append(html);
    }

    var left = $(t).offset().left + $(t).width() / 2 - 10;
    var top = $(t).offset().top + $(t).outerHeight(true);
    $(".msgflay").css({ left: left, top: top }).find(".msgflayb span").text(msg);
    $(".msgflay").fadeIn(300, function () {
        timer_prompts = setTimeout(function () {
            $(".msgflay").fadeOut();
        }, 5000);
    });
    $(t).blur(function () {
        $(".msgflay").fadeOut();
        if (timer_prompts)
            clearTimeout(timer_prompts);
    });
    $(t).keypress(function () {
        $(".msgflay").fadeOut();
        if (timer_prompts)
            clearTimeout(timer_prompts);
    });
}


function showLoading(msg) {
    if ($(".overlay").length == 0) {
        var html = "<div class=\"overlay\"></div>";
        $("body").append(html);
    }
    if ($(".showbox").length == 0) {
        var html = "<div id=\"AjaxLoading\" class=\"showbox\">"
         + "<div class=\"loadingWord\"><img src=\"/images/waiting.gif\">" + msg + "</div>"
         + "</div>";
        $("body").append(html);
    }

    $(".overlay").css({ 'display': 'block', 'opacity': '0.8' });
    $(".showbox").css({ 'margin-top': '200px', 'opacity': '1' });
}

function closeLoading() {
    $(".overlay").remove();
    $(".showbox").remove();
}