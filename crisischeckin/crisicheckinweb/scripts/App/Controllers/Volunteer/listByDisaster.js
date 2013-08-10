require(['jquery'], function($) {
    var controller = function() {
        var init = function() {
            console.log("disasters!  woo!");
        };

        return {
            init: init
        };
    }();

    controller.init();
});