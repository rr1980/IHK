
window.ViewModels = (function (module) {
    module.WaitViewData = function (data) {
        var self = this;
        ko.mapping.fromJS(data, {}, self);

        socket.onmessage = function (event) {
            var datas = JSON.parse(event.data);
            console.log("-------------------------- onmessage");
            console.debug(datas);
            console.log("--------------------------");

            if (datas.command === 1) {
                ko.mapping.fromJS(datas.waits, {}, self.mubBlock.waits);
            }
            else if (datas.command === 2) {
                location.reload();
            }
        };


        self.sendMsg = function () {
            self.mubBlock.command ="Ping";
            sendMessage(JSON.stringify(ko.mapping.toJS(self.mubBlock)));
        }
        setInterval(function () {
            self.sendMsg();
        }, 2000);

    };
    return module;
}(this.ViewModels || {}));


