$(function () {
    //  移除modal内数据，才可重新加载
    $("#popup_askshow").on("hidden", function () {
        $("#EmailUsForm")[0].reset();
        $(".uploadify-queue").html("");
        $(".success_wrap").hide();
    });

    $("#mailattachment").uploadify({
        'uploader': '/Product/Upload',           // 服务器端处理地址
        'swf': '/Js/uploadify/uploadify.swf',    // 上传使用的 Flash
        'width': 248,                          // 按钮的宽度
        'height': 30,                         // 按钮的高度
        'buttonText': Message.TipAdd,                 // 按钮上的文字
        'buttonCursor': 'hand',               // 按钮的鼠标图标
        'fileObjName': 'Filedata',            // 上传参数名称
        // 两个配套使用
        'fileTypeExts': "*.jpg;*.png;*.zip;*.rar;*.doc;*.docx;",   // 扩展名
        'fileTypeDesc': Message.TipPleaseSelectFiles.format("jpg png zip rar doc docx"),     // 文件说明
        'removeCompleted': false,
        'auto': true,                // 选择之后，自动开始上传
        'multi': true,               // 是否支持同时上传多个文件
        'queueSizeLimit': 5,          // 允许多文件上传的时候，同时上传文件的个数
        'uploadLimit': 5,
        'onUploadSuccess': function (file, data, response) {
            if (response) {
                 //alert('The file ' + file.name + ' was successfully uploaded with a response of ' + response + ':' + data);
                 $("#mailattachment").after("<input id='input_" + file.id + "' type='hidden' name='attachmentfile' value='" + data + "' />");
                 var cancel = $("#" + file.id + " .cancel a");//事件
                 if (cancel) {
                     cancel.on("click", function () {
                         $("input#input_" + file.id).remove();
                     });
                 }
            } 
        }
    });

    //  ask a question
    $("#SubmitEmail").on("click", function () {
        $("#email").next(".fred").html("");
        $("#fullname").next(".fred").html("");
        $("#VerificationCode").next().next(".fred").html("");
        $("#question").next().next(".fred").html("");

        $("input[name=attachmentfile]").each(function () {
            var val = $(this).remove();
        });


        $("#EmailUsForm").ajaxSubmit({
            beforeSubmit: function () {
                var flag = true;
                if ($("#email").val().length<1) {
                    $("#email").next(".fred").text(Message.ErrorEmailEmpty);
                    flag= false;
                }

                if (!DataType.isEmail($("#email").val())) {
                    $("#email").next(".fred").text(Message.TipCheckEmail);
                    flag = false;
                }

                if ($("#fullname").val().length < 1) {
                    $("#fullname").next(".fred").text(Message.ErrorEnterYourName);
                    flag= false;
                }
                
                if ($("#question").val().length < 1) {
                    $("#question").next().next(".fred").text(Message.ErrorAddFewQueLesChar);
                    flag = false;
                }
                
                if ($("#VerificationCode").hasClass("icon_code_error")) {
                    $("#VerificationCode").next().next(".fred").text(Message.ErrorVerCodeError);
                    flag= false;
                }
                if (!flag) {
                    return false;
                } else {
                   // $('#mailattachment').uploadify('upload', '*');
                    return true;
                }

            },
            success: function (responseText) {
                $(".success_wrap").show();
                setTimeout(function () { $("#popup_askshow").modal("hide"); }, 5000);
            }
        });
    });
    


});