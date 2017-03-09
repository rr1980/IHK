﻿
window.ViewModels = (function (module) {
    module.AdminViewData = function (data) {
        var self = this;
        ko.mapping.fromJS(data, {}, self);
        var adminService = new Services.AdminService();

        self.selectedUserId = ko.observable();
        self.selectedRoles = ko.observableArray([1]);

        self.user = ko.observable(self.users()[0]);


        self.onClickDelete = function () {
            adminService.delUser(ko.mapping.toJS(self.user)).done(function (response) {
                ko.mapping.fromJS(response.users, {}, self.users);
                //$(".selectpicker").selectpicker('refresh');
            });
        };

        self.OnClickPwClear = function () {
            adminService.resetPw(ko.mapping.toJS(self.user)).done(function (response) {
            });
        };

        self.onClickSave = function () {
            var userna = self.user().username();
            adminService.saveUser(ko.mapping.toJS(self.user)).done(function (response) {
                $("#errors").html("");
                if (response.errors === null) {
                    ko.mapping.fromJS(response.users, {}, self.users);
                    for (var i = 0; i < self.users().length; i++) {
                        if (self.users()[i].username() === userna) {
                            self.selectedUserId(self.users()[i].userId());
                            break;
                        }
                    }
                    //$(".selectpicker").selectpicker('refresh');
                }
                else {
                    for (var j = 0; j < response.errors.length; j++) {
                        $("#errors").append("<li style='color:red;'>" + response.errors[j] + "</li>");
                    }
                }
            });
        };

        self.selectedUserId.subscribe(function () {
            $("#errors").html("");
            for (var i = 0; i < self.users().length; i++) {
                if (self.selectedUserId() === self.users()[i].userId()) {
                    self.user(self.users()[i]);
                    $('select').select2().trigger('change');
                    break;
                }
            }
        });



        //$('select').select2().trigger('change');


        //$("#observationSelection").select2({
        //    dropdownCssClass: 'no-search'
        //});

        //$("#observationSelection").select2({
        //    minimumResultsForSearch: -1
        //});

        //$(".select2-search input").prop("readonly", true);
    };
    return module;
}(this.ViewModels || {}));