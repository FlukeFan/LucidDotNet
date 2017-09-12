var pjax = {};
(function()
{
    pjax.init = init;
    var lastButton = null;
    function init()
    {
        $(document).on('click', '[data-pjax] a', navigate);
        $(document).on('submit', '[data-pjax] form', submit);
        $(window).on('popstate', onpopstate);
        $(document).on('click', 'input[type=submit], button', function(e)
        {
            if (lastButton)
            {
                lastButton.removeAttr('clicked')
            }
            lastButton = $(e.target);
            lastButton.attr('clicked', 'true')
        })
    }
    function navigate(e)
    {
        var anchor = $(e.target);
        if (anchor.attr('data-nopjax'))
        {
            return
        }
        var container = anchor.closest('[data-pjax]');
        var url = anchor.attr('href');
        if (!container.attr('id'))
        {
            container.attr('id', 'pjax_' + (new Date).valueOf())
        }
        if (url.toLowerCase().substr(0, 7) === 'http://' || url.toLowerCase().substr(0, 8) === 'https://')
        {
            return
        }
        if (history.state === null || history.state.containerId !== container.attr('id'))
        {
            history.replaceState({
                url: location.pathname + location.search + location.hash, containerId: container.attr('id')
            }, null, '')
        }
        var context = {
                url: url, verb: "GET", container: container
            };
        load(context, navigateSuccess);
        e.preventDefault()
    }
    function navigateSuccess(context, data, textStatus, jqXHR)
    {
        var container = context.container;
        render(container, data);
        var url = stripInternalParams(jqXHR.getResponseHeader('X-PJAX-URL') || context.url);
        if (url !== location.href)
        {
            history.pushState({
                url: url, containerId: context.container.attr('id')
            }, null, url)
        }
    }
    function submit(e)
    {
        var form = $(e.currentTarget);
        var clickedButton = form.find('*[clicked=true]');
        if (clickedButton.attr('data-nopjax'))
        {
            return
        }
        var container = form.closest('[data-pjax]');
        var data = form.serialize();
        if (clickedButton.length > 0)
        {
            data += "&" + clickedButton.attr('name') + "=" + clickedButton.attr('value')
        }
        var context = {
                url: form.attr('action') || (location.pathname + location.search + location.hash), data: data, verb: form.attr('method') || 'POST', container: container
            };
        load(context, navigateSuccess);
        e.preventDefault()
    }
    function stripInternalParams(url)
    {
        return url.replace(/([?&])(_pjax|_)=[^&]*/g, '')
    }
    function render(container, data)
    {
        container.html(data);
        var pjaxContent = container.children(':first');
        document.title = pjaxContent.attr('data-title')
    }
    function onpopstate(e)
    {
        var popstate = e.originalEvent.state;
        var container = $('#' + popstate.containerId);
        if (container.length === 0)
        {
            location.reload();
            return
        }
        var context = {
                verb: "GET", url: popstate.url, container: container
            };
        load(context, function(context, data)
        {
            render(container, data)
        })
    }
    function hideOverlay()
    {
        $("#pjax_overlay").stop(true).fadeTo(50, 0, function()
        {
            $(this).remove()
        })
    }
    function load(context, callback)
    {
        $("<table id='pjax_overlay'><tbody><tr><td></td></tr></tbody></table>").css({
            position: "fixed", top: 0, left: 0, width: "100%", height: "100%", "background-color": "rgba(0,0,0,1)", opacity: "0", "z-index": 10000, "vertical-align": "middle", "text-align": "center", color: "#fff", "font-size": "30px", "font-weight": "bold", cursor: "wait"
        }).fadeTo(100, 0).fadeTo(1500, 0.2).appendTo("body");
        $.ajax({
            headers: {
                'X-PJAX': 'true', 'X-PJAX-URL': context.url
            }, cache: false, type: context.verb, url: context.url, data: context.data, timeout: 29000, dataType: "html", success: function(data, textStatus, jqXHR)
                {
                    hideOverlay();
                    callback(context, data, textStatus, jqXHR)
                }, error: function(jqXHR, textStatus)
                {
                    hideOverlay();
                    callback(context, jqXHR.responseText, textStatus, jqXHR)
                }
        })
    }
}());
var mvcForms = {};
(function()
{
    mvcForms.init = init;
    function init()
    {
        pjax.init()
    }
}())