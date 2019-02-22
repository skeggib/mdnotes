import { Key } from './database';
import Express from 'express';

/**
 * Checks if request.token exists. If it does, retreive the associated user and stores its ID in response.locals.UserId.
 * @param request 
 * @param response 
 * @param next 
 */
export function Authorization(request: Express.Request, response, next) {

    var token = (request as any).token;
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