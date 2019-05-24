# chit-chat-api
Supports simple discussion forum.

Dev
```
docker-compose -f docker-compose.yml -f docker-compose.dev.yml up -d
docker-compose -f docker-compose.yml -f docker-compose.test.override.yml down
```

Prod
```
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d
docker-compose -f docker-compose.yml -f docker-compose.test.override.yml down
```

Install .NET CLI tools
```
 dotnet tool install --global dotnet-aspnet-codegenerator
```

Add MVC controller (after installing dotnet-aspnet-codegenerator)
```
dotnet-aspnet-codegenerator
```

Inidividual docker build
```
docker build -f src/user-api/dockerfile -t user.api .
```