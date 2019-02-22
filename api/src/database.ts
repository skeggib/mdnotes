import Sequelize from 'sequelize';
import config from 'config';

var host = config.get('database.host');
var port = config.get('database.port');
var user = config.get('database.user');
var password = config.get('database.password');
var name = config.get('database.name');

export const dbConnexionString = 
    "postgres://" +
    `${user}:${password}` +
    `@${host}:${port}` +
    `/${name}`;

export const sequelize = new Sequelize(dbConnexionString);

export class UserModel {
    name: String
}

export const User = sequelize.define<UserModel, {}>('user', {
    name: {
        type: Sequelize.STRING
    }
});

export const Note = sequelize.define('note', {
    title: {
        type: Sequelize.STRING
    },
    content: {
        type: Sequelize.STRING
    },
    owner: {
        type: Sequelize.INTEGER
    }
});

export const Key = sequelize.define('key', {
    key: {
        type: Sequelize.STRING
    },
    owner: {
        type: Sequelize.INTEGER
    }
});