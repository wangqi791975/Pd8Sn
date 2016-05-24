/*!
 * pStrength jQuery Plugin v1.0.6
 * http://accountspassword.com/pstrength-jquery-plugin
 *
 * Created by AccountsPassword.com
 * Released under the GPL General Public License (Feel free to copy, modify or redistribute this plugin.)
 *
 * Modified by LvXiaoyong
 */

(function ($) {
    var numbersArray = new Array(),
        upperLettersArray = new Array(),
        lowerLettersArray = new Array(),
        specialCharsArray = new Array(),
        settings,
        methods = {
            init: function (options, callbacks) {
                settings = $.extend({
                    'bind': 'keyup change',
                    'onPasswordStrengthChanged': function (passwordStrength, percentage) { }
                }, options);

                for (var i = 48; i < 58; i++)
                    numbersArray.push(i);
                for (i = 65; i < 91; i++)
                    upperLettersArray.push(i);
                for (i = 97; i < 123; i++)
                    lowerLettersArray.push(i);
                for (i = 32; i < 48; i++)
                    specialCharsArray.push(i);
                for (i = 58; i < 65; i++)
                    specialCharsArray.push(i);
                for (i = 91; i < 97; i++)
                    specialCharsArray.push(i);
                for (i = 123; i < 127; i++)
                    specialCharsArray.push(i);

                return this.each($.proxy(function (idx, pStrengthElement) {
                    calculatePasswordStrength.call(pStrengthElement);

                    $(pStrengthElement).bind(settings.bind, function () {
                        calculatePasswordStrength.call(pStrengthElement);
                    });
                }, this));
            }
        };

    var ord = function (string) {
        var str = string + '',
            code = str.charCodeAt(0);
        if (0xD800 <= code && code <= 0xDBFF) {
            var hi = code;
            if (str.length === 1) {
                return code;
            }
            var low = str.charCodeAt(1);
            return ((hi - 0xD800) * 0x400) + (low - 0xDC00) + 0x10000;
        }

        if (0xDC00 <= code && code <= 0xDFFF) {
            return code;
        }
        return code;
    }

    var calculatePasswordStrength = function () {
        var passwordStrength = 0,
            numbersFound = 0,
            upperLettersFound = 0,
            lowerLettersFound = 0,
            specialCharsFound = 0,
            text = $(this).val();

        passwordStrength += 2 * Math.floor(text.length / 8);

        for (var i = 0; i < text.length; i++) {
            if ($.inArray(ord(text.charAt(i)), numbersArray) != -1 && numbersFound < 2) {
                passwordStrength++;
                numbersFound++;
                continue;
            }
            if ($.inArray(ord(text.charAt(i)), upperLettersArray) != -1 && upperLettersFound < 2) {
                passwordStrength++;
                upperLettersFound++;
                continue;
            }
            if ($.inArray(ord(text.charAt(i)), lowerLettersArray) != -1 && lowerLettersFound < 2) {
                passwordStrength++;
                lowerLettersFound++;
                continue;
            }
            if ($.inArray(ord(text.charAt(i)), specialCharsArray) != -1 && specialCharsFound < 2) {
                passwordStrength++;
                specialCharsFound++;
                continue;
            }
        }

        behaviour.call($(this), passwordStrength);

        return passwordStrength;
    }

    var behaviour = function (passwordStrength) {
        var strengthPercentage = Math.ceil(passwordStrength * 100 / 12);
        strengthPercentage = strengthPercentage > 100 ? 100 : strengthPercentage;
        settings.onPasswordStrengthChanged.call($(this), passwordStrength, strengthPercentage);
    }

    $.fn.pStrength = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exists on jQuery.pStrength');
        }
    };
})(jQuery);