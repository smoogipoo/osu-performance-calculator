﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using MySqlConnector;

namespace osu.Server.PerformanceCalculator
{
    public class Database
    {
        public static MySqlConnection GetConnection()
        {
            string host = (Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost");
            string user = (Environment.GetEnvironmentVariable("DB_USER") ?? "root");
            string db = (Environment.GetEnvironmentVariable("DB_NAME") ?? "osu");

            var connection = new MySqlConnection($"Server={host};Database={db};User ID={user};ConnectionTimeout=5;");
            connection.Open();
            return connection;
        }

        public static MySqlConnection GetSlaveConnection()
        {
            string host = Environment.GetEnvironmentVariable("DB_HOST_SLAVE");

            if (string.IsNullOrEmpty(host))
                // fallback to master connection if no slave host has been specified.
                return GetConnection();

            string user = (Environment.GetEnvironmentVariable("DB_USER_SLAVE") ?? "root");
            string db = (Environment.GetEnvironmentVariable("DB_NAME") ?? "osu");

            var connection = new MySqlConnection($"Server={host};Database={db};User ID={user};ConnectionTimeout=5;");
            connection.Open();
            return connection;
        }
    }
}
