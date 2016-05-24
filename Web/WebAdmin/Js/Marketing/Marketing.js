$(function () {
    $("#FD_CustomerType").on("change", function () {
        var val = $(this).val();
        $("div[id^=FD_CustomerType_]").hide();
        $("#FD_CustomerType_" + val).show();
    });
    $("#FD_RewardType").on("change", function () {
        var val = $(this).val();
        $("div[id^=FD_RewardType_]").hide();
        $("#FD_RewardType_" + val).show();
    });

    //----------coupon开始----------
    $("#languageall").click(function () {
        if ($(this).attr("checked")) {
            $("[name='language']").attr("checked", "true");
        } else {
            $("[name='language']").removeAttr("checked");
        }
    });
    $("#countryall").click(function () {
        if ($(this).attr("checked")) {
            $("[name='continent']").attr("checked", "true");
            $("[name='country']").attr("checked", "true");
        } else {
            $("[name='continent']").removeAttr("checked");
            $("[name='country']").removeAttr("checked");
        }
    });
    $(".continent").click(function () {
        $(this).parent().nextAll("div").first().toggle();
    });
    $(".continentcountry").click(function () {
        if ($(this).attr("checked")) {
            $(this).parent().nextAll("div").first().find("[name='country']").attr("checked", "true");
        } else {
            $(this).parent().nextAll("div").first().find("[name='country']").removeAttr("checked");
        }
    });
    $(":radio[name='LimitType']").first().click(function () {
        $("#pickdiv").hide();
    });
    $(":radio[name='LimitType']").last().click(function () {
        $("#pickdiv").show();
    });

    //页面初始化加载
    $(document).ready(function () {
        if ($("#languageall").attr("checked")) {
            $("[name='language']").attr("checked", "true");
        }

        if ($("#countryall").attr("checked")) {
            $("[name='continent']").attr("checked", "true");
            $("[name='country']").attr("checked", "true");
        } else {
            $(".continentcountry").each(function () {
                if ($(this).attr("checked")) {
                    $(this).parent().nextAll("div").first().find("[name='country']").attr("checked", "true");
                }
            });
        }
    });

    $("[name='country']").click(function () {
        if (!$(this).attr("checked"))
            $("#countryall").removeAttr("checked");
    });

    //coupon保存
    $("#couponsavebtn").click(function () {
        $("#updatecoupon").prev('div.alert').remove();
        var languages = "";
        var countrys = "";
        var descs = "";
        //获取选中语种
        if ($("#languageall").attr("checked")) {
            languages = "All";
        } else {
            $("[name='language']").each(function () {
                if ($(this).attr("checked")) {
                    languages = languages + $(this).val() + ",";
                }
            });
            if (languages != "")
                languages = languages.substr(0, languages.length - 1);
        }
        //获取国家
        if ($("#countryall").attr("checked")) {
            countrys = "All";
        } else {
            //洲显示
            //$("[name='continent']").each(function () {
            //    var continent = "";
            //    var country = "";
            //    if ($(this).attr("checked")) {
            //        continent = $(this).val() + ":" + "All";
            //    } else {
            //        $(this).parent().nextAll("div").first().find("[name='country']").each(function () {

            //            if ($(this).attr("checked")) {
            //                country = country + "," + $(this).val();
            //            }
            //        });
            //        if (country != "") {
            //            country = country + ",";
            //            continent = $(this).val() + ":" + country;
            //        }
            //    }
            //    if (continent != "")
            //        countrys = countrys + continent + ";";
            //});
            $("[name='country']").each(function () {
                if ($(this).attr("checked")) {
                    countrys = countrys + $(this).val() + ",";
                }
            });
            if (countrys != "")
                countrys = countrys.substr(0, countrys.length - 1);
        }

        //获取描述多语言
        $(".tab-pane textarea").each(function () {
            var languageid = $(this).attr("id");
            var desc = $(this).val();
            descs = descs + languageid + "{:}" + desc + "{;}";
        });
        descs = descs.substr(0, descs.length - 3);

        $("#updatecoupon").ajaxSubmit({
            data: { languages: languages, countrys: countrys, descs: descs },
            success: function (responseText) {
                var msg;
                if (responseText.error != true) {
                    msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '操作成功!' : responseText.msg;
                    $("#updatecoupon").before('<div class="alert alert-success"><a class="close" data-dismiss="alert">&times;</a>' + msg + '</div>');
                    setTimeout(function () {
                        location.href = $("#couponsavebtn").next().attr("href");
                    }, 3000);
                } else {
                    msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '操作失败!' : responseText.msg;
                    $("#updatecoupon").before('<div class="alert alert-error"><a class="close" data-dismiss="alert">&times;</a>' + msg + '</div>');
                }
            }
        });
    });

    //----------coupon结束----------
    //----------couponmarketing开始----------
    $("#coumarsavebtn").click(function () {
        $("#updatecoupon").prev('div.alert').remove();
        //目标对象
        var customervipids = "";
        var clublevels = "";
        var ispclub = false;
        var ispchannel = false;
        //语种
        var languageids = "";

        $("[name='exvip']").each(function () {
            if ($(this).attr("checked"))
                customervipids = customervipids + $(this).val() + "|";
        });
        if (customervipids != "") {
            customervipids = customervipids.substr(0, customervipids.length - 1);
        }

        $("[name='exclub']").each(function () {
            if ($(this).attr("checked"))
                clublevels = clublevels + $(this).val() + "|";
        });
        if (clublevels != "") {
            clublevels = clublevels.substr(0, clublevels.length - 1);
        }

        $("[name='language']").each(function () {
            if ($(this).attr("checked"))
                languageids = languageids + $(this).val() + "|";
        });
        if (languageids != "")
            languageids = languageids.substr(0, languageids.length - 1);

        ispclub = $("[name='pclub']").attr("checked") == "checked";
        ispchannel = $("[name='pchannel']").attr("checked") == "checked";

        //error校验
        var mark = true;
        var customerobj = $("#FD_CustomerType option:selected").val();
        if ((customerobj == 1 && customervipids == "") || (customerobj == 2 && clublevels == "")) {
            $("#customerobjerror").html("客户对象不允许为空，请选择");
            mark = false;
        } else if (customerobj == 4 && $("[name='uploadcustomer']").val() == "") {
            $("#customerobjerror").html("客户对象不允许为空，请导入对象客户");
            mark = false;
        } else {
            $("#customerobjerror").html("");
        }
        if (languageids == "") {
            $("#languageerror").html("请选择针对的语种，如果不限制，请勾选“所有”");
            mark = false;
        } else {
            $("#languageerror").html("");
        }
        if ($("[name='limitBeginDate']").val() == "") {
            $("#beginerror").html("开始时间不允许为空，请设置！");
            mark = false;
        } else if (new Date($("[name='limitBeginDate']").val() + " " + $("[name='limitBeginTime']").val()) < new Date()) {
            $("#beginerror").html("开始时间不能小于当前时间，请修改！");
            mark = false;
        } else {
            $("#beginerror").html("");
        }

        if ($("[name='limitEndDate']").val() == "") {
            $("#enderror").html("结束时间不允许为空，请设置！");
            mark = false;
        } else if ($("[name='limitBeginDate']").val() + " " + $("[name='limitBeginTime']").val() >= $("[name='limitEndDate']").val() + " " + $("[name='limitEndTime']").val()) {
            $("#enderror").html("结束时间不能小于开始时间，请修改！");
            mark = false;
        } else {
            $("#enderror").html("");
        }
        checkcoupon();
        //error校验

        if (mark && tip)
            $("#updatecoupon").ajaxSubmit({
                data: { customervipids: customervipids, clublevels: clublevels, languageids: languageids, statue: $("[name='Status']:checked").val(), ispclub: ispclub, ispchannel: ispchannel },
                success: function (responseText) {
                    var msg;
                    if (responseText.error != true) {
                        msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '操作成功!' : responseText.msg;
                        $("#updatecoupon").before('<div class="alert alert-success"><a class="close" data-dismiss="alert">&times;</a>' + msg + '</div>');
                    } else {
                        msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '操作失败!' : responseText.msg;
                        $("#updatecoupon").before('<div class="alert alert-error"><a class="close" data-dismiss="alert">&times;</a>' + msg + '</div>');
                    }
                }
            });
    });

    $("#CouponCode").blur(checkcoupon);

    $("#FD_CustomerType").change("change", function () {
        if ($("#FD_CustomerType option:selected").val() == 2 || $("#FD_CustomerType option:selected").val() == 3 || $("#MarketingType option:selected").val() == 0) {
            $("[name='pclub']").parent().hide();
            $("[name='pchannel']").parent().hide();
        } else {
            $("[name='pclub']").parent().show();
            $("[name='pchannel']").parent().show();
        }
    });
    //----------couponmarketing结束----------
    //----------couponcustomer开始----------
    $("#sendcouponbtn").bind("click", function () {
        $("#sendcouponForm").prev('div.alert').remove();
        $("#sendcouponForm").ajaxSubmit({
            success: function (responseText) {
                var msg;
                if (responseText.error != true) {
                    msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '操作成功!' : responseText.msg;
                    $("#sendcouponForm").before('<div class="alert alert-success"><a class="close" data-dismiss="alert">&times;</a>' + msg + '</div>');
                } else {
                    msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '操作失败!' : responseText.msg;
                    $("#sendcouponForm").before('<div class="alert alert-error"><a class="close" data-dismiss="alert">&times;</a>' + msg + '</div>');
                }
            }
        });
    });
    //----------couponcustomer结束----------

    //----------giftmarketing开始----------
    $("#RewardType").change(function () {
        if ($(this).val() != -1) {
            $(".giftsubdiv").show();
            $("#giftregieterinfo").show();

        } else {
            $(".giftsubdiv").hide();
            $("#giftregieterinfo").hide();
        }
    });


    //----------giftmarketing结束----------
});

