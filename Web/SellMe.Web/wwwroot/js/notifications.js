setupConnection = () => {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/message")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("MessageCount", (count) => {
        document.getElementById("countMsg").innerHTML = count;
    });

    connection.start()
        .catch(err => console.error(err.toString()));

};
setupConnection();
