window.Services = (function (module) {
    module.WohnungService = function () {
        var service = {};
        var serviceurl = "http://" + window.location.host + "/Wohnung/";

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

        service.searchWohnung = function (datas) {
            start();1
            var res = $.ajax({
                url: serviceurl + "SearchWohnung",
                data: {
                    datas: datas
                },
                type: "POST",
                cache: false
            }).fail(error).always(complete);
            
            return res;
        };

        service.saveWohnung = function (wohnung) {
            start(); 1
            var res = $.ajax({
                url: serviceurl + "SaveWohnung",
                data: {
                    wohnung: wohnung
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