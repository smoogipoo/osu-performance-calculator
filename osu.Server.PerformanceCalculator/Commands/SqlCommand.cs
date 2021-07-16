// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using McMaster.Extensions.CommandLineUtils;

namespace osu.Server.PerformanceCalculator.Commands
{
    [Command(Name = "sql", Description = "Compute performance of users given by a SQL select statement.")]
    public class SqlCommand : CalculatorCommand
    {
        [Argument(0, "statement", "The SQL statement selecting the user IDs to compute performance for.")]
        public string Statement { get; set; } = null!;

        protected override IEnumerable<int> GetScores()
        {
            throw new System.NotImplementedException();
        }
    }
}
