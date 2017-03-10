
window.ViewModels = (function (module) {
    module.HomeViewData = function (data) {
        var self = this;
        ko.mapping.fromJS(data, {}, self);

        self.items = ko.observable({
            mieter: [
                {
                    name: "Rene",
                    url:"\Home\GetUser?id=2"
                }
            ]
        })

        self.onClickSuche = function () {
            console.log("Klaaa");
        };

    };
    return module;
}(this.ViewModels || {}));


