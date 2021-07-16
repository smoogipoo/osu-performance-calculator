// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using McMaster.Extensions.CommandLineUtils;

namespace osu.Server.PerformanceCalculator.Commands
{
    public abstract class CalculatorCommand : CommandBase
    {
        [Option(CommandOptionType.SingleValue, Template = "-m|--mode <RULESET_ID>", Description = "Ruleset to compute difficulty for.\n"
                                                                                                  + "0 - osu!\n"
                                                                                                  + "1 - osu!taiko\n"
                                                                                                  + "2 - osu!catch\n"
                                                                                                  + "3 - osu!mania")]
        public int Ruleset { get; set; }

        [Option(CommandOptionType.SingleValue, Template = "-c|--concurrency", Description = "Number of threads to use. Default 1.")]
        public int Concurrency { get; set; } = 1;

        [Option(CommandOptionType.NoValue, Template = "-v|--verbose", Description = "Provide verbose console output.")]
        public bool Verbose { get; set; }

        [Option(CommandOptionType.NoValue, Template = "-q|--quiet", Description = "Disable all console output.")]
        public bool Quiet { get; set; }

        [Option(CommandOptionType.SingleValue, Template = "-l|--log-file", Description = "The file to log output to.")]
        public string? LogFile { get; set; }

        [Option(CommandOptionType.NoValue, Template = "-dry|--dry-run", Description = "Whether to run the process without writing to the database.")]
        public bool DryRun { get; set; }

        private IReporter reporter = null!;

        public void OnExecute(CommandLineApplication app, IConsole console)
        {
            reporter = new Reporter(console, LogFile)
            {
                IsQuiet = Quiet,
                IsVerbose = Verbose
            };

            if (Concurrency < 1)
            {
                reporter.Error("Concurrency level must be above 1.");
                return;
            }

            foreach (var score in GetScores())
            {
                new ServerPerformanceCalculator(Ruleset, DryRun).ProcessScore(score);
            }
        }

        protected abstract IEnumerable<int> GetScores();
    }
}
