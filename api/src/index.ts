import express from 'express';
import bodyParser from 'body-parser';
import {User} from './database';
import {Note} from './database';

const app = express();
const port = 3000;

app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());

app.get('/users', (request, response) => {
    User.findAll().then(users => {
        response.json(users);
    });
});

app.post('/users', (request, response) => {
    User.create({
        name: request.body.name
    }).then(value => {
        response.status(201).json(value);
    }).catch(error => {
        response.status(400).json({ "error": error });
    });
});

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

// //Supprime User avec user_id = id
// app.delete('/users/:id', function(req, res) {
//     let user_id = req.params.id;

//     User.findById(user_id).then(function(user) {
//         if(user != null)
//         {
//             user.destroy().then(function(){
//                 res.status(200).end();
//             })
//         }else{
//             res.status(404).end();
//         }
        
//     })
// });

// //Modifier le name du user avec l'id user_id
// app.put('/users/:id', function(req, res) {
//     let user_id = req.params.id;

//     User.findById(user_id).then(function(user) {
//         if(user != null)
//         {
//             user.name = req.body.name;
//             user.save().then(function(user_update) {
//                 res.status(200).json(user_update).end();
//             });
//         }else{
//             res.status(404).json('The User with the id : ' + user_id + ' does not exist.');
//         }
//     })
// });

//-------------------------NOTES----------------------------------

//Retourne toutes les notes
// app.get('/notes', function (req, res) {
//     Note.findAll().then(function (value) {
//         let result = [];

//         for (let val of value) {
//             result.push(val);
//         }
//         res.json(result);
//     })
// });

// app.get('/notes/:id', function (req, res) {
//     Note.findById(req.params.id).then(function (note) {
//         if(note != null)
//         {
//             res.status(200).json(note).end();
//         }else{
//             res.status(404).end();
//         }
//     })
// });

// //Ajoute une note
// app.post('/notes', function (req, res) {
//     Note.create({
//         title: req.body.title,
//         content: req.body.content,
//         owner: req.query.user_id
//     }).then(function (noteNew) {
//         res.status(200).json(noteNew).end();
//     }).catch(function () {
//         res.status(404).end();
//     });
// });

app.listen(port, function () {
    console.log(`Your app is listening on ${port}!`);
});