
window.ViewModels = (function (module) {
    module.MieterSearchViewData = function (data) {
        var self = this;
        self.searchText = ko.observable();
        ko.mapping.fromJS(data, {}, self);
        var mieterervice = new Services.MieterService();

        self.onSearch = function (sender) {
            mieterervice.searchMieter(ko.mapping.toJS(self.searchText)).done(function (response) {
                ko.mapping.fromJS(response, {}, self.mieter);
                //$(".selectpicker").selectpicker('refresh');
                //console.debug(response);
            });
        };

        self.onClickResult = function (data, event) {
            window.open("/Mieter/Index?id="+data.id(),"_blank");
        };

    };
    return module;
}(this.ViewModels || {}));


