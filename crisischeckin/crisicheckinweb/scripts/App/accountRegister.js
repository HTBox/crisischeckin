(function ($) {
  $(document).ready(function () {
    $("#txt_userName").delay({
      delay: 300,
      event: 'keyup',
      fn: function () {
        checkUsernameExists($("#txt_userName").val(), $("#txt_userName"));
      }
    });
  });

  function checkUsernameExists(userName, parent) {
    if (userName == "") {
      $(parent).removeClass("success").removeClass("failure");
      if ($(parent).next().hasClass("feedbackHelper")) $(parent).next().remove();
      return;
    }
    $.ajax({
      url: "/Account/UsernameAvailable",
      type: "POST",
      data: { userName: userName },
      success: function (e) {
        $(parent).removeClass("success").removeClass("failure");
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
        $(parent).addClass(vals.class);
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