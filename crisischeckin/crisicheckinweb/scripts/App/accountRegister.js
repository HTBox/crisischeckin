(function ($) {
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
  });

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
})(jQuery);