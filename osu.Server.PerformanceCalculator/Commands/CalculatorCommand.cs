// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using osu.Server.PerformanceCalculator.DatabaseModels;

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
        private long processedScores;
        private long totalScores;

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

            var scoreQueue = new ConcurrentQueue<DatabasedScore>();

            var tasks = new Task[Concurrency];
            bool allAdded = false;

            for (int i = 0; i < Concurrency; i++)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    using var updateQueue = new UpdateQueue(Ruleset);
                    var calc = new ServerPerformanceCalculator(Ruleset);

                    while (!allAdded || !scoreQueue.IsEmpty)
                    {
                        if (!scoreQueue.TryDequeue(out var score))
                            continue;

                        try
                        {
                            calc.UpdateScore(score, DryRun ? null : updateQueue);
                        }
                        catch (Exception e)
                        {
                            reporter.Error($"{score.score_id} failed with {e}");
                        }

                        Interlocked.Increment(ref processedScores);
                    }
                }, TaskCreationOptions.LongRunning);
            }

            Task.Factory.StartNew(() =>
            {
                foreach (var score in GetScores())
                {
                    scoreQueue.Enqueue(score);
                    totalScores++;
                }

                allAdded = true;
            });

            using (new Timer(_ => outputProgress(), null, 1000, 1000))
                Task.WaitAll(tasks);
        }

        private long lastProgress;

        private void outputProgress()
        {
            long processed = processedScores;
            reporter.Output($"Processed {processed} / {totalScores} ({processed - lastProgress}/sec)");
            lastProgress = processed;
        }

        protected abstract IEnumerable<DatabasedScore> GetScores();
    }
}
