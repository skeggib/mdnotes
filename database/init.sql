CREATE TABLE users
(
    name VARCHAR(256),
    "createdAt" TIMESTAMP,
    "updatedAt" TIMESTAMP,

    PRIMARY KEY (name)
);

CREATE TABLE notes
(
    id SERIAL,
    title VARCHAR(4096) NOT NULL,
    content TEXT NOT NULL,
    owner VARCHAR(256) NOT NULL,
    "createdAt" TIMESTAMP,
    "updatedAt" TIMESTAMP,

    PRIMARY KEY (id),
    FOREIGN KEY (owner) REFERENCES users(name)
);
