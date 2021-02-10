const serverState = require('./ServerState');

function setup(socket) {
    socket.emit('hello', socket.id);
    socket.on('disconnect', () => {
        socket.broadcast.emit('goodbye');
        serverState.unlinkSocket(socket);
        console.log("Client disconnected");
    })
    socket.on('auth', data => {
        let signIn = JSON.parse(data);
        if(serverState.isCookieValid(signIn) && serverState.userIsLoggedIn(signIn)) {
            serverState.linkSocket(socket, signIn);
            socket.broadcast.emit('new client');
            console.log(`User ${data} exists.`)
        }
    })

    console.info(`Socket set up`);
}

module.exports = setup;