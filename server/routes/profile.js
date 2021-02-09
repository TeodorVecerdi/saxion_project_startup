let express = require('express');
let router = express.Router();
const serverState = require("../js/ServerState");

router.get('/', (req, res) => {
    if(!serverState.isAuthenticated(req)) {
        res.redirect("/login");
        return;
    }
    let cookie = req.cookies['logged-in'];
    if(!serverState.isCookieValid(cookie) || !serverState.userExists(cookie)) {
        res.clearCookie('logged-in');
        res.redirect("/login");
        return;
    }

    res.render("pages/profile")
})

router.post('/', (req, res) => {
    serverState.setupProfile(req);
    res.status(200).end();
});

module.exports = {url: "/profile", router: router};