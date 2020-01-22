$(document).ready(function (e) {
    //image fancybox

    $('.fancybox').fancybox();


    /***********************************BASKET***************************************/
    /**
     * Dodawanie do koszyka  submit
     * */
    $('.addToCart').click(function (e) {
        e.preventDefault();
        $this = $(this);
        var productId = $this.data("productid");

        $("span.loader").addClass("ib");
        //set new va,lue to cart
        var url = "/cart/AddToCartPartial";

        //get cart
        $.get(url, { id: productId }, function (data) {
            $(".ajaxCart").html(data);
        }).done(function () {
            $("span.loader").removeClass("ib");
            $("span.ajMsg").addClass("id");
            setTimeout(function () {
                $("span.ajMsg").fadeOut("fast");
                $("span.ajMsg").removeClass("ib");
            }, 2000);

        });
    })

    ///////////////////INC
    $(".ChangeQuantity").click(function (e) {
        e.preventDefault;
        $this = $(this);
        //product Id
        var ProductId = $this.data("id");
        //increment 1 dec 2 remove 3
        var method = $this.data("me");

        var url = "/cart/ChangeQuantity";
        //method getJSON
        $.getJSON(url, { productid: ProductId, me: method }, function (data) {
            //find td element with class quanty@(item.ProductId)
            var qty = data.qty; if (qty == null || qty == 0) {
                //action remove product
                removeItem(ProductId);
            } $(".quanty" + ProductId).html(qty);
            //price
            var price = data.qty * data.price;
            var PriceHtml = price.toFixed(2) + "zł";
            console.log(data);
            $(".total" + ProductId).html(PriceHtml);

            //change BasketPaartialView QuantityValue
            var oldCartQuantity = parseFloat($("span.CartQuantity").text());
            //set new val in grandTotalVal
            //set totalPrice
            $(".grandTotalVal").text(data.total + "zł");
            //set totalQuantity
            $(".CartQuantity").text(data.totalQuantity);
        });
    });


    $(".RemoveProduct").click(function (e) {
        e.preventDefault;
        $this = $(this);
        var ProductId = $this.data("id");
        removeItem(ProductId);
    });


});

/**
 * 
 * @param {any} ProductId
 */
function removeItem(ProductId) {
    $.getJSON("/cart/RemoveItem", { productId: ProductId }, function (data) {
        setTimeout(function () {
            $(".cart" + ProductId).fadeOut("fast").remove();
        }, 200);
        console.log(data);
        if (data.TotalItems == 0) {
            location.reload();
            return true;
        }
    });
    return false;
}