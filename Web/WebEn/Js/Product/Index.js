$(document).ready(function () {
    //第一次访问产品相关页面 弹出注册框
    LoadReg();
    //  for Product/Index
    $('.scrollLoading').scrollLoading({
        callback: function () {
            if ($(this).hasClass("recommend_cont")) {
                $(this).find("div.img_noborder").each(function () {
                    var idx = $(this).parent().index();
                    if (idx > 1) {
                        $(this).addClass("img_noborder2");
                    }
                });
            }
        }
    });
    $('.list_pro_img a.list_pro_img_a').hover(function () {
        var idx = $('.list_pro_img a.list_pro_img_a').index(this);
        var target = $('.list_pro_img .products_list_popup').eq(idx);
        $(target).show();
        if (!$(target).hasClass("hasLoadOver")) {
            $(target).find("img").attr("src", $(target).find("img").attr("data-original"));
            $(target).addClass("hasLoadOver");
        }
    }, function () {
        var idx = $('.list_pro_img a.list_pro_img_a').index(this);
        $('.list_pro_img .products_list_popup').eq(idx).hide();
    });
    $(".gallery_img a.gallery_pro_img_a,.gallery_img2 a.gallery_pro_img_a").hover(function () {
        $(this).offsetParent().next().show();
    }, function () {
        $(this).offsetParent().next().hide();
    });

    //  for Product/Detail
    $(".small_imgshow li img").click(function () {
        $(".big_imgshow img#big_imgshow_img").attr("src", $(this).data("src"));
        $(".small_imgshow li").removeClass("current");
        $(this).parent().addClass("current");
    });

    $(".big_imgshow").click(function () {
        $("#popup_imgshow").modal("show");
        var idx = $(".small_imgshow li.current").index();
        var target = $(".pro_imgshow ul li:eq(" + idx + ")");
        $(".pro_imgshow ul li").removeClass("current");
        $(target).addClass("current");
        $(".pro_imgshow img#pop_imgshow_img").attr("src", $(target).find("img").data("src"));
    });

    $(".pro_imgshow li img").click(function () {
        $(".pro_imgshow img#pop_imgshow_img").attr("src", $(this).data("src"));
        $(".pro_imgshow li").removeClass("current");
        $(this).parent().addClass("current");
    });

    $(".pro_imgshow a.arrow_up,.pro_imgshow a.arrow_down").click(function () {
        var idx = $(".pro_imgshow li.current").index();
        var len = $(".pro_imgshow li").length;
        if ($(this).hasClass("arrow_up")) {
            var newIdx = (idx - 1) < 0 ? len - 1 : idx - 1;
        } else {
            var newIdx = (idx + 1) >= len ? 0 : idx + 1;
        }
        var target = $(".pro_imgshow ul li:eq(" + newIdx + ")");
        $(".pro_imgshow ul li").removeClass("current");
        $(target).addClass("current");
        $(".pro_imgshow img#pop_imgshow_img").attr("src", $(target).find("img").data("src"));
    });

    $("div.price_img").on("click", function () {
        $(this).children().toggle();
    });
});
