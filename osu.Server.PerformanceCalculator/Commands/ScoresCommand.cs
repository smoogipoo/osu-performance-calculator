// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using Dapper;
using McMaster.Extensions.CommandLineUtils;
using osu.Server.PerformanceCalculator.DatabaseModels;

namespace osu.Server.PerformanceCalculator.Commands
{
    [Command(Name = "scores", Description = "Compute performance of specific scores.")]
    public class ScoresCommand : CalculatorCommand
    {
        [Argument(0, "scores", "The score IDs to recompute performance for.")]
        public long[] Scores { get; set; } = null!;

        protected override IEnumerable<DatabasedScore> GetScores()
        {
            using (var conn = Database.GetConnection())
            {
                return conn.Query<DatabasedScore>($"SELECT * FROM `{TableUtils.ScoresTable(Ruleset)}` WHERE `score_id` IN @ScoreIds", new
                {
                    ScoreIds = Scores
                });
            }
        }
    }
}
