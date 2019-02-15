const Sequelize = require('sequelize');
const express = require('express')
const bodyParser = require('body-parser');
const app = express()
const port = 3000

const sequelize = new Sequelize('postgres://user:pass@skeggib.com:6543/notes');


const User = sequelize.define('user', {
    name: {
        type: Sequelize.STRING
    }
}
);

const Note = sequelize.define('note', {
    title: {
        type: Sequelize.STRING
    },
    content: {
        type: Sequelize.STRING
    }
}
);

app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());

app.get('/users', function (request, response) {
    User.findAll().then(function (value) {
        let result = [];
        console.log(value);
        for (let val of value) {
            result.push(val);
        }

        response.json(result);
    })
});


app.get('/notes', function (request, response) {
    Note.findAll().then(function (value) {
        let result = [];

        for (let val of value) {
            result.push(val);
        }
        response.json(result);
    })
});

app.listen(port, function () {
    console.log(`Your app is listening on ${port}!`);
});

app.post('/users', function (req, res) {
    User.create({
        name: req.body.name
    }).then(function () {
        res.status(201).end();

    }).catch(function () {
        res.status(400).end();
    });
}
);


