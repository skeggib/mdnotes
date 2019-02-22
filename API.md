# API documentation

`mdnotes` is a markdown notebook API that allows users to store, update and get 
markdown documents.

## Users

### Create an user

`POST /users`

Body:

```json
{
    "name": "<username>"
}
```

Response:

- `201 Created`: the user was created
    ```json
    {   
        "user": {
            "id": "<id>",
            "name": "<name>",
            "updatedAt": "<updatedAt>",
            "createdAt": "<createdAt>"
        },
        "defaultKey": "<defaultApiKey>"
    }
    ```
- `400 Bad Request`: invalid body content
- `409 Conflict`: the username already exists

### Get an user

`GET /users/<id>`

Response:

- `200 OK`
    ```json
    {   
        "id": "<id>",
        "name": "<name>",
        "updatedAt": "<updatedAt>",
        "createdAt": "<createdAt>" 
    }
    ```
- `404 Not Found`: the user does not exist

### Get all users

`GET /users`

- `200 OK`: a list of users in JSON format

### Update an user

`PUT /users/<id>`

Headers:

- `Authorization: Bearer <apiKey>`

Body:

```json
{
    "name": "<username>"
}
```

Response:

- `200 OK`: the user was updated
- `400 Bad Request`: invalid body content
- `400 Bad Request`: missing API key
- `401 Unauthorized`: invalid API key
- `404 Not Found`: the user does not exist

### Delete an user

`DELETE /users/<id>`

Headers:

- `Authorization: Bearer <apiKey>`

Response:

- `200 OK`: the user was deleted
- `400 Bad Request`: missing API key
- `401 Unauthorized`: invalid API key
- `404 Not Found`: the user does not exist

## Markdown notes

### Create a note

`POST /notes`

Headers:

- `Authorization: Bearer <apiKey>`

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
    {   
        "id": "<id>",
        "title": "<title>",
        "content": "<content>",
        "owner": "<owner>",
        "updatedAt": "<updatedAt>",
        "createdAt": "<createdAt>"
    }
    ```
- `400 Bad Request`: invalid body content
- `400 Bad Request`: missing API key
- `401 Unauthorized`: invalid API key


### Get a note

`GET /notes/<id>`

Headers:

- `Authorization: Bearer <apiKey>`

Response:

- `200 OK`
    ```json
    {   
        "id": "<id>",
        "title": "<title>",
        "content": "<content>",
        "owner": "<owner>",
        "updatedAt": "<updatedAt>",
        "createdAt": "<createdAt>"
    }
    ```
- `400 Bad Request`: missing API key
- `401 Unauthorized`: invalid API key
- `404 Not Found`: the note does not exist

### Get all notes

`GET /notes`

Headers:

- `Authorization: Bearer <apiKey>`

Response:

- `200 OK`, list of all notes in JSON format
    ```json
    [
        { /* note */ },
        { /* note */ },
        /* ... */
    ]
    ```
- `400 Bad Request`: missing API key
- `401 Unauthorized`: invalid API key

### Edit a note

`PUT /notes/<id>`

Headers:

- `Authorization: Bearer <apiKey>`

Body:

```json
{
    "title": "<title>",
    "content": "<markdown>"
}
```

Response:

- `200 OK`: the note was updated
- `400 Bad Request`: invalid body content
- `400 Bad Request`: missing API key
- `401 Unauthorized`: invalid API key
- `404 Not Found`: the note does not exist

### Delete a note

`DELETE /notes/<id>`

Headers:

- `Authorization: Bearer <apiKey>`

Response:

- `200 OK`: the note was deleted
- `400 Bad Request`: missing API key
- `401 Unauthorized`: invalid API key
- `404 Not Found`: the note does not exist