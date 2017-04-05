
window.ViewModels = (function (module) {
    module.AdresseViewData = function (data) {
        var self = this;
        var koPath = $("#wohRow").data("kopath");
        ko.mapping.fromJS(data, {}, self);
        var adresseservice = new Services.AdresseService();

        self.onClickSave = function () {
            adresseservice.saveAdresse(ko.mapping.toJS(self.adresse)).done(function (response) {
                $("#errors").html("");
                if (response.errors === null) {

                }
                else {
                    for (var j = 0; j < response.errors.length; j++) {
                        $("#errors").append("<li style='color:red;'>" + response.errors[j] + "</li>");
                    }
                }
            });
        };
    };
    return module;
}(this.ViewModels || {}));


