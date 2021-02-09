let express = require('express');
let router = express.Router();
const serverState = require("../js/ServerState");

router.get('/', (req, res) => {
    res.render("pages/profile")
})

module.exports = {url: "/profile", router: router};