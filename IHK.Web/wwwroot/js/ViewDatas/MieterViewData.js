
window.ViewModels = (function (module) {
    module.MieterViewData = function (data) {
        var self = this;
        ko.mapping.fromJS(data, {}, self);
        var mieterervice = new Services.MieterService();

        console.debug(data);

    };
    return module;
}(this.ViewModels || {}));


