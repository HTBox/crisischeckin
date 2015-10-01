(function ($) {

    function validateEmail(email, regex) {
        if (!regex) {
            regex = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
        }
        return regex.test(email);
    }

    function checkUsernameExists(userName, parent) {
        if (!userName || userName.trim().length < 3) {
            if ($(parent).next().hasClass("feedbackHelper")) $(parent).next().remove();
            return;
        }
        $.ajax({
            url: "/Account/UsernameAvailable",
            type: "POST",
            data: { userName: userName },
            success: function (e) {
                if ($(parent).next().hasClass("feedbackHelper")) $(parent).next().remove();
                var vals = { output: "", class: "" };
                if (e == "False") {
                    vals.class = "failure";
                    vals.output = "Sorry, the username requested is not available.";
                } else {
                    vals.class = "success";
                    vals.output = "The username requested is available.";
                }
                var feedBack = $('<div class=\"feedbackHelper ' + vals.class + '\">' + vals.output + '</div>');
                $(parent).after(feedBack);
            }
        });
    }

    function checkPasswordValidity(userName, password, parent) {
        if (!password || password.trim().length < 1) {
            if ($(parent).next().hasClass("feedbackHelper")) $(parent).next().remove();
            return;
        }
        $.ajax({
            url: "/Account/CheckPasswordValidity",
            type: "POST",
            data: { userName: userName, password: password },
            success: function (validationResult) {
                if ($(parent).next().hasClass("feedbackHelper")) $(parent).next().remove();
                var vals = { output: "", class: "" };
                if (validationResult && validationResult.length > 0) {
                    vals.class = "failure";
                    vals.output = validationResult;
                } else {
                    vals.class = "success";
                    vals.output = "The password meets the requirements.";
                }
                var feedBack = $('<div class=\"feedbackHelper ' + vals.class + '\">' + vals.output + '</div>');
                $(parent).after(feedBack);
            }
        });
    }

    $.fn.delay = function (options) {
        var timer;
        var delayImpl = function (eventObj) {
            if (timer != null) {
                clearTimeout(timer);
            }
            var newFn = function () {
                options.fn(eventObj);
            };
            timer = setTimeout(newFn, options.delay);
        }
        return this.each(function () {
            var obj = $(this);
            obj.bind(options.event, function (eventObj) {
                delayImpl(eventObj);
            });
        });
    };

    function requiredMessage(element, fieldType) {
        element.after('<div class=\"feedbackHelper failure\">' + fieldType + ' is required.</div>');
    }

    $(document).ready(function () {
        $("#txt_userName").delay({
            delay: 300,
            event: 'keyup',
            fn: function () {
                checkUsernameExists($("#txt_userName").val(), $("#txt_userName"));
            }
        });
        $("#txt_password").delay({
            delay: 300,
            event: 'keyup',
            fn: function () {
                checkPasswordValidity($("#txt_userName").val(), $("#txt_password").val(), $("#txt_password"));
            }
        });

        $("#Email").focusout(function () {
            var ele = $(this);
            if (ele.next().hasClass("feedbackHelper"))
                ele.next().remove();

            if (ele.val().length === 0) {
                requiredMessage(ele, "Email Address");
            } else {
                var regex = ele.attr("data-val-regex-pattern");
                if (regex) {
                    regex = new RegExp(regex);
                }

                if (!validateEmail(ele.val(), regex)) {
                    ele.after('<div class=\"feedbackHelper failure\">This is not a valid email address</div>');
                }
            }
        });
    });
})(jQuery);