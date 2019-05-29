using System;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace ChitChatAPI.UserAPI
{
    public class DataBaseSeedAsync 
    {
        public static async Task SeedAsync(string connectionString) 
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                await CreateDataBase(connection);
                await CreateTables(connection);
            }
        }

        private static async Task CreateDataBase(NpgsqlConnection cnx)
        {
            var sql = @"
                -- Create chitchat_users database.
                CREATE DATABASE chitchat_users
                    WITH 
                    OWNER = postgres
                    ENCODING = 'UTF8'
                    LC_COLLATE = 'C'
                    LC_CTYPE = 'C'
                    TABLESPACE = pg_default
                    CONNECTION LIMIT = -1;
            ";

            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = cnx;
                cmd.CommandText = sql;
                await cmd.ExecuteNonQueryAsync();
            }
        }

        private static async Task CreateTables(NpgsqlConnection cnx)
        {
            var sql = @"
                CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";
                CREATE EXTENSION IF NOT EXISTS pgcrypto;
                CREATE TYPE roles AS ENUM ('member', 'admin', 'moderator');

                -- Create users table.
                CREATE TABLE users
                (
                    uuid uuid NOT NULL DEFAULT uuid_generate_v4(),
                    first_name character varying(255) NOT NULL,
                    last_name character varying(255) NOT NULL,
                    CONSTRAINT users_pkey PRIMARY KEY (uuid)
                )

                -- Create account table.
                CREATE TABLE account
                (
                    username character varying(255) NOT NULL,
                    password character varying(255) NOT NULL,
                    uuid uuid NOT NULL DEFAULT uuid_generate_v4(),
                    active boolean NOT NULL DEFAULT true,
                    role roles NOT NULL DEFAULT 'member'::roles,
                    user_id uuid NOT NULL,
                    CONSTRAINT account_pkey PRIMARY KEY (uuid)
                    CONSTRAINT account_fkey FOREIGN KEY (user_id),
                        REFERENCES users (uuid) MATCH SIMPLE
                        ON UPDATE NO ACTION
                        ON DELETE NO ACTION
                )

                -- Create stored procedre to create a member
                CREATE OR REPLACE PROCEDURE create_member_user(un character varying, pw character varying, fn character varying, ln character varying)
                LANGUAGE 'sql'
                AS $BODY$
                    WITH user_insert AS (
                        INSERT 
                        INTO users (first_name, last_name)
                        VALUES (fn, ln)
                        RETURNING uuid
                    )

                    INSERT
                    INTO account (
                        username,
                        password,
                        role,
                        user_id)
                    VALUES (
                        un,
                        crypt(pw, gen_salt('bf', 8)),
                        'member',
                        (SELECT uuid FROM user_insert)
                    );
                $BODY$;

                -- Create initial admin user.
                CALL create_member_user('admin', 'admin', 'Admin', 'User')
            ";

            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = cnx;
                cmd.CommandText = sql;
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}