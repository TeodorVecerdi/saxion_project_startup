let express = require('express');
let router = express.Router();
let serverState = require("../js/ServerState");

router.get('/', (req, res, next) => {
    if(serverState.isAuthenticated(req)) {
        let cookie = req.cookies['logged-in'];
        if(serverState.isCookieValid(cookie)) {
            res.redirect("/");
            return;
        } else {
            res.clearCookie('logged-in');
        }
    }
    res.render('pages/register');
});

router.post('/', (req, res, next) => {
    let username = req.body.username;
    let password = req.body.password;
    if(username === undefined || password === undefined) {
        res.status(400).end();
        return;
    }

    if(serverState.usernameExists(username)) {
        res.status(409).end();
        return;
    }

    serverState.registerUser(username, password);
    res.status(201).end();
});

module.exports = {url: "/register", router: router};