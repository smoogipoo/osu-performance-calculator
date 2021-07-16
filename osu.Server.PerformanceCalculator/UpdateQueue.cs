// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using osu.Server.PerformanceCalculator.DatabaseModels;

namespace osu.Server.PerformanceCalculator
{
    public class UpdateQueue : IDisposable
    {
        private const int batch_size = 10000;

        private readonly int rulesetId;
        private readonly Queue<(DatabasedScore, double)> queue = new Queue<(DatabasedScore, double)>();

        public UpdateQueue(int rulesetId)
        {
            this.rulesetId = rulesetId;
        }

        public void Add(DatabasedScore score, double newPp)
        {
            queue.Enqueue((score, newPp));

            if (queue.Count == batch_size)
                Flush();
        }

        public void Flush()
        {
            if (queue.Count == 0)
                return;

            var queryBuilder = new StringBuilder();

            queryBuilder.Append("START TRANSACTION;");

            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                queryBuilder.Append($"UPDATE `{TableUtils.ScoresTable(rulesetId)}` SET `pp` = {item.Item2} WHERE `score_id` = {item.Item1.score_id};");
            }

            queryBuilder.Append("COMMIT;");

            using (var conn = Database.GetConnection())
                conn.Execute(queryBuilder.ToString());
        }

        public void Dispose()
        {
            Flush();
            GC.SuppressFinalize(this);
        }
    }
}
