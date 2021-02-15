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

function makeProfile(jsonString) {
    let values = JSON.parse(jsonString)
    let profile = {
        name: values["name"],
        birthDate: values["birthDate"],
        about: values["about"],
        gender: values["gender"],
        genderPreference: values["genderPreference"],
        relationshipPreference: values["relationshipPreference"],
        genrePreference: values["genrePreference"],
        playedGames: values["playedGames"],
        profilePictureType: values["profilePictureType"]
    }

    if(profile.profilePictureType === 0) {
        profile["avatar"] = values["avatar"];
    } else {
        profile["pictures"] = values["profilePictures"]
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
            this.restoreBackup()
        });
    }

    loginUser(username) {
        let account = this.getUserByUsername(username);

        this.loggedUsers[account.id] = {
            id: account.id,
            username: username
        }
        this.createBackup();
        return account;
    }
    logoutUser(uuid) {
        delete this.loggedUsers[uuid]
        this.createBackup();
        return true;
    }
    userIsLoggedIn(id) {
        return this.loggedUsers.hasOwnProperty(id);
    }
    isAuthenticated(request) {
        return request.body.hasOwnProperty("id");
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

    linkSocket(socket, userId) {
        this.socketToId[socket.id] = userId;
        this.idToSocket[userId] = socket;
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
        let userId = request.body.id;
        let profile = makeProfile(request.body.profile);

        let accIdx = this.registeredUsers.findIndex(account => account.id == userId);
        this.registeredUsers[accIdx].profile = profile;
    }
}

module.exports = new ServerState();