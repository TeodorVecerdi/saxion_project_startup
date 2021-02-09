let express = require('express');
let router = express.Router();
let serverState = require("../js/ServerState");

/* GET home page. */
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
    res.render('pages/login');
});

router.post('/', (req, res, next) => {
    let username = req.body.username;
    let password = req.body.password;

    if(username === undefined || password === undefined) {
        res.status(400).end();
        return;
    }

    if(!serverState.usernameExists(username) || !serverState.passwordMatches(username, password)) {
        res.status(404).end();
        return;
    }

    let userID = serverState.loginUser(username);
    res.cookie('logged-in', {'username': username, 'id': userID});
    res.status(200).end();
});

module.exports = {url: "/login", router: router};