define('amplify', ['amplify-lib'], function(amplify) {
    "use strict";

    // disasters
    // ----------------------------------------------------------
    amplify.request.define("getDisasterList", "ajax", {
        url: "/Disaster/GetActiveDisasters",
        dataType: "json",
        type: "GET"
    });
    // ----------------------------------------------------------
    // end disasters
});