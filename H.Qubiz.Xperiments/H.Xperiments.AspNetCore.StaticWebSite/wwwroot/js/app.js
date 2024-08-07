(function (console) {

    console.debug('App Started');

    const socket = new WebSocket("ws://localhost:11080?client=DevTest");

    socket.addEventListener("open", (event) => {
        console.info(`WS connection @ ${socket.url} OPENED`);
    });

    socket.addEventListener("close", (event) => {
        console.info(`WS connection @ ${socket.url} CLOSED`);
    });

    socket.addEventListener("message", (event) => {
        console.info(`WS message received from ${socket.url}`);
        console.debug(event);
    });

})(console);