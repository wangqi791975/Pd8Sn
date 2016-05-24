var Payment = {
    init: function () {
        this._paymentPanel = $("div.pay_method_cont");
        this._paymentRadio = $("input.method_payment");

        //  切换支付方式
        this._paymentRadio.click(function () {
            $(".method_info_cont").hide();
            var id = $(this).attr("id");
            $(".method_info_cont#cont_" + id).show();

            if (id === "visa") {
                Payment.GC.loadGc();
            }
        });

        //  norton图标
        $(".norton").html('<script type="text/javascript" src="https://seal.websecurity.norton.com/getseal?host_name=www.8seasons.com&amp;size=S&amp;use_flash=NO&amp;use_transparent=NO&amp;lang=en"></script>');

        //  是否是订单详情页的支付
        this.fromOrderDetail = $("#hidFromOrderDetail").val() == "1";
        if (this.fromOrderDetail) {
            //  view detail
            $("#paymentViewDetail").click(function () {
                $("#tabPaymentViewDetail").show();
                $(this).parent().remove();
            });

            //  日历控件
            $(".datepicker").datepicker($.datepicker.regional["en"]);

            //  隐藏提示语
            $(".hide").each(function () {
                $(this).hide();
            });

            //  公用的uploadify配置
            this.uploadifyOption = {
                'uploader': '/Payment/UploadPaymentReceipt',
                'swf': '/Js/uploadify/uploadify.swf',
                'width': 300,
                'height': 30,
                'buttonText': "Browse",
                'fileTypeExts': "*.gif;*.jpg;*.jpeg;*.png;*.bmp;*.pdf;",
                'fileTypeDesc': Message.TipPleaseSelectFiles.format("jpg jpeg gif png bmp pdf"),
                'fileSizeLimit': "2MB",
                'uploadLimit': 1,
                'auto': true,
                'multi': false,
                'removeCompleted': false
            };
        }

        //  初始化各支付方式
        Payment.Order.init();
        Payment.Cash.init();
        Payment.Hsbc.init();
        Payment.China.init();
        Payment.WesternUnion.init();
        Payment.MoneyGram.init();
        Payment.PayPal.init();
        Payment.GC.init();
        Payment.Ocean.init();

        this.load();
    },
    //  加载数据、支付版面
    load: function () {
        var flag = Payment.Cash.refresh();
        if (!flag || !Payment.Cash.isPayAll()) {
            Payment.showPayment(false);
        } else {
            Payment.hidePayment(false);
        }
    },
    //  变更币种
    changeCurrency: function (obj, target) {
        var $this = $(obj);
        var value = $this.data("value");
        var text = $this.text();
        var $par = $this.parents(".select_cont").filter(":first");
        $this.addClass("active").siblings().removeClass("active");
        $par.children(".select_cont_span").text(text);
        $par.children("input:hidden").val(value);

        var $target = $(target);
        if (value == "-1") {
            $target.show();
        } else {
            $target.hide();
        }
    },
    //  Checkout/Payment 页面线下支付
    changePaymentType: function (btn) {
        var urlChangePaymentType = "/Payment/ChangePaymentType";
        var params = {
            orderNo: Payment.Order.orderNo,
            paymentType: $(btn).data("paymenttype")
        };
        $.post(urlChangePaymentType, params, function (jsonData) {
            if (jsonData.Message != null && jsonData.Message != "") {
                DivOs.showErrorModal(DivOs.getMessage(jsonData.Message));
                return false;
            } else {
                window.location.href = "/Checkout/Succeed";
                return false;
            }
        });
    },
    //  Order/OrderDetailPayment 页面线下支付
    _payOrder: function (sender, url, params) {
        params.orderNo = Payment.Order.orderNo;
        params.isUseCashPartPay = Payment.Cash.isUse;

        $(sender).attr("disabled", "disabled");
        $.post(url, params, function (jsonData) {
            if (typeof (jsonData.Succeed) != "undefined" && jsonData.Succeed) {
                window.location.href = "/Order/OrderDetail?orderno=" + params.orderNo;
            } else {
                DivOs.showErrorModal(DivOs.getMessage(jsonData.Message));
            }
        });
        $(sender).removeAttr("disabled");
    },
    //  获取当前选中的支付方式按钮
    getSelectedItem: function () {
        return this._paymentRadio.filter(":checked");
    },
    //  显示支付方式版面
    showPayment: function (containsCash) {
        this._paymentPanel.show();
        Payment.refreshSelectedPayment();
    },
    //  隐藏支付方式版面
    hidePayment: function (containsCash) {
        if (containsCash == null) containsCash = true;
        if (!containsCash) {
            this._paymentPanel.hide();
        }
    },
    //  显示选中的支付方式
    refreshSelectedPayment: function () {
        var obj = this.getSelectedItem();
        if (obj.length > 0) obj.click();
    }
};

