
//页面加载完成后立即执行代码 已经移至main.js
/*$(document).ready(function () {
    //客户切换币种
    $(".currency_on a").unbind("click").bind("click", function () {
        var currencyCode = $.trim(this.title);
        if ($.trim(currencyCode) === "") {
            alert("Currency invalid");
        } else
            $.post("/Public/ChangeCurrency", { "currencyCode": currencyCode }, function (data) { if (data.result === "refresh") { window.location.reload(); } });
    });
});*/
