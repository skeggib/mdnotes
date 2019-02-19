# API documentation

`mdnotes` is a markdown notebook API that allows users to store, update and get 
markdown documents.

## Users

### Create an user

`POST /users`

Query parameters: none.

Body:

```json
{
    "name": "<username>"
}
```

Response:

- `201 Created`: the user was created
    ```json
    {   "id": "<id>",
        "name": "<name>",
        "updatedAt": "<updatedAt>",
        "createdAt": "<createdAt>" 
    }
    ```
- `400 Bad Request`: invalid body content
- `409 Conflict`: the username already exists

### Get an user

`GET /users/`

Query parameters: none.

Body: none.

Response:

- `200 OK`
    ```json
    {   "id": "<id>",
        "name": "<name>",
        "updatedAt": "<updatedAt>",
        "createdAt": "<createdAt>" 
    }
    ```

### Update an user

`PUT /users/<id>`

Query parameters: none.

Body:

```json
{
    "name": "<username>"
}
```

Response:

- `200 OK`: the user was updated
- `400 Bad Request`: invalid body content
- `404 Not Found`: the user does not exists

### Delete an user

`DELETE /users/<id>`

Query parameters: none.

Body: none.

Response:

- `200 OK`: the user was deleted
- `400 Bad Request`: invalid param content
- `404 Not Found`: the user does not exists

## Markdown notes

### Create a note

`POST /notes`

Query parameters:

- `user_id=<id>`

Body:

```json
{
    "title": "<title>",
    "content": "<markdown>"
}
```

Response:

- `201 Created`: the note was created
    ```json
    {   "id": "<id>",
        "title": "<title>",
        "content": "<content>",
        "owner": "<owner>",
        "updatedAt": "<updatedAt>",
        "createdAt": "<createdAt>"
    }
    ```
- `400 Bad Request`: invalid body content

### Get a note

`GET /notes/<id>`

Query parameters: none.

Body: none.

Response:

- `200 OK`
    ```json
    {   "id": "<id>",
        "title": "<title>",
        "content": "<content>",
        "owner": "<owner>",
        "updatedAt": "<updatedAt>",
        "createdAt": "<createdAt>"
    }
    ```
- `404 Not Found`: the note does not exists

### Get all notes

`GET /notes`

Query parameters:

- `user_id=<id>` (optional)

Body: none.

Response:

- `200 OK`, list of all notes in JSON format
- `404 Not Found`: the user does not exists


### Edit a note

`PUT /notes/<id>`

Query parameters: none.

Body:

```json
{
    "title": "<title>",
    "content": "<markdown>"
}
```

Response:

- `200 OK`: the note was updated
    ```json
    {   "id": "<id>",
        "title": "<title>",
        "content": "<content>",
        "owner": "<owner>",
        "updatedAt": "<updatedAt>",
        "createdAt": "<createdAt>"
    }
    ```
- `400 Bad Request`: invalid body content
- `404 Not Found`: the note does not exists

### Delete a note

`PUT /notes/<id>`

Query parameters: none.

Body: none.

Response:

- `200 OK`: the note was deleted
- `404 Not Found`: the note does not exists