var tip = true;
function checkcoupon() {
    $("#CouponCode").nextAll().first().html("");
    $("#CouponCode").nextAll().first().hide();
    $("#CouponCode").nextAll().last().html("");
    $("#CouponCode").nextAll().last().hide();
    var couponCode = $("#CouponCode").val();
    var languageids = "";
    $("[name='language']").each(function () {
        if ($(this).attr("checked"))
            languageids = languageids + $(this).val() + "|";
    });
    if (languageids != "")
        languageids = languageids.substr(0, languageids.length - 1);

    $.ajax({
        type: "POST",
        url: "/Marketing/GetCouponInfo",
        data: { couponCode: couponCode, languageids: languageids, couponmarketid: $("#Id").val() },
        async: false,
        success: function (responseText) {
            tip = true;
            if (!responseText.isnull) {
                var pickdate;
                if (responseText.pickdate == "")
                    pickdate = "";
                else
                    pickdate = "领取周期：" + responseText.pickdate + "；";
                var msg = "面额：" + responseText.amount + "；最低消费金额：" + responseText.minamount + "；有效期：" + responseText.validate + "；" + pickdate;
                $("#CouponCode").nextAll().first().html(msg);
                $("#CouponCode").nextAll().first().show();
            }
            if (responseText.error) {
                tip = false;
                $("#CouponCode").nextAll().last().html(responseText.errormsg);
                $("#CouponCode").nextAll().last().show();
            }
        }
    });
}

