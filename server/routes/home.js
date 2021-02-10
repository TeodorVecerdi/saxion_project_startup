let express = require('express');
let router = express.Router();
let serverState = require("../js/ServerState");

/* GET home page. */
router.get('/', (req, res, next) => {
    if(!serverState.isAuthenticated(req) || !!serverState.userIsLoggedIn(req)) {
        res.redirect("/login");
        return;
    }

    let account = serverState.getUserByID(req.query.id);
    if(JSON.stringify(account.profile) === JSON.stringify({}))
        res.render('pages/profile');
    else res.render('pages/index');
});

router.post('/logout', (req, res, next) => {
    if(serverState.isAuthenticated(req)) {
        if(!serverState.userIsLoggedIn(req.body.id)) {
            res.status(403).send('You are not logged in!').end();
            return;
        }
        serverState.logoutUser(req.body.id);
        res.status(200).end();
        return;
    }
    res.status(400).end();
})

module.exports = {url: "/", router: router};