$(function () {
    //  日历控件
    $(".datepicker").datepicker({
        changeMonth: true,
        changeYear: true
        //$.datepicker.regional["en"]
        //showButtonPanel: true
    });

    $("#profilecont").click(function () {
        $(".accout_tab h3").attr("class", "nostock");
        $(this).attr("class", "current");
        $(".profile_cont").css("display", "block");
        $(".password_cont").css("display", "none");
        $(".email_cont").css("display", "none");
    });

    $("#passwordcont").click(function () {
        $("#profilecont").attr("class", "nostock2");
        $("#emailcont").attr("class", "nostock");
        $(this).attr("class", "current");
        $(".profile_cont").css("display", "none");
        $(".password_cont").css("display", "block");
        $(".email_cont").css("display", "none");
    });

    $("#emailcont").click(function () {
        $("#profilecont").attr("class", "nostock2");
        $("#passwordcont").attr("class", "nostock2");
        $(this).attr("class", "current");
        $(".profile_cont").css("display", "none");
        $(".password_cont").css("display", "none");
        $(".email_cont").css("display", "block");
    });

    //修改密码
    $("#changepwdbtn").click(function () {
        var mark = true;
        $(".password_cont .warning_wrap").hide();
        var curpassword = $.trim($("#curpassword").val());
        var newpassword = $.trim($("#newpassword").val());
        var conpassword = $.trim($("#conpassword").val());
        $("#curpassword").next(".fred").html("");
        $("#newpassword").next(".fred").html("");
        $("#conpassword").next(".fred").html("");

        $(".password_cont.fred").html("");

        if (curpassword.length <= 5) {
            if (curpassword == "")
                $("#curpassword").next(".fred").html(Message.ErrorCurrentPwdEmpty);
            else
                $("#curpassword").next(".fred").html(Message.ErrorShortPassword);
            $("#curpassword").next(".fred").show();
            mark = false;
        } else {
            $.ajax({
                type: "POST",
                url: "/Account/CheckPassword",
                data: { "curPassword": curpassword },
                async: false,
                success: function (result) {
                    if (result == "False") {
                        $("#curpassword").next(".fred").html(Message.ErrorPasswordTryAgain);
                        $("#curpassword").next(".fred").show();
                        mark = false;
                    }
                }
            });
        }
        if (newpassword.length <= 5) {
            if (newpassword == "")
                $("#newpassword").next(".fred").html(Message.ErrorNewPwdEmpty);
            else
                $("#newpassword").next(".fred").html(Message.ErrorShortPassword);
            $("#newpassword").next(".fred").show();
            mark = false;
        } else {
            if (newpassword != conpassword) {
                $("#conpassword").next(".fred").html(Message.ErrorPasswordNotMatch);
                $("#conpassword").next(".fred").show();
                mark = false;
            }
        }
        if (conpassword.length <= 5) {
            if (conpassword == "")
                $("#conpassword").next(".fred").html(Message.ErrorConfirmPwdEmpty);
            else
                $("#conpassword").next(".fred").html(Message.ErrorShortPassword);
            $("#conpassword").next(".fred").show();
            mark = false;
        }
        if (!mark)
            return false;
        $("#changepwd").ajaxSubmit({
            success: function (result) {
                $(".password_cont .warning_wrap").show();
                $("#curpassword").val("");
                $("#newpassword").val("");
                $("#conpassword").val("");
            }
        });
        return false;
    });

    //密码强度设置
    $(":password#newpassword").pStrength({
        'onPasswordStrengthChanged': function (passwordStrength, strengthPercentage) {
            $(".bar_wrap .pwd_bar").removeClass("pwd_bar_weak").removeClass("pwd_bar_medium").removeClass("pwd_bar_strong");
            if (strengthPercentage < 30) {
                $(".bar_wrap p.lf").html("Week");
                $(".bar_wrap .pwd_bar").addClass("pwd_bar_weak");
            } else if (strengthPercentage < 70) {
                $(".bar_wrap p.lf").html("Medium");
                $(".bar_wrap .pwd_bar").addClass("pwd_bar_medium");
            } else {
                $(".bar_wrap p.lf").html("Strong");
                $(".bar_wrap .pwd_bar").addClass("pwd_bar_strong");
            }
        }
    });

    //  密码校验显示隐藏
    $("#newpassword").keyup(function () {
        if ($("#newpassword").val() == "")
            $(".bar_wrap").css("display", "none");
        else
            $(".bar_wrap").css("display", "block");
    });

    //修改个人信息
    $("#changeprobtn").click(function () {
        var mark = true;
        var fullname = $.trim($("#FullName").val());
        var birthday = $.trim($("#Birthday").val());
        var telphone = $.trim($("#Telphone").val());
        var countryid = $.trim($("#countryId").val());
        var skype = $.trim($("#Skype").val());
        $("#FullName").next(".fred").html("");
        $("#Birthday").next(".fred").html("");
        $("#Telphone").next(".fred").html("");
        $("#CountryId").next(".fred").html("");
        $("#Skype").next(".fred").html("");
        $(".profile_cont .warning_wrap").hide();

        if ($(":radio[name=Gender]:checked").val() == undefined)
            mark = false;

        if (fullname == "") {
            $("#FullName").next(".fred").html(Message.ErrorFullNameEmpty);
            mark = false;
        } else {
            if (fullname.length < 2) {
                $("#FullName").next(".fred").html(Message.ErrorShortFullName);
                mark = false;
            }
        }
        //if (birthday != "" && !datecheck(birthday)) {
        //    $("#Birthday").next(".fred").html(Message.ErrorDateFormat);
        //    mark = false;
        //}
        if (telphone == "") {
            $("#Telphone").next(".fred").html(Message.ErrorTelphoneEmpty);
            mark = false;
        } else {
            if (telphone.length < 3) {
                $("#Telphone").next(".fred").html(Message.ErrorShortTelphone);
                mark = false;
            }
        }
        
        if (skype.length < 2 && skype.length > 0) {
            $("#Skype").next(".fred").html(Message.ErrorShortSkype);
            mark = false;
        }

        if (countryid == "")
            mark = false;

        if ($(":radio[name=CustomerType]:checked").val() == undefined)
            mark = false;

        if (!mark)
            return false;

        $("#changepro").ajaxSubmit({
            success: function (result) {
                $(".profile_cont .warning_wrap").show();
            }
        });
    });


    $('#uploadify').uploadify({
        'uploader': '/Handler/UploadAvatarHandler.ashx',           // 服务器端处理地址
        'swf': '/Js/uploadify/uploadify.swf',    // 上传使用的 Flash
        'buttonCursor': 'hand',               // 按钮的鼠标图标
        'fileObjName': 'Filedata',            // 上传参数名称
        'fileTypeExts': "*.jpg;*.png;*.gif;*.bmp",   // 扩展名
        'fileTypeDesc': Message.TipPleaseSelectFiles.format("jpg , png , gif, bmp"),     // 文件说明
        'removeCompleted': false,
        'auto': true,                // 选择之后，自动开始上传
        'multi': false,
        'width': 300,
        'buttonText': Message.TipAdd,
        'sizeLimit': 51200,
        'onUploadSuccess': function (fileObj, response, event) {
            if (response.indexOf('Temp') != -1) {
                $("#bgDiv img").remove();                      //移除截图区里image标签
                $("#btnSave").show();                          //保存按钮显示   
                var result = response.split('$');              //得返回参数

                var maxVal = 0;
                if (result[1] > result[2]) {
                    maxVal = result[2];
                }
                else {
                    maxVal = result[1];
                }
                if (maxVal > 75)
                    maxVal = 75;
                $("#maxVal").val(maxVal);                     //设置截图区大小

                $("#hidImageUrl").val(result[0]);             //上传路径存入隐藏域

                ShowImg(result[0], result[1], result[2]);       //在截图区显示

                //initResize(maxVal);
            }
            else {
                alert(response);
            }
        },
        'onSelect': function () {
            $(".uploadify-queue").css("width", "500px");
            if ($(".uploadify-queue-item").length > 1) {
                $(".uploadify-queue-item").first().remove();
            }
        }
    });

    $('#uploadifyskip').uploadify({
        'uploader': '/Handler/UploadAvatarHandler.ashx',           // 服务器端处理地址
        'swf': '/Js/uploadify/uploadify.swf',    // 上传使用的 Flash
        'buttonCursor': 'hand',               // 按钮的鼠标图标
        'fileObjName': 'Filedata',            // 上传参数名称
        'fileTypeExts': "*.jpg;*.png;*.gif;*.bmp",   // 扩展名
        'fileTypeDesc': Message.TipPleaseSelectFiles.format("jpg , png , gif, bmp"),     // 文件说明
        'removeCompleted': false,
        'auto': true,                // 选择之后，自动开始上传
        'multi': false,
        'width': 300,
        'buttonText': Message.TipAdd,
        'sizeLimit': 51200,
        'onUploadSuccess': function (fileObj, response, event) {
            if (response.indexOf('Temp') != -1) {
                $("#avatareditsys").modal("hide");
                $("#avatareditcut").modal("show");
                $("#bgDiv img").remove();                      //移除截图区里image标签
                $("#btnSave").show();                          //保存按钮显示   
                var result = response.split('$');              //得返回参数

                var maxVal = 0;
                if (result[1] > result[2]) {
                    maxVal = result[2];
                }
                else {
                    maxVal = result[1];
                }
                if (maxVal > 75)
                    maxVal = 75;

                $("#maxVal").val(maxVal);                     //设置截图区大小

                $("#hidImageUrl").val(result[0]);             //上传路径存入隐藏域

                ShowImg(result[0], result[1], result[2]);       //在截图区显示

                //initResize(maxVal);
            }
            else {
                alert(response);
            }
        },
        'onSelect': function () {
            $(".uploadify-queue").css("width", "500px");
            if ($(".uploadify-queue-item").length > 1) {
                $(".uploadify-queue-item").first().remove();
            }
        }
    });

    $("#btnCrop").click(function () {
        $("#maxVal").val($("#dragDiv").width());
        $("#btnSubmitAvatar").hide();
        $.ajax({
            type: "POST",
            url: "/Handler/CutAvatarHandler.ashx",
            data: { imgUrl: $('#hidImageUrl').val(), pointX: $("#x").val(), pointY: $("#y").val(), maxVal: $("#maxVal").val() },
            async: false,
            success: function (msg) {
                if (msg.indexOf('User') != -1) {
                    $("#imgCut").attr("src", msg);
                    $("#btnSubmitAvatar").show();
                }
                else {
                    alert("error");
                }
            },
            error: function () {
                alert("error");
            }
        });
    });

    //截取头像保存
    $("#btnSubmitAvatar").click(function () {
        $.ajax({
            type: "POST",
            url: "/Account/SaveAvatar",
            data: { avatarImg: $("#imgCut").attr("src") },
            async: false,
            success: function (responseText) {
                if (!responseText.error) {
                    $("#avatareditcut").modal("hide");
                    $("#avatarcutsuccess").modal("show");
                    $("#changepro img").first().attr("src", $("#imgCut").attr("src"));
                }
            }
        });
    });

    //系统头像保存
    $("#btnSaveAvatar").click(function () {
        $.ajax({
            type: "POST",
            url: "/Account/SaveAvatar",
            data: { avatarImg: $("#avatarImg").attr("src") },
            async: false,
            success: function (responseText) {
                if (!responseText.error) {
                    $("#avatareditsys").modal("hide");
                    $("#avatarsysuccess").modal("show");
                    $("#changepro img").first().attr("src", $("#avatarImg").attr("src"));
                    setTimeout(function () { $("#avatarsysuccess").modal("hide"); }, 3000);
                }
            }
        });
    });

    $(".avatar_cont li img").click(function () {
        $("#avatarImg").attr("src", $(this).attr("src"));
    });
    //头像上传结束

    //偏好设置保存按钮点击
    $("#btnpresave").click(function () {
        $("#preform").ajaxSubmit({
            success: function (result) {
                $(".warning_wrap").show();
            }
        });
    });

    //订阅保存按钮点击
    $("#newsletterbtn").click(function () {
        $("#newsletterform").ajaxSubmit({
            data: { isnewletter: $("#newsletterform input:checkbox").is(":checked") },
            success: function (reslut) {
                $(".warning_wrap").show();
            }
        });
    });

    $("#presubmit").click(function() {
        $("#preform").ajaxSubmit({            
            success:function() {
                $("#main .warning_wrap").show();
            }
        });
    });
});