function coumardel(id) {
    $.ajax({
        type: "POST",
        url: "/Marketing/CouponDelete/" + id,
        success: function (responseText) {
            var msg;
            $(".alert").remove();
            if (responseText.error != true) {
                $("#pull-right").before('');
                msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '操作成功!' : responseText.msg;
                $("#pull-right").before('<div class="alert alert-success"><a class="close" data-dismiss="alert">&times;</a>' + msg + '</div>');
                $(".btn[type='submit']").click();
            } else {
                msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '操作失败!' : responseText.msg;
                $("#pull-right").before('<div class="alert alert-error"><a class="close" data-dismiss="alert">&times;</a>' + msg + '</div>');
            }
        }
    });
}

function changestatus(id) {
    var status = $("#" + id).val();
    $.ajax({
        type: "POST",
        url: "/Marketing/ChangeStatus/" + id,
        data: { status: status },
        success: function (responseText) {
            var msg;
            $(".alert").remove();
            if (responseText.error != true) {
                msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '操作成功!' : responseText.msg;
                $("#pull-right").before('<div class="alert alert-success"><a class="close" data-dismiss="alert">&times;</a>' + msg + '</div>');
                $(".btn[type='submit']").click();
            } else {
                msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '操作失败!' : responseText.msg;
                $("#pull-right").before('<div class="alert alert-error"><a class="close" data-dismiss="alert">&times;</a>' + msg + '</div>');
            }
        }
    });
}

//删除coupon
function deletecoupon(id, couponinfo) {
    $.ajax({
        type: "POST",
        url: "/Coupon/CheckMarketingCoupon/" + id,
        async: false,
        success: function (responseText) {
            var msg;
            if (responseText.error != true) {
                if (confirm("确定删除Coupon --" + couponinfo + "?")) {
                    $.ajax({
                        type: "POST",
                        url: "/Coupon/DeleteCoupon/" + id,
                        success: function (result) {
                            msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '操作成功!' : responseText.msg;
                            $("#main").before('<div class="alert alert-success"><a class="close" data-dismiss="alert">&times;</a>' + msg + '</div>');
                            location.reload();
                        }
                    });
                }
            } else {
                msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '操作失败!' : responseText.msg;
                $("#main").before('<div class="alert alert-error"><a class="close" data-dismiss="alert">&times;</a>' + msg + '</div>');
            }
        }
    });
}

function changegiftstatus(obj, id) {
    $.ajax({
        type: "POST",
        url: "/Marketing/UpdateGiftMarketingStatus/" + id,
        success: function (responseText) {
            if (!responseText.error) {
                if ($(obj).attr("class").indexOf("btn-success") > 0) {
                    $(obj).attr("class", "btn btn-mini btn-danger");
                    $(obj).html("启用");
                } else {
                    $(obj).attr("class", "btn btn-mini btn-success");
                    $(obj).html("禁用");
                }
            }
        }
    });
}