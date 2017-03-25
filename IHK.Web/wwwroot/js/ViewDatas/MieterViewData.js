
window.ViewModels = (function (module) {
    module.MieterViewData = function (data) {
        var self = this;
        self.searchText = ko.observable();
        ko.mapping.fromJS(data, {}, self);

        self.onSearch = function (sender) {
            //Todo search....
        };

    };
    return module;
}(this.ViewModels || {}));


