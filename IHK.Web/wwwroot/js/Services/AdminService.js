window.Services = (function (module) {
    module.AdminService = function () {
        var service = {};
        var serviceurl = "http://" + window.location.host + "/Admin/";

        var start = function (message) {
            window.isLoading(true);
        };

        var complete = function () {
            window.isLoading(false);
        };

        var error = function (a, b, c) {
            $("body").html(a.responseText);
            console.debug(a);
            console.debug(b);
            console.debug(c);
        };

        service.saveUser = function (user) {
            start();
            return $.ajax({
                url: serviceurl + "SaveUser",
                data: {
                    user: user
                },
                type: "POST",
                cache: false
            }).fail(error)
                .always(complete);
        };

        service.resetPw = function (user) {
            start();
            return $.ajax({
                url: serviceurl + "ResetPassord",
                data: {
                    user: user
                },
                type: "POST",
                cache: false
            }).fail(error)
                .always(complete);
        };

        service.delUser = function (user) {
            start();
            return $.ajax({
                url: serviceurl + "DelUser",
                data: {
                    user: user
                },
                type: "POST",
                cache: false
            }).fail(error).always(complete);
        };

        return service;
    };
    return module;
}(this.Services || {}));