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
- `400 Bad Request`: invalid body content
- `409 Conflict`: the username already exists

### Delete an user

`DELETE /users/<username>`

Query parameters: none.

Body: none.

Response:

- `200 OK`: the user was deleted
- `404 Not Found`: the user does not exists

## Markdown notes

### Create a note

`POST /<username>/notes`

Query parameters: none.

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
        "id": <note_id>
    }
    ```
- `400 Bad Request`: invalid body content

### Get a note

`GET /<username>/notes/<id>`

Query parameters: none.

Body: none.

Response:

- `200 OK`
    ```json
    {
        "id": <note_id>
        "title": "<title>",
        "content": "<markdown>"
    }
    ```
- `404 Not Found`: the note does not exists

### Edit a note

`PUT /<username>/notes/<id>`

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
- `400 Bad Request`: invalid body content
- `404 Not Found`: the note does not exists

### Delete a note

`PUT /<username>/notes/<id>`

Query parameters: none.

Body: none.

Response:

- `200 OK`: the note was deleted
- `404 Not Found`: the note does not exists

