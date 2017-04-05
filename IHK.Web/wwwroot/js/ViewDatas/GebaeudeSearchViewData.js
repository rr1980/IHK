
window.ViewModels = (function (module) {
    module.GebaeudeSearchViewData = function (data) {
        var self = this;
        self.searchText = ko.observable();
        ko.mapping.fromJS(data, {}, self);
        var gebaeudeservice = new Services.GebaeudeService();

        self.onSearch = function (sender) {
            gebaeudeservice.searchGebaeude(ko.mapping.toJS(self.searchText)).done(function (response) {
                ko.mapping.fromJS(response, {}, self.gebaeude);
                //$(".selectpicker").selectpicker('refresh');
                //console.debug(response);
            });
        };

        self.onClickResult = function (data, event) {
            window.open("/Gebaeude/Index?id="+data.id(),"_blank");
        };

    };
    return module;
}(this.ViewModels || {}));


