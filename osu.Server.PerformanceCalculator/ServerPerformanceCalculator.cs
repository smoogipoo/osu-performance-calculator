// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Dapper;
using osu.Game.Beatmaps.Legacy;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Difficulty;
using osu.Server.PerformanceCalculator.DatabaseModels;

namespace osu.Server.PerformanceCalculator
{
    public class ServerPerformanceCalculator
    {
        private static readonly List<Ruleset> available_rulesets = getRulesets();

        private static readonly ConcurrentDictionary<uint, DatabasedBeatmap> cached_beatmaps = new ConcurrentDictionary<uint, DatabasedBeatmap>();
        private static readonly ConcurrentDictionary<uint, DatabasedBeatmapDifficultyAttrib[]> cached_beatmap_attribs = new ConcurrentDictionary<uint, DatabasedBeatmapDifficultyAttrib[]>();

        private readonly Ruleset ruleset;
        private readonly int rulesetId;
        private readonly bool dryRun;
        private readonly UpdateQueue updateQueue;

        public ServerPerformanceCalculator(int ruleset, bool dryRun, UpdateQueue updateQueue)
        {
            this.dryRun = dryRun;
            this.updateQueue = updateQueue;
            this.ruleset = available_rulesets.Single(r => r.RulesetInfo.ID == ruleset);
            rulesetId = ruleset;
        }

        public void ProcessScore(DatabasedScore score)
        {
            if (!cached_beatmaps.TryGetValue(score.beatmap_id, out var databasedBeatmap))
            {
                using (var conn = Database.GetConnection())
                {
                    cached_beatmaps[score.beatmap_id] = databasedBeatmap = conn.QuerySingle<DatabasedBeatmap>(
                        "SELECT * FROM `osu_beatmaps` WHERE `beatmap_id` = @BeatmapId",
                        new
                        {
                            BeatmapId = score.beatmap_id,
                        });
                }
            }

            if (!cached_beatmap_attribs.TryGetValue(score.beatmap_id, out var databasedAttribs))
            {
                using (var conn = Database.GetConnection())
                {
                    cached_beatmap_attribs[score.beatmap_id] = databasedAttribs = conn.Query<DatabasedBeatmapDifficultyAttrib>(
                        "SELECT * FROM `osu_beatmap_difficulty_attribs` WHERE `beatmap_id` = @BeatmapId",
                        new
                        {
                            BeatmapId = score.beatmap_id,
                        }).ToArray();
                }
            }

            // Todo: Log.
            if (databasedAttribs.Length == 0)
                return;

            DifficultyAttributes difficultyAttribs = databasedAttribs.Where(a => a.mode == rulesetId && a.mods == (int)((LegacyMods)score.enabled_mods).MaskRelevantMods())
                                                                     .ToDictionary(a => (int)a.attrib_id).Map(rulesetId, databasedBeatmap);
            difficultyAttribs.Mods = ruleset.ConvertFromLegacyMods((LegacyMods)score.enabled_mods).ToArray();

            var rating = ruleset.CreatePerformanceCalculator(difficultyAttribs, score.ToScore(ruleset))
                                .Calculate(new Dictionary<string, double>());

            updateQueue.Add(score, rating);
        }

        private static List<Ruleset> getRulesets()
        {
            const string ruleset_library_prefix = "osu.Game.Rulesets";

            var rulesetsToProcess = new List<Ruleset>();

            foreach (string file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, $"{ruleset_library_prefix}.*.dll"))
            {
                try
                {
                    var assembly = Assembly.LoadFrom(file);
                    Type type = assembly.GetTypes().First(t => t.IsPublic && t.IsSubclassOf(typeof(Ruleset)));
                    rulesetsToProcess.Add((Ruleset)Activator.CreateInstance(type)!);
                }
                catch
                {
                    throw new Exception($"Failed to load ruleset ({file})");
                }
            }

            return rulesetsToProcess;
        }
    }
}
