# WebApi
A small project to demonstrate my understanding of WebApi using EF Core, Newtonsoft, and In-Memory database framework.

## Json-file as database
From what I have experienced, there is no way to access a json-file as a database and perform CRUD operations that presist into that file. Insead, I decided to use the framework EF Core InMemory. This allows me to load the content of the file onto memory and perform all desired operations as if it was on a database server.

## Swagger UI
The Swagger UI has a pleasant look and intuitive controls to test the APIs that are being built. However, I was faced with one small challange: to make the input parameters not required. To come around that obstacle, I used the web browser for testing some of the APIs that have optional parameters. It should be possible to carry out all tests with Postman as well.

## POST and PUT
Those two operations create (POST) and update (PUT) records into the in-memory database and (for the time being) don't save those newly created records into the json-file.
