
window.ViewModels = (function (module) {
    module.AdresseSearchViewData = function (data) {
        var self = this;
        self.searchText = ko.observable();
        ko.mapping.fromJS(data, {}, self);
        var adresseservice = new Services.AdresseService();

        self.onSearch = function (sender) {
            adresseservice.searchAdresse(ko.mapping.toJS(self.searchText)).done(function (response) {
                ko.mapping.fromJS(response, {}, self.adressen);
                //$(".selectpicker").selectpicker('refresh');
                //console.debug(response);
            });
        };

        self.onClickResult = function (data, event) {
            window.open("/Adresse/Index?id="+data.id(),"_blank");
        };

    };
    return module;
}(this.ViewModels || {}));


