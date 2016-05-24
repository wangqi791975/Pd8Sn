var customerEmail = "";
var clubManager = "";
jQuery(function () {
    $("#searchBtn").click(function () {
        customerEmail = $("#customerEmail").val();
        clubManager = $("#clubManager").val();
    });

    $("#exportbtn").click(function () {
        $("#exportbtn").attr("href", "/Club/ExportExcel?customerEmail" + customerEmail + "&clubManager=" + clubManager);
    });
});
function conapyment(clubId) {
    if (confirm("确认付款？")) {
        $.ajax({
            type: "POST",
            url: "/Club/ConfirmPayment/" + clubId,
            success: function (responseText) {
                var msg;
                if (responseText.error != true) {
                    msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '操作成功!' : responseText.msg;
                    $(".locationopt").before('<div class="alert alert-success"><a class="close" data-dismiss="alert">&times;</a>' + msg + '</div>');
                } else {
                    msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '操作失败!' : responseText.msg;
                    $(".locationopt").before('<div class="alert alert-error"><a class="close" data-dismiss="alert">&times;</a>' + msg + '</div>');
                }
            }
        });
    }
}

function deletemanager(managerId) {
    if (confirm("确认删除？")) {
        $.ajax({
            type: "POST",
            url: "/Club/DeleteManager/" + managerId,
            success: function (responseText) {
                var msg;
                if (responseText.error != true) {
                    msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '操作成功!' : responseText.msg;
                    $(".locationopt").before('<div class="alert alert-success"><a class="close" data-dismiss="alert">&times;</a>' + msg + '</div>');
                } else {
                    msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '操作失败!' : responseText.msg;
                    $(".locationopt").before('<div class="alert alert-error"><a class="close" data-dismiss="alert">&times;</a>' + msg + '</div>');
                }
            }
        });
    }
}