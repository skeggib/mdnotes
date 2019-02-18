const Sequelize = require('sequelize');
const express = require('express')
const bodyParser = require('body-parser');
const app = express()
const port = 3000

import * as config from '../config/config.json';

const sequelize = new Sequelize(`postgres://${config.database.user}:${config.database.password}@${config.database.host}:${config.database.port}/${config.database.name}`);


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
        },
        owner: {
            type: Sequelize.INTEGER
        }
    }
);

app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());


//-------------------------USER----------------------------------

//Retourne tous les users avec leur(s) note(s)
app.get('/users', function (req, res) {
    User.findAll().then(function (value) {
        let result = [];
        for (let val of value) {
            result.push(val);
        }
        res.status(201).json(result);
    })
});

//Ajoute un user
app.post('/users', function (req, res) {
        User.create({
            name: req.body.name
        }).then(function (userNew) {
            res.status(201).json(userNew).end();
        }).catch(function () {
            res.status(404).end();
        });
    }
);

//Supprime User avec user_id = id
app.delete('/users/:id', function(req, res) {
    let user_id = req.params.id;

    User.findById(user_id).then(function(user) {
        if(user != null)
        {
            user.destroy().then(function(){
                res.status(200).end();
            })
        }else{
            res.status(404).end();
        }
        
    })
});

//Modifie le name du user avec l'id user_id
app.put('/users/:id', function(req, res) {
    let user_id = req.params.id;

    User.findById(user_id).then(function(user) {
        if(user != null)
        {
            user.name = req.body.name;
            user.save().then(function(user_update) {
                res.status(200).json(user_update).end();
            });
        }else{
            res.status(404).json('The User with the id : ' + user_id + ' does not exist.');
        }
    })
});

//-------------------------NOTES----------------------------------

//Retourne toutes les notes
app.get('/notes', function (req, res) {
    Note.findAll().then(function (value) {
        let result = [];

        for (let val of value) {
            result.push(val);
        }
        res.json(result);
    })
});

//Retourne la note du user avec id=note_id
app.get('/notes/:id', function (req, res) {
    Note.findById(req.params.id).then(function (note) {
        if(note != null)
        {
            res.status(200).json(note).end();
        }else{
            res.status(404).end();
        }
    })
});

//Ajoute une note
app.post('/notes', function (req, res) {
    Note.create({
        title: req.body.title,
        content: req.body.content,
        owner: req.query.user_id
    }).then(function (noteNew) {
        res.status(200).json(noteNew).end();
    }).catch(function () {
        res.status(404).end();
    });
});

//Modifie la note avec id=note_id
app.put('/notes/:id', function (req, res) {
    Note.findById(req.params.id).then(function (note) {
        if(note != null)
        {
            note.title = req.body.title;
            note.content = req.body.content;

            note.save().then(function(note_update) {
                res.status(200).json(note_update).end();
            });
        }else{
            res.status(404).end();
        }
    })
});

//Supprime une note
app.delete('/notes/:id', function(req, res) {
    Note.findById(req.params.id).then(function (note) {
        if(note != null)
        {
            note.destroy().then(function(){
                res.status(200).end();
            })
        }else{
            res.status(404).end();
        }
        
    })
});

app.listen(port, function () {
    console.log(`Your app is listening on ${port}!`);
});