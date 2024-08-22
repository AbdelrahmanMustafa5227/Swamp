"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();
let btn = document.getElementById("sendMsgBtn").disabled = true;

connection.start().then(function () {
    document.getElementById("sendMsgBtn").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("RecieveMessage", function (user, message) {
    let msg = user + ' said ' + message;
    let messagelist = document.getElementById("messagesList");
    var li = document.createElement("li");
    li.textContent = msg;
    messagelist.appendChild(li);
});



function sendMessage () {
    let message = document.getElementById("Msg").value;

    connection.invoke("SendMessage", "admin", message ).catch(function (err) {
        return console.error(err.toString());
    });
};