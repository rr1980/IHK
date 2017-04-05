﻿
window.ViewModels = (function (module) {
    module.MieterViewData = function (data) {
        var self = this;
        var koPath = $("#wohRow").data("kopath");
        ko.mapping.fromJS(data, {}, self);
        var mieterservice = new Services.MieterService();

        self.onClickSave = function () {
            mieterservice.saveMieter(ko.mapping.toJS(self.mieter)).done(function (response) {
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

        function getValue(array) {
            var value = self;

            if (koPath != "") {
                var sp = koPath.split('.');
                for (var i = 0; i < sp.length; i++) {
                    if (sp[i] != "") {
                        value = value[sp[i]];
                    }
                }
            }

            for (var i = 0; i < array.length; i++) {
                value = value[array[i]];
            }

            return value;
        };

        self.onClickWohEtageUp = function (data, event) {
            var value = getValue(["wohnung", "etage"]);
            value(value() + 1);
        };

        self.onClickWohEtageDown = function (data, event) {
            var value = getValue(["wohnung", "etage"]);
            value(value() - 1);
        };

        self.onClickWohRaeumeUp = function (data, event) {
            var value = getValue(["wohnung", "raeume"]);
            value(value() + 1);
        };

        self.onClickWohRaeumeDown = function (data, event) {
            var value = getValue(["wohnung", "raeume"]);
            value(value() - 1);
        };

        self.onClickEtagenUp = function (data, event) {
            var value = getValue(["wohnung", "gebaeude","etagen"]);
            value(value() + 1);
        };

        self.onClickEtagenDown = function (data, event) {
            var value = getValue(["wohnung", "gebaeude", "etagen"]);
            value(value() - 1);
        };

        self.onClickGaertenUp = function (data, event) {
            var value = getValue(["wohnung", "gebaeude", "gaerten"]);
            value(value() + 1);
        };

        self.onClickGaertenDown = function (data, event) {
            var value = getValue(["wohnung", "gebaeude", "gaerten"]);
            value(value() - 1);
        };

    };
    return module;
}(this.ViewModels || {}));


