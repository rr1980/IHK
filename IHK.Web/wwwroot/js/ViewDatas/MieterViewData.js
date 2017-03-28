
window.ViewModels = (function (module) {
    module.MieterViewData = function (data) {
        var self = this;
        ko.mapping.fromJS(data, {}, self);
        var mieterervice = new Services.MieterService();



    };
    return module;
}(this.ViewModels || {}));


