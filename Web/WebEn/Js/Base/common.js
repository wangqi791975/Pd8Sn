
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
    return this.indexOf(s) == 0;
};
//判断是否以某个字符串结束 
String.prototype.endWith = function (s) {
    var d = this.length - s.length;
    return (d >= 0 && this.lastIndexOf(s) == d);
};

//**Common类**
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

//**扩展jQuery**
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

//**Datatype类**
var DataType = {};
//  验证是否email格式
DataType.isEmail = function (s) {
    var strReg = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return strReg.test(s);
};
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

//**Cookie类**
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

//**DivOs类**
var DivOs = {};
/*
* 打开div弹窗
* @param div的class
* @param 遮罩的class
* @param 关闭按钮的id
*/
DivOs.openDiv = function (aid, bgid, imgid) {
    var sheight = $(window).scrollTop();
    var rheight = $(window).height();
    var rwidth = $(window).width();
    var wwidth = $("." + aid).width();
    var wheight = $("." + aid).height();
    $("." + aid).css("top", (sheight + (rheight - wheight) / 2) + "px");
    $("." + aid).css("left", ((rwidth - wwidth) / 2) + "px");
    $("." + bgid).fadeTo(1000, 0.5);
    $("." + aid).show();
    $("." + bgid).click(function () {
        $("." + bgid).fadeOut();
        $("." + aid).hide();
    });
    $("." + imgid).find("img").click(function () {
        $("." + bgid).fadeOut();
        $("." + aid).hide();
    });
}
/*
* 关闭div弹窗
* @param
*/
DivOs.closeDiv = function () {
}
/*
* 移除div弹窗
* @param
*/
DivOs.removeDiv = function () {
}
/*
* 显示tips
* @param
*/
DivOs.showTip = function () {
}
/*
* 隐藏tips
* @param
*/
DivOs.hideTip = function () {
}
/*
* 根据code获取Message信息
* @param 信息code
*/
DivOs.getMessage = function (msg) {
    return Message[msg] ? Message[msg] : msg;
}
/*
* 显示错误弹窗
* @param 错误信息
*/
DivOs.showErrorModal = function (msg) {
    $("#pop_error_modal").on("show", function () {
        $(this).find(".error_tip").text(msg);
    }).modal("show");
}
/*
* 显示错误弹窗
* @param 错误信息
*/
DivOs.showConfirmModal = function (msg, callback, obj) {
    $("#pop_confirm_modal").on("show", function () {
        $(this).find(".confirm_tip").text(msg);
        var $this = $(this);
        $this.find(".confirm_yes").each(function () {
            $(this).off("click").on("click", function (e) {
                e.preventDefault();
                $this.modal("hide");
                if (typeof (callback) != "undefined") {
                    callback(obj);
                }
                return false;
            });
        });
    }).modal("show");
}

/*
* 显示自定义弹框
* @param 错误信息
*/
DivOs.showcCustomConfirmModal = function (objId, msg, callback, obj) {
    $("#" + objId).on("show", function () {
        $(this).find(".confirm_tip").text(msg);
        var $this = $(this);
        $this.find(".confirm_yes").each(function () {
            $(this).off("click").on("click", function (e) {
                e.preventDefault();
                $this.modal("hide");
                if (typeof (callback) != "undefined") {
                    callback(obj);
                }
                return false;
            });
        });
    }).modal("show");
}

