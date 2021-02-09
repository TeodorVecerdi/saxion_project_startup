let express = require('express');
let router = express.Router();
let serverState = require("../js/ServerState");

/* GET home page. */
router.get('/', (req, res, next) => {
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
    res.render('pages/index');
});

router.get('/logout', (req, res, next) => {
    if(serverState.isAuthenticated(req)) {
        let loginCookie = req.cookies['logged-in'];
        res.clearCookie('logged-in');

        if(!serverState.logoutUser(loginCookie['id'])) {
            res.status(403).render('pages/error.ejs', {
                message_main: `Trying to log out when user doesn't exist! (403)`,
                message_redirect: `Click <a href=\"/\">here</a> to go back to home`,
                message_page: ""
            });
            return;
        }

        res.redirect('/login');
        return;
    }
    res.status(404).render('pages/error.ejs', {
        message_main: `You are not logged in! (404)`,
        message_redirect: `Click <a href=\"/\">here</a> to login`,
        message_page: ""
    })
})

module.exports = {url: "/", router: router};