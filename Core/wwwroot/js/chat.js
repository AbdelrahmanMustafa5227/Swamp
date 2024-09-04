"use strict";
let recieverId = document.getElementById("chat").dataset.rid;
var connection = new signalR.HubConnectionBuilder().withUrl("/chat/" + recieverId).build();
let btn = document.getElementById("sendMsgBtn").disabled = true;
let messagelist = document.getElementById("messagesList");
let chat = document.getElementById("chat-container");


connection.start().then(function () {
    document.getElementById("sendMsgBtn").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("Announce", function (user , message) {
    let msg = user + message;
    var li = document.createElement("li");
    li.textContent = msg;
    li.classList.add("announce");
    messagelist.appendChild(li);
    $(chat).scrollTop($(chat)[0].scrollHeight);
});


connection.on("RecieveMessage", function (message, cls) {
    var li = document.createElement("li");
    li.textContent = message;
    li.classList.add(cls);
    messagelist.appendChild(li);
    $(chat).scrollTop($(chat)[0].scrollHeight);
});



function SendMsg(sender, receiver) {

    let message = document.getElementById("Msg");
    if (message.value != "") {

        connection.invoke("SendMsg", sender, receiver, message.value).catch(function (err) {
            return console.error(err.toString());
        });
        message.value = "";
    }
    
};