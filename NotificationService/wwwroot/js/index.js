"use strict";

var connection = null;

//Disable send button until connection is established
var connectButton = document.getElementById("connectButton");
var disconnectButton = document.getElementById("disconnectButton");
var sendButton = document.getElementById("sendButton");

disconnectButton.disabled = true;
connectButton.disabled = false;
sendButton.disabled = true;


disconnectButton.addEventListener("click", function (event) {


    connection.stop().then(function () {
        console.log("Server Notification: %cDisconnected", "color:red");

        connectButton.disabled = false;
        sendButton.disabled = true;
        disconnectButton.disabled = true;
    }).catch(function (err) {
        return console.error(err.toString());
    })
});

connectButton.addEventListener("click", function (event) {

    var username = document.getElementById("userInput").value;

    connection = new signalR.HubConnectionBuilder()
        .withUrl("/notificationHub?username=" + username)
        .build();

    connection.on("ReceiveMessage", function (user, message) {
        var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        var encodedMsg = user + " says " + msg;
        var li = document.createElement("li");
        li.textContent = encodedMsg;
        document.getElementById("messagesList").appendChild(li);
    });

    connection.start().then(function () {
        console.log("Server Notification: %cConnected", "color:red");

        connectButton.disabled = true;
        disconnectButton.disabled = false;
        sendButton.disabled = false;
    }).catch(function (err) {
        return console.error(err.toString());
    });

});





sendButton.addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;

    var messageInput = document.getElementById("messageInput");
    var message = messageInput.value;

    connection.invoke("BroadcastMessage", user, message).then(function () {
        messageInput.value = "";
    }).catch(function (err) {
            return console.error(err.toString());
        });
    event.preventDefault();
});