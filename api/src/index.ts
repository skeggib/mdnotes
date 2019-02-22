import express from 'express';
import bodyParser from 'body-parser';
import { User } from './database';
import { Note } from './database';
import { Key } from './database';
import crypto from 'crypto';
import bearerToken from 'express-bearer-token';

import MarkdownIt from 'markdown-it';
import MarkdownItMath from 'markdown-it-math';
import { Authorization } from './Authorization';

const md = MarkdownIt();
md.use(MarkdownItMath);

const app = express();
const port = 3000;

app.use(bearerToken());
app.use(Authorization);
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
app.delete('/users/:id', (request, response) => {
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
    User.findById(request.params.id).then(user => {
        if (user == null) {
            response.status(404).end();
            return;
        }

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
            response.json(result);
        }).catch(error => {
            response.status(500).json({ "error": error });
        });
    });
});

/**
 * Returns all notes of the user.
 */
app.get('/notes', (request, response) => {
    if (request.params.id != response.locals.userId) {
        response.status(401).end();
        return;
    }

    Note.findAll({ where: { owner: response.locals.userId } }).then(function (value) {
        let result = [];
        for (let val of value) {
            result.push(val);
        }
        response.status(200).json(result);
    }).catch(error => {
        response.status(500).json({ "error": error });
    });
});

/**
 * Gets a note.
 */
app.get('/notes/:id', (request, response) => {
    Note.findById(request.params.id).then((note:any) => {
        if (note != null) {
            if (note.owner != response.locals.userId)
                response.status(401).end();
            else
                response.status(200).json(note);
        }
        else
            response.status(404).end();
    }).catch(error => {
        response.status(500).json({ "error": error });
    });
});

/**
 * Gets a rendered note.
 */
app.get('/notes/:id/html', (request, response) => {
    Note.findById(request.params.id).then((note: any) => {
        if (note != null) {
            if (note.owner != response.locals.userId)
                response.status(401).end();
            else {
                var html = md.render(note.content);
                response.status(200).send(html);
            }
            
        } else {
            response.status(404).json({ "error": "Not found" });
        }
    });
});

/**
 * Creates a note.
 */
app.post('/notes', (request, response) => {
    if (response.locals.userId == null) {
        response.status(401).end();
        return;
    }

    if (request.body.title == null || request.body.content == null) {
        response.status(400).json({ "error": "Invalid request body content. "});
        return;
    }
    
    User.findById(response.locals.userId).then(user => {
        Note.create({
            title: request.body.title,
            content: request.body.content,
            owner: response.locals.userId
        }).then(function (noteNew) {
            response.status(201).json(noteNew).end();
        }).catch(function (error) {
            response.status(500).json({ "error": error });
        });
    }).catch(error => {
        response.status(500).json({ "error": error });
    });
});

/**
 * Updates a note.
 */
app.put('/notes/:id', (request, response) => {
    Note.findById(request.params.id).then((note: any) => {
        if (note != null) {
            if (note.owner == response.locals.userId) {
                note.title = request.body.title;
                note.content = request.body.content;
                note.save().then(updatedNote => {
                    response.status(200).json(updatedNote);
                });
            } else {
                response.status(401).end();
            }
        } else {
            response.status(404).json({ "error": "Not found" });
        }
    }).catch(error => {
        response.status(500).json({ "error": error });
    });
});

/**
 * Deletes a note.
 */
app.delete('/notes/:id', (request, response) => {
    Note.findById(request.params.id).then((note: any) => {
        if (note != null) {
            if (note.owner == response.locals.userId) {
                note.destroy().then(function () {
                    response.status(200).end();
                });
            } else {
                response.status(401).end();
            }
        } else {
            response.status(404).json({ "error": "Not found" });
        }
    });
});

app.listen(port, function () {
    console.log(`Your app is listening on ${port}!`);
});