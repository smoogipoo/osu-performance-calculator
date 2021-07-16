// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using McMaster.Extensions.CommandLineUtils;

namespace osu.Server.PerformanceCalculator.Commands
{
    [Command(Name = "users", Description = "Compute performance of specific users.")]
    public class UsersCommand : CalculatorCommand
    {
        [Argument(0, "users", "The user IDs to recompute performance for.")]
        public int[] Users { get; set; } = null!;

        protected override IEnumerable<long> GetScores()
        {
            throw new System.NotImplementedException();
        }
    }
}
