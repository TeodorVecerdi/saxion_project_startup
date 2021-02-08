let express = require('express');
let router = express.Router();
let serverState = require("../js/ServerState");

router.get('/active-users', (req, res) => {
    if(!serverState.isAuthenticated(req)) {
        res.status(403).end();
        return;
    }

    let users = serverState.loggedUsers;
    res.send(users).end();
})

module.exports = {url: "/chat", router: router};