//  订单信息
Payment.Order = {
    orderNo: "",
    init: function () {
        this.orderNo = $("#hidOrderNo").val();
    }
};

//  Cash支付
Payment.Cash = {
    enabled: false,
    isUse: false,
    init: function () {
        this.chkUseCash = $("#chkUseCash");
        this.btnSubmit = $("#cashSubmit");
        this.enabled = this.chkUseCash.length > 0;

        //  没有cash，不执行操作
        if (!this.enabled || this.getBalanceUsd() <= 0) {
            this.hidePayment();
            return;
        }

        //  初始选中checkbox、显示cash支付按钮
        this.showPayment(true);

        //  cash全额支付
        this.btnSubmit.click(function () {
            Payment.Cash.pay();
        });

        //  点击use的checkbox事件
        this.chkUseCash.click(function () {
            if (!$(this).attr("checked")) {
                Payment.Cash.btnSubmit.hide();
                Payment.Cash.isUse = false;
                if (Payment.Cash.isPayAll()) return;
                Payment.showPayment(false);
                Payment.Cash.setNeedToPay(false);
            } else {
                Payment.Cash.isUse = true;
                if (Payment.Cash.isPayAll()) {
                    Payment.Cash.btnSubmit.show();
                    Payment.hidePayment(false);
                } else {
                    Payment.refreshSelectedPayment();
                }
                Payment.Cash.setNeedToPay(true);
            }
        });
    },
    //  显示cash支付按钮
    showPayment: function (useCash) {
        if (!this.enabled) return;
        if (useCash == null) useCash = this.isUse;
        else this.isUse = useCash;
        this.chkUseCash.attr("checked", useCash ? "checked" : "");
        if (useCash) {
            this.btnSubmit.show();
        }
    },
    //  隐藏cash支付按钮
    hidePayment: function () {
        if (!this.enabled) return;
        this.btnSubmit.hide();
    },
    //  刷新页面
    refresh: function () {
        if (!this.enabled) return false;
        this.showPayment();
        return true;
    },
    //  重置needtopay数值
    setNeedToPay: function (checked) {
        if (!this.enabled) return;
        if (this.btnSubmit.length > 0) {
            var balanceLeft = !checked ? this.getBalance() : Payment.TryGetValue.toAmountValue(this.getBalance() - this.getOrderAmt());
            $("#spanBlanceLeft").text(balanceLeft);
        }
        var needToPay = !checked ? this.getOrderAmt() : (this.getOrderAmt() - this.getBalance() < 0 ? "0.00" : Payment.TryGetValue.toAmountValue(this.getOrderAmt() - this.getBalance()));
        $("#spanNeedToPay").text(needToPay);
    },
    getBalanceUsd: function () {
        if (!this.enabled) return 0;
        return Payment.TryGetValue.toAmountValue($("#hidCashBalanceUsd").val());
    },
    getBalance: function () {
        if (!this.enabled) return 0;
        return Payment.TryGetValue.toAmountValue($("#hidCashBalance").val());
    },
    getOrderAmtUsd: function () {
        if (!this.enabled) return 0;
        return Payment.TryGetValue.toAmountValue($("#hidTotalOrderAmtUsd").val());
    },
    getOrderAmt: function () {
        if (!this.enabled) return 0;
        return Payment.TryGetValue.toAmountValue($("#hidTotalOrderAmt").val());
    },
    //  是否全额支付
    isPayAll: function () {
        if (!this.enabled || !this.isUse) return false;
        return this.btnSubmit.length > 0;
    },
    //  cash支付
    pay: function () {
        if (!Payment.Cash.isPayAll()) return false;
        var params = {
            orderNo: Payment.Order.orderNo,
            isFullPay: true
        };
        this.btnSubmit.attr("disabled", "disabled");
        $.post("/Payment/PayOrderByCash", params, function (jsonData) {
            if (typeof (jsonData.Succeed) != "undefined" && jsonData.Succeed) {
                if (Payment.fromOrderDetail) {
                    window.location.href = "/Order/OrderDetail?orderno=" + params.orderNo;
                } else {
                    window.location.href = "/Checkout/Succeed";
                }
            } else {
                DivOs.showErrorModal(DivOs.getMessage(jsonData.Message));
            }
        });
        this.btnSubmit.removeAttr("disabled");
    }
};

