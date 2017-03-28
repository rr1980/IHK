
window.ViewModels = (function (module) {
    module.MieterViewData = function (data) {
        var self = this;
        self.searchText = ko.observable();
        ko.mapping.fromJS(data, {}, self);
        var mieterervice = new Services.MieterService();

        self.onSearch = function (sender) {
            mieterervice.searchMieter(ko.mapping.toJS(self.searchText)).done(function (response) {
                ko.mapping.fromJS(response, {}, self.mieter);
                //$(".selectpicker").selectpicker('refresh');
                console.debug(response);
            });
        };

    };
    return module;
}(this.ViewModels || {}));


