let express = require('express');
let router = express.Router();
let serverState = require("../js/ServerState");
const fs = require('fs');

router.get('/ping', (req, res) => {
    console.log(serverState);
    res.render("pages/empty")
})

router.get('/backup', (req, res) => {
    fs.writeFile("./data/backup.json", JSON.stringify(serverState), err => {
        if(err) throw err;
        console.log("Backup success!");
        res.render("pages/empty")
    })
})

router.get('/restore', (req, res) => {
    // todo: restore if exists
    fs.access("./data/backup.json", err => {
        if (err === null) {
            fs.readFile("./data/backup.json", (err1, data) => {
                if(err1) throw err1;
                let backup = JSON.parse(data);
                serverState.load(backup);
                serverState.restored = true;
                console.log(serverState);
            });
        } else {
            console.log(err);
        }
        res.render("pages/empty")
    })
})

module.exports = {url: "/dev", router: router};