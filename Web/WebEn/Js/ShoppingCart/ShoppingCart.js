$(document).ready(function() {
    //  滚动
    $("#cart_main,#cart_sidebar").stick_in_parent();
    $(".cart_main .nav-tabs .tab, .cart_main .nav-tabs .tab-click").on("click", function () {
        setTimeout(function () {
            $(document.body).trigger("sticky_kit:recalc");
        }, 800);
    });
});

