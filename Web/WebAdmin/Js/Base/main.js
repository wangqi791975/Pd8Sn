$(function () {
    //  营销关键词数
    $('.tree li:has(ul)').addClass('parent_li').find(' > span').attr('title', 'Collapse this branch');
    $('.tree li.parent_li > span').on('click', function (e) {
        var children = $(this).parent('li.parent_li').find(' > ul > li');
        if (children.is(":visible")) {
            children.hide('fast');
            $(this).attr('title', 'Expand this branch').find(' > i').addClass('icon-plus-sign').removeClass('icon-minus-sign');
        } else {
            children.show('fast');
            $(this).attr('title', 'Collapse this branch').find(' > i').addClass('icon-minus-sign').removeClass('icon-plus-sign');
        }
        e.stopPropagation();
    });

    //  全选反选
    $('body').on("click", "[data-toggle=checkall]", function (e) {
        var id = $(this).attr("id");
        $("[id^=" + id + "_]").attr("checked", this.checked);
    });
    $('body').on("click", "[data-toggle=uncheckall]", function (e) {
        var id = $(this).attr("id");
        $("[id^=" + id + "_]").attr("checked", !this.checked);
    });
    $('body').on("click", "[data-toggle=checkthis]", function (e) {
        var id = $(this).attr("id").split("_");
        id.pop();
        var idAll = id.join("_");
        var flag = true;
        $("[id^=" + idAll + "_]").each(function() {
            if (!this.checked) flag = false;
        });
        $("#" + idAll).attr("checked", flag);
    });

    //  新增N行
    $('body').on("click", ".add_tr", function (e) {
        e.preventDefault();
        var $this = $(this);
        var $target = $($this.data("target"));
        var loop = $this.data("length");
        if (typeof (loop) == "undefined" || loop <= 0) loop = 1;
        for (var n = 1; n <= loop; n++) {
            var tr = $target.find("tr").last().clone(true);
            $(tr).find("input").val("");
            $target.append(tr);
        }
    });
});