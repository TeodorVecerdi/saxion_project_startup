const createError = require('http-errors');
const express = require('express');
const path = require('path');
const cookieParser = require('cookie-parser');
const logger = require('morgan');
const bodyParser = require('body-parser');
const https = require('https');
const sio = require('socket.io')
const fs = require('fs')
const cors = require('cors');

const socketSetup = require('./js/SocketSetup');

let app = express();
const server = https.createServer({
    key: fs.readFileSync('cert/server.key'),
    cert: fs.readFileSync('cert/server.cert')
}, app);
let io = sio(server);

app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'ejs');

app.use(logger('dev'));
app.use(express.json());
app.use(express.urlencoded({extended: true}));
app.use(express.static(path.join(__dirname, 'public')));
app.use(cookieParser());
app.use(bodyParser.json());
app.use(cors())

let routes = require('./routes/_router');
routes.forEach(route => app.use(route.url, route.router));

io.on('connection', socket => {
    socketSetup(socket);
})

// catch 404 and forward to error handler
app.use(function (req, res, next) {
    next(createError(404));
});

// error handler
app.use(function (err, req, res, next) {
    // set locals, only providing error in development
    res.locals.message = err.message;
    res.locals.error = req.app.get('env') === 'development' ? err : {};
    console.error(err);
    // render the error page
    let status = err.status || 500;
    res.status(err.status || 500);
    res.render('pages/error.ejs', {
        message_main: `The page you're looking for could not be found (${status})`,
        message_redirect: `Click <a href=\"/\">here</a> to go back to home`,
        message_page: "Requested page: " + req.url.substr(0)
    })
});

module.exports = {app: app, server: server};