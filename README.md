# Computorv1

On macOS: spin up a linux machine with Docker
```shellscript
docker-compose up --detach
docker-compose exec computorv1 bash
```

And then build and test like this:
```shellscript
make;
make test
```
