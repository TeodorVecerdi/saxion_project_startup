const {v4} = require('uuid')

class ServerState {
    constructor() {
        this.loggedUsers = {};
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
        if(!cookie.hasOwnProperty('username') || !cookie.hasOwnProperty('id')) return false;
        let id = cookie['id'];
        if(!this.loggedUsers.hasOwnProperty(id)) return false;
        return this.loggedUsers[id].username === cookie['username'];
    }

    isAuthenticated(cookies) {
        if(!cookies.hasOwnProperty('logged-in')) return false;
        return this.isCookieValid(cookies['logged-in']);
    }

    getUsers() {
        return this.loggedUsers;
    }
}

module.exports = new ServerState();