
window.ViewModels = (function (module) {
    module.OptionViewData = function (data) {
        var self = this;
        var optionService = new Services.OptionService();
        self.test = ko.observableArray(["1"],["2"]);
        self.saveConfirm = ko.observable();
        ko.mapping.fromJS(data, {}, self);

        self.selectedThemeId = ko.observable(self.currentUser.layoutThemeViewModel.id());


        self.passwordConfirm = ko.observable(self.currentUser.password()).extend({
            validation: {
                validator: function (val, other) {
                    return val === other();
                }, message: 'Passwords do not match.', params: self.currentUser.password
            }
        });
        self.errors = ko.validation.group(self);

        self.onClickSave = function () {
            if (self.errors().length === 0) {
                optionService.saveUser(ko.mapping.toJS(self.currentUser)).done(function (response) {
                    _showSaveConfirm(response);
                });
            } else {
                _showSaveConfirm(false);
            }
        };

        var _showSaveConfirm = function (save) {
            if (save === true) {
                self.saveConfirm("Erfolgreich gespeichert.");
                $("#saveConfirm").css("color", "green");
            }
            else {
                self.saveConfirm("Nicht erfolgreich gespeichert.");
                $("#saveConfirm").css("color", "red");
                console.debug(self.currentUser.layoutThemeViewModel.id());
            }

            $("#saveConfirm").fadeIn(500).delay(2500).fadeOut(500, function () {
                self.saveConfirm();
            });
        };

        self.selectedThemeId.subscribe(function (a) {
            for (var i = 0; i < self.layoutThemeViewModels().length; i++) {
                if (self.selectedThemeId() === self.layoutThemeViewModels()[i].id()) {
                    var themesheet = $('<link href="' + self.currentUser.layoutThemeViewModel.link() + '" rel="stylesheet" />');
                    ko.mapping.fromJS(self.layoutThemeViewModels()[i], {}, self.currentUser.layoutThemeViewModel);
                    self.selectedThemeId(self.layoutThemeViewModels()[i].id());
                    $("#themeLink").attr('href', self.layoutThemeViewModels()[i].link());
                    break;
                }
            }
        });

        self.onClickDelete = function () { };
        self.errors.showAllMessages();


        //$(".selectpicker").selectpicker('refresh');
        ////$('.selectpicker').selectpicker();
    };
    return module;
}(this.ViewModels || {}));