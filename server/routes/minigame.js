let express = require('express');
let router = express.Router();
const serverState = require("../js/ServerState");

router.get('/requests', (req, res) => {
    let id = req.query.id;
    res.send(serverState.getGameRequests(id)).status(200).end();
});
router.get('/request', (req, res) => {
    let from = req.query.from;
    let to = req.query.to;
    res.send(serverState.getGameRequest(from, to)).status(200).end();
});
router.get('/game', (req, res) => {
    let from = req.query.from;
    let to = req.query.to;
    res.send(serverState.getGame(from, to)).status(200).end();
})
router.post('/acknowledge-request-status', (req, res) => {
    let from = req.body.from;
    let to = req.body.to;
    serverState.acknowledgeRequestStatus(from, to);
    res.status(200).end();
})
router.post('/confirm-request', (req, res) => {
    let from = req.body.from;
    let to = req.body.to;
    let status = req.body.status;
    serverState.confirmGameRequest(from, to, status);
    res.status(200).end();
});
router.post('/request-game', (req, res) => {
    let from = req.body.from;
    let to = req.body.to;
    serverState.requestGame(from, to);
    res.status(200).end();
});
router.post('/add-prompts', (req, res) => {
    let from = req.body.from;
    let to = req.body.to;
    let prompts = JSON.parse(req.body.prompts);
    serverState.sendGamePrompts(from, to, prompts);
    res.status(200).end();
});
router.post('/add-answer', (req, res) => {
    let from = req.body.from;
    let to = req.body.to;
    let answer = req.body.answer;
    serverState.sendGameAnswer(from, to, answer);
    res.status(200).end();
});
router.post('/new-game', (req, res) => {
    let from = req.body.from;
    let to = req.body.to;
    serverState.newGame(from, to);
    res.status(200).end();
});
router.post('/cancel-game', (req, res) => {
    let from = req.body.from;
    let to = req.body.to;
    serverState.cancelGame(from, to);
    res.status(200).end();
});
router.post('/finish-game', (req, res) => {
    let from = req.body.from;
    let to = req.body.to;
    serverState.finishGame(from, to);
    res.status(200).end();
});
module.exports = {url: "/game", router: router};
