let express = require('express');
let router = express.Router();
const serverState = require("../js/ServerState");

router.post('/', (req, res) => {
    console.log(req.body.profile);
    console.log(JSON.stringify(JSON.parse(req.body.profile)));

    serverState.setupProfile(req);
    res.status(200).end();
});

module.exports = {url: "/profile", router: router};