import express from 'express';
import bodyParser from 'body-parser';
import { User } from './database';
import { Note } from './database';
import jwt from 'jsonwebtoken';
const app = express();
const port = 3000;
const ProtectedRoutes = express.Router();

app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());
app.set('Secret', "application_cd_dm_sk");

app.use('/api', ProtectedRoutes);

ProtectedRoutes.use((req, res, next) => {


    // check header for the token
    var token = req.headers['access-token'];

    // decode token
    if (token) {

        // verifies secret and checks if the token is expired
        jwt.verify(token, app.get('Secret'), (err, decoded) => {
            if (err) {
                return res.status(404).json({ message: 'invalid token' });
            } else {
                next();
            }
        });

    } else {

        // if there is no token  

        res.send({

            message: 'No token provided.'
        });

    }
});

app.post('/authenticate', (req, res) => {
    User.count({ where: { 'name': req.body.name } }).then(c => {
        if (c >= 1) {

            const payload = {

                check: true

            };

            var token = jwt.sign(payload, app.get('Secret'), {
                expiresIn: 1440 // expires in 24 hours

            });


            res.json({
                message: 'authentication done ',
                token: token
            });
        } else {

            res.status(404).json({ message: "user not found !" })

        }

    }).catch(error => {
        res.status(400).json({ "error": error });
    });
});

/**
 * Gets a list of all users.
 */
app.get('/api/users', (request, response) => {
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
 * Adds an user.
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
            }).then(value => {
                response.status(201).json(value);
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
    User.destroy({
        "where": {
            "id": request.params.id
        }
    }).then(count => {
        if (count > 0)
            response.end();
        else
            response.status(404).json({ "error": "Not found" });
    }).catch(error => {
        response.status(500).json({ "error": error });
    });
});

/**
 * Updates an user by ID.
 */
app.put('/users/:id', (request, response) => {
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
 * Returns all notes (filtered by user if user_id is set).
 */
app.get('/notes', function (req, res) {
    if (req.query.user_id != null) {
        Note.findAll({ where: { owner: req.query.user_id } }).then(function (value) {
            let result = [];

            for (let val of value) {
                result.push(val);
            }
            res.status(200).json(result);
        }).catch(error => {
            res.status(500).json({ "error": error });
        });
    }
    else {
        Note.findAll().then(function (value) {
            let result = [];

            for (let val of value) {
                result.push(val);
            }
            res.status(200).json(result);
        }).catch(error => {
            res.status(500).json({ "error": error });
        });;
    }

});

/**
 * Gets a note.
 */
app.get('/notes/:id', function (req, res) {
    Note.findById(req.params.id).then(note => {
        if (note != null)
            res.status(200).json(note);
        else
            res.status(404).json({ "error": "Not found" });
    }).catch(error => {
        res.status(500).json({ "error": error });
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

    if (req.query.user_id == null) {
        res.status(400).json({ "error": "Missing parameter 'user_id'. "});
        return;
    }
    
    User.findById(req.query.user_id).then(user => {
        if (user == null)
            res.status(400).json({ "error": "The user_id parameter references a user that does not exists." });
        else {
            Note.create({
                title: req.body.title,
                content: req.body.content,
                owner: req.query.user_id
            }).then(function (noteNew) {
                res.status(201).json(noteNew).end();
            }).catch(function (error) {
                res.status(500).json({ "error": error });
            });
        }
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
            note.title = req.body.title;
            note.content = req.body.content;

            note.save().then(updatedNote => {
                res.status(200).json(updatedNote);
            });
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
            note.destroy().then(function () {
                res.status(200).end();
            })
        } else {
            res.status(404).json({ "error": "Not found" });
        }

    });
});

app.listen(port, function () {
    console.log(`Your app is listening on ${port}!`);
});