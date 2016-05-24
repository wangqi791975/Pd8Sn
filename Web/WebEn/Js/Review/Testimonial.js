$(function () {
    $(".testimonial_language .list_item").click(function () {
        $.ajax({
            type: "GET",
            url: "/Reviews/Testimonial?filter=" + $(this).attr("id"),
            success: function (result) {
                $("#divtestimonialist").html(result);
            }
        });
    });
});