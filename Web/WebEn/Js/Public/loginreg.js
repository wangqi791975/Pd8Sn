
//登录页面和注册页面点击图片时，刷新验证码
$(function () {
    $(document).ready(function () {
        if (parseInt($("#valoginerrcount").val()) > 2) {
            $("#valiCode")[0].src = "/Account/GetValidatorGraphics?time=" + (new Date()).getTime();
        }
        if (parseInt($("#valregistercount").val()) >= 3) {
            $("#valiCode")[0].src = "/Account/GetValidatorGraphics?time=" + (new Date()).getTime();
        }
    });

    $("#valiCode").bind("click", function () {
        this.src = "/Account/GetValidatorGraphics?time=" + (new Date()).getTime();
        $("#iconcheck").attr("class", "icon_code_error");
    });

    //验证码正确错误图标改变
    $("#validatecode").keyup(function () {
        if ($("#validatecode").val().length == 4) {
            $.ajax({
                type: "POST",
                url: "/Account/CheckIcon",
                dataType: "text",
                success: function (code) {
                    if (code == $("#validatecode").val().toUpperCase()) {
                        $("#iconcheck").attr("class", "icon_code_ok");
                    }
                }
            });
        } else {
            $("#iconcheck").attr("class", "icon_code_error");
        }
    });

});