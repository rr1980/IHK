function sendMessage(message, callback) {
    //console.debug(message);
    WaitForConnection(function () {
        socket.send(message);
        if (typeof callback !== 'undefined') {
            callback();
        }
    }, 500);
};

function WaitForConnection (callback, interval) {
    if (socket.readyState === 1) {
        callback();
    } else {
        var that = this;
        // optional: implement backoff for interval here
        setTimeout(function () {
            WaitForConnection(callback, interval);
        }, interval);
    }
};


function connect(path) {
    var uri = "ws://" + window.location.host + "/" + path;
    socket = new WebSocket(uri);
    socket.onopen = function (event) {
        console.log("opened connection to " + uri);
    };
    socket.onclose = function (event) {
        console.log("closed connection from " + uri);
    };
    //socket.onmessage = function (event) {
    //    //appendItem(list, event.data);
    //    console.log(event.data);
    //};
    socket.onerror = function (event) {
        console.log("error: " + event.data);
    };
}
//connect();
//var list = document.getElementById("messages");
var button = document.getElementById("sendButton");
//button.addEventListener("click", function () {

//    //var input = document.getElementById("textInput");
//    //sendMessage(input.value);
//    sendMessage("TEST!!!");

//    input.value = "";
//});
//function sendMessage(message) {
//    console.log("Sending: " + message);
//    v.send(message);
//}
//function appendItem(list, message) {
//    var item = document.createElement("li");
//    item.appendChild(document.createTextNode(message));
//    list.appendChild(item);
//}    