function CurrencyInfo(code, sign) {
    this.Code = code || "";
    this.Sign = sign || "";
}
function PayAmountInfo(amount, currency) {
    this.Amount = amount || 0;
    this.Currency = currency || "";
}
function MaxLimitAmountResult(isOver, maxAmount) {
    this.IsOver = isOver || false;
    this.MaxAmount = maxAmount || 0;
}

//  公共获取输入框值的方法
Payment.TryGetValue = {
    toAmountValue: function (s) {
        return parseFloat(parseFloat(s).toFixed(2));
    },
    tryGetCurrencyCode: function (id) {
        $(id + "Err").length > 0 && $(id + "Err").hide();
        var obj = $(id);
        var val = obj.val();
        if (val != "" && val != "-1") {
            return { currencyCode: val, isStandard: true };
        } else {
            val = $(id + "Other").val();
            if ($.trim(val) == "") {
                $(id + "Err").show();
                return false;
            }
            return { currencyCode: val, isStandard: false };
        }
    },
    tryGetAmount: function (id) {
        $(id + "Err").length > 0 && $(id + "Err").hide();
        $(id + "Err1").length > 0 && $(id + "Err1").hide();
        var obj = $(id);
        var s = $.trim(obj.val());
        if (s.length == 0) {
            $(id + "Err").length > 0 && $(id + "Err").show();
            obj.focus();
            return false;
        }
        if (!DataType.isFloatNum(s) || s <= 0) {
            $(id + "Err1").length > 0 && $(id + "Err1").show();
            obj.focus();
            return false;
        }
        s = parseFloat(s).toFixed(2);
        obj.val(s);
        return parseFloat(s);
    },
    tryGetControlNo: function (id, len) {
        $(id + "Err").length > 0 && $(id + "Err").hide();
        $(id + "Err1").length > 0 && $(id + "Err1").hide();
        $(id + "Err2").length > 0 && $(id + "Err2").hide();
        var obj = $(id);
        var s = $.trim(obj.val());
        if (s.length == 0 && $(id + "Err").length > 0) {
            $(id + "Err").show();
            return false;
        }
        if (!DataType.isNumber(s) && $(id + "Err1").length > 0) {
            $(id + "Err1").show();
            return false;
        }
        if (s.length != len && $(id + "Err2").length > 0) {
            $(id + "Err2").show();
            return false;
        }
        return s;
    },
    tryGetFullName: function (id) {
        $(id + "Err").length > 0 && $(id + "Err").hide();
        $(id + "Err1").length > 0 && $(id + "Err1").hide();
        $(id + "Err2").length > 0 && $(id + "Err2").hide();
        var obj = $(id);
        var s = $.trim(obj.val());
        if (s.length == 0 && $(id + "Err").length > 0) {
            $(id + "Err").show();
            return false;
        }
        if (s.length < 2 && $(id + "Err1").length > 0) {
            $(id + "Err1").show();
            return false;
        }
        if (! /[aeiouAEIOU]/.test(s) && $(id + "Err2").length > 0) {
            $(id + "Err2").show();
            return false;
        }
        return s;
    },
    cheInputLength: function (obj, len) {
        var $t = $(obj);
        if ($t.val().length > len) {
            $t.val($t.val().substr(0, len));
        }
    },
    tryGetValue: function (id) {
        $(id + "Err").length > 0 && $(id + "Err").hide();
        var obj = $(id);
        var s = $.trim(obj.val());
        if (s.length == 0 && $(id + "Err").length > 0) {
            $(id + "Err").show();
            return false;
        }
        return s;
    }
}

