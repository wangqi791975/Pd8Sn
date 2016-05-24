jQuery(function () {
    //  ajax错误处理，未登录
    $(document).ajaxError(function (event, jqXhr, settings, thrownError) {
        if (jqXhr.status == 488) {
            if ($("div.popup_wrap:visible").length > 0) {
                $("div.popup_wrap:visible").each(function () {
                    $(this).modal("hide");
                });
            }
            logincap();   //弹出登陆的弹框
            return;
        } else if (jqXhr.status === 0) {
            DivOs.showErrorModal('Not connect.\n Verify Network.');
        } else if (jqXhr.status == 404) {
            DivOs.showErrorModal('Requested page not found. [404]');
        } else if (jqXhr.status == 500) {
            DivOs.showErrorModal('Internal Server Error [500].');
        } else if (jqXhr.statusText === 'parsererror') {
            DivOs.showErrorModal('Requested JSON parse failed.');
        } else if (jqXhr.statusText === 'timeout') {
            DivOs.showErrorModal('Time out error.');
        } else if (jqXhr.statusText === 'abort') {
            DivOs.showErrorModal('abort.');
        } else {
            DivOs.showErrorModal("Time out!");
        }

        return;
    });

    //  左侧类别显示隐藏
    $('body').on("mouseover", "li.categories_have_sub", function (e) {
        e.preventDefault();
        $("li.categories_have_sub ul").hide();
        $(this).find("ul").show();
        $(this).find("span").addClass("hover");
    });
    $('body').on("mouseout", "li.categories_have_sub", function (e) {
        e.preventDefault();
        $(this).find("ul").hide();
        $(this).find("span").removeClass("hover");
    });
    $(".index_sidebar #categories_sub_ul li:not(.all_categories)").hover(function (e) {
        var idx = $(this).index();
        e.preventDefault();
        $(".index_sidebar .categories_sub_list").hide();
        $(".index_sidebar .categories_sub_list:eq(" + idx + ")").show();
    }, function (e) {
        var idx = $(this).index();
        e.preventDefault();
        var self = this;
        var hideSubList = function () {
            if ($(self).hasClass("hover")) return;
            $(".index_sidebar .categories_sub_list:eq(" + idx + ")").hide();
        }
        setTimeout(hideSubList, 50);
    });
    $(".index_sidebar .categories_sub_list").hover(function (e) {
        var idx = $(".index_sidebar .categories_sub_list").index(this);
        e.preventDefault();
        $(".index_sidebar #categories_sub_ul li:not(.all_categories):eq(" + idx + ")").addClass("hover");
    }, function (e) {
        var idx = $(".index_sidebar .categories_sub_list").index(this);
        e.preventDefault();
        $(this).hide();
        $(".index_sidebar #categories_sub_ul li:not(.all_categories):eq(" + idx + ")").removeClass("hover");
    });

    //  refine by more&less
    $(".categories_refineby p a").on("click", function () {
        var par = $(this).parent();
        var target = $(par).prev().find("li:gt(6)");
        if ($(par).hasClass("categories_refineby_submore")) {
            $(this).html(Message.TextLess + "<ins></ins>");
            $(par).removeClass("categories_refineby_submore").addClass("categories_refineby_subless");
            $(target).show();
        } else {
            $(this).html(Message.TextMore + "<ins></ins>");
            $(par).removeClass("categories_refineby_subless").addClass("categories_refineby_submore");
            $(target).hide();
        }
    });

    //  back to top
    $("#fix_backtop").click(function () {
        $("body,html").animate({ scrollTop: 0 }, 500);
    });
    var backTopLeft = function () {
        var btLeft = $(window).width() / 2 + 500;
        if (btLeft <= 1002) {
            $(".fixed_pane").css({
                "left": ($(window).width() - 45)
            });
        } else {
            $(".fixed_pane").css({
                "left": (btLeft + 13)
            });
        }
        if ($(document).scrollTop() === 0) {
            $("#fix_backtop").hide(200);
        } else {
            $("#fix_backtop").show(200);
        }
    }
    backTopLeft();
    $(window).resize(backTopLeft);
    $(window).scroll(function () {
        if ($(document).scrollTop() === 0) {
            $("#fix_backtop").hide(200);
        } else {
            $("#fix_backtop").show(200);
        }
    });

    //  移除modal内数据，才可重新加载
    $("#otherpack").on("hidden", function () {
        $(this).removeData("modal");
    });

    //  打开modal后，调整位置
    $("div.popup_wrap").each(function () {
        $(this).on("shown", function () {
            var h = $(this).height();
            $(this).css("margin-top", -(h / 2));
        });
    });

    //  标签页鼠标上移切换
    $('body').on('hover.tab.data-api', '.nav-tabs .tab-hover', function (e) {
        e.preventDefault();
        $(this).tab('show');
    });

    //  标签页鼠标点击切换
    $('body').on('click.tab.data-api', '.nav-tabs .tab-click', function (e) {
        e.preventDefault();
        $(this).tab('show');

        var url = $(this).data("url");
        if (typeof (url) !== "undefined" && !url.isEmpty()) {
            var loaded = $(this).data("loaded");
            if (typeof (loaded) === "undefined" || loaded === false) {
                var $this = $(this);
                var target = $this.data("targetload") ? $this.data("targetload") : $this.data("target");
                var $target = $(target);
                $.get(url, function (data) {
                    $target.filter(':first').html(data);
                    $this.data("loaded", "true");
                });
            }
        }
    });

    //  checkbox触发a标签
    $('input[type="checkbox"].chk_href').on("click", function () {
        window.location.href = $(this).next("a").attr("href");
    });

    //  输入框remain字符个数的提示
    $('[data-toggle="remain"]').bind('keyup keydown paste', function (e) {
        var $t = $(this);
        setTimeout(function () {
            var target = $t.attr("data-target");
            var max = $t.attr('data-maxlen') * 1;
            if ($t.val().length > max) {
                $t.val($t.val().substr(0, max));
            }
            $(target).text(max - $t.val().length);
        }, 0);
    });

    //  阶梯价格提示
    $(".amount50_down,amount50_up").on("click", function () {
        if ($(this).hasClass("amount50_down")) {
            $(this).removeClass("amount50_down").addClass("amount50_up").find("div.poptip").show();
        } else {
            $(this).removeClass("amount50_up").addClass("amount50_down").find("div.poptip").hide();
        }
    });

    //  similar item 图片切换
    $(".recommend_cont li .img_noborder").each(function () {
        var idx = $(this).parent().index();
        if (idx > 1) {
            $(this).addClass("img_noborder2");
        }
    });
    $(document).on({
        mouseenter: function () {
            if ($(this).hasClass("img_noborder2")) {
                $(this).removeClass("img_noborder").removeClass("img_noborder2").addClass("img_border2");
                $(this).next().show().children("div").addClass("pro_popup_detail2");
            } else {
                $(this).removeClass("img_noborder").addClass("img_border");
                $(this).next().show().children("div").addClass("pro_popup_detail");
            }
        },
        mouseleave: function () {
            if ($(this).hasClass("img_border2")) {
                $(this).removeClass("img_border2").addClass("img_noborder").addClass("img_noborder2");
                $(this).next().hide().children("div").removeClass("pro_popup_detail2");
            } else {
                $(this).removeClass("img_border").addClass("img_noborder");
                $(this).next().hide().children("div").removeClass("pro_popup_detail");
            }
        }
    }, ".recommend_cont li .img_noborder,.recommend_cont li .img_border,.recommend_cont li .img_border2");

    //  搜索框tip
    $('#head_search').typeahead({
        ajax: {
            url: '/Public/SearchTip',
            timeout: 300,                   // 延时
            method: 'post',
            triggerLength: 2,               // 输入几个字符之后，开始请求
            loadingClass: null,             //
            preDispatch: null,
            preProcess: function (data) {
                var result = new Array();
                $.each(data, function (key, val) {
                    result[key] = { id: key, name: val };
                });
                return result;
            }
        },
        //display: "name",     // 默认的对象属性名称为 name 属性
        //val: "id",           // 默认的标识属性名称为 id 属性
        //items: 8,            // 最多显示项目数量
        //itemSelected: function (item, val, text) {      // 当选中一个项目的时候，回调函数
        //},
        //menu: '<div class="search_filter"><ul></ul></div>',
        item: '<li><a href="javascript:void(0)"></a></li>'
    });

    //产品搜索
    $("#head_search_btn").on("click", function () { fnProductSearch(); });
    $('#head_search').keydown(function (e) {
        var curKey = Common.getKeyCode(e);;
        if (curKey == 13) {
            fnProductSearch();
        }
    });
    var fnProductSearch = function () {
        var keyword = $("#head_search_btn").prev().val().trim().toLowerCase();
        keyword = keyword.replace(/[ |!|@|#|$|%|^|&|*|'|:|<|>|?|=|`|*|？|{|}|"|,|.|(|)|+|;|_|\[|\]|（|）|\/]+/g, "-").replace(/-$/, ""); //Special characters to replace
        if ($.trim(keyword) === "") {
            DivOs.showErrorModal(Message.MsgKeywordError);
            return;
        }
        if (keyword.length > 1) {
            var searchUrl = $("#head_search_btn").data("url");
            if ($.trim(searchUrl) === "")
                searchUrl = window.location.host + "/productsearch?keyword=|keyword|&page=1";
            searchUrl = searchUrl.replace("|keyword|", keyword);
            window.location.href = searchUrl;
            //$.get(searchUrl);
        }
    }

    //产品列表页顶部分页 GO 按钮跳转
    $("#jumppagetop").on("click", function () { fnProductGoPage($(this)); });
    //产品列表页底部分页 GO 按钮跳转
    $("#jumppagebottom").on("click", function () { fnProductGoPage($(this)); });
    $(".pagelist_jump input").keydown(function (e) {
        var curKey = Common.getKeyCode(e);;
        if (curKey == 13) {
            fnProductGoPage($(this).next());
        }
    });
    var fnProductGoPage = function (obj) {
        var pageindex = obj.prev().val();
        if ($.trim(pageindex) === "")
            pageindex = 1;
        var gotoUrl = obj.data("href");
        gotoUrl = gotoUrl.replace("-page-", pageindex);
        window.location.href = gotoUrl;
    }

    //  登出
    $("#head_logout").click(function () {
        $.post("/Account/Logout", {}, function () {
            location.reload();
        });
    });

    //  分页按钮点击 ajax分页
    var pagination = function (obj) {
        var $this = obj,
            $parent = $this.parents(".ajax-pagination").filter(":first"),
            href = $this.attr("href"),
            url = $this.data("url") || (href && href.replace(/.*(?=#[^\s]+$)/, "")),
            $target = $($parent.data("target")),
            mode = $parent.data("mode") || "replace",
            extra = $parent.data("extra");

        //  取额外参数
        if (typeof (extra) === "string" && extra !== "") {
            var fn = window[extra];
            if (typeof (fn) === "function") {
                var extraParams = fn();
                if (typeof (extraParams) != "undefined")
                    $.each(extraParams, function (k, v) {
                        url = url.urlReplaceParmeter(k, v);
                    });
            } else {
                var extraParams = extra.split("&");
                if (extraParams.length) for (var i in extraParams) {
                    var extraParam = extraParams[i].split("=");
                    if (extraParam.length === 2) {
                        url = url.urlReplaceParmeter(extraParam[0], extraParam[1]);
                    }
                }
            }
        }

        $.get(url, function (data) {
            mode === "replace" ? $target.filter(':first').html(data) : $target.filter(':first').append(data);
        });
        return false;
    };
    //  点击数字分页
    $(document).on("click", '[data-toggle="pagination"]', function (e) {
        e.preventDefault();
        pagination($(this));
    });
    //  点击GO分页
    $(document).on("click", '[data-toggle="pagination_a"]', function (e) {
        e.preventDefault();
        var $this = $(this);
        var pageindex = $this.prev().val();
        if ($.trim(pageindex) === "")
            pageindex = 1;
        var gotoUrl = $this.data("url");
        gotoUrl = gotoUrl.replace("-page-", pageindex);
        $this.data("url", gotoUrl);
        pagination($(this));
    });
    //  输入框回车分页
    $(document).on("keydown", '[data-toggle="pagination_input"]', function (e) {
        var curKey = Common.getKeyCode(e);
        if (curKey == 13) {
            var $next = $(this).next();
            var pageindex = $(this).val();
            if ($.trim(pageindex) === "")
                pageindex = 1;
            var gotoUrl = $next.data("url");
            gotoUrl = gotoUrl.replace("-page-", pageindex);
            $next.data("url", gotoUrl);
            pagination($next);
        }
    });

});

//  注册验证
//  邮箱正则
var reg = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
function registervalid() {
    var fullname = $.trim($("#FullName").val());
    var fullnameErr = $("#FullName").next();
    var email = $.trim($("#Email").val());
    var emailErr = $("#Email").next();
    var password1 = $.trim($("#password1").val());
    var password1Err = $("#password1").next();
    var password2 = $.trim($("#password2").val());
    var password2Err = $("#password2").next();
    var customertype = $('input[name="CustomerType"]:checked').val();
    var customertypeErr = $('input[name="CustomerType"]:last').parent().next();

    //清空error
    $(fullnameErr).html("");
    $(emailErr).html("");
    $(password1Err).html("");
    $(password2Err).html("");
    $(customertypeErr).html("");
    $("[name='ValidateCode']").parent().children().last().html("");


    if (fullname.length > 32) {
        $(fullnameErr).html(Message.ErrorFullNameTooLong);
        resetPwd();
        return false;
    }
    if (email == "") {
        $(emailErr).html(Message.ErrorEmailEmpty);
        resetPwd();
        return false;
    }
    if (!reg.test(email)) {
        $(emailErr).html(Message.ErrorEmailFormat);
        resetPwd();
        return false;
    }
    var isreturnfalse = false;
    $.ajax({
        type: "POST",
        url: "/Account/CheckEmail",
        data: { email: email, type: "register" },
        async: false,
        success: function (responseText) {
            if (responseText.error) {
                $(emailErr).html(responseText.msg);
                isreturnfalse = true;
            }
        }
    });
    if (isreturnfalse) {
        resetPwd();
        return false;
    }
    if (password1 == "") {
        $(password1Err).html(Message.ErrorPasswordEmpty);
        resetPwd();
        return false;
    }
    if (password1.length < 5) {
        $(password1Err).html(Message.ErrorShortPassword);
        resetPwd();
        return false;
    }
    if (password1.length > 32) {
        $(password1Err).html(Message.ErrorLongPassword);
        resetPwd();
        return false;
    }
    if (password2 == "") {
        $(password2Err).html(Message.ErrorConfPasswordEmpty);
        resetPwd();
        return false;
    }
    if (password1 != password2) {
        $(password2Err).html(Message.ErrorPasswordNotMatch);
        resetPwd();
        return false;
    }
    if (customertype == undefined) {
        $(customertypeErr).html(Message.ErrorCustomerType);
        resetPwd();
        return false;
    }
    $('#registerbtn').click();
}

//  登录验证
function loginvalid() {
    var email = $.trim($("#Email").val());
    var emailErr = $("#Email").next();
    var password = $.trim($("#Password").val());
    var passwordErr = $("#Password").next();
    $(emailErr).html("");
    $(passwordErr).html("");
    if (email == "") {
        $(emailErr).html(Message.ErrorEmailEmpty);
        $("#Password").val("");
        return false;
    }
    if (!reg.test(email)) {
        $(emailErr).html(Message.ErrorEmailFormat);
        $("#Password").val("");
        return false;
    }
    var isreturnfalse = false;
    $.ajax({
        type: "POST",
        url: "/Account/CheckEmail",
        data: { email: email, type: "login" },
        async: false,
        success: function (responseText) {
            if (responseText.error) {
                $(emailErr).html(responseText.msg);
                isreturnfalse = true;
            }
        }
    });
    if (isreturnfalse) {
        $("#Password").val("");
        return false;
    }
    if (password == "") {
        $(passwordErr).html(Message.ErrorPasswordEmpty);
        $("#Password").val("");
        return false;
    }
    $('#loginbtn').click();
}

//  弹框登录
function logincap() {
    $.post("/Account/GetLoginErrCount", {}, function (count) {
        if (count > 2) {
            $("#valiCodeLo")[0].src = "/Account/GetValidatorGraphics?time=" + (new Date()).getTime();
            $("#validatecodelo").parent().parent().removeAttr("style");
        }
    });
    $("#logindiv").modal("show");
}
var islogin = false;
var testimonialname = "";
var testimonialemail = "";
var successfun;
//  登录按钮
function monialloginsuccess(result) {
    testimonialname = result.Name;
    testimonialemail = result.Email;
    $("#tdname").html(testimonialname);
    $("#tdemail").html(testimonialemail);
    $("#testimonialdiv").modal("show");
}
function loginsubmit() {
    var validatecode = "";
    if (parseInt($("#valoginerrcount").val()) > 2)
        validatecode = $("#validatecodelo").val();
    $.ajax({
        type: "POST",
        url: "/Account/WinLogin",
        data: { "email": $("#loginemail").val(), "password": $("#loginpassword").val(), "validatecode": validatecode },
        success: function (result) {
            if (result.IsLoginSuccess) {
                $("#logindiv").modal("hide");
                islogin = true;
                if (successfun != undefined)
                    successfun(result);
                else
                    window.location.reload();
            } else {
                $("#loginpassword").val("");
                if (result.ErrorEmail != undefined) {
                    $("#loginemail").parent().children(".fred").html(result.ErrorEmail);
                    $("#loginemail").parent().children(".fred").css("display", "block");
                } else {
                    $("#loginemail").parent().children(".fred").html("");
                    $("#loginemail").parent().children(".fred").css("display", "none");
                }
                if (result.ErrorPassword != undefined) {
                    $("#loginpassword").parent().children(".fred").html(result.ErrorPassword);
                    $("#loginpassword").parent().children(".fred").css("display", "block");
                } else {
                    $("#loginpassword").parent().children(".fred").html("");
                    $("#loginpassword").parent().children(".fred").css("display", "none");
                }
                if (result.ErrorValidateCode != undefined) {
                    $("#validatecodelo").parent().children(".fred").html(result.ErrorValidateCode);
                    $("#validatecodelo").parent().children(".fred").css("display", "block");
                } else {
                    $("#validatecodelo").parent().children(".fred").html("");
                    $("#validatecodelo").parent().children(".fred").css("display", "none");
                }
                $.post("/Account/GetLoginErrCount", {}, function (count) {
                    if (count > 2) {
                        $("#valiCodeLo")[0].src = "/Account/GetValidatorGraphics?time=" + (new Date()).getTime();
                        $("#validatecodelo").parent().parent().removeAttr("style");
                    }
                });
            }
        }
    });
}

jQuery(function () {
    //  当点击图片时，刷新验证码
    $("#valiCodeLo").bind("click", function () {
        this.src = "/Account/GetValidatorGraphics?time=" + (new Date()).getTime();
        $("#iconcheckLo").attr("class", "icon_code_error");
    });

    //  验证码正确错误图标改变
    $("#validatecodelo").keyup(function () {
        if ($("#validatecodelo").val().length == 4) {
            $.ajax({
                type: "POST",
                url: "/Account/CheckIcon",
                dataType: "text",
                success: function (code) {
                    if (code == $("#validatecodelo").val().toUpperCase()) {
                        $("#iconcheckLo").attr("class", "icon_code_ok");
                    }
                }
            });
        } else {
            $("#iconcheckLo").attr("class", "icon_code_error");
        }
    });

    //  关闭登录弹框清空所有内容
    $("#logindiv .close").click(function () {
        $("#loginemail").val("");
        $("#loginemail").parent().children(".fred").html("");
        $("#loginemail").parent().children(".fred").css("display", "none");
        $("#loginpassword").val("");
        $("#loginpassword").parent().children(".fred").html("");
        $("#loginpassword").parent().children(".fred").css("display", "none");
        $("#validatecodelo").val("");
        $("#validatecodelo").parent().children(".fred").html("");
        $("#validatecodelo").parent().children(".fred").css("display", "none");
    });
});

function testimonialcap() {
    if (parseInt($("#valcustomerid").val()) != 0 || islogin) {
        $("#testimonialdiv .success_wrap").hide();
        $("#testimonialdiv").modal("show");
    } else {
        successfun = monialloginsuccess;
        logincap();
    }
}

//  弹框注册
function registercap() {
    $.post("/Account/GetRegisterCount", {}, function (count) {
        if (count >= 2) {
            $("#valiCodeRe")[0].src = "/Account/GetValidatorGraphics?time=" + (new Date()).getTime();
            $("#validatecodere").parent().parent().removeAttr("style");
        }
    });
    $("#registerdiv").modal("show");
}
//  注册按钮
function registersubmit() {
    var isnewsletter = false;
    if ($("#newsletter").attr("checked")) {
        isnewsletter = true;
    } else {
        isnewsletter = false;
    }
    var validatecodere = $("#validatecodere").val();
    $.ajax({
        type: "POST",
        url: "/Account/WinRegister",
        data: { "email": $("#regemail").val(), "password": $("#regpassword").val(), "name": $("#regname").val(), "comfPassword": $("#regcomfPassword").val(), "customerType": $('input[name="describes"]:checked').val(), "validatecode": validatecodere, "newsletter": isnewsletter },
        success: function (result) {
            if (result.IsRegisterSuccess) {
                $("#registerdiv").modal("hide");
                $.post("/Account/GetRegisterCount", {}, function (count) {
                    if (count >= 2) {
                        $("#valiCodeRe")[0].src = "/Account/GetValidatorGraphics?time=" + (new Date()).getTime();
                        $("#validatecodere").parent().parent().removeAttr("style");
                    }
                });
            } else {
                $("#regpassword").val("");
                $("#regcomfPassword").val("");
                if (result.ErrorEmail != undefined) {
                    $("#regemail").parent().children(".fred").html(result.ErrorEmail);
                    $("#regemail").parent().children(".fred").css("display", "block");
                } else {
                    $("#regemail").parent().children(".fred").html("");
                    $("#regemail").parent().children(".fred").css("display", "none");
                }
                if (result.ErrorPassword != undefined) {
                    $("#regpassword").parent().children(".fred").html(result.ErrorPassword);
                    $("#regpassword").parent().children(".fred").css("display", "block");
                } else {
                    $("#regpassword").parent().children(".fred").html("");
                    $("#regpassword").parent().children(".fred").css("display", "none");
                }
                if (result.ErrorConfPassword != undefined) {
                    $("#regcomfPassword").parent().children(".fred").html(result.ErrorConfPassword);
                    $("#regcomfPassword").parent().children(".fred").css("display", "block");
                } else {
                    $("#regcomfPassword").parent().children(".fred").html("");
                    $("#regcomfPassword").parent().children(".fred").css("display", "none");
                }
                if (result.ErrorCustomerType != undefined) {
                    $("input[name='describes']").parent().parent().children(".fred").html(result.ErrorCustomerType);
                    $("input[name='describes']").parent().parent().children(".fred").css("display", "block");
                } else {
                    $("input[name='describes']").parent().parent().children(".fred").html("");
                    $("input[name='describes']").parent().parent().children(".fred").css("display", "none");
                }
                if (result.ErrorValidateCode != undefined) {
                    $("#validatecodere").parent().children(".fred").html(result.ErrorValidateCode);
                    $("#validatecodere").parent().children(".fred").css("display", "block");
                } else {
                    $("#validatecodere").parent().children(".fred").html("");
                    $("#validatecodere").parent().children(".fred").css("display", "none");
                }
            }
        }
    });
};

$(function () {
    //  初始化验证码
    $(".valiCode").attr("src", "/Account/GetValidatorGraphics?time=" + (new Date()).getTime());
    $(".valiCodeChk").hide();
    //  当点击图片时，刷新验证码
    $(".valiCode").on("click", function () {
        this.src = "/Account/GetValidatorGraphics?time=" + (new Date()).getTime();
        $(this).offsetParent().next(".valiCodeChk").show().removeClass("icon_code_ok").addClass("icon_code_error");
    });
    //  验证码正确错误图标改变
    $(".valiCodeInput").on("keyup", function () {
        var v = $(this).val();
        var self = this;
        if (v.length == 4) {
            $.ajax({
                type: "POST",
                url: "/Account/CheckIcon",
                dataType: "text",
                success: function (code) {
                    if (code == v.toUpperCase()) {
                        $(self).next().next().show().removeClass("icon_code_error").addClass("icon_code_ok");
                    }
                }
            });
        } else {
            $(this).next().next().show().removeClass("icon_code_ok").addClass("icon_code_error");
        }
    });

    //  当点击图片时，刷新验证码
    $("#valiCodeRe").bind("click", function () {
        this.src = "/Account/GetValidatorGraphics?time=" + (new Date()).getTime();
        $("#iconcheckre").attr("class", "icon_code_error");
    });

    //  验证码正确错误图标改变
    $("#validatecodere").keyup(function () {
        if ($("#validatecodere").val().length == 4) {
            $.ajax({
                type: "POST",
                url: "/Account/CheckIcon",
                dataType: "text",
                success: function (code) {
                    if (code == $("#validatecodere").val().toUpperCase()) {
                        $("#iconcheckre").attr("class", "icon_code_ok");
                    }
                }
            });
        } else {
            $("#iconcheckre").attr("class", "icon_code_error");
        }
    });

    //  提交评论
    $("#testimonialsub").click(function () {
        if ($.trim($("#testimonials").val()) != "") {
            $.ajax({
                type: "POST",
                url: "/Home/SubTestimonial",
                data: { "testimonials": $("#testimonials").val() },
                success: function (result) {
                    if (result) {
                        $("#testimonialdiv .success_wrap").show();
                        $("#testimonials").val("");
                        setTimeout("$('#testimonialdiv').modal('hide')", 5000);
                    }
                }
            });
        } else {
            $("#testimonials").after("<em>" + Message.ErrorMoreThanOnrChar + "</em>");
        }
    });

    //  模拟select
    $('body').on('click', '.select_cont', function (e) {
        var event = e || window.event;
        var elem = event.srcElement || event.target;
        while (elem) {
            if ($(elem).hasClass("input_text_wrap") || $(elem).hasClass("js_no_search")) {
                return true;
            }
            elem = elem.parentNode;
        }
        e.preventDefault();
        $(this).children("div.pop_select_cont").toggle();
        return true;
    });
    $('body').on('click', '.select_cont .pop_select_cont li.list_item', function (e) {
        e.preventDefault();
        var $this = $(this);
        var value = $this.data("value");
        var text = $this.text();
        var $par = $this.parents(".select_cont").filter(":first");

        $this.addClass("active").siblings().removeClass("active");
        $par.children(".select_cont_span").text(text);
        $par.children("input:hidden").val(value);

        extra = $par.data("callback");
        if (typeof (extra) === "string" && extra !== "") {
            var fn = window[extra];
            if (typeof (fn) === "function") {
                fn(this);
            }
        }
    });



    //  模拟select之搜索
    $('body').on('keyup', ".select_cont .pop_select_cont .input_text_wrap", function () {
        if ($(this).hasClass("js_no_search")) {
            return false;
        }
        var key = $(this).val();
        key = key.replace(/^\s*|\s*$/g, "");
        $(".select_cont .pop_select_cont ul li").show();
        if (key != "") {
            $('.pop_select_cont ul').scrollTop(0);
            $(".select_cont .pop_select_cont ul li.list_line").hide();
            $(".select_cont .pop_select_cont ul li.list_item").each(function () {
                var cTextVal = $(this).text();
                var re1 = new RegExp("^" + key, 'i');
                var re2 = new RegExp("\\s+" + key, 'i');
                if (cTextVal.match(re1) || cTextVal.match(re2)) {
                } else {
                    $(this).hide();
                }
            });
        }
    });
    //  模拟select之隐藏
    document.onclick = function (event) {
        $(".pop_select_cont").each(function () {
            var $this = $(this);
            var $par = $this.parents(".select_cont").filter(":first");
            if ($this.is(":visible")) {
                var e = event || window.event;
                var elem = e.srcElement || e.target;
                while (elem) {
                    if ($(elem).hasClass("select_cont") && $(elem).closest($par).length == 1) {
                        return true;
                    }
                    elem = elem.parentNode;
                }
                $this.hide();
                return true;
            }
        });
    }

    //  tip显示隐藏
    $("ins.question").hover(function () {
        $(this).children().show();
    }, function () {
        $(this).children().hide();
    });

    //客户切换币种
    $(".site_nav_currency .currency_on a").unbind("click").bind("click", function () {
        var currencyCode = $.trim(this.title);
        if ($.trim(currencyCode) === "") {
            alert("Currency invalid");
        } else
            $.post("/Public/ChangeCurrency", { "currencyCode": currencyCode }, function (data) { if (data.result === "refresh") { window.location.reload(); } });
    });

});

function getProvinceByCountry(obj) {
    var value = $(obj).data("value");
    $(".province_name").show();
    $(".province_id").hide();
    $(".province_id select").empty();
    $(".has_province").val(0);

    var jqueryForm = $(obj).parents("form").filter(":first");
    $.ajax({
        type: "POST",
        url: "/PlaceOrder/GetProvinces",
        data: { "country_id": value },
        success: function (result) {
            if (result.result == "success") {
                if (result.msg.length > 0) {
                    $(".province_name").hide();
                    $(".province_id").show();
                    $(".has_province").val(1);
                    jqueryForm[0].province_id.options[0] = new Option("Please select ...", "0");
                    for (i = 0; i < result.msg.length; i++) {
                        jqueryForm[0].province_id.options[i + 1] = new Option(result.msg[i].ProvinceName, result.msg[i].ProvinceId);
                    }
                }
                $("#province_id").val($("#province_id").parents("td").filter(":first").data("value"));
            }
        }
    });
}

function resetPwd() {
    $("#password1").val("");
    $("#password2").val("");
    $(".bar_wrap").hide();
}

function GetCookie(name) {
    var search = name + "=";
    var returnvalue = "";
    var offset, end;
    if (document.cookie.length > 0) {
        offset = document.cookie.indexOf(search);
        if (offset != -1) {
            offset += search.length;
            end = document.cookie.indexOf(";", offset);
            if (end == -1) end = document.cookie.length;
            returnvalue = unescape(document.cookie.substring(offset, end));
        }
    }
    return returnvalue;
}
//兼容firefox和chrome
function SetCookie(name, value, time) {
    var exp = new Date();
    exp.setTime(exp.getTime() + time * 24 * 60 * 60 * 1000);
    document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
}

function LoadReg() {
    if (GetCookie("registerf") == "") {
        registercap();
        SetCookie("registerf", "yes", 30);
    }
}