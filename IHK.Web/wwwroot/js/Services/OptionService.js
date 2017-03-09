window.Services = (function (module) {
    module.OptionService = function () {
        var service = {};
        var serviceurl = "http://" + window.location.host + "/Option/";

        var start = function (message) {
            window.isLoading(true);
        };

        var complete = function () {
            window.isLoading(false);
        };

        service.saveUser = function (user) {
            start();
            var res = $.ajax({
                url: serviceurl + "SaveUser",
                data: {
                    user: user
                },
                type: "POST",
                cache: false
            }).fail(function (a, b, c) {
                console.debug(a);
                console.debug(b);
                console.debug(c);
            }).always(complete);



            //.complete(complete);

            return res;
        };


        return service;
    };
    return module;
}(this.Services || {}));