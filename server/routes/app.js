let express = require('express');
let router = express.Router();

router.get('/', (req, res) => {
    res.redirect("https://teodorvecerdi.github.io/saxion_project_startup/");
})


module.exports = {url: "/app", router: router};
