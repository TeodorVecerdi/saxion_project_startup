const {v4} = require('uuid')
const fs = require('fs');
const v8 = require('v8')

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
        this.swipes = {};
        this.matches = {};
        this.unconfirmedMatches = {};
        this.restoreBackup();
    }

    getJSON() {
        return JSON.stringify({
            registeredUsers: this.registeredUsers,
            messages: this.messages,
            swipes: this.swipes,
            matches: this.matches,
            unconfirmedMatches: this.unconfirmedMatches
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

                    this.registeredUsers = backupJSON.registeredUsers || [];
                    this.messages = backupJSON.messages || {};
                    this.swipes = backupJSON.swipes || {};
                    this.matches = backupJSON.matches || {};
                    this.unconfirmedMatches = backupJSON.unconfirmedMatches || {};

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
        let users = [];
        for(let key of Object.keys(this.registeredUsers)) {
            let user = v8.deserialize(v8.serialize(this.registeredUsers[key]));
            delete user['password'];
            users.push(user);
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
        this.createBackup();
    }

    swipe(from, to, swipe) {
        if(!this.swipes.hasOwnProperty(from))
            this.swipes[from] = {};
        this.swipes[from][to] = swipe;
        this.createBackup();

        this.checkMatches(from, to);
    }
    checkMatches(from, to) {
        if(!this.swipes.hasOwnProperty(from) || !this.swipes.hasOwnProperty(to) || !this.swipes[from].hasOwnProperty(to) || !this.swipes[to].hasOwnProperty(from)) return;

        if(this.swipes[from][to] == 1 && this.swipes[to][from] == 1) {
            if(!this.matches.hasOwnProperty(from)) this.matches[from] = {};
            if(!this.matches.hasOwnProperty(to)) this.matches[to] = {};

            if(!this.unconfirmedMatches.hasOwnProperty(from)) this.unconfirmedMatches[from] = {};
            if(!this.unconfirmedMatches.hasOwnProperty(to)) this.unconfirmedMatches[to] = {};


            this.matches[from][to] = {};
            this.matches[to][from] = {};
            this.unconfirmedMatches[from][to] = {}
            this.unconfirmedMatches[to][from] = {}

            this.createBackup();
        }
    }
    hasMatch(from, to) {
        if(!this.matches.hasOwnProperty(from) || !this.matches.hasOwnProperty(to) || !this.matches[from].hasOwnProperty(to) || !this.matches[to].hasOwnProperty(from)) return false;
        return true;
    }
    getMatches(userId) {
        if(!this.matches.hasOwnProperty(userId)) return [];
        return Object.keys(this.matches[userId]);
    }
    getSwipes(userId) {
        if(!this.swipes.hasOwnProperty(userId)) return [];
        let swipes = [];
        for(let key of Object.keys(this.swipes[userId])) {
            swipes.push({"id": key, "swipe": this.swipes[userId][key]})
        }
        return swipes;
    }
    getUnconfirmedMatches(userId) {
        if(!this.unconfirmedMatches.hasOwnProperty(userId)) return [];
        return Object.keys(this.unconfirmedMatches[userId]);
    }
    confirmMatches(userId, ids) {
        if(!this.unconfirmedMatches.hasOwnProperty(userId)) return;

        for(let id of ids) {
            if(this.unconfirmedMatches[userId].hasOwnProperty(id))
                delete this.unconfirmedMatches[userId][id];
        }
        this.createBackup();
    }
}

module.exports = new ServerState();