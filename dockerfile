FROM microsoft/microsoft/aspnetcore
ARG source
WORKDIR /app
EXPOSE 80
COPY $(source:~obj/Docker/publish) .
ENTRYPOINT [ "dotnet", "chit-chat-api.dll" ]