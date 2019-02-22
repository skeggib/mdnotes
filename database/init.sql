CREATE TABLE users
(
    id SERIAL,
    name VARCHAR(256) NOT NULL,
    "createdAt" TIMESTAMP,
    "updatedAt" TIMESTAMP,

    PRIMARY KEY (id)
);

CREATE TABLE notes
(
    id SERIAL,
    title VARCHAR(4096) NOT NULL,
    content TEXT NOT NULL,
    owner INTEGER NOT NULL,
    "createdAt" TIMESTAMP,
    "updatedAt" TIMESTAMP,

    PRIMARY KEY (id),
    FOREIGN KEY (owner) REFERENCES users(id)
);

CREATE TABLE keys
(
    id SERIAL,
    key VARCHAR UNIQUE NOT NULL,
    owner INTEGER NOT NULL,
    "createdAt" TIMESTAMP,
    "updatedAt" TIMESTAMP,

    PRIMARY KEY (id),
    FOREIGN KEY (owner) REFERENCES users(id)
);