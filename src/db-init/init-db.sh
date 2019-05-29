#!/bin/bash

psql -U postgres -c "CREATE DATABASE chitchat_users"
psql -U postgres -c "CREATE DATABASE chitchat_reports"
psql -U postgres -c "CREATE DATABASE chitchat_discussions"
psql -U postgres -d chitchat_users -f /docker-entrypoint-initdb.d/init-chitchat-users.txt
# psql -U postgres -d chitchat_users -f /docker-entrypoint-initdb.d/init-chitchat-reports.txt
# psql -U postgres -d chitchat_users -f /docker-entrypoint-initdb.d/init-chitchat-discussions.txt