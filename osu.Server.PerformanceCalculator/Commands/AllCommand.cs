// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using McMaster.Extensions.CommandLineUtils;

namespace osu.Server.PerformanceCalculator.Commands
{
    [Command(Name = "all", Description = "Calculates the performance of all users in the database.")]
    public class AllCommand : CalculatorCommand
    {
        [Option(CommandOptionType.SingleValue, Template = "-C|--continue", Description = "Continue where a previously aborted 'all' run was left off.")]
        public bool Continue { get; set; }

        protected override IEnumerable<long> GetScores()
        {
            throw new System.NotImplementedException();
        }
    }
}
