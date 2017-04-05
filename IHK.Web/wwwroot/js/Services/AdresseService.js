window.Services = (function (module) {
    module.AdresseService = function () {
        var service = {};
        var serviceurl = "http://" + window.location.host + "/Adresse/";

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

        service.searchAdresse = function (datas) {
            start();1
            var res = $.ajax({
                url: serviceurl + "SearchAdresse",
                data: {
                    datas: datas
                },
                type: "POST",
                cache: false
            }).fail(error).always(complete);
            
            return res;
        };

        service.saveAdresse = function (adresse) {
            start(); 1
            var res = $.ajax({
                url: serviceurl + "SaveAdresse",
                data: {
                    adresse: adresse
                },
                type: "POST",
                cache: false
            }).fail(error).always(complete);

            return res;
        };


        return service;
    };
    return module;
}(this.Services || {}));