
var mfoOverlay = {};

(function () {

    mfoOverlay.add = add;
    mfoOverlay.remove = remove;

    function add(fadeTime1, fadeTime2) {

        var zIndex = maxZIndex() + 500;

        var overlay = $('<div class="mfo-overlay"></div>').css({
            'opacity': '0',
            'margin': '0',
            'border': '0',
            'position': 'fixed',
            'top': 0,
            'left': 0,
            'width': '100%',
            'height': '100%',
            'z-index': zIndex
        })
            .fadeTo(fadeTime1 || 100, 0)
            .fadeTo(fadeTime2 || 1500, 0.2)
            .appendTo('body');

        var undisableActions = [];

        $('input, select, textarea, button, a, area').each(function () {

            var el = $(this);
            var disabled = el.prop('disabled');
            var tabIndex = el.prop('tabIndex');

            undisableActions.push(function () {

                if (disabled !== undefined) {
                    el.prop('disabled', disabled);
                }

                if (tabIndex) {
                    el.prop('tabindex', tabIndex);
                } else {
                    el.removeProp('tabindex');
                }

            });

            if (disabled === false) {
                el.prop('disabled', true);
            }

            el.prop('tabindex', -1);

        });

        overlay.undisableActions = undisableActions;
        return overlay;

    }

    function remove(overlay) {

        for (var i = 0; i < overlay.undisableActions.length; i++) {
            overlay.undisableActions[i]();
        }

        overlay.stop(true)
            .fadeTo(50, 0, function () {
                overlay.remove();
            });

    }

    function maxZIndex() {

        // https://stackoverflow.com/a/1118216/357728
        return Math.max.apply(null,
            $.map($('body *'), function (e) {
                if ($(e).css('position') !== 'static') {
                    return parseInt($(e).css('z-index')) || 1;
                }
            }).concat(500));

    }

}());


var mfoPjax = {};

