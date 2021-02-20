let express = require('express');
let router = express.Router();
const serverState = require("../js/ServerState");

router.get('/', (req, res) => {
    res.send(serverState.getUsers()).status(200).end();
});

router.get('/swipes', (req, res) => {
    let swipes = serverState.getSwipes(req.query.id)
    console.log(`Getting swipes: ${JSON.stringify(swipes)}`)
    res.send(swipes).status(200).end();
})
router.get('/matches', (req, res) => {
    let matches = serverState.getMatches(req.query.id)
    console.log(`Getting matches: ${JSON.stringify(matches)}`)
    res.send(matches).status(200).end();
})
router.get('/unconfirmed-matches', (req, res) => {
    let matches = serverState.getUnconfirmedMatches(req.query.id)
    console.log(`Getting unconfirmed matches: ${JSON.stringify(matches)}`)
    res.send(matches).status(200).end();
})
router.get('/has-match', (req, res) => {
    let hasMatch = serverState.hasMatch(req.query.from, req.query.to);
    console.log(`Has match?: ${JSON.stringify(hasMatch)}`)
    res.send(hasMatch).status(200).end();
})
router.post('/swipe', (req, res) => {
    console.log(`Swiping from ${req.body.from} on ${req.body.to} with ${req.body.swipe}`);
    serverState.swipe(req.body.from, req.body.to, req.body.swipe);
    res.status(200).end();
})

router.post('/confirm-matches', (req, res) => {
    console.log(`Confirming matches from ${req.body.id} with ${req.body.ids}`);
    serverState.confirmMatches(req.body.id, JSON.parse(req.body.ids));
    res.status(200).end();
})

router.get('/likes-count', (req, res) => {
    let likesCount = serverState.getLikesCount(req.query.id);
    console.log(`Getting likes count for user ${req.query.id} => ${likesCount}`);
    res.send(`${likesCount}`).status(200).end();
})

module.exports = {url: "/users", router: router};