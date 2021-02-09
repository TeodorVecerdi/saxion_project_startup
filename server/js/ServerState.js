const {v4} = require('uuid')

class ServerState {
    constructor() {
        this.loggedUsers = {};
        this.socketToId = {};
        this.idToSocket = {};
        this.restored = false;
    }

    load(backupJSON) {
        this.loggedUsers = backupJSON.loggedUsers || {};
    }

    loginUser(username) {
        let uuidUser = v4();

        this.loggedUsers[uuidUser] = {
            id: uuidUser,
            username: username
        }

        return uuidUser;
    }

    logoutUser(uuid) {
        if(!this.loggedUsers.hasOwnProperty(uuid)) {
            console.warn(`Trying to log out user with id ${uuid} when it doesn't exist!`);
            return false;
        }

        delete this.loggedUsers[uuid]
        return true;
    }

    isCookieValid(cookie) {
        return (cookie.hasOwnProperty('username') && cookie.hasOwnProperty('id'));
    }

    userExists(cookie) {
        let id = cookie['id'];
        if(!this.loggedUsers.hasOwnProperty(id)) return false;
        return this.loggedUsers[id].username === cookie['username'];
    }

    isAuthenticated(request) {
        if(request.headers.cookie === undefined) return false;
        if(!request.cookies.hasOwnProperty('logged-in')) return false;
        return true;
    }

    getUsers() {
        let users = {};
        for(let key of Object.keys(this.loggedUsers)) {
            users[key] = {
                username: this.loggedUsers[key].username,
                id: this.loggedUsers[key].id
            }
        }
        return users;
    }

    linkSocket(socket, cookie) {
        this.socketToId[socket.id] = cookie.id;
        this.idToSocket[cookie.id] = socket;
    }

    unlinkSocket(socket) {
        if(this.socketToId.hasOwnProperty(socket.id)) {
            let userId = this.socketToId[socket.id]
            delete this.socketToId[socket.id];

            if(this.idToSocket.hasOwnProperty(userId)) {
                delete this.idToSocket[userId];
            }
        }
    }
module.exports = new ServerState();