//头像上传开始
function ShowImg(imagePath, imgWidth, imgHeight) {
    var imgPath = imagePath != "" ? imagePath : "Images/ef_pic.jpg";

    var options = {
        Opacity: 50,//透明度(0到100)	
        //拖放位置和宽高
        dragTop: 3,
        dragLeft: 3,
        dragWidth: document.getElementById("maxVal").value,
        dragHeight: document.getElementById("maxVal").value,
        //缩放触发对象
        Right: "rRight",
        Left: "rLeft",
        Up: "rUp",
        Down: "rDown",
        RightDown: "rRightDown",
        LeftDown: "rLeftDown",
        RightUp: "rRightUp",
        LeftUp: "rLeftUp",
        Scale: true,//是否按比例缩放
        //预览对象设置
        View: "",//预览对象
        viewWidth: 75,//预览宽度
        viewHeight: 75//预览高度
    };

    var ic = new ImgCropper("bgDiv", "dragDiv", imgPath, imgWidth, imgHeight, options);
}

//头像上传结束

function datecheck(date) {
    var reg = /((^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(10|12|0?[13578])([-\/\._])(3[01]|[12][0-9]|0?[1-9])$)|(^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(11|0?[469])([-\/\._])(30|[12][0-9]|0?[1-9])$)|(^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(0?2)([-\/\._])(2[0-8]|1[0-9]|0?[1-9])$)|(^([2468][048]00)([-\/\._])(0?2)([-\/\._])(29)$)|(^([3579][26]00)([-\/\._])(0?2)([-\/\._])(29)$)|(^([1][89][0][48])([-\/\._])(0?2)([-\/\._])(29)$)|(^([2-9][0-9][0][48])([-\/\._])(0?2)([-\/\._])(29)$)|(^([1][89][2468][048])([-\/\._])(0?2)([-\/\._])(29)$)|(^([2-9][0-9][2468][048])([-\/\._])(0?2)([-\/\._])(29)$)|(^([1][89][13579][26])([-\/\._])(0?2)([-\/\._])(29)$)|(^([2-9][0-9][13579][26])([-\/\._])(0?2)([-\/\._])(29)$))/;
    return reg.test(date);
}
