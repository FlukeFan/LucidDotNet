
(function () {
    var pjaxOnError = mfoPjax.onError;

    mfoPjax.onError = function (jqXHR, textStatus, errorThrown, context, callback) {
        if (!pjaxOnError(jqXHR, textStatus, errorThrown, context, callback))
        alert("Error: " + textStatus);
    };

    mvcForms.init();
}());
