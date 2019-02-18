import express from 'express';
import bodyParser from 'body-parser';
import {User} from './database';
import {Note} from './database';

const app = express();
const port = 3000;

app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());

/**
 * Gets a list of all users.
 */
app.get('/users', (request, response) => {
    User.findAll().then(users => {
        response.json(users);
    });
});

/**
 * Adds an user.
 */
app.post('/users', (request, response) => {
    User.create({
        name: request.body.name
    }).then(value => {
        response.status(201).json(value);
    }).catch(error => {
        response.status(400).json({ "error": error });
    });
});


/**
 * Deletes an user by ID.
 */
app.delete('/users/:id', (request, response) => {
    User.destroy({
        "where": {
            "id": request.params.id
        }
    }).then(count => {
        if (count > 0)
            response.end();
        else
            response.status(404).end();
    }).catch(error => {
        response.status(400).json({ "error": error });
    });
});

/**
 * Updates an user by ID.
 */
app.put('/users/:id', (request, response) => {
    User.update(request.body, {
        where: { id: request.params.id }
    }).then(result => {
        var count = result[0];
        if (count > 0)
            response.end();
        else
            response.status(404).end();
    }).catch(error => {
        response.status(400).json({ "error": error });
    });
});

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
    Note.findById(req.params.id).then((note: any) => {
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
    Note.findById(req.params.id).then((note: any) => {
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