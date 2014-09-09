/// <reference path="jquery-1.3.2.min.js" />

/*
* bitmapcutter
* version: 1.0.0 (05/24/2009)
* @ jQuery v1.2.*
*
* Licensed under the GPL:
*   http://gplv3.fsf.org
*
* Copyright 2008, 2009 Jericho [ thisnamemeansnothing[at]gmail.com ] 
*  
*/
//using global variables to store the data
window.$bcglobal = {
    $originalSize: { width: 0, top: 0 },
    $zoomValue: 1.0,
    $thumbimg: null,
    $img: null,
    $cutter: null,
    $allow: true
};
$.extend(
    $.fn, {
        ///<summary>
        /// fetch css value
        ///</summary>
        ///<param name="c">style name</param>
        f: function (c) {
            return parseInt($(this).css(c));
        },
        ///<summary>
        /// preload bitmap
        ///</summary>
        ///<param name="callback">callback function, it'll be fired after the 'preload' has completed</param>
        loadBitmap: function (callback) {
            var me = this,
                  bitmapCutterHolder = new Image(),
                  src = me.attr('rel');
            bitmapCutterHolder.src = src;

            this.onCompleted = function () {
                $(me).attr('src', src);
                $bcglobal.$originalSize = { width: bitmapCutterHolder.width, height: bitmapCutterHolder.height };

                callback(src);
            };
            if ($.browser.msie) {
                if (bitmapCutterHolder.readyState == "complete") {
                    me.onCompleted();
                }
                else bitmapCutterHolder.onreadystatechange = function () {
                    if (this.readyState == "complete") {
                        me.onCompleted();
                    }
                }
            }
            else {
                bitmapCutterHolder.onload = me.onCompleted;
            }
        },
        ///<summary>
        /// zoom in or zoom out the bitmap
        ///<remarks>
        /// use the global variable 'zoomValue' as zoom rate
        ///</remarks>
        ///</summary>
        scaleBitmap: function () {
            return this.each(function () {
                var me = $(this),
                      os = $bcglobal.$originalSize,
                      zoomValue = $bcglobal.$zoomValue;

                me.hide();
                if (os.width > 0 && os.height > 0) {
                    me.height(os.height * zoomValue)
                          .width(os.width * zoomValue);
                }
                var p = me.parent(),
                      w = me.width(),
                      h = me.height(),
                      t = (p.height() - h) / 2,
                      l = (p.width() - w) / 2;
                me.css({ 'top': t, 'left': l }).show();
                $('.jquery-bitmapcutter-info').html(w + ' : ' + h);
            });
        },
        ///<summary>
        /// resize, drag and drop
        ///</summary>
        ///<param name="setting">parameters, include limited value(left and top),condition and callback function</param>
        dragndrop: function (setting) {
            var ps = $.fn.extend({
                limited: {
                    lw: { min: 0, max: 100 },
                    th: { min: 0, max: 100 }
                },
                handler: null,
                callback: function (e) { }
            }, setting);
            var dragndrop = {
                drag: function (e) {
                    if ($bcglobal.$allow) {
                        var d = e.data.d;
                        var p = {
                            left: Math.min(Math.max(e.pageX + d.left, ps.limited.lw.min), ps.holderSize.width - $(".jquery-bitmapcutter-cutter").width()),
                            top: Math.min(Math.max(e.pageY + d.top, ps.limited.lw.min), ps.holderSize.height - $(".jquery-bitmapcutter-cutter").height()),
                            target: d.target
                        };
                        ps.callback(p);
                    }
                },
                drop: function (e) {
                    $().unbind('mousemove', dragndrop.drag).unbind('mouseup', dragndrop.drop);
                }
            };
            return this.each(function () {
                if (ps.handler == null) { ps.handler = $(this) };
                var handler = (typeof ps.handler == 'string' ? $(ps.handler) : ps.handler);
                handler.bind('mousedown', function (e) {
                    var data = {
                        target: $(this),
                        left: $(this).f('left') - e.pageX,
                        top: $(this).f('top') - e.pageY
                    };
                    $().bind('mousemove', { d: data }, dragndrop.drag).bind('mouseup', dragndrop.drop);
                });
            });
        },
        ///<summary>
        /// bitmap cutter, main function
        ///</summary>
        ///<param name="setting">parameters</param>
        bitmapCutter: function (settings) {
            var lang = {
                zoomout: 'Zoom out',
                zoomin: 'Zoom in',
                original: 'Original size',
                clockwise: 'Clockwise rotation({0} degrees)',
                counterclockwise: 'Counterclockwise rotation({0} degrees)',
                generate: 'Generate!',
                process: 'Please wait, transaction is processing......',
                left: 'Left',
                right: 'Right',
                up: 'Up',
                down: 'Down'
            };
            var ps = $.fn.extend({
                src: '',
                renderTo: $(document.body),
                holderSize: { width: 300, height: 400 },
                cutterSize: { width: 70, height: 70 },
                zoomStep: 0.2,
                zoomIn: 2.0,
                zoomOut: 0.1,
                rotateAngle: 90,
                //pixel
                moveStep: 100,
                onGenerated: function (src) { },
                onComplete: function () { },
                lang: lang
            }, settings);

            //fill parameters
            ps.lang = $.extend(lang, ps.lang);
            ps.lang.clockwise = ps.lang.clockwise.format(ps.rotateAngle);
            ps.lang.counterclockwise = ps.lang.counterclockwise.format(ps.rotateAngle);
            ///<sammary>
            /// zoom interface
            ///</sammary>
            ///<param name="zv">current zoom value</param>
            function izoom(zv) {
                $bcglobal.$zoomValue = zv;
                $bcglobal.$img.scaleBitmap($bcglobal.$zoomValue);
                $bcglobal.$thumbimg.scaleBitmap($bcglobal.$zoomValue);
                scissors.createThumbnail();
            }
            ///<sammary>
            /// image rotation interface
            ///</sammary>
            ///<param name="angle">rotate angle(degree)</param>
            function irotate(angle) {
                var img = $bcglobal.$img,
                  thumbimg = $bcglobal.$thumbimg,
                  zoomValue = $bcglobal.$zoomValue,
                //Use 'rel' but not 'src',  this helps to make sure that the 'src' attribute was not start with 'http://localhost:8888/...'(msie)
                  src = img.attr('rel');
                $.ajax({
                    url: 'cutimage.ashx',
                    dataType: 'json',
                    data: { action: 'RotateBitmap', src: src, angle: angle, t: Math.random(), code: $("#key").val(), st: $("#hdnServerType").val() },
                    error: function (msg) {
                        alert('rotate failed!');
                    },
                    success: function (json) {
                        if (json.msg == 'success') {
                            $bcglobal.$originalSize = json.size;
                            //clear cache of img
                            src += '?t=' + Math.random();
                            img.attr('src', src).scaleBitmap();
                            thumbimg.attr('src', src).scaleBitmap();
                            scissors.createThumbnail();
                        }
                        else {
                            alert(json.msg);
                        }
                    }
                });
            }
            ///<sammary>
            /// image movement
            ///</sammary>
            ///<param name="direction">move direction(left, up, right, down)</param>
            function imove(direction) {
                var thumbimg = $bcglobal.$thumbimg,
                      img = $bcglobal.$img,
                      cutter = $bcglobal.$cutter,
                      w = img.width(),
                      h = img.height(),
                      l = img.f('left'),
                      t = img.f('top');

                if (w <= ps.holderSize.width && h <= ps.holderSize.height) {
                    return;
                }
                var limited = {
                    left: { min: Math.min(ps.holderSize.width - w, 0), max: Math.max(ps.holderSize.width - w, 0) },
                    top: { min: Math.min(ps.holderSize.height - h, 0), max: Math.max(ps.holderSize.height - h, 0) }
                };
                /*
                * it's really a weird thing that i cant use '
                *  img.animate({
                *       d: v
                *    }, function() {
                *       thumbimg.fadeIn();
                *        scissors.createThumbnail();
                *   });
                * '
                * here (d was the direction-'left' and 'top', v was the position data to be calculated!)
                * maybe it's the json to haunt me;-)
                */
                if (!img.is(':animated')) {
                    thumbimg.fadeOut();
                    var v = 0, d = {};
                    switch (direction) {
                        case 'left':
                            v = Math.min(limited.left.max, l + ps.moveStep);
                            d = { left: v };
                            break;
                        case 'right':
                            v = Math.max(limited.left.min, l - ps.moveStep);
                            d = { left: v };
                            break;
                        case 'up':
                            v = Math.min(limited.top.max, t + ps.moveStep);
                            d = { top: v };
                            break;
                        case 'down':
                            v = Math.max(limited.top.min, t - ps.moveStep);
                            d = { top: v };
                            break;
                    }

                    img.animate(d, function () {
                        thumbimg.fadeIn();
                        scissors.createThumbnail();
                    });
                }
            }
            var scissors = {
                createThumbnail: function () {
                    var thumbimg = $bcglobal.$thumbimg,
                          img = $bcglobal.$img,
                          cutter = $bcglobal.$cutter;

                    thumbimg.css({
                        'left': -cutter.f('left') + img.f('left'),
                        'top': -cutter.f('top') + img.f('top')
                    });
                },
                zoomin: function () {
                    //window.console && console.log('zoom value: %s', zoomValue);
                    izoom.call(this, Math.min($bcglobal.$zoomValue + ps.zoomStep, ps.zoomIn));
                },
                zoomout: function () {
                    //window.console && console.log('zoom value: %s', zoomValue);
                    izoom.call(this, Math.max($bcglobal.$zoomValue - ps.zoomStep, ps.zoomOut));
                },
                original: function () {
                    izoom.call(this, 1, 1);
                },
                clockwise: function () {
                    irotate.call(this, ps.rotateAngle);
                },
                counterclockwise: function (e) {
                    irotate.call(this, -ps.rotateAngle);
                },
                left: function () {
                    imove.call(this, 'left');
                },
                up: function () {
                    imove.call(this, 'up');
                },
                right: function () {
                    imove.call(this, 'right');
                },
                down: function () {
                    imove.call(this, 'down');
                }
            };
            ps.renderTo = (typeof ps.renderTo == 'string' ? $(ps.renderTo) : ps.renderTo);

            var $cl = $('<div class="jquery-bitmapcutter-cl" onselectstart="return false;"></div>').appendTo(ps.renderTo);
            var $cr = $('<div class="jquery-bitmapcutter-cr" style="height: 515px;position:relative;"></div>').appendTo(ps.renderTo);

            //bitmap holder
            var $holder = $('<div class="jquery-bitmapcutter-holder jquery-loader" />')
                                    .css(ps.holderSize)
                                        .appendTo($cl);

            //options
            var $opts = $('<div class="jquery-bitmapcutter-opts" >' +
                                    '<div class="r1c1"><a href="javascript:void(0)" onfocus="this.blur()" class="up">&nbsp</a></div>' +
                                    '<div class="r2c1">' +
                                        '<a href="javascript:void(0)" onfocus="this.blur()" class="zoomout">&nbsp</a>' +
                                        '<a href="javascript:void(0)" onfocus="this.blur()" class="zoomin">&nbsp</a>' +
                                        '<a href="javascript:void(0)" onfocus="this.blur()" class="left">&nbsp</a>' +
                                        '<a href="javascript:void(0)" onfocus="this.blur()" class="original">&nbsp</a>' +
                                        '<a href="javascript:void(0)" onfocus="this.blur()" class="right">&nbsp</a>' +
                                        '<a href="javascript:void(0)" onfocus="this.blur()" class="counterclockwise">&nbsp</a>' +
                                        '<a href="javascript:void(0)" onfocus="this.blur()" class="clockwise">&nbsp</a>' +
                                    '</div>' +
                                     '<div class="r3c1"><a href="javascript:void(0)" onfocus="this.blur()" class="down">&nbsp</a></div>' +
                                '</div>').insertAfter($holder);

            $opts.css('width', ps.holderSize.width)
                        .find('div.r2c1>a:eq(0)')
                            .css('margin-left', (ps.holderSize.width - (16 + 6) * 7 + 3) / 2);

            //informations of bitmap
            var $info = $('<div title="Size Of Bitmap" class="jquery-bitmapcutter-info" />')
                                .insertAfter($opts)
                                    .css('width', ps.holderSize.width);

            //cutter
            var $cutter = $('<div class="jquery-bitmapcutter-cutter" >&nbsp</div>')
                                    .css(ps.cutterSize)
                                        .css({
                                            'left': (ps.holderSize.width - ps.cutterSize.width) / 2,
                                            'top': (ps.holderSize.height - ps.cutterSize.height) / 2,
                                            'opacity': 0.4
                                        }).appendTo($holder);
            //initialize zoom value


            //image
            var $img = $('<img alt="" rel="' + ps.src + '" />')
                                        .appendTo($holder);

            //thumbnail
            var $thumbimg = $('<img alt="" rel="' + ps.src + '" />')
                                        .appendTo(
                                            $('<div class="jquery-bitmapcutter-thumbnail" />')
                                                .css(ps.cutterSize)
                                                    .appendTo($cr)
                                                );

            var imgSize = "";
            var fn = function () {
                imgSize = $(this).width() + "*" + $(this).height();
                ps.cutterSize.width = $(this).width();
                ps.cutterSize.height = $(this).height();
                $cutter.remove();
                $cutter = $('<div class="jquery-bitmapcutter-cutter" >&nbsp</div>').css(ps.cutterSize).css({
                    'left': (ps.holderSize.width - ps.cutterSize.width) / 2,
                    'top': (ps.holderSize.height - ps.cutterSize.height) / 2,
                    'opacity': 0.4
                }).appendTo($holder);
                $cutter.dragndrop({
                    limited: {
                        lw: { min: 0, max: ps.holderSize.width - ps.cutterSize.width },
                        th: { min: 0, max: ps.holderSize.height - ps.cutterSize.height }
                    },
                    callback: function (e) {
                        $cutter.css({
                            left: e.left,
                            top: e.top
                        });
                        scissors.createThumbnail();
                    },
                    holderSize: {
                        width: ps.holderSize.width,
                        height: ps.holderSize.height
                    }
                });
                $(".jquery-bitmapcutter-cutter").resizable({
                    aspectRatio: ps.cutterSize.width / ps.cutterSize.height,
                    minHeight: ps.cutterSize.height,
                    minWidth: ps.cutterSize.width,
                    containment: ".jquery-bitmapcutter-holder",
                    start: function (event, ui) {
                        $(".jquery-bitmapcutter-cutter").addClass("");
                        $bcglobal.$allow = false;
                    },
                    stop: function (event, ui) {
                        $(".jquery-bitmapcutter-cutter").removeClass("");
                        //                        ps.holderSize.width = parseInt($(this).css("width"));
                        //                        ps.holderSize.height = parseInt($(this).css("width"));
                        $bcglobal.$allow = true;
                    }
                }).css("position", "absolute");
                $bcglobal.$cutter = $cutter;
            };
            var $newimg = $('<img class="jquery-bitmapcutter-newimg" alt="" src="" width="' + ps.cutterSize.width + '" height="' + ps.cutterSize.height + '" />').appendTo($cr).hide();
            var $newimgcheck = $('<input type="checkbox" class="newimgcheck" val="c" checked="checked" />').appendTo($cr).hide();
            $newimg.click(fn);
            var $newimg_b, $newimg_m, $newimgcheck_b, $newimgcheck_m;
            if ($("#hdnAction").length > 0 && $("#hdnAction").val() == "AnimationGenerateBitmap") {
                $newimg_m = $('<img class="jquery-bitmapcutter-newimg" alt="" style="margin-left:20px;" src="" width="74" height="74" />').appendTo($cr).hide();
                $newimgcheck_m = $('<input type="checkbox" class="newimgcheck" val="m" checked="checked" />').appendTo($cr).hide();
                $cr.append("<br >");
                $newimg_b = $('<img class="jquery-bitmapcutter-newimg" alt="" src="" width="200" height="160" />').appendTo($cr).hide();
                $newimgcheck_b = $('<input type="checkbox" class="newimgcheck" val="b" checked="checked" />').appendTo($cr).hide();
                $newimg_m.click(fn);
                $newimg_b.click(fn);
            } else if ($("#hdnAction").length > 0 && $("#hdnAction").val() == "ComicsGenerateBitmap") {
                $newimg_m = $('<img class="jquery-bitmapcutter-newimg" alt="" style="margin-left:20px;" src="" width="74" height="74" />').appendTo($cr).hide();
                $newimgcheck_m = $('<input type="checkbox" class="newimgcheck" val="m" checked="checked" />').appendTo($cr).hide();
                $cr.append("<br >");
                $newimg_b = $('<img class="jquery-bitmapcutter-newimg" alt="" src="" width="180" height="240" />').appendTo($cr).hide();
                $newimgcheck_b = $('<input type="checkbox" class="newimgcheck" val="b" checked="checked" />').appendTo($cr).hide();
                $newimg_m.click(fn);
                $newimg_b.click(fn);
            }
            var $generate = $('<a href="javascript:void(0)" class="generate" onfocus="this.blur()" >' + ps.lang.generate + '</a>')
                                        .appendTo($cr);
            //            var $newimg = $('<img class="jquery-bitmapcutter-newimg" alt="" src=""' + ' width=" ' + ps.cutterSize.width + '" height=" ' + ps.cutterSize.height + '"/>')
            //                                        .appendTo($cr).hide();
            var $processed = $('<div class="process">' + ps.lang.process + '</div>')
                                            .hide()
                                                .appendTo($cr);
            var $saveimg = $('<div class=".saveimg" style="position: absolute; bottom: 0pt; left: 100px;"><input class="savebt" type="button" value="确认保存"/></div>').hide().appendTo($cr);

            $img.loadBitmap(function (e) {
                //alert(bitmapCutterHolder);
                $bcglobal.$zoomValue = 1;
                var height = $bcglobal.$originalSize.height;
                var he = ps.holderSize.height;
                var tmph = height;
                while (tmph > he) {
                    $bcglobal.$zoomValue = $bcglobal.$zoomValue - ps.zoomStep;
                    tmph = parseInt(height * $bcglobal.$zoomValue);
                }
                var d = { top: 0, left: 0 };
                $img.animate(d, function () {
                    scissors.createThumbnail();
                });
                $img.scaleBitmap();
                $holder.removeClass('jquery-loader');


                var ks = {
                    k37: 'left',
                    k38: 'up',
                    k39: 'right',
                    k40: 'down',
                    k45: 'zoomout',
                    k61: 'zoomin'
                };

                $().keypress(function (e) {
                    var k = (e.keyCode || e.which);
                    //window.console && console.log('key code: %s', k);
                    if ((k >= 37 && k <= 40) || k == 45 || k == 61) {
                        var func = eval('scissors.' + eval('(ks.k' + k + ')') + '');
                        func();
                    }
                });

                $thumbimg.attr('src', $img.attr('src')).scaleBitmap();

                $opts.find('a').each(function () {
                    var me = $(this), c = me.attr('class');
                    me.attr('title', eval('(ps.lang.' + c + ')'))
                        .bind('click', eval('(scissors.' + c + ')'));
                });

                $cutter.dragndrop({
                    limited: {
                        lw: { min: 0, max: ps.holderSize.width - ps.cutterSize.width },
                        th: { min: 0, max: ps.holderSize.height - ps.cutterSize.height }
                    },
                    callback: function (e) {
                        $cutter.css({
                            left: e.left,
                            top: e.top
                        });
                        scissors.createThumbnail();
                    },
                    holderSize: {
                        width: ps.holderSize.width,
                        height: ps.holderSize.height
                    }
                });
                $(".jquery-bitmapcutter-cutter").resizable({
                    aspectRatio: ps.cutterSize.width / ps.cutterSize.height,
                    minHeight: ps.cutterSize.height,
                    minWidth: ps.cutterSize.width,
                    containment: ".jquery-bitmapcutter-holder",
                    start: function (event, ui) {
                        $(".jquery-bitmapcutter-cutter").addClass("");
                        $bcglobal.$allow = false;
                    },
                    stop: function (event, ui) {
                        //                        var vu = ps.cutterSize.width / parseInt($(this).css("width"));
                        //                        izoom(vu);
                        $(".jquery-bitmapcutter-cutter").removeClass("");
                        //                        ps.holderSize.width = parseInt($(this).css("width"));
                        //                        ps.holderSize.height = parseInt($(this).css("width"));
                        $bcglobal.$allow = true;
                    }
                });
                $(".ui-resizable").css("position", "absolute");
                $(".jquery-bitmapcutter-thumbnail").hide();
                //$(".jquery-bitmapcutter-info").hide();
                $info.hide();
                $generate.click(function () {
                    var me = $(this);
                    var _action = "GenerateBitmap";
                    if ($("#hdnAction").length > 0)
                        _action = $("#hdnAction").val();
                    me.fadeOut();
                    $opts.fadeOut();
                    $cutter.fadeOut();
                    //$info.hide();
                    $processed.fadeIn();
                    $.ajax({
                        url: 'cutimage.ashx',
                        dataType: 'json',
                        data: {
                            action: _action,
                            src: ps.src,
                            zoom: $bcglobal.$zoomValue,
                            x: $thumbimg.f('left'),
                            y: $thumbimg.f('top'),
                            width: $(".jquery-bitmapcutter-cutter").width(),
                            height: $(".jquery-bitmapcutter-cutter").height(),
                            t: Math.random(),
                            size: imgSize
                        },
                        error: function (err) {
                            alert('cant generate it!');
                        },
                        success: function (json) {
                            if (json.msg == 'success') {
                                me.fadeIn();
                                $opts.fadeIn();
                                $cutter.fadeIn();
                                //$info.show();
                                $processed.fadeOut();
                                $saveimg.fadeIn();
                                $(".savebt").click(function () { ps.onComplete(); });
                                if (json.src.c) {
                                    $newimg.attr('src', json.src.c + "?date=" + new Date()).show();
                                    $newimg_b.attr('src', json.src.b + "?date=" + new Date()).show();
                                    $newimg_m.attr('src', json.src.m + "?date=" + new Date()).show();
                                    $newimgcheck.show();
                                    $newimgcheck_b.show();
                                    $newimgcheck_m.show();
                                }
                                else
                                    $newimg.attr('src', json.src + "?date=" + new Date()).show();
                                ps.onGenerated(json.src);
                            }
                            else {
                                alert(json.msg);
                            }
                        }
                    });
                });
                $bcglobal.$cutter = $cutter;
                $bcglobal.$img = $img;
                $bcglobal.$thumbimg = $thumbimg;

            });
        }
    });
///<summary>
/// text format(same as c-sharp,
/// e.g.: 'example {0}: {2}'.format('A','none'))
/// output: example A: none
///</summary>
String.prototype.format = function () {
    var args = arguments;
    return this.replace(/{(\d{1})}/g, function () {
        return args[arguments[1]];
    });
};