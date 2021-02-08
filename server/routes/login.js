let express = require('express');
let router = express.Router();
let serverState = require("../js/ServerState");

/* GET home page. */
router.get('/', (req, res, next) => {
    if(req.cookies.hasOwnProperty("logged-in")) {
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

router.post('/set-user', (req, res, next) => {
    let username = req.body.username;
    if(username === undefined || username === "") {
        console.log(`Username is undefined: ${username}`)
        res.status(404).end()
        return;
    }

    let userID = serverState.loginUser(username);
    res.cookie('logged-in', {'username': req.body.username, 'id': userID});
    res.status(201).end();
});

module.exports = {url: "/login", router: router};