let express = require('express');
let router = express.Router();
const serverState = require("../js/ServerState");

router.get('/', (req, res) => {
    res.send(serverState.getUsers()).status(200).end();
});

module.exports = {url: "/users", router: router};