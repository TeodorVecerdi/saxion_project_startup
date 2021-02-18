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

module.exports = {url: "/", router: router};