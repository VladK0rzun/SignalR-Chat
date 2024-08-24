var connectionChat = new signalR.HubConnectionBuilder().withUrl("/hubs/basicchat").build();

document.getElementById("sendMessage").disabled = true;

connectionChat.on("MessageReceived", function (user, message, sentimentLabel) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);

    // Apply different styles based on the sentiment label
    if (sentimentLabel === "Positive") {
        li.style.color = "green"; // Green text for positive sentiment
        li.textContent = `${user} says 😊: ${message} (${sentimentLabel})`;
    } else if (sentimentLabel === "Negative") {
        li.style.color = "red"; // Red text for negative sentiment
        li.textContent = `${user} says 😡: ${message} (${sentimentLabel})`;
    } else if (sentimentLabel === "Neutral") {
        li.style.color = "gray"; // Gray text for neutral sentiment
        li.textContent = `${user} says 😐: ${message} (${sentimentLabel})`;
    } else {
        li.textContent = `${user} - ${message} (${sentimentLabel})`; // Default text
    }
});

document.getElementById("sendMessage").addEventListener("click", function (event) {
    var sender = document.getElementById("senderEmail").value;
    var message = document.getElementById("chatMessage").value;
    var receiver = document.getElementById("receiverEmail").value;

    if (receiver.length > 0) {
        connectionChat.invoke("SendMessageToReceiver", sender, receiver, message).catch(function (err) {
            return console.error(err.toString());
        });
    } else {
        connectionChat.invoke("SendMessageToAll", sender, message).catch(function (err) {
            return console.error(err.toString());
        });
    }

    event.preventDefault();
});

connectionChat.start().then(function () {
    document.getElementById("sendMessage").disabled = false;
});