//  HSBC支付
Payment.Hsbc = {
    init: function () {
        if ($("#cont_hsbc").length == 0) return;
        this.btnSubmit = "#hsbcSubmit";

        if (Payment.fromOrderDetail) {
            $("#divHsbcCurrencyCode .pop_select_cont li.list_item").off("click").on("click", function (e) {
                e.preventDefault();
                Payment.changeCurrency(this, "#divHsbcCurrencyCodeOther");
            });
            this.uploadifyOption = {
                'onUploadSuccess': function (file, data, response) {
                    $("input#hsbcReceipt").val(data);
                    var cancel = $("#" + file.id + " .cancel a");
                    if (cancel) {
                        cancel.on("click", function () {
                            $("input#hsbcReceipt").val("");
                            var uploadLimit = $('#hsbcReceipt_ud').uploadify('settings', 'uploadLimit');
                            $("#hsbcReceipt_ud").uploadify('settings', 'uploadLimit', ++uploadLimit);
                        });
                    }
                }
            };
            $("#hsbcReceipt_ud").uploadify($.extend(Payment.uploadifyOption, this.uploadifyOption));
        }
        $(this.btnSubmit).click(function () {
            Payment.Hsbc.pay();
        });
    },
    pay: function () {
        if (Payment.fromOrderDetail) {
            Payment.Hsbc.payOrder();
        } else {
            Payment.changePaymentType(this.btnSubmit);
        }
    },
    payOrder: function () {
        var params = {
            currencyCode: "",
            isStandard: true,
            amount: "",
            paymentDate: "",
            paymentReceipt: ""
        };
        var currencyCode = "";
        var flag = true;
        if (!(currencyCode = Payment.TryGetValue.tryGetCurrencyCode("#hsbcCurrencyCode"))) {
            flag = false;
        }
        if (!(params.amount = Payment.TryGetValue.tryGetAmount("#hsbcAmount"))) {
            flag = false;
        }
        if (!(params.paymentDate = Payment.TryGetValue.tryGetValue("#hsbcDate"))) {
            flag = false;
        }
        params.paymentReceipt = Payment.TryGetValue.tryGetValue("#hsbcReceipt");
        if (!flag) return false;
        params.currencyCode = currencyCode.currencyCode;
        params.isStandard = currencyCode.isStandard;
        Payment._payOrder(this.btnSubmit, "/Payment/PayOrderByHsbc", params);
        return true;
    }
};

