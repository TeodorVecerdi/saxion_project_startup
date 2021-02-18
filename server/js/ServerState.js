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
        this.messages = {};
        this.unconfirmedMessages = {};
        this.swipes = {};
        this.matches = {};
        this.unconfirmedMatches = {};
        this.restoreBackup();
    }

    getJSON() {
        return JSON.stringify({
            registeredUsers: this.registeredUsers,
            messages: this.messages,
            unconfirmedMessages: this.unconfirmedMessages,
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
                    this.unconfirmedMessages = backupJSON.unconfirmedMessages || {};
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
        return this.getUserByUsername(username);
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
    getLikesCount(userId) {
        let likesCount = 0;
        for(let id of Object.keys(this.swipes)) {
            if(this.swipes[id].hasOwnProperty(userId) && this.swipes[id][userId] == 1)
                likesCount++;
        }
        return likesCount;
    }

    newMessage(req, timestamp, msgId) {
        let from = req.body.from;
        let to = req.body.to;
        let message = req.body.message;

        if(!this.messages.hasOwnProperty(from)) this.messages[from] = {};
        if(!this.messages[from].hasOwnProperty(to)) this.messages[from][to] = {};
        if(!this.messages.hasOwnProperty(to)) this.messages[to] = {};
        if(!this.messages[to].hasOwnProperty(from)) this.messages[to][from] = {};

        this.messages[from][to][msgId] = {
            id: msgId,
            from: from,
            to: to,
            message: message,
            timestamp: timestamp
        }
        this.messages[to][from][msgId] = this.messages[from][to][msgId];

        if(!this.unconfirmedMessages.hasOwnProperty(to)) this.unconfirmedMessages[to] = {};
        if(!this.unconfirmedMessages[to].hasOwnProperty(from)) this.unconfirmedMessages[to][from] = {};
        this.unconfirmedMessages[to][from][msgId] = {};

        this.createBackup();
    }
    confirmMessages(from, to, ids) {
        if(!this.unconfirmedMessages.hasOwnProperty(from)) return;
        if(!this.unconfirmedMessages[from].hasOwnProperty(to)) return;

        for (let id of ids) {
            if(this.unconfirmedMessages[from][to].hasOwnProperty(id))
                delete this.unconfirmedMessages[from][to][id];
        }

        this.createBackup();
    }
    confirmAllMessages(from, to) {
        if(!this.unconfirmedMessages.hasOwnProperty(from)) return;
        if(!this.unconfirmedMessages[from].hasOwnProperty(to)) return;

        for (let id of Object.keys(this.unconfirmedMessages[from][to])) {
            delete this.unconfirmedMessages[from][to][id];
        }

        this.createBackup();
    }
    getMessages(from, to) {
        if (!this.messages.hasOwnProperty(from) || !this.messages[from].hasOwnProperty(to)) return [];
        let messages = [];
        for(let id of Object.keys(this.messages[from][to])) {
            messages.push(this.messages[from][to][id])
        }
        return messages;
    }
    getUnconfirmedMessages(from, to) {
        if (!this.unconfirmedMessages.hasOwnProperty(from) || !this.unconfirmedMessages[from].hasOwnProperty(to)) return [];
        let messages = [];
        for(let id of Object.keys(this.unconfirmedMessages[from][to])) {
            messages.push(this.messages[from][to][id]);
        }
        return messages;
    }
}

module.exports = new ServerState();