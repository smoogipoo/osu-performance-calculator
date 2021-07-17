// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections;
using System.Collections.Generic;
using Dapper;
using McMaster.Extensions.CommandLineUtils;
using MySqlConnector;
using osu.Server.PerformanceCalculator.DatabaseModels;

namespace osu.Server.PerformanceCalculator.Commands
{
    [Command(Name = "all-scores", Description = "Calculates the performance of all scores in the database.")]
    public class AllScoresCommand : CalculatorCommand
    {
        protected override IEnumerable<DatabasedScore> GetScores()
        {
            AllScoresEnumerator enumerator = new AllScoresEnumerator(Ruleset);
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }

        private struct AllScoresEnumerator : IEnumerator<DatabasedScore>
        {
            private const int batch_size = 100000;

            private readonly int ruleset;
            private readonly MySqlConnection connection;

            private IEnumerator<DatabasedScore>? currentScoreEnumerator;
            private ulong currentScoreId;

            public AllScoresEnumerator(int ruleset)
            {
                this.ruleset = ruleset;
                connection = Database.GetConnection();

                Current = null!;
                currentScoreEnumerator = null;
                currentScoreId = 0;
            }

            public bool MoveNext()
            {
                if (currentScoreEnumerator == null || !currentScoreEnumerator.MoveNext())
                {
                    currentScoreEnumerator = connection.Query<DatabasedScore>($"SELECT * FROM {TableUtils.ScoresTable(ruleset)} WHERE `score_id` > @ScoreId ORDER BY `score_id` ASC LIMIT @BatchSize",
                        new
                        {
                            ScoreId = currentScoreId,
                            BatchSize = batch_size
                        }).GetEnumerator();

                    if (!currentScoreEnumerator.MoveNext())
                        return false;
                }

                Current = currentScoreEnumerator.Current;
                currentScoreId = Current.score_id;
                return true;
            }

            public void Reset()
            {
                currentScoreEnumerator = null;
                currentScoreId = 0;
            }

            public DatabasedScore Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                connection.Dispose();
            }
        }
    }
}