//  BankOfChina支付
Payment.China = {
    init: function () {
        if ($("#cont_china").length == 0) return;
        this.btnSubmit = "#chinaSubmit";

        if (Payment.fromOrderDetail) {
            $("#divChinaCurrencyCode .pop_select_cont li.list_item").off("click").on("click", function (e) {
                e.preventDefault();
                Payment.changeCurrency(this, "#divChinaCurrencyCodeOther");
            });
            this.uploadifyOption = {
                'onUploadSuccess': function (file, data, response) {
                    $("input#chinaReceipt").val(data);
                    var cancel = $("#" + file.id + " .cancel a");
                    if (cancel) {
                        cancel.on("click", function () {
                            $("input#chinaReceipt").val("");
                            var uploadLimit = $('#chinaReceipt_ud').uploadify('settings', 'uploadLimit');
                            $("#chinaReceipt_ud").uploadify('settings', 'uploadLimit', ++uploadLimit);
                        });
                    }
                }
            };
            $("#chinaReceipt_ud").uploadify($.extend(Payment.uploadifyOption, this.uploadifyOption));
        }
        $(this.btnSubmit).click(function () {
            Payment.China.pay();
        });
    },
    pay: function () {
        if (Payment.fromOrderDetail) {
            Payment.China.payOrder();
        } else {
            Payment.changePaymentType(this.btnSubmit);
        }
    },
    payOrder: function () {
        var params = {
            currencyCode: "",
            isStandard: true,
            amount: "",
            paymentDate: "",
            paymentReceipt: ""
        };
        var currencyCode = "";
        var flag = true;
        if (!(currencyCode = Payment.TryGetValue.tryGetCurrencyCode("#chinaCurrencyCode"))) {
            flag = false;
        }
        if (!(params.amount = Payment.TryGetValue.tryGetAmount("#chinaAmount"))) {
            flag = false;
        }
        if (!(params.paymentDate = Payment.TryGetValue.tryGetValue("#chinaDate"))) {
            flag = false;
        }
        params.paymentReceipt = Payment.TryGetValue.tryGetValue("#chinaReceipt");
        if (!flag) return false;
        params.currencyCode = currencyCode.currencyCode;
        params.isStandard = currencyCode.isStandard;
        Payment._payOrder(this.btnSubmit, "/Payment/PayOrderByBankOfChina", params);
        return true;
    }
};

//  WesternUnion支付
Payment.WesternUnion = {
    init: function () {
        if ($("#cont_union").length == 0) return;
        this.btnSubmit = "#unionSubmit";

        if (Payment.fromOrderDetail) {
            $("#divUnionCurrencyCode .pop_select_cont li.list_item").off("click").on("click", function (e) {
                e.preventDefault();
                Payment.changeCurrency(this, "#divUnionCurrencyCodeOther");
            });
            this.uploadifyOption = {
                'onUploadSuccess': function (file, data, response) {
                    $("input#unionReceipt").val(data);
                    var cancel = $("#" + file.id + " .cancel a");
                    if (cancel) {
                        cancel.on("click", function () {
                            $("input#unionReceipt").val("");
                            var uploadLimit = $('#unionReceipt_ud').uploadify('settings', 'uploadLimit');
                            $("#unionReceipt_ud").uploadify('settings', 'uploadLimit', ++uploadLimit);
                        });
                    }
                }
            };
            $("#unionReceipt_ud").uploadify($.extend(Payment.uploadifyOption, this.uploadifyOption));
        }
        $(this.btnSubmit).click(function () {
            Payment.WesternUnion.pay();
        });
    },
    pay: function () {
        if (Payment.fromOrderDetail) {
            Payment.WesternUnion.payOrder();
        } else {
            Payment.changePaymentType(this.btnSubmit);
        }
    },
    payOrder: function () {
        var params = {
            currencyCode: "",
            isStandard: true,
            amount: "",
            controlNo: "",
            paymentReceipt: ""
        };
        var currencyCode = "";
        var flag = true;
        if (!(currencyCode = Payment.TryGetValue.tryGetCurrencyCode("#unionCurrencyCode"))) {
            flag = false;
        }
        if (!(params.amount = Payment.TryGetValue.tryGetAmount("#unionAmount"))) {
            flag = false;
        }
        if (!(params.controlNo = Payment.TryGetValue.tryGetControlNo("#unionControlNo", 10))) {
            flag = false;
        }
        params.paymentReceipt = Payment.TryGetValue.tryGetValue("#unionReceipt");
        if (!flag) return false;
        params.currencyCode = currencyCode.currencyCode;
        params.isStandard = currencyCode.isStandard;
        Payment._payOrder(this.btnSubmit, "/Payment/PayOrderByWesternUnion", params);
        return true;
    }
};

