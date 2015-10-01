(function ($) {

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

    function validateEmail(email, regex) {
        if (!regex) {
            regex = new RegExp(/^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i);
        }
        return regex.test(email);
    }

    function requiredMessage(element, fieldType) {
        element.after('<div class=\"feedbackHelper failure\">' + fieldType + ' is required.</div>');
    }

    function clearWarning(element) {
        if (element.next().hasClass("feedbackHelper"))
            element.next().remove();
    }

    function addErrorMessage(element, message) {
        element.after('<div class=\"feedbackHelper failure\">' + message + '</div>');
    }

    function validateRequiredWithMaxLength(element, fieldName) {
        if (element.val().length === 0) {
            requiredMessage(element, fieldName);
        } else {
            var lengthRequirement = element.attr("data-val-length-max");
            if (lengthRequirement) {
                try {
                    lengthRequirement = parseInt(lengthRequirement);
                    if (element.val().length > lengthRequirement) {
                        var message = element.attr("data-val-length");
                        if (!message)
                            message = fieldName + " length is not valid.";
                        addErrorMessage(element, message);
                    }
                } catch (e) {

                }
            }
        }
    }

    $(document).ready(function () {
        $("#txt_userName").delay({
            delay: 300,
            event: 'keyup',
            fn: function () {
                if ($("#txt_userName").val().length > 0) {
                    checkUsernameExists($("#txt_userName").val(), $("#txt_userName"));
                }
            }
        });

        $("#txt_password").delay({
            delay: 300,
            event: 'keyup',
            fn: function () {
                if ($("#txt_password").val().length > 0) {
                    checkPasswordValidity($("#txt_userName").val(), $("#txt_password").val(), $("#txt_password"));
                }
            }
        });

        $("#Email").focusout(function () {
            var element = $(this);
            clearWarning(element);

            if (element.val().length === 0) {
                requiredMessage(element, "Email Address");
            } else {
                var regex = element.attr("data-val-regex-pattern"); //follow pattern set in code
                if (regex) {
                    regex = new RegExp(regex);
                }
                if (!validateEmail(element.val(), regex)) {
                    var message = element.attr("data-val-regex");
                    if (!message)
                        message = "This is not a valid email address.";
                    addErrorMessage(element, message);
                }
            }
        });

        $("#FirstName").focusout(function () {
            var element = $(this);
            clearWarning(element);
            validateRequiredWithMaxLength(element, "First Name");
        });

        $("#LastName").focusout(function () {
            var element = $(this);
            clearWarning(element);
            validateRequiredWithMaxLength(element, "Last Name");
        });

        $("#PhoneNumber").focusout(function () {
            var element = $(this);
            clearWarning(element);
            validateRequiredWithMaxLength(element, "Phone Number");
        });

        $("#txt_userName").focusout(function () {
            var element = $(this);
            clearWarning(element);
            if (element.val().length === 0) {
                requiredMessage(element, "User Name");
            }
        });

        $("#txt_password").focusout(function () {
            var element = $(this);
            clearWarning(element);
            if (element.val().length === 0) {
                requiredMessage(element, "Password");
            }
        });
    });


})(jQuery);