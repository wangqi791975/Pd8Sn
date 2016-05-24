
//记录js脚本错误
window.onerror = function (message, url, line) {
    message = message || arguments[0];
    url = url || "";
    line = line || 0;
    //window.onerror = null;
    Logger.log({ msg: message, url: url, line: line, type: 'onerror' });
    return true;
};

var Logger = {
    toQueryString: function (o) {
        var res = [], p;
        for (p in o) {
            if (o.hasOwnProperty(p)) res.push(escape(p) + '=' + escape(o[p]));
        }
        return res.join('&');
    },
    log: function (info) {
        if (info == null) return;
        try {
            var img = new Image();
            img.src = 'script-error.html?' + Logger.toQueryString(info);
        } catch (ex) { }
    },
    logByError: function (error) {
        var info = {};
        info.msg = error.message || "";
        if (error.stack) {
            info.line = error.lineNumber || 0;
            info.url = error.fileName || "";
            info.stack = error.stack;
        }
        else {
            info.number = error.number || 0;
        }
        info.type = 'try-catch';
        Logger.log(info);
    },
    runMethod: function (method) {
        try {
            method();
        } catch (ex) {
            Logger.logByError(ex);
        }
    }

};

//多个空格替换成一个空格
String.prototype.clearSpace = function () {
    return this.replace(/\s+/g, "");
};
//多个空格替换成一个空格
String.prototype.trimSpace = function () {
    return this.replace(/\s+/g, " ");
};
//清除左右空格
String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
};
//清除左空格
String.prototype.lTrim = function () {
    return this.replace(/(^\s*)/g, "");
};
//清除右空格
String.prototype.rTrim = function () {
    return this.replace(/(\s*$)/g, "");
};
//替换所有
String.prototype.replaceAll = function (source, target, ignoreCase) {
    if (ignoreCase == null) {
        ignoreCase = false;
    }
    source = source.replace(/([\\.$^{[(|)*+?\\\\])/g, "\\$1");
    if (!RegExp.prototype.isPrototypeOf(source)) {
        return this.replace(new RegExp(source, (ignoreCase ? "gi" : "g")), target);
    } else {
        return this.replace(source, target);
    }
};
//C#格式化字符串
String.prototype.format = function () {
    if (arguments == null || arguments.length == 0) {
        return this;
    }
    var s = this;
    for (var i = 0, len = arguments.length; i < len; i++) {
        s = s.replaceAll("{" + i.toString() + "}", arguments[i].toString());
    }
    return s;
};
//Url参数设置
String.prototype.urlReplaceParmeter = function (parmeterName, parmeterVlaue) {
    var url = this, pIndex = url.indexOf(parmeterName);
    if (pIndex > -1) {
        var endIndex = url.indexOf("&", pIndex);
        if (endIndex == -1) {
            endIndex = url.length;
        }
        var str = url.substring(pIndex, endIndex);
        return url.replace(str, "{0}={1}".format(parmeterName, parmeterVlaue));
    } else {
        if (this.indexOf("?") > -1) {
            return (url + "&{0}={1}").format(parmeterName, parmeterVlaue);
        } else {
            return (url + "?{0}={1}").format(parmeterName, parmeterVlaue);
        }
    }
};
//字符串是否为空(空、undefined、null)
String.prototype.isEmpty = function () {
    return this == "" || this == undefined || this == null;
};
// 返回字符串的实际长度, 一个汉字算2个长度  
String.prototype.strlen = function () {
    return this.replace(/[^\x00-\xff]/g, "**").length;
};
//字符串超出长度则省略 
String.prototype.cutstr = function (len) {
    var restr = this;
    var wlength = this.replace(/[^\x00-\xff]/g, "**").length;
    if (wlength > len) {
        for (var k = len / 2; k < this.length; k++) {
            if (this.substr(0, k).replace(/[^\x00-\xff]/g, "**").length >= len) {
                restr = this.substr(0, k) + "...";
                break;
            }
        }
    }
    return restr;
};
//判断是否以某个字符串开头 
String.prototype.startWith = function (s) {
    return this.indexOf(s) == 0
};
//判断是否以某个字符串结束 
String.prototype.endWith = function (s) {
    var d = this.length - s.length;
    return (d >= 0 && this.lastIndexOf(s) == d)
};


var Common = {
    AntiForgeryToken: "__RequestVerificationToken"
};
//  验证
Common.Validator = {
    isEmail: function (str) {
        var strReg = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        return strReg.test(str);
    }, isNaturalNumber: function (s) {
        //验证非０开头的正整数，可以为0．（既自然数）
        var reg = /^(([0-9]){1}|[1-9]\d*)$/;
        return reg.test(s);
    }, isPositiveNum: function (s) {
        //验证非0开头且大于0的正整数
        var reg = /^[1-9]\d*$/;
        return reg.test(s);
    }, isParseFloat: function (s) {
        //验证包含小数位的金额（允许输入整数、一位小数以及两位小数）
        var reg = /^((\d{1,}\.\d{2,2})|(\d{1,}\.\d{1,1})|(\d{1,}))$/;
        return reg.test(s);
    }, isNumber: function (s) {
        if (s == "") {
            return false;
        }
        return !isNaN(s);
    }, isFloatNum: function (s) {
        var reg = /^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$/;
        return reg.test(s);
    }, isPwd: function (str) {
        var strReg = /^[A-Za-z0-9]{6,20}$/;
        return strReg.test(str);
    }
};
//  获取键盘key值
Common.getKeyCode = function (e) {
    var evt = e || window.event;
    return evt.keyCode || evt.which || evt.charCode;
};
//  只允许输入数字 
Common.onlyNumber = function (sender, e) {
    var keyCode = Common.getKeyCode(e);
    if (keyCode == 48 || keyCode == 96) {
        if (sender.value != "") {
            return true;
        }
    } else {
        if (keyCode == 8 || keyCode == 9 || keyCode == 37 || keyCode == 39) {
            return true;
        } else {
            if (keyCode > 95 && keyCode < 106) {
                return true;
            } else {
                if (keyCode > 47 && keyCode < 58) {
                    return true;
                }
            }
        }
    }
    return false;
};
//  验证是否为IE浏览器
Common.isIE = function () {
    return document.all ? true : false;
};
//  切换显示文本默认提示
Common.IePlaceholder = function (fieldsObj) {
    if (Common.isIE()) {
        var txtItems = fieldsObj != undefined ? fieldsObj : $("[placeholder]");
        txtItems.each(function () {
            var inputs = $(this);
            var placeText = inputs.attr("placeholder");
            inputs.attr("placeholder", "")
            var inputText = $.trim(inputs.val());
            if (inputText.length < 1 || inputText.toLowerCase() === placeText.toLowerCase()) {
                inputs.addClass("txtTips").val(placeText);
            }
            inputs.focus(function () {
                var _e = $(this);
                inputs.attr("placeholder", "")
                var _inputText = $.trim(_e.val());
                if (_inputText.length < 1 || _inputText.toLowerCase() === placeText.toLowerCase()) {
                    _e.removeClass("txtTips").val("");
                }
            }).blur(function () {
                var _e = $(this);
                var _inputText = $.trim(_e.val());
                if (_inputText.length < 1 || _inputText.toLowerCase() === placeText.toLowerCase()) {
                    _e.addClass("txtTips").val(placeText);
                }
                inputs.attr("placeholder", placeText)
            });
        });
    }
};
//  加入收藏夹 
Common.AddFavorite = function (sURL, sTitle) {
    try {
        window.external.addFavorite(sURL, sTitle);
    } catch (e) {
        try {
            window.sidebar.addPanel(sTitle, sURL, "");
        } catch (e) {
            alert("加入收藏失败，请使用Ctrl+D进行添加");
        }
    }
};
//  设为首页 
Common.setHomepage = function (homeurl) {
    if (document.all) {
        document.body.style.behavior = 'url(#default#homepage)';
        document.body.setHomePage(homeurl);
    } else if (window.sidebar) {
        if (window.netscape) {
            try {
                netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
            } catch (e) {
                alert("该操作被浏览器拒绝，如果想启用该功能，请在地址栏内输入about:config,然后将项 signed.applets.codebase_principal_support 值该为true");
            }
        }
        var prefs = Components.classes['@mozilla.org/preferences-service;1'].getService(Components.interfaces.nsIPrefBranch);
        prefs.setCharPref('browser.startup.homepage', homeurl);
    }
};
//  随机数时间戳 
Common.uniqueId = function () {
    var a = Math.random, b = parseInt;
    return Number(new Date()).toString() + b(10 * a()) + b(10 * a()) + b(10 * a());
}

//Ajax附加AntiForgeryToken令牌
$.ajaxPrefilter(function (options, originalOptions, jqXHR) {
    if (options.type.toUpperCase() === "POST") {
        // We need to add the verificationToken to all POSTs
        var tokenName = Common.AntiForgeryToken;
        var tokens = $("input[name^=" + tokenName + "]");
        var token = null;
        if (!tokens.length) return;
        tokens.each(function () {
            if (!$(this).val().isEmpty()) {
                token = $(this);
                return false;
            }
        });

        if (token == null) {
            return;
        }

        // If the data is JSON, then we need to put the token in the QueryString:
        if (options.contentType.indexOf('application/json') === 0) {
            // Add the token to the URL, because we can't add it to the JSON data:
            options.url += ((options.url.indexOf("?") === -1) ? "?" : "&") + token.serialize();
        } else if (typeof options.data === 'string' && options.data.indexOf(tokenName) === -1) {
            // Append to the data string:
            options.data += (options.data ? "&" : "") + token.serialize();
        }
    }
});

(function ($) {
    jQuery.fn.extend({
        //参数childrenChkName：单个复选的name， 使用例如：$(":checkbox[name=ChooseAll]").ChooseAll("childrenChkName")
        ChooseAll: function (childrenChkName) {
            var sender = $(this);
            sender.click(function () {
                var isChecked = $(this).attr("checked");
                jQuery(":checkbox[name=" + childrenChkName + "]:not(:disabled)").attr({ checked: isChecked });
                sender.attr({ checked: isChecked });
            });
            jQuery(":checkbox[name=" + childrenChkName + "]").click(function () {
                var isChecked = $(this).attr("checked");
                var checkedCount = jQuery(":checkbox[name=" + childrenChkName + "]:checked,:checkbox[name=" + childrenChkName + "]:disabled").size();
                var totalCount = jQuery(":checkbox[name=" + childrenChkName + "]").size();
                if (checkedCount == totalCount) {
                    sender.attr({ checked: true });
                } else {
                    sender.attr({ checked: false });
                }
                ;
            })
        },

        //获取选中的checkbox的值,返回值以逗号分隔的字符串值. 使用例如：var value = $(":checkbox[name=checkboxName]").GetChooseVal()
        GetChooseVal: function () {
            var chooseCheckbox = jQuery(this).filter(":checked");
            var chkValArr = jQuery.map(chooseCheckbox, function (item) {
                return jQuery(item).val();
            });
            return chkValArr.toString();
        },
        //判断是否有选中的数据 返回true or false
        IsCheck: function () {
            var thisObj = jQuery(this);
            var flag = false;
            thisObj.each(function () {
                if ($(this).attr("checked") == true) {
                    flag = true;
                    return;
                }
            });
            return flag;
        }
    });
    $.extend({});
})(jQuery);

/*
*   Datatype类
*/
var DataType = {};
//  验证是否email格式
DataType.isEmail = function (s) {
    var strReg = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return strReg.test(s);
};
DataType.isURL = function (s) {
    var strRegex = "^((https|http|ftp|rtsp|mms)?://)"
		+ "?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?" // ftp的user@
		+ "(([0-9]{1,3}\.){3}[0-9]{1,3}" // IP形式的URL- 199.194.52.184
		+ "|" // 允许IP和DOMAIN（域名）
		+ "([0-9a-z_!~*'()-]+\.)*" // 域名- www.
		+ "([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]\." // 二级域名
		+ "[a-z]{2,6})" // first level domain- .com or .museum
		+ "(:[0-9]{1,4})?" // 端口- :80
		+ "((/?)|" // a slash isn't required if there is no file name
		+ "(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";
    var re = new RegExp(strRegex);
    return re.test(s);
}
//  验证非０开头的正整数，可以为0．（既自然数）
DataType.isNaturalNumber = function (s) {
    var reg = /^(([0-9]){1}|[1-9]\d*)$/;
    return reg.test(s);
};
//  验证非0开头且大于0的正整数
DataType.isPositiveNum = function (s) {
    var reg = /^[1-9]\d*$/;
    return reg.test(s);
};
//  验证包含小数位的金额（允许输入整数、一位小数以及两位小数）
DataType.isParseFloat = function (s) {
    var reg = /^((\d{1,}\.\d{2,2})|(\d{1,}\.\d{1,1})|(\d{1,}))$/;
    return reg.test(s);
};
//  验证数字
DataType.isNumber = function (s) {
    if (s == "") {
        return false;
    }
    return !isNaN(s);
};
//  验证浮点数
DataType.isFloatNum = function (s) {
    var reg = /^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$/;
    return reg.test(s);
};
//  验证密码格式
DataType.isPwd = function (s) {
    var strReg = /^[A-Za-z0-9]{6,20}$/;
    return strReg.test(s);
};
//  只允许输入数字 
DataType.onlyNumber = function (sender, e) {
    var keyCode = Common.getKeyCode(e);
    if (keyCode == 48 || keyCode == 96) {
        if (sender.value != "") {
            return true;
        }
    } else {
        if (keyCode == 8 || keyCode == 9 || keyCode == 37 || keyCode == 39) {
            return true;
        } else {
            if (keyCode > 95 && keyCode < 106) {
                return true;
            } else {
                if (keyCode > 47 && keyCode < 58) {
                    return true;
                }
            }
        }
    }
    return false;
};

/*
*   Cookie类
*/
var Cookie = {};
//  写入Cookie，key为键，value是值，duration过期时间（天为单位，默认1天）
Cookie.set = function (key, value, duration) {
    Cookie.remove(key);
    var d = new Date();
    if (duration <= 0)
        duration = 1;
    d.setDate(d.getDay() + duration);
    document.cookie = key + "=" + encodeURI(value) + "; expires=" + d.toGMTString() + ";path=/";
};
//  移除Cookie，key为键
Cookie.remove = function (key) {
    var d = new Date();
    if (Cookie.read(key) != "") {
        d.setDate(d.getDay() - 1);
        document.cookie = key + "=;expires=" + d.toGMTString();
    }
};
//  读取Cookie，key是键
Cookie.get = function (key) {
    var arr = document.cookie.match(new RegExp("(^| )" + key + "=([^;]*)(;|$)"));
    if (arr != null) {
        return decodeURIComponent(arr[2]);
    }
    return "";
};

$(function () {
    /**
	* 分页按钮点击
	*/
    $('body').delegate('.ajax-pagination a', 'click', function () {
        $this = $(this);
        var pageUrl = '';
        pageUrl = $this.attr('data-url');
        if (pageUrl == '' || typeof (pageUrl) == 'undefined')
            pageUrl = $this.attr('href');
        if (pageUrl == '' || typeof (pageUrl) == 'undefined') return false;
        $.get(pageUrl, function (data) {
            //$this.parents('div.ajax-list').filter(':first').html(data);
            $('.ajax-list').filter(':first').html(data);	//让别处点击时也能重新更新该div
        });
        return false;
    });

    /**
	* 使用Modal显示弹窗前/关闭弹窗后
	*/
    $('.ajax_detail').on('show', function (e) {

        //	点击了 tab 不操作 lvxiaoyong 20140403
        var targ;
        if (!e) var e = window.event;
        if (e.target) targ = e.target;
        else if (e.srcElement) targ = e.srcElement;
        if (targ.nodeType == 3) // defeat Safari bug
            targ = targ.parentNode;
        if ($(targ).attr('data-toggle') == 'tab') return;

        $this = $(this);
        var pageUrl = $this.attr('data-url');
        var id = $this.attr('data-id');
        $.get(pageUrl + '/' + id, function (data) {
            $this.children('.ajax_info').html(data);
            var jqForm = $this.find('form').filter(':first');
            //用于刷新当前页面
            var currenturl = $("#current_url").val();
            var currentpage = $(".current_page").val();
            if (currentpage != undefined)
                $("#current_url").val(currenturl + "?page=" + currentpage);
            
            jqForm.append($('.hidden-div').html());
        });
    }).on('hidden', function () {
        var id = $(this).attr('data-id');
        $('.btn-info-view').button('reset');
        var jqForm = $(this).find('form').filter(':first');
        if (typeof (jqForm[0]) != 'undefined') jqForm[0].reset();	//	重置表单
    });

    $('.ajax_detail_1').on('show', function (e) {

        //	点击了 tab 不操作 lvxiaoyong 20140403
        var targ;
        if (!e) var e = window.event;
        if (e.target) targ = e.target;
        else if (e.srcElement) targ = e.srcElement;
        if (targ.nodeType == 3) // defeat Safari bug
            targ = targ.parentNode;
        if ($(targ).attr('data-toggle') == 'tab') return;

        $this = $(this);
        var pageUrl = $this.attr('data-url');
        var id = $this.attr('data-id');
        $.get(pageUrl + '/' + id, function (data) {
            $this.children('.ajax_info').html(data);
            var jqForm = $this.find('form').filter(':first');
            //用于刷新当前页面
            var currenturl = $("#current_url").val();
            var currentpage = $(".current_page").val();
            if (currentpage != undefined)
                $("#current_url").val(currenturl + "?page=" + currentpage);
            
            jqForm.append($('.hidden-div').html());
            $('div.bfh-datepicker').each(function () {
                var $datepicker;

                $datepicker = $(this);

                $datepicker.bfhdatepicker($datepicker.data());
            });
            $('div.bfh-timepicker').each(function () {
                var $timepicker;

                $timepicker = $(this);

                $timepicker.bfhtimepicker($timepicker.data());
            });
        });
    }).on('hidden', function () {
        var id = $(this).attr('data-id');
        $('.btn-info-view-1').button('reset');
        var jqForm = $(this).find('form').filter(':first');
        if (typeof (jqForm[0]) != 'undefined') jqForm[0].reset();	//	重置表单
    });

    $('.ajax_detail_2').on('show', function (e) {

        //	点击了 tab 不操作 lvxiaoyong 20140403
        var targ;
        if (!e) var e = window.event;
        if (e.target) targ = e.target;
        else if (e.srcElement) targ = e.srcElement;
        if (targ.nodeType == 3) // defeat Safari bug
            targ = targ.parentNode;
        if ($(targ).attr('data-toggle') == 'tab') return;

        $this = $(this);
        var pageUrl = $this.attr('data-url');
        var id = $this.attr('data-id');
        $.get(pageUrl + '/' + id, function (data) {
            $this.children('.ajax_info').html(data);
            var jqForm = $this.find('form').filter(':first');
            //用于刷新当前页面
            var currenturl = $("#current_url").val();
            var currentpage = $(".current_page").val();
            if (currentpage != undefined)
                $("#current_url").val(currenturl + "?page=" + currentpage);

            jqForm.append($('.hidden-div').html());
        });
    }).on('hidden', function () {
        var id = $(this).attr('data-id');
        $('.btn-info-view-2').button('reset');
        var jqForm = $(this).find('form').filter(':first');
        if (typeof (jqForm[0]) != 'undefined') jqForm[0].reset();	//	重置表单
    });

    $('.ajax_show_add').on('show', function () {
        var self = this;
        var pageUrl = $(self).data('url');
        if (!pageUrl || typeof (pageUrl) == 'undefined') {
        } else {
            $.get(pageUrl + '/0', function (data) {
                $(self).children('.ajax_info').html(data);
            });
        }
        var jqForm = $(self).find('form').filter(':first');
        jqForm.append($('.hidden-div').html());
    }).on('hidden', function () {
        var jqForm = $(this).find('form').filter(':first');
        if (typeof (jqForm[0]) != 'undefined') jqForm[0].reset();	//	重置表单
    });

    /**
	* 查看或编辑按钮点击打开Modal弹窗
	*/
    $('.btn-info-view').live('click', function () {
        var id = $(this).attr('data-id');
        if ($(this).hasClass('disabled')) return false;
        if (!id || typeof (id) == 'undefined') return false;
        $('.btn-info-view').button('reset');
        $(this).button('loading');
        $('.ajax_detail').attr('data-id', id).modal({ backdrop: true });

    });

    $('.btn-info-view-1').live('click', function () {
        var id = $(this).attr('data-id');
        if ($(this).hasClass('disabled')) return false;
        if (!id || typeof (id) == 'undefined') return false;
        $('.btn-info-view-1').button('reset');
        $(this).button('loading');
        $('.ajax_detail_1').attr('data-id', id).modal({ backdrop: true });

    });

    $('.btn-info-view-2').live('click', function () {
        var id = $(this).attr('data-id');
        if ($(this).hasClass('disabled')) return false;
        if (!id || typeof (id) == 'undefined') return false;
        $('.btn-info-view-2').button('reset');
        $(this).button('loading');
        $('.ajax_detail_2').attr('data-id', id).modal({ backdrop: true });

    });


    $('.btn-info-view-div').live('click', function () {
        var id = $(this).attr('data-id');
        if ($(this).hasClass('disabled')) return false;
        if (!id || typeof (id) == 'undefined') return false;
        $('.btn-info-view-div').button('reset');
        $(this).button('loading');
        $('.ajax_detail').attr('data-id', id).modal({ backdrop: false });

    });

    /**
	* 行删除按钮事件
	*/
    $('.btn-ajax-delete').live('click', function () {
        var id = $(this).data('id');
        if ($(this).hasClass('disabled')) return false;
        if (!id || typeof (id) == 'undefined') return false;
        var confirm_word = $(this).data('confirm');
        if (!confirm_word || typeof (confirm_word) == 'undefined') confirm_word = '确定要删除吗?';
        if (confirm(confirm_word)) {
            var url = $(".table").data('delaction');
            OperateBybatch(url, id);
        }
    });
    /**
	* 显示add弹框
	*/
    $('.btn-info-add').live('click', function () {
        $('.alert-success').remove();
        $('.ajax_show_add').modal({ backdrop: true });
    });

    /**
	* 初始化加载列表数据
	*/
    if ($('div.ajax-list').attr('data-url')) {
        $.get($('div.ajax-list').attr('data-url'), function (data) {
            $('div.ajax-list').filter(':first').html(data);
        });
    }

    /**
	* 搜索框搜索
	*/
    $("#searchForm").submit(function () {
        var args = $(this).serialize();
        var pageUrl = $(this).attr('action');
        $.get(pageUrl + '?' + args, function (data) {
            $('div.ajax-list').filter(':first').html(data);
        });
        return false;
    });


    /**
	* 弹出框中保存按钮点击
	*/
    $('body').delegate('.ajax_update', 'click', function () {

        $this = $(this);
        jqForm = $this.parents('.ajax_detail').filter(':first').find('form').filter(':first');
        //		jqForm.append($('.hidden-div').html());
        //	修正ckeditor不兼容ajax的问题
        if (typeof (CKEDITOR) != 'undefined') for (instance in CKEDITOR.instances) {
            CKEDITOR.instances[instance].updateElement();
        }

        var url = $(jqForm).attr('action');
        jqForm.ajaxSubmit({
            url: url,
            dataType: 'json',
            beforeSubmit: beforeFormSubmit,
            success: function (responseText) {
                $this.attr('disabled', true);
                if (responseText.error != true) {
                    var msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '操作成功!' : responseText.msg;
                    $(jqForm).before('<div class="alert alert-success"><a class="close" data-dismiss="alert">&times;</a>' + msg + '</div>');
                    if (responseText.getlist == true) {
                        $('div.ajax-list').filter(':first').load($("#current_url").val());
                    }
                    if (responseText.goback == true) {	//	是否跳转回前一个页面
                        setTimeout("$('.ajax-pagination a.btn_goback').filter(':first').click();", 2000);
                        //						$('.ajax-pagination a.btn_goback').filter(':first').click();
                    }
                } else {
                    var msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '操作失败!' : responseText.msg;
                    $(jqForm).before('<div class="alert alert-error"><a class="close" data-dismiss="alert">&times;</a>' + msg + '</div>');
                }

                $this.attr('disabled', false);
            }
            //			type: 'post'	
        });
        return false;
    });

    $('body').delegate('.ajax_update_1', 'click', function () {

        $this = $(this);
        jqForm = $this.parents('.ajax_detail_1').filter(':first').find('form').filter(':first');
        //		jqForm.append($('.hidden-div').html());
        //	修正ckeditor不兼容ajax的问题
        if (typeof (CKEDITOR) != 'undefined') for (instance in CKEDITOR.instances) {
            CKEDITOR.instances[instance].updateElement();
        }

        var url = $(jqForm).attr('action');
        jqForm.ajaxSubmit({
            url: url,
            dataType: 'json',
            beforeSubmit: beforeFormSubmit,
            success: function (responseText) {
                $this.attr('disabled', true);
                if (responseText.error != true) {
                    var msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '操作成功!' : responseText.msg;
                    $(jqForm).before('<div class="alert alert-success"><a class="close" data-dismiss="alert">&times;</a>' + msg + '</div>');
                    if (responseText.getlist == true) {
                        $('div.ajax-list').filter(':first').load($("#current_url").val());
                    }
                    if (responseText.goback == true) {	//	是否跳转回前一个页面
                        setTimeout("$('.ajax-pagination a.btn_goback').filter(':first').click();", 2000);
                        //						$('.ajax-pagination a.btn_goback').filter(':first').click();
                    }
                } else {
                    var msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '操作失败!' : responseText.msg;
                    $(jqForm).before('<div class="alert alert-error"><a class="close" data-dismiss="alert">&times;</a>' + msg + '</div>');
                }

                $this.attr('disabled', false);
            }
            //			type: 'post'	
        });
        return false;
    });

    $('body').delegate('.ajax_update_2', 'click', function () {

        $this = $(this);
        jqForm = $this.parents('.ajax_detail_2').filter(':first').find('form').filter(':first');
        //		jqForm.append($('.hidden-div').html());
        //	修正ckeditor不兼容ajax的问题
        if (typeof (CKEDITOR) != 'undefined') for (instance in CKEDITOR.instances) {
            CKEDITOR.instances[instance].updateElement();
        }

        var url = $(jqForm).attr('action');
        jqForm.ajaxSubmit({
            url: url,
            dataType: 'json',
            beforeSubmit: beforeFormSubmit,
            success: function (responseText) {
                $this.attr('disabled', true);
                if (responseText.error != true) {
                    var msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '操作成功!' : responseText.msg;
                    $(jqForm).before('<div class="alert alert-success"><a class="close" data-dismiss="alert">&times;</a>' + msg + '</div>');
                    if (responseText.getlist == true) {
                        $('div.ajax-list').filter(':first').load($("#current_url").val());
                    }
                    if (responseText.goback == true) {	//	是否跳转回前一个页面
                        setTimeout("$('.ajax-pagination a.btn_goback').filter(':first').click();", 2000);
                        //						$('.ajax-pagination a.btn_goback').filter(':first').click();
                    }
                } else {
                    var msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '操作失败!' : responseText.msg;
                    $(jqForm).before('<div class="alert alert-error"><a class="close" data-dismiss="alert">&times;</a>' + msg + '</div>');
                }

                $this.attr('disabled', false);
            }
            //			type: 'post'	
        });
        return false;
    });


    /**
	* 弹出框中保存按钮点击
	*/
    $('body').delegate('.ajax_add', 'click', function () {
        $this = $(this);
        jqForm = $this.parents('.ajax_show_add').filter(':first').find('form').filter(':first');
        //		jqForm.append($('.hidden-div').html());

        //	修正ckeditor不兼容ajax的问题
        if (typeof (CKEDITOR) != 'undefined') for (instance in CKEDITOR.instances) {
            CKEDITOR.instances[instance].updateElement();
        }

        var url = $(jqForm).attr('action');
        jqForm.ajaxSubmit({
            url: url,
            dataType: 'json',
            beforeSubmit: beforeFormSubmit,
            success: function (responseText) {
                $this.attr('disabled', true);
                if (responseText.error != true) {
                    var msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '操作成功!' : responseText.msg;
                    $(jqForm).before('<div class="alert alert-success"><a class="close" data-dismiss="alert">×</a>' + msg + '</div>');
                    if (typeof (jqForm[0]) != 'undefined') jqForm[0].reset();	//	重置表单
                    if (responseText.getlist == true) {
                        $('div.ajax-list').filter(':first').load($("#current_url").val());
                    }
                    if (responseText.goback == true) {	//	是否跳转回前一个页面
                        setTimeout("$('.ajax-pagination a.btn_goback').filter(':first').click();", 2000);
                        //						$('.ajax-pagination a.btn_goback').filter(':first').click();
                    }
                } else {
                    var msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '操作失败!' : responseText.msg;
                    $(jqForm).before('<div class="alert alert-error"><a class="close" data-dismiss="alert">×</a>' + msg + '</div>');
                }
                $this.attr('disabled', false);

            }
            //			type: 'post'	
        });
        return false;
    });

    /**
	* 表单提交前
	*/
    var beforeFormSubmit = function (formData, jqForm, options) {
        jqForm.prevAll('.alert').remove();
        return form_validator(formData, jqForm);
    };

    /**
	* 表单提交后
	*/
    var afterFormSubmit = function (responseText, statusText) {
    };

    /**
	* 表单验证
	* todo. 现在只处理必填
	*/
    var form_validator = function (formData, jqForm) {
        var form = jqForm[0];
        var flag = true;

        for (var i = 0; i < formData.length; i++) {
            //			var obj = eval("form."+formData[i].name);
            var obj = eval("form.elements[i]");
            if ($(obj).parents('div.control-group').filter(':first').length == 1) {
                var par = $(obj).parents('div.control-group').filter(':first');
                $(par).removeClass('error').find('span.help-inline').remove();
                var control = true;
            } else {
                var par = $(obj).parents('td').filter(':first');
                $(par).removeClass('control-group').removeClass('error').find('span.help-inline').remove();
                var control = false;
            }
            //			if($(obj).attr('required') && !formData[i].value){
            if ($(obj).attr('required') && !$(obj).val()) {	//	验证必填
                if (!control) $(par).addClass('control-group');
                $(par).addClass('error');
                $(obj).after('<span class="help-inline">必填!</span>');
                flag = false;
            }
            if (flag) if ($(obj).attr('check-email')) {	//	验证EMAIL
                var v = $(obj).val();
                if (!DataType.isEmail(v)) {
                    if (!control) $(par).addClass('control-group');
                    $(par).addClass('error');
                    $(obj).after('<span class="help-inline">EMAIL格式!</span>');
                    flag = false;
                }
            }
            if (flag) if ($(obj).attr('check-url')) {	//	验证URL
                var v = $(obj).val();
                if (!DataType.isURL(v)) {
                    if (!control) $(par).addClass('control-group');
                    $(par).addClass('error');
                    $(obj).after('<span class="help-inline">URL格式!</span>');
                    flag = false;
                }
            }
        }
        //	其他都验证通过后，验证密码确认
        if (flag) {
            if ($(form).find('#password2').length == 1) {
                flag = validatePwdSame($(form).find('#password2'));
            }
        }

        return flag;
    };

    /**
	* 再次输入密码 验证
	*/
    $('#password2').on('blur', function () {
        validatePwdSame($(this));
    });
    var validatePwdSame = function (obj) {
        var pwd = $(obj).parents('form').filter(':first').find('#password');
        var flag = true;
        if ($(obj).parents('div.control-group').filter(':first').length == 1) {
            var par = $(obj).parents('div.control-group').filter(':first');
            $(par).removeClass('error').find('span.help-inline').remove();
            var control = true;
        } else {
            var par = $(obj).parents('td').filter(':first');
            $(par).removeClass('control-group').removeClass('error').find('span.help-inline').remove();
            var control = false;
        }
        if ($(obj).val() != $(pwd).val()) {
            if (!control) $(par).addClass('control-group');
            $(par).addClass('error');
            $(obj).after('<span class="help-inline">两次密码输入必须一致！</span>');
            flag = false;
        }
        return flag;
    };

    $('.table tbody tr').live('click', function (e) { if (e.target.tagName != "TD") return; $(this).find('input[name=row_sel]').each(function () { this.checked = !this.checked; }); });
    $('.table th input[name=select_rows]').live("click", function () { $('.table tr').find('input[name=row_sel]').attr('checked', this.checked); });

    //列表批量删除
    $("#deleteBybatch").live("click", function () {
        var url = $(".table").data('delaction');
        var ids = "";
        $(".table tr input[name=row_sel]:checked").each(function () { ids += $(this).data("id") + ","; });
        if (ids.length == 0) {
            alert("请选择数据行.");
            return false;
        }
        else {
            ids = ids.substring(0, ids.length - 1);
            if (!confirm("你确认将" + ids.split(",").length + "条数据彻底删除？"))
                return false;
        }
        OperateBybatch(url, ids);
    });

    //  input输入限制
    $(":text").each(function () {
        $(this).on("blur", function () {
            var t = $(this).attr("datatype");
            if (t != undefined) {
                var reg = "";
                var err = "";
                var v = $(this).val();
                switch (t) {
                    case "i":  //  整数
                        reg = /^-?\d+$/;
                        err = "整数";
                        break;
                    case "i1":  //  正整数
                        reg = /^[0-9]*[1-9][0-9]*$/;
                        err = "正整数";
                        break;
                    case "i0":  //  非负整数
                        reg = /^\d+$/;
                        err = "非负整数";
                        break;
                    case "i-0":  //  非正整数
                        reg = /^((-\d+)|(0+))$/;
                        err = "非正整数";
                        break;
                    case "i-1":  //  负整数
                        reg = /^-[0-9]*[1-9][0-9]*$/;
                        err = "负整数";
                        break;

                    case "f":  //  浮点数
                        reg = /^(-?\d+)(\.\d+)?$/;
                        err = "浮点数";
                        break;
                    case "f1":  //  正浮点数
                        reg = /^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$/;
                        err = "正浮点数";
                        break;
                    case "f0":  //  非负浮点数
                        reg = /^\d+(\.\d+)?$/;
                        err = "非负浮点数";
                        break;
                    case "f-0":  //  非正浮点数
                        reg = /^((-\d+(\.\d+)?)|(0+(\.0+)?))$/;
                        err = "非正浮点数";
                        break;
                    case "f-1":  //  负浮点数
                        reg = /^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$/;
                        err = "负浮点数";
                        break;

                    default:
                        break;
                }
                if (reg != "" && !reg.test(v)) {
                    //alert();
                    $(this).val("").attr("placeholder", err).focus();
                }
            }
        });
    });
});

var OperateBybatch = function (url, ids) {
    $('div.ajax-list .alert').remove();
    $.post(url, { 'ids': ids }, function (responseText) {
        if (responseText.error != true) {
            var msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '删除成功!' : responseText.msg;
            if (responseText.getlist == true)
                $('div.ajax-list').filter(':first').load(responseText.url, function () {
                    $('div.ajax-list').filter(':first').prepend('<div class="alert alert-success"><a class="close" data-dismiss="alert">&times;</a>' + msg + '</div>');
                });
        } else {
            var msg = responseText.msg == '' || typeof (responseText.msg) == 'undefined' ? '删除失败!' : responseText.msg;
            $('div.ajax-list').filter(':first').prepend('<div class="alert alert-error"><a class="close" data-dismiss="alert">&times;</a>' + msg + '</div>');
        }
    }, 'json');
};