//  MoneyGram支付
Payment.MoneyGram = {
    init: function () {
        if ($("#cont_mg").length == 0) return;
        this.btnSubmit = "#mgSubmit";

        if (Payment.fromOrderDetail) {
            $("#divMgCurrencyCode .pop_select_cont li.list_item").off("click").on("click", function (e) {
                e.preventDefault();
                Payment.changeCurrency(this, "#divMgCurrencyCodeOther");
            });
            this.uploadifyOption = {
                'onUploadSuccess': function (file, data, response) {
                    $("input#mgReceipt").val(data);
                    var cancel = $("#" + file.id + " .cancel a");
                    if (cancel) {
                        cancel.on("click", function () {
                            $("input#mgReceipt").val("");
                            var uploadLimit = $('#mgReceipt_ud').uploadify('settings', 'uploadLimit');
                            $("#mgReceipt_ud").uploadify('settings', 'uploadLimit', ++uploadLimit);
                        });
                    }
                }
            };
            $("#mgReceipt_ud").uploadify($.extend(Payment.uploadifyOption, this.uploadifyOption));

            $("#mgFullNameOfRemitter").on("keyup keydown paste", function (e) {
                Payment.TryGetValue.cheInputLength(this, 100);
            });
        }
        $(this.btnSubmit).click(function () {
            Payment.MoneyGram.pay();
        });
    },
    pay: function () {
        if (Payment.fromOrderDetail) {
            Payment.MoneyGram.payOrder();
        } else {
            Payment.changePaymentType(this.btnSubmit);
        }
    },
    payOrder: function () {
        var params = {
            fullNameOfRemitter: "",
            countryId: "",
            currencyCode: "",
            isStandard: true,
            amount: "",
            controlNo: "",
            paymentReceipt: ""
        };
        var currencyCode = "";
        var flag = true;
        if (!(params.fullNameOfRemitter = Payment.TryGetValue.tryGetFullName("#mgFullNameOfRemitter"))) {
            flag = false;
        }
        if (!(params.countryId = Payment.TryGetValue.tryGetValue("#mgCountryId"))) {
            flag = false;
        }
        if (!(currencyCode = Payment.TryGetValue.tryGetCurrencyCode("#mgCurrencyCode"))) {
            flag = false;
        }
        if (!(params.amount = Payment.TryGetValue.tryGetAmount("#mgAmount"))) {
            flag = false;
        }
        if (!(params.controlNo = Payment.TryGetValue.tryGetControlNo("#mgControlNo", 8))) {
            flag = false;
        }
        params.paymentReceipt = Payment.TryGetValue.tryGetValue("#mgReceipt");
        if (!flag) return false;
        params.currencyCode = currencyCode.currencyCode;
        params.isStandard = currencyCode.isStandard;
        Payment._payOrder(this.btnSubmit, "/Payment/PayOrderByMoneyGram", params);
        return true;
    }
};

//  Paypal支付
Payment.PayPal = {
    init: function () {
        this.btnSubmit = "#paypalSubmit";
        this.btnExpressSubmit = "#paypalExpressSubmit";
        this.btnCreditSubmit = "#paypalCreditSubmit";

        if ($("#cont_paypal").length == 0 && $(this.btnExpressSubmit).length == 0 && $(this.btnCreditSubmit).length == 0) return;

        $(this.btnSubmit).click(function () {
            Payment.PayPal.pay();
        });

        $(this.btnCreditSubmit).click(function () {
            Payment.PayPal.pay();
        });

        $(this.btnExpressSubmit).click(function () {
            Payment.PayPal.expressCheckOut();
        });
    },
    pay: function () {
        var orderNo = Payment.Order.orderNo;
        var isUseCashPartPay = Payment.Cash.isUse;
        var urlRequestPayOrderByPaypalVertify = "/Payment/RequestPayOrderByPaypalVertify?orderNo=" + orderNo + "&isUseCashPartPay=" + isUseCashPartPay;
        var urlRequestPayOrderByPaypal = "/Payment/RequestPayOrderByPaypal?orderNo=" + orderNo;
        $.post(urlRequestPayOrderByPaypalVertify, {}, function (jsonData) {
            if (jsonData.Message != null && jsonData.Message != "") {
                var msg = Message[jsonData.Message] ? Message[jsonData.Message] : jsonData.Message;
                DivOs.showErrorModal(msg);
                return false;
            } else {
                window.location.href = urlRequestPayOrderByPaypal;
                return false;
            }
        });
    },
    expressCheckOut: function () {
        $(this.btnExpressSubmit).attr("disabled", "disabled");
        var urlDoPaypalExpressCheckoutPayment = "/Payment/DoPaypalExpressCheckoutPayment";
        var params = {
            orderNo: Payment.Order.orderNo,
            isUseCashPartPay: Payment.Cash.isUse
        }
        $.post(urlDoPaypalExpressCheckoutPayment, params, function (jsonData) {
            if (jsonData.Succeed != null && jsonData.Succeed) {
                window.location.href = "/Checkout/Succeed";
            } else if (jsonData.ExtraData != null && jsonData.ExtraData != "") {
                window.location.href = jsonData.ExtraData;
            } else if (jsonData.Message != null && jsonData.Message != "") {
                DivOs.showErrorModal(DivOs.getMessage(jsonData.Message));
            }
        });
        $(this.btnExpressSubmit).attr("disabled", "");
    }
};

