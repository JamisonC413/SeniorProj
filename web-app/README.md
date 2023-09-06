## Installation Instructions

### Software Requirements

You must have the following installed before running this project:
* Docker
* Node.js
* Python3
* MySQL

### Setup

Start by cloning this repository. Add an **.env** file at the root of the project, with the following contents, making sure to add values for each
of the empty fields. These values will be loaded into the backend and used to access the database we will be using.

```
MYSQL_HOST=database # Must match database service in docker-compose.yml
MYSQL_PORT=3306
MYSQL_DATABASE=
MYSQL_USER=
MYSQL_PASSWORD=
MYSQL_ROOT_PASSWORD=
```

Next, install the frontend dependencies:

```
$ cd frontend
$ npm install
```

Next, start up the project Docker containers. This may take a second.

```
$ docker-compose up --build
```

Next, **within the backend container** (which can be accessed via the Docker Desktop app by clicking on "Open in terminal" next to the running backend container), run the following:

```
$ python manage.py makemigrations
$ python manage.py migrate
```

<hr>

If you see an error similar to:

```
web-app-backend-1   | django.db.utils.OperationalError: (2002, "Can't connect to server on 'database' (115)")
```

... you may need to restart your backend container.