(function (console, document) {

    const $logsSection = document.getElementById('logsSection');
    const $logs = document.getElementById('logs');

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
        const logEntry = `<pre>${new Date()} | ${event.data}</pre>`;
        $logs.innerHTML = `${logEntry}${$logs.innerHTML}`;
    });

})(console, document);