//  GC支付
Payment.GC = {
    init: function () {
        if ($("#cont_visa").length == 0) return;

        this._gcCardTypes = $("input[name='gcType'][type='radio']");
        this._gcIframe = $("#iframeGc");
        this._gcDiv = $("#iframeGcDiv");
        this._gcLoading = $("#iframeGcLoading");

        //  点击切换卡种
        this._gcCardTypes.click(function () {
            Payment.GC.loadGc();
        });

        //  刷新页面时候自动加载
        this._gcIframe.load(function () {
            var src = $(this).attr("src");
            if (src == "" || src == "about:blank") return;
            Payment.GC._showLoading(true);
            Payment.GC._gcDiv.show();
        });
    },
    getGlobalCollectType: function () {
        return $("input[name='gcType'][type='radio']:checked").val();
    },
    loadGc: function () {
        Payment.GC._setIframe(null);
        Payment.GC._showLoading();

        var params = {
            orderNo: Payment.Order.orderNo,
            isUseCashPartPay: Payment.Cash.isUse,
            globalCollectType: Payment.GC.getGlobalCollectType()
        };
        $.post("/Payment/LoadGcIframe", params, function (jsonData) {
            if (typeof (jsonData.Succeed) != "undefined" && jsonData.Succeed) {
                Payment.GC._setIframe(jsonData.Data);
            } else if (jsonData.Message != null && jsonData.Message != "") {
                DivOs.showErrorModal(DivOs.getMessage(jsonData.Message));
                Payment.GC._showLoading(true);
            }
        });
    },
    _showLoading: function (isHide) {
        var obj = $(this._gcLoading);
        if (isHide == null) isHide = false;
        if (isHide) {
            obj.hide();
        } else {
            obj.show();
        }
    },
    _setIframe: function (url) {
        if (url == null || url == "") {
            Payment.GC._gcDiv.hide();
            Payment.GC._gcIframe.attr("src", "about:blank");
        } else {
            Payment.GC._gcIframe.attr("src", url);
        }
    }
};

//  OceanPayment支付
Payment.Ocean = {
    init: function () {
        this.btnSubmit = ".btnOceanPayment";
        if ($(this.btnSubmit).length === 0) return;

        $(this.btnSubmit).each(function () {
            $(this).click(function () {
                Payment.Ocean.pay(this);
            });
        });
    },
    pay: function (obj) {
        var params = {
            orderNo: Payment.Order.orderNo,
            isUseCashPartPay: Payment.Cash.isUse,
            oceanPaymentType: $(obj).data("paymenttype")
        }
        $.post("/Payment/RequestPayOrderByOceanPaymentVertify", params, function (jsonData) {
            if (jsonData.Message != null && jsonData.Message != "") {
                var msg = Message[jsonData.Message] ? Message[jsonData.Message] : jsonData.Message;
                DivOs.showErrorModal(msg);
                return false;
            } else {
                window.location.href = "/Payment/RequestPayOrderByOceanPayment?orderNo=" + params.orderNo + "&oceanPaymentType=" + params.oceanPaymentType;
                return false;
            }
        });
    }
};

$(function () {
    Payment.init();
})