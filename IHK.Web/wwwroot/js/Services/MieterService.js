window.Services = (function (module) {
    module.MieterService = function () {
        var service = {};
        var serviceurl = "http://" + window.location.host + "/Mieter/";

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

        service.searchMieter = function (datas) {
            start();1
            var res = $.ajax({
                url: serviceurl + "SearchMieter",
                data: {
                    datas: datas
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