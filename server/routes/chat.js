let express = require('express');
let router = express.Router();
const {v4} = require("uuid");
let serverState = require("../js/ServerState");

router.post('/message', (req, res) => {
    let timestamp = Date.now();
    let msgId = v4();

    serverState.newMessage(req, timestamp, msgId);
    res.status(200).end();
})

router.post('/confirm-messages', (req, res) => {
    console.log(`Confirming messages ${req.body.ids}`);
    serverState.confirmMessages(req.body.from, req.body.to, JSON.parse(req.body.ids));
    res.status(200).end();
})

router.get('/messages', (req, res) => {
    let messages = serverState.getMessages(req.query.from, req.query.to);
    console.log(`Getting messages: ${JSON.stringify(messages)}`)
    res.send(messages).status(200).end();
})

router.get('/unconfirmed-messages', (req, res) => {
    let messages = serverState.getUnconfirmedMessages(req.query.from, req.query.to)
    console.log(`Getting unconfirmed messages: ${JSON.stringify(messages)}`)
    res.send(messages).status(200).end();
})

module.exports = {url: "/chat", router: router};