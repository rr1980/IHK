window.Services = (function (module) {
    module.GebaeudeService = function () {
        var service = {};
        var serviceurl = "http://" + window.location.host + "/Gebaeude/";

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

        service.searchGebaeude = function (datas) {
            start();1
            var res = $.ajax({
                url: serviceurl + "SearchGebaeude",
                data: {
                    datas: datas
                },
                type: "POST",
                cache: false
            }).fail(error).always(complete);
            
            return res;
        };

        service.saveGebaeude = function (gebaeude) {
            start(); 1
            var res = $.ajax({
                url: serviceurl + "SaveGebaeude",
                data: {
                    gebaeude: gebaeude
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