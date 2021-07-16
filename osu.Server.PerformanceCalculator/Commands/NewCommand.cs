// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using McMaster.Extensions.CommandLineUtils;
using osu.Server.PerformanceCalculator.DatabaseModels;

namespace osu.Server.PerformanceCalculator.Commands
{
    [Command(Name = "new", Description = "Continually polls for new scores to compute the performance for.")]
    public class NewCommand : CalculatorCommand
    {
        protected override IEnumerable<DatabasedScore> GetScores()
        {
            throw new System.NotImplementedException();
        }
    }
}
