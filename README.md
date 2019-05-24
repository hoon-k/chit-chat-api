# chit-chat-api
Supports simple discussion forum.

Dev
```
docker-compose -f docker-compose.yml -f docker-compose.dev.yml up -d
docker-compose -f docker-compose.yml -f docker-compose.dev.yml down
```

Prod
```
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d
docker-compose -f docker-compose.yml -f docker-compose.prod.yml down
```

Install .NET CLI tools
```
 dotnet tool install --global dotnet-aspnet-codegenerator
```
Add ```~/.dotnet/tools``` to PATH.

Add MVC controller (after installing dotnet-aspnet-codegenerator)
```
dotnet aspnet-codegenerator controller -name <controller_class_name> -actions -api -outDir Controllers
```

Inidividual docker build
```
docker build -f src/user-api/dockerfile -t user.api .
```

Individual docker run
```
docker run -it --rm -p 8000:80  --name userapi  user.api:latest
```