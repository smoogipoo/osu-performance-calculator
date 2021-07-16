// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using McMaster.Extensions.CommandLineUtils;

namespace osu.Server.PerformanceCalculator.Commands
{
    public abstract class CalculatorCommand : CommandBase
    {
        [Option(CommandOptionType.MultipleValue, Template = "-m|--mode <RULESET_ID>", Description = "Ruleset(s) to compute difficulty for.\n"
                                                                                                    + "0 - osu!\n"
                                                                                                    + "1 - osu!taiko\n"
                                                                                                    + "2 - osu!catch\n"
                                                                                                    + "3 - osu!mania")]
        public int[] Rulesets { get; set; }

        [Option(CommandOptionType.SingleValue, Template = "-c|--concurrency", Description = "Number of threads to use. Default 1.")]
        public int Concurrency { get; set; } = 1;

        protected abstract IEnumerable<int> GetScores();
    }
}
