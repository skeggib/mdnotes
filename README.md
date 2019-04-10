# mdnotes

Markdown notebook API

## API documentation

[API.md](API.md)

## Building and running docker containers

```bash
docker build database -t mdnotes_db
docker build api -t mdnotes_api
docker-compose up
```

## Testing

With dotnet and an API running on `localhost:3000`:

```bash
dotnet test tests/MdNotes.Tests
```