(function () {

    mfoPjax.init = init;
    mfoPjax.onError = onError;
    mfoPjax.load = load;
    mfoPjax.reload = reload;

    var lastButton = null;

    function init() {

        $(document).on('click', '[data-pjax] a', onNavigate);
        $(document).on('submit', '[data-pjax] form', onSubmitForm);
        $(window).on('popstate', onPopState);
        $(document).on('click', 'input[type=submit], button', onSubmitClicked);

    }

    function onSubmitClicked(e) {

        if (lastButton) {
            lastButton.removeAttr('clicked');
        }

        lastButton = $(e.currentTarget);
        lastButton.attr('clicked', 'true');

    }

    function onNavigate(e) {

        var anchor = $(e.currentTarget);

        if (anchor.attr('data-nopjax')) {
            return;
        }

        var container = anchor.closest('[data-pjax]');
        var url = anchor.attr('href');

        if (!url || url.substr(0, 1) === '#') {
            return;
        }

        if (!container.attr('id')) {
            container.attr('id', 'pjax_' + new Date().valueOf());
        }

        if (url.toLowerCase().substr(0, 7) === 'http://' || url.toLowerCase().substr(0, 8) === 'https://') {
            return; // we don't pjax external links
        }

        var context = {
            anchor: anchor,
            url: url,
            verb: 'GET',
            container: container
        };

        container.trigger("pjax:navigate", context);

        if (context.cancel === true) {
            return;
        }

        if (history.state === null || history.state.containerId !== container.attr('id')) {
            var state = history.state || {};
            state.url = location.pathname + location.search + location.hash;
            state.containerId = container.attr('id');
            history.replaceState(state, null, '');
        }

        mfoPjax.load(context, navigateSuccess);
        e.preventDefault();

    }

    function onSubmitForm(e) {

        var form = $(e.currentTarget);

        var clickedButton = form.find('*[clicked=true]');

        if (clickedButton.attr('data-nopjax')) {
            return;
        }

        var container = form.closest('[data-pjax]');
        var data = form.serialize();

        if (clickedButton.length > 0) {
            var name = clickedButton.attr('name');
            if (name) {
                data += '&' + name + '=' + (clickedButton.attr('value') || "");
            }
        }

        var context = {
            form: form,
            url: form.attr('action') || location.pathname + location.search + location.hash,
            data: data,
            verb: form.attr('method') || 'POST',
            container: container
        };

        container.trigger("pjax:submitform", context);

        if (context.cancel === true) {
            return;
        }

        mfoPjax.load(context, navigateSuccess);
        e.preventDefault();
    }

    function onPopState(e) {

        var popState = e.originalEvent.state;

        if (popState === null || !popState.containerId) {
            return;
        }

        var container = $('#' + popState.containerId);

        if (container.length === 0) {
            // no container - just refresh the page
            location.reload();
            return;
        }

        var context = {
            verb: 'GET',
            url: popState.url,
            container: container
        };

        mfoPjax.load(context, function (context, data) {
            render(context, data);
        });
    }

    function onError(jqXHR, textStatus, errorThrown, context, callback) {
        // default is to do nothing if there was no response (and return false) and
        // to display the error otherwise (and return true)
        // clients can change pjax.onError to their requirements

        if (!jqXHR.responseText) {
            return false;
        }

        callback(context, jqXHR.responseText, textStatus, jqXHR);
        return true;
    }

    function navigateSuccess(context, data, textStatus, jqXHR) {

        render(context, data);

        if (context.noPushState === true) {
            return;
        }

        var url = stripInternalParams(jqXHR.getResponseHeader('X-PJAX-URL') || context.url);

        if (url !== location.href) {
            history.pushState({ url: url, containerId: context.container.attr('id') }, null, url);
        }

    }

    function stripInternalParams(url) {
        return url.replace(/([?&])(_pjax|_)=[^&]*/g, '');
    }

    function render(context, data) {

        var first100 = data.substr(0, 100);

        if (first100.indexOf('<html') >= 0 || first100.indexOf('<HTML') >= 0) {

            // loading a full HTML page into the PJAX body is an error, so
            // attempt to scrape out the <body> part of the page and
            // disable the scripts
            var bodyStart = Math.max(data.indexOf('<body'), data.indexOf('<BODY'));

            if (bodyStart >= 0) {
                data = data.substr(bodyStart + 5);
                var bodyStartClose = data.indexOf('>');

                if (bodyStartClose >= 0) {
                    data = data.substr(bodyStartClose + 1);
                }
            }

            var bodyEnd = Math.max(data.indexOf('body>'), data.indexOf('BODY>'));

            if (bodyEnd >= 0) {
                data = data.substr(0, bodyEnd);
                var bodyEndOpen = data.lastIndexOf('</');

                if (bodyEndOpen >= 0) {
                    data = data.substr(0, bodyEndOpen);
                }
            }

            data = data.replace(/<script/g, 'script_tag_disabled_pjax_error: ');
            data = data.replace(/<SCRIPT/g, 'SCRIPT_TAG_DISABLED_PJAX_ERROR: ');

            data = '<div data-title="Error - Attempt to load non-PJAX page into container">'
                + '<h1 style="background:white; color: red">Error - Attempt to load non-PJAX page into container</h1>'
                + '<div>' + data + '</div></div>';

        }

        var container = context.container;
        container.html(data);

        if (context.noPushState !== true) {
            var pjaxContent = container.children(':first');
            var title = pjaxContent.attr('data-title');
            document.title = title;
        }

    }

    function load(context, callback) {

        callback = callback || navigateSuccess;

        var overlay = mfoOverlay.add()
            .css('cursor', 'wait');

        var headers = {
            'X-PJAX': 'true',
            'X-PJAX-URL': context.url
        };

        $.extend(headers, context.headers);

        $.ajax({
            headers: headers,
            cache: false,
            type: context.verb,
            url: context.url,
            data: context.data,
            timeout: 29000,
            dataType: 'html',
            success: function (data, textStatus, jqXHR) {
                mfoOverlay.remove(overlay);
                callback(context, data, textStatus, jqXHR);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                mfoOverlay.remove(overlay);
                mfoPjax.onError(jqXHR, textStatus, errorThrown, context, callback);
            }
        });

    }

    function reload(context) {

        var url = context.container.attr('data-pjax');

        if (url === 'true') {
            url = location.pathname + location.search + location.hash;
        }

        context.url = url;

        context.verb = 'GET';
        context.noPushState = true;

        load(context);

    }

}());


var mfoDialog = {};

