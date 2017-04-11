
window.ViewModels = (function (module) {
    module.WaitViewData = function (data) {
        var self = this;
        ko.mapping.fromJS(data, {}, self);

        socket.onmessage = function (event) {
            var data = JSON.parse(event.data);
            console.log(data);
            if (data.Command === 2 && data.Position === 0) {
                location.reload();
            }
        };


        self.sendMsg = function () {
            var msg = {
                Command: "Block",
                EntityType: self.mubBlock.entityType(),
                UserId: self.mubBlock.userId(),
                EntityId: self.mubBlock.entityId(),
                SocketId: self.mubBlock.socketId()
            };
            console.debug(msg);
            sendMessage(JSON.stringify(msg));
        }

        self.sendMsg();

    };
    return module;
}(this.ViewModels || {}));


