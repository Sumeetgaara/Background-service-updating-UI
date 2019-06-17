"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:5000/updatehub").build();



connection.on("ReceiveMessage", function (value) {
    console.log(value);
    document.getElementById("some").innerHTML=value;
});

connection.start().then(function () {
    console.log("signalr client started");
}).catch(function (err) {
    return console.error(err.toString());
});
