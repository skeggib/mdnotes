import express from 'express';
import bodyParser from 'body-parser';
import { User } from './database';
import { Note } from './database';
import { Key } from './database';
import crypto from 'crypto';
import bearerToken from 'express-bearer-token'

import MarkdownIt from 'markdown-it';
import MarkdownItMath from 'markdown-it-math';

const md = MarkdownIt();
md.use(MarkdownItMath);

const app = express();
const port = 3000;

function authorizationMiddleware(request: any, response, next) {

    var token = request.token;
    if (token) {
        Key.find({
            where: { key: token }
        }).then((token: any) => {
            if (token != null)
                response.locals.userId = token.owner;
            next();
        }).catch(error => {
            response.status(500).json({ "error": error });
        });
    } else {
        next();
    }
}

app.use(bearerToken());
app.use(authorizationMiddleware);
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
 * Gets an user by ID.
 */
app.get('/users/:id', function (request, response) {
    User.findById(request.params.id).then(function (user) {
        if (user != null)
            response.status(200).json(user);
        else
            response.status(404).end();
    });
});

/**
 * Creates an user.
 */
app.post('/users', (request, response) => {
    if (request.body.name == null) {
        response.status(400).json({ "error": "Invalid request body content. "});
        return;
    }

    User.count({ where: { 'name': request.body.name } }).then(c => {
        if (c == 0) {
            User.create({
                name: request.body.name
            }).then((user: any) => {
                Key.create({ 
                    "owner": user.id, 
                    "key": crypto.randomBytes(20).toString('hex')
                }).then((key: any) => {
                    response.status(201).json({ "user": user, "defaultKey": key.key });
                }).catch(error => {
                    response.status(500).json({ "error": error });
                });
            }).catch(error => {
                response.status(500).json({ "error": error });
            });
        }
        else {
            response.status(409).json({ "error": "Conflict" });
        }
    }).catch(error => {
        response.status(500).json({ "error": error });
    });
});


/**
 * Deletes an user by ID.
 */
app.delete('/users/:id', authorizationMiddleware, (request, response) => {
    User.findById(request.params.id).then(user => {
        if (user == null) {
            response.status(404).end();
        } else {
            if (request.params.id != response.locals.userId) {
                response.status(401).end();
                return;
            }
        
            User.destroy({
                "where": {
                    "id": request.params.id
                }
            }).then(count => {
                response.end();
            }).catch(error => {
                response.status(500).json({ "error": error });
            });
        }
    });
});

/**
 * Updates an user by ID.
 */
app.put('/users/:id', (request, response) => {
    if (request.params.id != response.locals.userId) {
        response.status(401).end();
        return;
    }
    if (request.body.name == null) {
        response.status(400).json({ "error": "Invalid request body content." });
        return;
    }

    User.update(request.body, {
        where: { id: request.params.id }
    }).then(result => {
        var count = result[0];
        if (count > 0)
            response.end();
        else
            response.status(404).json({ "error": "Not found" });
    }).catch(error => {
        response.status(500).json({ "error": error });
    });
});

/**
 * Returns all notes of the user.
 */
app.get('/notes', function (req, res) {
    if (req.params.id != res.locals.userId) {
        res.status(401).end();
        return;
    }

    Note.findAll({ where: { owner: res.locals.userId } }).then(function (value) {
        let result = [];
        for (let val of value) {
            result.push(val);
        }
        res.status(200).json(result);
    }).catch(error => {
        res.status(500).json({ "error": error });
    });
});

/**
 * Gets a note.
 */
app.get('/notes/:id', function (req, res) {
    Note.findById(req.params.id).then((note:any) => {
        if (note != null) {
            if (note.owner != res.locals.userId)
                res.status(401).end();
            else
                res.status(200).json(note);
        }
        else
            res.status(404).end();
    }).catch(error => {
        res.status(500).json({ "error": error });
    });
});

/**
 * Gets a rendered note.
 */
app.get('/notes/:id/html', function (req, res) {
    Note.findById(req.params.id).then((note: any) => {
        if (note != null) {
            if (note.owner != res.locals.userId)
                res.status(401).end();
            else {
                var html = md.render(note.content);
                res.status(200).send(html);
            }
            
        } else {
            res.status(404).json({ "error": "Not found" });
        }
    });
});

/**
 * Creates a note.
 */
app.post('/notes', function (req, res) {
    if (req.body.title == null || req.body.content == null) {
        res.status(400).json({ "error": "Invalid request body content. "});
        return;
    }
    
    User.findById(res.locals.userId).then(user => {
        Note.create({
            title: req.body.title,
            content: req.body.content,
            owner: res.locals.userId
        }).then(function (noteNew) {
            res.status(201).json(noteNew).end();
        }).catch(function (error) {
            res.status(500).json({ "error": error });
        });
    }).catch(error => {
        res.status(500).json({ "error": error });
    });
});

/**
 * Updates a note.
 */
app.put('/notes/:id', function (req, res) {
    Note.findById(req.params.id).then((note: any) => {
        if (note != null) {
            if (note.owner == res.locals.userId) {
                note.title = req.body.title;
                note.content = req.body.content;
                note.save().then(updatedNote => {
                    res.status(200).json(updatedNote);
                });
            } else {
                res.status(401).end();
            }
        } else {
            res.status(404).json({ "error": "Not found" });
        }
    }).catch(error => {
        res.status(500).json({ "error": error });
    });
});

/**
 * Deletes a note.
 */
app.delete('/notes/:id', function (req, res) {
    Note.findById(req.params.id).then((note: any) => {
        if (note != null) {
            if (note.owner == res.locals.userId) {
                note.destroy().then(function () {
                    res.status(200).end();
                });
            } else {
                res.status(401).end();
            }
        } else {
            res.status(404).json({ "error": "Not found" });
        }
    });
});

app.listen(port, function () {
    console.log(`Your app is listening on ${port}!`);
});