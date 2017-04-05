
window.ViewModels = (function (module) {
    module.WohnungSearchViewData = function (data) {
        var self = this;
        self.searchText = ko.observable();
        ko.mapping.fromJS(data, {}, self);
        var wohnungservice = new Services.WohnungService();

        self.onSearch = function (sender) {
            wohnungservice.searchWohnung(ko.mapping.toJS(self.searchText)).done(function (response) {
                ko.mapping.fromJS(response, {}, self.wohnungen);
                //$(".selectpicker").selectpicker('refresh');
                //console.debug(response);
            });
        };

        self.onClickResult = function (data, event) {
            window.open("/Wohnung/Index?id="+data.id(),"_blank");
        };

    };
    return module;
}(this.ViewModels || {}));


