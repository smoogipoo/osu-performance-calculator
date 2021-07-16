// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Net;
using McMaster.Extensions.CommandLineUtils;
using osu.Server.PerformanceCalculator.Commands;

namespace osu.Server.PerformanceCalculator
{
    [Command]
    [Subcommand(typeof(AllCommand))]
    [Subcommand(typeof(NewCommand))]
    [Subcommand(typeof(ScoresCommand))]
    [Subcommand(typeof(SqlCommand))]
    [Subcommand(typeof(UsersCommand))]
    public class Program
    {
        public static void Main(string[] args)
        {
            ServicePointManager.DefaultConnectionLimit = 128;
            CommandLineApplication.Execute<Program>(args);
        }

        public int OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
            return 1;
        }
    }
}