(function () {

    mfoDialog.init = init;
    mfoDialog.showModal = showModal;
    mfoDialog.dialogCount = dialogCount;
    mfoDialog.topDialog = topDialog;
    mfoDialog.closeDialog = closeDialog;

    removeModalHistory();

    var autoWidthStart = 700;
    var dialogBorder = 10;
    var maxHeight;
    var maxWidth;
    var originalOverflow;
    var dialogStack = [];
    var closeResponse;

    function init() {

        $(document).on('click', '[data-close-dialog]', onClickCloseDialog);
        $(window).on('popstate', onPopState);
        $(document).on('keyup', onDocKeyup);
        $(window).on('resize', onWindowResize);

        onWindowResize();

    }

    function onWindowResize() {

        maxHeight = $(window).height() - dialogBorder;
        maxWidth = $(window).width() - dialogBorder;
        resizeDialogs();

    }

    function onDocKeyup(e) {

        if (dialogCount() > 0 && e.keyCode === 27) {
            history.back();
        }

    }

    function onPopState() {

        if (!history.state || history.state.dialogCount === undefined) {
            return;
        }

        var count = history.state.dialogCount;

        if (count < dialogCount()) {
            removeTopDialog(closeResponse);
        } else if (count > dialogCount()) {
            history.back();
        }

    }

    function onClickCloseDialog(e) {

        var anchor = $(e.currentTarget);
        var response = anchor.attr('data-close-dialog');
        closeResponse = JSON.parse(response);
        history.back();
        e.preventDefault();

    }

    function closeDialog(response) {

        closeResponse = response;
        history.back();

    }

    function removeModalHistory() {
        if (history.state && history.state.dialogCount > 0) {
            history.back();
            setTimeout(removeModalHistory, 1);
        }
    }

    function dialogCount() {
        return dialogStack.length;
    }

    function topDialog() {

        if (dialogCount() === 0) {
            return $('body');
        } else {
            return dialogStack[dialogStack.length - 1];
        }
    }

    function showModal(html, callback) {

        var count = dialogCount();

        if (count === 0) {
            // prevent scroll on underlying page
            var body = $('body');
            originalOverflow = body.css('overflow');
            body.css('overflow', 'hidden');
        }

        var overlay = mfoOverlay.add(1, 100);

        var zIndex = parseInt(overlay.css('z-index')) + 500;

        var container = $('<div></div>').css({
            'margin': '0',
            'border': '0',
            'position': 'fixed',
            'top': 0,
            'left': 0,
            'width': '100%',
            'height': '100%',
            'z-index': zIndex
        })
            .appendTo('body');

        var dialog = $('<div class="mfo-dialog"></div>').css({
            'margin': 'auto',
            'position': 'absolute',
            'left': '0',
            'right': '0',
            'top': '50%',
            'transform': 'translateY(-50%)',
            'overflow': 'auto'
        })
            .appendTo(container);

        dialog.html(html);

        var dialogInfo = {
            dialog: dialog,
            container: container,
            previousTitle: document.title,
            callback: callback,
            overlay: overlay
        };

        dialogStack.push(dialogInfo);

        resizeDialogs();

        var dialogContent = dialog.children(':first');
        var title = dialogContent.attr('data-title');

        if (title) {
            document.title = document.title + ' - ' + title;
        }

        if (history.state === null || !history.dialogCount) {
            var state = history.state || {};
            state.dialogCount = count;
            history.replaceState(state, null, '');
        }

        var url = location.pathname + location.search + location.hash;
        history.pushState({ dialogCount: count + 1 }, null, url);

        return dialogInfo;

    }

    function removeTopDialog(response) {

        if (dialogCount() === 0) {
            return;
        }

        var topDialog = dialogStack.pop();

        topDialog.dialog.remove();
        topDialog.container.remove();
        mfoOverlay.remove(topDialog.overlay);
        document.title = topDialog.previousTitle;

        if (dialogCount() === 0) {
            $('body').css('overflow', originalOverflow);
        }

        if (topDialog.callback) {
            topDialog.callback(response);
        }

    }

    function resizeDialogs() {

        var currentWidth = autoWidthStart + dialogBorder;

        for (var i = 0; i < dialogStack.length; i++) {

            var dialogInfo = dialogStack[i];
            var dialog = dialogInfo.dialog;
            var dialogContent = dialog.children(':first');
            var width = dialogContent.attr('data-modal-width');

            var autoWidth = currentWidth - dialogBorder;

            if (!width) {
                width = autoWidth;
            }

            dialog.width(width);
            dialog.css('max-height', maxHeight + 'px');

            if (dialog.width() > maxWidth) {
                dialog.width(maxWidth);
            }

            currentWidth = dialog.width();

        }

    }

}());


var mfoPjaxDialog = {};

(function () {

    mfoPjaxDialog.init = init;

    function init() {

        $(document).on('click', '[data-modal-dialog]', onClickOpenModalDialog);
        $(document).on('pjax:navigate', onPjaxNavigate);
        $(document).on('pjax:submitform', onPjaxSubmitForm);

    }

    function onPjaxNavigate(e, context) {

        var anchor = context.anchor;
        var closesDialog = anchor.attr('data-close-dialog');
        var opensDialog = anchor.attr('data-modal-dialog');

        if (closesDialog || opensDialog) {
            context.cancel = true;
        }

    }

    function onPjaxSubmitForm(e, context) {

        var form = context.form;
        var dialog = form.closest('.mfo-dialog');

        if (dialog.length === 0) {
            return;
        }

        context.headers = { 'X-PJAX-MODAL': 'true' };
        context.noPushState = true;

    }

    function onClickOpenModalDialog(e) {

        var anchor = $(e.currentTarget);
        var pjaxContainer = anchor.closest('[data-pjax]');
        var url = anchor.attr('href');

        var context = {
            url: url,
            verb: 'GET',
            headers: { 'X-PJAX-MODAL': 'true' }
        };

        mfoPjax.load(context, function (context, data) {
            onDisplayModalContent(context, data, pjaxContainer);
        });

        e.preventDefault();

    }

    function onDisplayModalContent(context, data, pjaxContainer) {

        var dialogInfo = mfoDialog.showModal(data, function (response) {
            onModalClosed(response, pjaxContainer);
        });

        var dialog = dialogInfo.dialog;
        dialog.attr('data-pjax', context.url);

    }

    function onModalClosed(response, pjaxContainer) {

        if (response) {

            if (pjaxContainer.length === 0) {

                location.reload(true);

            } else {

                var context = {
                    container: pjaxContainer
                };

                if (mfoDialog.dialogCount() !== 0) {
                    context.headers = { 'X-PJAX-MODAL': 'true' };
                }

                mfoPjax.reload(context);

            }

        }

    }

}());


var mvcForms = {};

(function () {

    mvcForms.init = init;

    function init() {

        mfoPjax.init();
        mfoDialog.init();
        mfoPjaxDialog.init();

    }

}());
