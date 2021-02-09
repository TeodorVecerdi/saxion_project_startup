let express = require('express');
let router = express.Router();
let serverState = require("../js/ServerState");

router.get('/active-users', (req, res) => {
    if(!serverState.isAuthenticated(req)) {
        res.status(403).end();
        return;
    }

    let users = serverState.getUsers();
    res.send(users).end();
});

router.get('/messages', (req, res) => {
    if(!serverState.isAuthenticated(req)) {
        res.status(403).end();
        return;
    }

    let messages = serverState.getMessages(req.query.self, req.query.userId);
    res.send(messages).end();
});

router.put('/message', (req, res) => {
    if(!serverState.isAuthenticated(req)) {
        res.status(403).end();
        return;
    }

    serverState.addMessage(req.body.from, req.body.to, req.body.message);
    res.status(200).end();
})

module.exports = {url: "/chat", router: router};