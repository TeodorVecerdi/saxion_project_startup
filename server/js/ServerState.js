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

function makeProfile(firstName, lastName, dateOfBirth, romantic, friendship) {
    let profile = {
        name: {first: firstName, last: lastName},
        dateOfBirth: dateOfBirth,
        interests: {}
    }

    if(romantic !== undefined) {
        profile.interests['romantic'] = {
            minimumAge: romantic.ageMin,
            maximumAge: romantic.ageMax,
        }
    }

    if(friendship !== undefined) {
        profile.interests['friendship'] = {
            minimumAge: friendship.ageMin,
            maximumAge: friendship.ageMax,
        }
    }

    return profile;
}

class ServerState {
    constructor() {
        this.registeredUsers = [];
        this.loggedUsers = {};
        this.socketToId = {};
        this.idToSocket = {};
        this.messages = {};
        this.restoreBackup();
    }

    getJSON() {
        return JSON.stringify({
            registeredUsers: this.registeredUsers,
            loggedUsers: this.loggedUsers,
            messages: this.messages
        });
    }

    createBackup() {
        let backupJSON = this.getJSON();

        fs.writeFile("./data/backup.json", backupJSON, err => {
            if(err) throw err;
            console.log("Backup success!");
        });
    }

    restoreBackup() {
        fs.access("./data/backup.json", err => {
            if (err === null) {
                fs.readFile("./data/backup.json", (err1, data) => {
                    if(err1) throw err1;
                    let backupJSON = JSON.parse(data);

                    this.loggedUsers = backupJSON.loggedUsers || {};
                    this.registeredUsers = backupJSON.registeredUsers || [];
                    this.messages = backupJSON.messages || {};

                    console.log("Backup restored successfully!")
                });
            }
        })
    }

    clearBackup() {
        fs.writeFile("./data/backup.json", "{}", err => {
            if(err) throw err;
            console.log("Cleared backup successfully!");
        });
    }

    loginUser(username) {
        let account = this.getUserByUsername(username);

        this.loggedUsers[account.id] = {
            id: account.id,
            username: username
        }
        this.createBackup();
        return account.id;
    }
    logoutUser(uuid) {
        if(!this.loggedUsers.hasOwnProperty(uuid)) {
            console.warn(`Trying to log out user with id ${uuid} when it doesn't exist!`);
            return false;
        }

        delete this.loggedUsers[uuid]
        this.createBackup();
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

        this.createBackup();
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
    setupProfile(request) {
        let userId = request.cookies['logged-in'].id;
        let profile = makeProfile(request.body.firstName, request.body.lastName, request.body.dateOfBirth, request.body.romantic, request.body.friendship);

        let accIdx = this.registeredUsers.findIndex(account => account.id == userId);
        this.registeredUsers[accIdx].profile = profile;
    }
}

module.exports = new ServerState();