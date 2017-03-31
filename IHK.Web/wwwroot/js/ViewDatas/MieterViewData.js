
window.ViewModels = (function (module) {
    module.MieterViewData = function (data) {
        var self = this;
        ko.mapping.fromJS(data, {}, self);
        var mieterervice = new Services.MieterService();

        self.onClickSave = function () {
            mieterervice.saveMieter(ko.mapping.toJS(self.mieter)).done(function (response) {
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

        self.onClickWohEtageUp = function (data, event) {
            self.mieter.wohnung.etage(self.mieter.wohnung.etage() + 1);
        };

        self.onClickWohEtageDown = function (data, event) {
            self.mieter.wohnung.etage(self.mieter.wohnung.etage() - 1);
        };

        self.onClickWohRaeumeUp = function (data, event) {
            self.mieter.wohnung.raeume(self.mieter.wohnung.raeume() + 1);
        };

        self.onClickWohRaeumeDown = function (data, event) {
            self.mieter.wohnung.raeume(self.mieter.wohnung.raeume() - 1);
        };

        self.onClickEtagenUp = function (data, event) {
            self.mieter.wohnung.gebaeude.etagen(self.mieter.wohnung.gebaeude.etagen() + 1);
        };

        self.onClickEtagenDown = function (data, event) {
            self.mieter.wohnung.gebaeude.etagen(self.mieter.wohnung.gebaeude.etagen() - 1);
        };

        self.onClickGaertenUp = function (data, event) {
            self.mieter.wohnung.gebaeude.gaerten(self.mieter.wohnung.gebaeude.gaerten() + 1);
        };

        self.onClickGaertenDown = function (data, event) {
            self.mieter.wohnung.gebaeude.gaerten(self.mieter.wohnung.gebaeude.gaerten() - 1);
        };

    };
    return module;
}(this.ViewModels || {}));


