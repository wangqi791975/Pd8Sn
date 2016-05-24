$(function () {

});
function changeadminstatus(adminId) {
    $.ajax({
        type: "POST",
        url: "/AdminUser/ChangeAdminStatus/" + adminId,
        success: function (result) {
            if (result == "True") {
                $("#" + adminId).attr("class", "btn btn-mini btn-success");
                $("#" + adminId).html("启用");
            } else {
                $("#" + adminId).attr("class", "btn btn-mini btn-danger");
                $("#" + adminId).html("禁用");
            }
        }
    });
}