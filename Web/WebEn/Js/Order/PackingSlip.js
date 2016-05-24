$(function () {
    //Packing Slip列表查询
    $("#PackingSlipFilter").on("click", function () {
        $("#SlipSearchForm").ajaxSubmit({
            url:"/Order/PackingSlip",
            success: function (responseText) {
                $("#divsliplist").html(responseText);
            }
        });

        var t = $("#divsliplist").html();
        t = t.replace(/[\r\n]/g,"").replace(/[ ]/g,"");
        if (t == '') {
            $(".warning_wrap").css("display", "block");
            $(".slip_list_cont").css("display", "none");
        } else {
            $(".slip_list_cont").css("display", "block");
            $(".warning_wrap").css("display", "none");
        }
    });


    $('body').on("click","#bottomdownload,#topdownload", function () {
        $("#bottomdownload,#topdownload").attr("href", "/order/DownloadPackingSlip?" + $("#SlipSearchForm").serialize());
    });

});
