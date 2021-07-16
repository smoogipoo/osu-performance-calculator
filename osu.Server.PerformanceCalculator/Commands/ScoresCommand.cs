// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using McMaster.Extensions.CommandLineUtils;

namespace osu.Server.PerformanceCalculator.Commands
{
    [Command(Name = "scores", Description = "Compute performance of specific scores.")]
    public class ScoresCommand : CalculatorCommand
    {
        [Argument(0, "scores", "The score IDs to recompute performance for.")]
        public int[] Scores { get; set; }

        protected override IEnumerable<int> GetScores()
        {
            throw new System.NotImplementedException();
        }
    }
}