//**让低版本浏览器支持placeholder**
var JPlaceHolder = {
    //检测
    _check: function () {
        return "placeholder" in document.createElement("input");
    },
    //初始化
    init: function () {
        if (!this._check()) {
            this.fix();
        }
    },
    //修复
    fix: function () {
        jQuery(":input[placeholder]").each(function (index, element) {
            var self = $(this), txt = self.attr("placeholder");
            //self.wrap($('<div></div>').css({ position: 'relative', zoom: '1', border: 'none', background: 'none', padding: 'none', margin: 'none' }));
            var pos = self.position(), h = self.outerHeight(true), paddingleft = self.css("padding-left");
            var holder = $("<span></span>").text(txt).css({ position: "absolute", left: pos.left, top: pos.top, height: h, lineHeight: h + "px", paddingLeft: paddingleft, color: "#aaa" }).appendTo(self.parent());
            self.focusin(function (e) {
                holder.hide();
            }).focusout(function (e) {
                if (!self.val()) {
                    holder.show();
                }
            });
            holder.click(function (e) {
                holder.hide();
                self.focus();
            });
        });
    }
};
//执行
jQuery(function () {
    JPlaceHolder.init();

    function fnGetShoppingCartQty(obj, productId) {
        var productQty = obj.parent().find("[name='input_qty_" + productId + "']").val();
        if (productQty == undefined)
            productQty = obj.parents("li").find("[name='input_qty_" + productId + "']").val();
        if (productQty == undefined)
            productQty = obj.parents("tr").find("[name='input_qty_" + productId + "']").val();
        if (productQty == undefined)
            productQty = obj.parents("div").find("[name='input_qty_" + productId + "']").val();
        if (productQty == undefined)
            productQty = obj.parents(".quantity").find("#input_qty").val();
        if (productQty)
            return productQty <= 0 ? 1 : productQty;
        return 1;
    }

    function fnGetProdCountToCart() {
        $.post("/ShoppingCart/GetProdCount", function (data) { $("#lblcart_prodcount").html(data.prodcount); });
    }

    //产品添加到购物车
    function AddProdToCart(obj) {
        var self = $(obj);
        var productId = self.data("productid");
        var productQty = fnGetShoppingCartQty(self, productId);
        var remark = $('#input_remark_' + productId).val();
        var updType = self.data("pagesource");
        var tplstr = '<div class="poptip {0}" style="display:block;"><i class="bottom"></i><em class="bottom"></em>Only {1} packs are available now, and they have been added into your cart successfully.';
        var tip = "";
        $.post('/ShoppingCart/AddToCart', { 'productId': productId, 'productQty': productQty, 'remark': remark }, function (data) {
            if (data.result === 'refresh') {
                window.location.reload();
            }
            if (data.result === 'success' || data.result === 'warning') {

                switch (updType) {
                    case "productlist":
                        if (data.result === 'warning') {
                            tip = formatStr(tplstr, "poptip_w350_lf", data.msg);
                        }
                        self.removeClass("btn_orange btn_w144").addClass("btn_success_w144").html('<ins class="btn_success_cart_big"></ins>Success' + tip + '</div>');
                        setTimeout(function () {
                            self.removeClass("btn_success_w144").addClass("btn_orange btn_w144").html('<ins class="btn_update"></ins><span>Update</span>');
                        }, 500);
                        break;
                    case "productgallery":
                        if (data.result === 'warning') {
                            tip = formatStr(tplstr, "poptip_w350_lf", data.msg);
                        }
                        self.prev().hide();
                        self.removeClass("btn_orange btn_w100").addClass("btn_success_w175").html('<ins class="btn_success_cart_big"></ins>Success' + tip + '</div>');
                        setTimeout(function () {
                            self.prev().show();
                            self.removeClass("btn_success_w175").addClass("btn_orange btn_w100").html('<ins class="btn_update"></ins>');
                        }, 500);

                        break;
                    case "productdetail":
                        if (data.result === 'warning') {
                            tip = formatStr(tplstr, "poptip_w350", data.msg);
                        }
                        self.removeClass("btn_orange btn_w170").addClass("btn_success_w170").html('<ins class="btn_success_cart_big"></ins>Success' + tip + '</div>');
                        setTimeout(function () {
                            self.removeClass("btn_success_w170").addClass("btn_orange btn_w170").html('<ins class="btn_update"></ins><span>Update</span>');
                        }, 500);
                        break;
                    case "productsimiliar":
                        if (data.result === 'warning') {
                            tip = formatStr(tplstr, "poptip_w350", data.msg);
                        }
                        self.prev().hide();
                        self.removeClass("btn_orange btn_w50").addClass("btn_success_w119 btn_w80").html('<ins class="btn_success_cart"></ins>Success' + tip + '</div>');
                        setTimeout(function () {
                            self.prev().show();
                            self.removeClass("btn_success_w119 btn_w80").addClass("btn_orange btn_w50").html('<ins class="btn_update"></ins>');
                        }, 500);
                        break;
                    case "productrecommenditem"://小的
                        if (data.result === 'warning') {
                            tip = formatStr(tplstr, "poptip_w350", data.msg);
                        }
                        self.prev().hide();
                        self.removeClass("btn_orange btn_w38").addClass("btn_success btn_w80").html('<ins class="btn_success_cart_big"></ins>Success' + tip + '</div>');
                        setTimeout(function () {
                            self.prev().show();
                            self.removeClass("btn_success btn_w80").addClass("lf btn_orange btn_w38").html('<ins class="btn_update"></ins>');
                        }, 500);
                        break;
                    case "productwishlist":
                        if (data.result === 'warning') {
                            tip = formatStr(tplstr, "poptip_w350_lf", data.msg);
                        }
                        self.removeClass("fblue_mid").addClass("btn_success btn_w80").html('<ins class="btn_success_cart"></ins>Success' + tip + '</div>');
                        setTimeout(function () {
                            self.prev().show();
                            self.removeClass("btn_success btn_w80").addClass("fblue_mid").html('update');

                            $.post('/ShoppingCart/GetShoppingCartItem', { 'productId': productId }, function (data) {
                                if (data) {
                                    self.parent().parent().find('td.td_w220 p').last().html('Quantity in cart: ' + data.quantity);
                                }
                            });

                        }, 500);
                        break;
                    case "backorderaddtocart":
                        self.html("<ins class='btn_success_cart'></ins>Success");
                        setTimeout(function () {
                            self.html('<ins class="btn_update"></ins>Update'); self.next().click();
                        }, 500);
                        break;
                    case "orderitemaddtocart":
                        var gotourl = data.shoppingcarturl;
                        self.prev().after('<div class="pop_tip" style="display:block;"><div class="tip_cont"><i class="bottom"></i><em class="bottom"></em>Add to cart successfully!  <a class="fblue_mid" href="' + gotourl + '">View Cart</a></div></div>');
                        setTimeout(function () {
                            self.prev().hide();
                        }, 5000);
                        break;
                    case "dailydealhome":
                        //<div class="tip_cont"><i class="bottom"></i><em class="bottom"></em>Add to cart successfully!  <a class="fblue_mid" href="/shoppingcart.html">View Cart &gt;&gt;</a></div>
                        self.html("Success");
                        self.parents("table").find(".addtocart_tip").show();
                        setTimeout(function () {
                            self.html('Add to Cart');
                        }, 500);
                        break;
                    default:
                        break;
                }
                fnGetProdCountToCart();
            }

        });
    };

    //产品添加到wishlist
    function AddProductToWishList(obj) {
        var self = $(obj);
        var productId = self.data("productid");
        var productQty = fnGetShoppingCartQty(self, productId);
        var updType = self.data("pagesource");

        $.post('/WishList/AddToWishList', { 'productId': productId, 'productQty': productQty }, function (data) {
            if (data.result === 'refresh') {
                window.location.reload();
            }
            if (data.result === 'success') {
                switch (updType) {
                    case "productlist":
                        self.html('<ins class="btn_wishlist_current"></ins><span>Add To Wishlist</span>');
                        break;
                    case "productgallery":
                        self.html('<ins class="btn_wishlist_current"></ins>');
                        break;
                    case "productdetail":
                        self.html('<ins class="btn_wishlist_current"></ins><span>Add To Wishlist</span>');
                        break;
                    case "productsimiliar":
                        self.html('<ins class="btn_wishlist_current"></ins>');
                        break;
                    case "productrecommenditem"://小的
                        self.html('<ins class="btn_wishlist_current"></ins>');
                        break;
                    default:
                        self.html('<ins class="btn_wishlist_current"></ins><span>Add To Wishlist</span>');
                        break;
                }
            }

        });
    };

    function formatStr() {
        var ary = [];
        for (var i = 1 ; i < arguments.length ; i++) {
            ary.push(arguments[i]);
        }
        return arguments[0].replace(/\{(\d+)\}/g, function (m, i) {
            return ary[i];
        });
    }

    //绑定添加到购物车按钮事件
    $('body').on("click", ".cart_btn_add", function () { AddProdToCart(this); });

    //绑定添加到wishlist按钮事件
    $('body').on("click", ".wishlist_btn_add", function () { AddProductToWishList(this); });

    //添加到购物车的数量必须是正整数
    $("input[name^=input_qty]").attr("maxlength", 5).unbind('keydown').bind('keydown',
            function (e) {
                var code = Common.getKeyCode(e);
                switch (code) {
                    case 13:
                        $(this).blur();
                        return true;
                }
                if (((code < 48 || code > 57) && (code < 96 || code > 105) && code != 8 && code != 46 && code != 37 && code != 39) || e.shiftKey) {
                    e.preventDefault();
                    return false;
                }
                return true;
            });
    
    //	Newsletter订阅
    $('#btnSubscribe').bind('click', function () {
        //避免重复提交
        $('#btnSubscribe').unbind("click");

        setTimeout(function () {
            //3秒之后重新绑定事件
            $('#btnSubscribe').bind('click', function () { Subscribe(); });
        }, 3000);

        var subscribeEmail = $('#txtSubscribeEmail').val();
        if ($.trim(subscribeEmail) === "" || !new RegExp(/^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/).test(subscribeEmail)) {
            alert(Message.TipCheckEmail);
            $('#txtSubscribeEmail').focus();
            return false;
        }
        var objData = [{ name: 'subscribeEmail', value: subscribeEmail }];

        $.ajax({
            url: '/Public/Newsletter',
            data: objData,
            dataType: 'json',
            type: 'POST',
            success: function (data) {
                switch (data.Result) {
                    case "success":
                        //弹窗 :Thank you for subscribing to 8Seasons Newsletter!
                        alert(Message.MsgThankSubscibe);
                        break;
                    case "error":
                        alert(Message.TipCheckEmail);
                        $('#txtSubscribeEmail').focus();
                        break;
                    case "exists":
                        //提示 :This email has already subscribed. Please add service@8seasons.com to your address book to ensure delivery.
                        break;
                    default:
                        break;
                }
            },
            error: function (msg) {
                //http://diaosbook.com/Post/2012/8/1/invoking-jsonresult-and-return-error-message-in-aspnet-mvc-ajax
                //异常信息处理 请参考上面链接
                alert(msg);
            }
        });
    });

});
function fnBackorderAddToCart(obj) {
    $('#divBackorderAddToCart').find('.cart_btn_add').data('productid', $(obj).data('productid'));
    $('#divBackorderAddToCart').find('.cart_btn_add').html("Add to Cart");
    $('#divBackorderAddToCart').find('.cart_btn_add').parents(".quantity").find("#input_qty").val('');
    var days = $(obj).data('restocktime');
    var showdays = "";
    if (days > 0)
        showdays = days + " days";
    else
        showdays = "in 15 days";
    $('#divBackorderAddToCart').find('#lblRestockTime').text(showdays);
    $('#divBackorderAddToCart').modal('show');
}


