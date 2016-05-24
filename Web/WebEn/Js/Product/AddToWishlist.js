//客户点击[Add To Wishlist]按钮执行的操作
AddToWishlist = function () {
    var productQty = $('#txtQty').val();
    if ($.trim(productQty) === "" || productQty <= 0) {
        alert("product qty invalid.");
        return false;
    }
    var productId = $('#txtProductId').val();
    if ($.trim(productId) === "") {
        return false;
    }
    var objData = [{ name: 'productId', value: productId }, { name: 'productQty', value: productQty }];

    $.ajax({
        url: '/Wishlist/AddToWishlist',
        data: objData,
        dataType: 'json',
        type: 'POST',
        success: function (data) {
            switch (data.Result) {
                case "success":
                    //变更按钮样式 View Wishlist
                    break;
                case "refresh":
                    window.location.reload();
                    break;
                default:
                    break;
            }
        },
        error: function (msg) {
            //http://diaosbook.com/Post/2012/8/1/invoking-jsonresult-and-return-error-message-in-aspnet-mvc-ajax
            //异常信息处理 请参考上面链接
        }
    });
};

//页面加载完成后立即执行代码
$(document).ready(function () {
    //商品列表（包括List和Gallery）页面Add to Wishlist
    //商品详情页面Add to Wishlist
    //购物车Move to Wishlist/Move all to wishlist
    $('.currency_on a').bind('click', function () { AddToWishlist(); });
});
