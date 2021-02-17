let express = require('express');
let router = express.Router();
const serverState = require("../js/ServerState");

router.get('/ping', (req, res) => {
    console.log(serverState.getJSON());
    res.send(serverState.getJSON());
})

router.get('/clear', (req, res) => {
    serverState.clearBackup();
    res.render("pages/empty")
})

module.exports = {url: "/dev", router: router};
