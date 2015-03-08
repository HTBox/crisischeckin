(function ($) {
  $(document).ready(function () {
    $("#txt_password").delay({
        delay: 300,
        event: 'keyup',
        fn: function () {
            checkPasswordValidity($("#txt_password").val(), $("#txt_password"));
        }
    });
  });

  function checkPasswordValidity(password, parent) {
    if (!password || password.trim().length < 1) {
      if ($(parent).next().hasClass("feedbackHelper")) $(parent).next().remove();
      return;
    }
    $.ajax({
      url: "/Account/CheckPasswordValidity",
      type: "POST",
      data: { password: password },
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
})(jQuery);