const {v4} = require('uuid')
const fs = require('fs');

function makeAccount(username, password) {
    let userId = v4();
    return {
        id: userId,
        username: username,
        password: password,
        profile: {}
    }
}

function makeProfile() {}

class ServerState {
    constructor() {
        this.registeredUsers = [];
        this.loggedUsers = {};
        this.socketToId = {};
        this.idToSocket = {};
        this.messages = {};
        this.restored = false;
    }

    getJSON() {
        return JSON.stringify({
           loggedUsers: this.loggedUsers,
           messages: this.messages
        });
    }

    load(backupJSON) {
        this.loggedUsers = backupJSON.loggedUsers || {};
        this.messages = backupJSON.messages || {};
    }

    loginUser(username) {
        let account = this.getUserByUsername(username);

        this.loggedUsers[account.id] = {
            id: account.id,
            username: username
        }

        return account.id;
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

    addMessage(from, to, message) {
        if(!this.messages.hasOwnProperty(from) || !this.messages.hasOwnProperty(to)) {
            this.messages[from] = [];
            this.messages[to] = [];
        }
        let messageObj = ({from: from, to: to, message: message});
        this.messages[from].push(messageObj);
        this.messages[to].push(messageObj);

        this.emitMessage(messageObj)
    }
    getMessages(from, to) {
        if(!this.messages.hasOwnProperty(from) || !this.messages.hasOwnProperty(to)) return [];
        return this.messages[from].filter(message => (message.to == to && message.from == from) || (message.to == from && message.from == to));
    }
    emitMessage(message) {
        let sendTo = message.to;
        if(this.idToSocket.hasOwnProperty(sendTo)) {
            let socket = this.idToSocket[sendTo];
            socket.emit('new message', JSON.stringify(message));
        }
    }

    usernameExists(username) {
        for(let account of this.registeredUsers) {
            if(account.username === username) return true;
        }
        return false;
    }
    registerUser(username, password) {
        let account = makeAccount(username, password);
        this.registeredUsers.push(account);
        this.createBackup();
    }
    passwordMatches(username, password) {
        return this.registeredUsers.find(account => account.username === username).password === password;
    }
    getUserByUsername(username) {
        return this.registeredUsers.find(account => account.username === username)
    }
    getUserByID(ID) {
        return this.registeredUsers.find(account => account.id === ID)
    }
}

module.exports = new ServerState();