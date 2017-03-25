
window.getCookie = function getCookie(name) {
    var regexp = new RegExp("(?:^" + name + "|;\s*" + name + ")=(.*?)(?:;|$)", "g");
    var result = regexp.exec(document.cookie);
    return (result === null) ? null : result[1];
};

window.isLoading = ko.observable(false);

ko.validation.rules.pattern.message = 'Invalid.';
ko.validation.init({
    registerExtenders: true,
    messagesOnModified: true,
    insertMessages: true,
    parseInputAttributes: true,
    messageTemplate: null
});

ko.validation.rules['mustEqual'] = {
    validator: function (val, otherVal) {
        return val === otherVal;
    },
    message: 'The field must equal {0}'
};

ko.bindingHandlers.returnAction = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = ko.utils.unwrapObservable(valueAccessor());

        $(element).keyup(function (e) {
            if (e.which === 13) {
                value(viewModel);
            }
        });
    }
};

ko.bindingHandlers.bindIframe = {
    init: function (element, valueAccessor) {
        function bindIframe() {
            try {
                var iframeInit = element.contentWindow.initChildFrame,
                    iframedoc = element.contentDocument.body;
            } catch (e) {
                // ignored
            }
            if (iframeInit)
                iframeInit(ko, valueAccessor());
            else if (iframedoc)
                ko.applyBindings(valueAccessor(), iframedoc);
        };
        bindIframe();
        ko.utils.registerEventHandler(element, 'load', bindIframe);
    }
};
