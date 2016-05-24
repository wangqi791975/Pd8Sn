$(function () {
    // wishlist批量移除
    $('body').on("click", "#TopWishListRemove,#TopHisWishListRemove,#BottomWishListRemove,#BottomHisWishListRemove", function () {
        var jqForm = $(this).parents('form').filter(':first');
        if (jqForm.find("input[type=checkbox][name=ckb]:checked").length < 1) {
            DivOs.showErrorModal(Message.TipRemoveWishList);
        }
        else {
            DivOs.showConfirmModal(Message.TipConfirmRemoveWishList, g, this);
        }
    });
    //wishlist批量修改
    $('body').on("click", "#TopWishListConfirm,#BottomWishListConfirm,#TopHisWishListConfirm,#BottomHisWishListConfirm", function () {
        var jqForm = $(this).parents('form').filter(':first');
        //if (jqForm.find("input[type=checkbox][name=ckb]:checked").length < 1) {
        //    DivOs.showErrorModal(Message.TipConfirmClassification);
        //}
        if(jqForm.find("input[type=radio]:checked").length < 1)
        {
            DivOs.showErrorModal(Message.TipConfirmClassification);
        }
       
        else {
            var ckbs =jqForm.find("input:radio:checked");
            var items = "";
            ckbs.each(function (i, item) {
                var prodid = $(item).attr("name").replace('a_',"");
                items = items + prodid + ",";
            });


            jqForm.ajaxSubmit({
                url: "/WishList/SetMyWishList",
                data: {ckb: items },
                success: function (responseText) {
                    $("#wishlistsuccess").modal("show");
                    setTimeout(function () {
                        $("#wishlistsuccess").modal("hide");
                    }, 1500);

                }
            });
        }
    });
    
    //wishlist批量ToCart
    $('body').on("click", "#TopWishListToCart,#BottomWishListToCart,#TopHisWishListToCart,#BottomHisWishListToCart", function () {
        var jqForm = $(this).parents('form').filter(':first');
        if (jqForm.find("input[type=checkbox][name=ckb]:checked").length < 1) {
            DivOs.showErrorModal(Message.TipRemoveWishList);
        }
        else {
            var ckbs = jqForm.find("input[type=checkbox][name=ckb]:checked");
            var items = "";
            ckbs.each(function (i, item) {
                var prodid = $(item).val();
                var qty = $(item).parents('table').find('[name=input_qty_' + prodid + ']').val();
                if (qty.length > 0) {
                    items = items + prodid + "," + qty + "|";
                }
            });
            items = items.substring(0, items.length - 1);
            $.post('/ShoppingCart/BatchAddToCart', { 'items': items }, function (data) {
                if (data.result === "success") {
                    location.href = data.shoppingcarturl;
                }
            });
        }
    });


    // 全选，反选
    $('body').on("click", "input[type=checkbox][name=ck_wishlist_all]", function () {
        var jqForm = $(this).parents('form').filter(':first');
        if ($(this).attr("checked")) {
            jqForm.find("input[type=checkbox][name=ckb]").attr("checked", true);
            jqForm.find("input[type=checkbox][name=ck_wishlist_all]").attr("checked", true);
        }
        else {
            jqForm.find("input[type=checkbox][name=ckb]").attr("checked", false);
            jqForm.find("input[type=checkbox][name=ck_wishlist_all]").attr("checked", false);
        }
    });

    //$('body').on("click", ".pop_select_cont li", function () {  
    //    var jqForm = $(this).parents('form').filter(':first');
    //    var $target = $(jqForm.data("target"));
    //    jqForm.ajaxSubmit({
    //        success: function (responseText) {
    //            $target.html(responseText);
    //        }
    //    });
    //});

    $('body').on("click", ".btn_orange.btn_p30", function () {
        var jqForm = $(this).parents('form').filter(':first');
        var $target = $(jqForm.data("target"));
        jqForm.ajaxSubmit({
            success: function (responseText) {
                $target.html(responseText);
            }
        });
    });

    $('body').on("click", ".hm", function () {
        DivOs.showConfirmModal(Message.MsgConfirmDelItem, WishlistRemoveOne, this);
    });

});


function WishlistRemoveOne(obj) {
    var jqForm = $(obj).parents('form').filter(':first');
    var $target = $(jqForm.data("target"));
    jqForm.ajaxSubmit({
        url: "/WishList/RemoveMyWishListOne?pid=" + $(obj).data("productid") + "&r=" + $(obj).data("removed"),
        success: function (responseText) {
            $target.html(responseText);
        }
    });
}

function g(obj)
{
    var jqForm = $(obj).parents('form').filter(':first');
    var $target = $(jqForm.data("target"));
    jqForm.ajaxSubmit({
        url: "/WishList/RemoveMyWishList",
        success: function (responseText) {
            $target.html(responseText);
        }
    });
}

function historyparm() {
  var c=$("#his_wishlist_category").val();
  var t = $("#his_wishlist_type").val();
  var s = $("#his_wishlist_sort").val();
  return { wishlist_category: c, wishlist_type: t, wishlist_sort: s };
}

function recentlyparm() {
    var c = $("#wishlist_category").val();
    var t = $("#wishlist_type").val();
    var s = $("#wishlist_sort").val();
    return { wishlist_category: c, wishlist_type: t, wishlist_sort: s };

}