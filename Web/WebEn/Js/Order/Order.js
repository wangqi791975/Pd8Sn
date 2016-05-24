$(function () {
    //  日历控件
    $(".datepicker").datepicker({
        //$.datepicker.regional["en"]
        //showButtonPanel: true
    });

    //  订单列表查询
    $("#OrderFilterBtn").on("click", function () {
        var status = $("#status").val();
        $("#OrderFilterForm").ajaxSubmit({
            success: function (responseText) {
                $("#divorderlist").html(responseText);
                if (status !== "") {
                    $("#h2StatusName").text($("#OrderFilterForm .pop_select_cont li.active").text());
                    $(".account_sidebar a[id^=order_status_]").removeClass("current");
                    $(".account_sidebar a#order_status_"+status).addClass("current");
                }
            }
        });
    });
    //下拉清除内容
    $(".pop_select_cont li:not('.active')").click(function () {
        $("#orderno").val('');
        $("#partno").val('');
        $("#startdate").val('');
        $("#enddate").val('');
    });
});


function orderparm() {
    return { orderno: $("#orderno").val(), partno: $("#partno").val(), status: $("#status").val(), startdate: $("#startdate").val(), enddate: $("#enddate").val() };
}
