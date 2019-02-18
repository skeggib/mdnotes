import Sequelize from 'sequelize';
import * as config from '../config/config.json';
import { userInfo } from 'os';

export const dbConnexionString = 
    "postgres://" +
    `${config.database.user}:${config.database.password}` +
    `@${config.database.host}:${config.database.port}` +
    `/${config.database.name}`;

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