// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using osu.Game.Rulesets;

namespace osu.Server.PerformanceCalculator
{
    public class ServerPerformanceCalculator
    {
        private static readonly List<Ruleset> available_rulesets = getRulesets();

        private readonly Ruleset ruleset;
        private readonly bool dryRun;

        public ServerPerformanceCalculator(int ruleset, bool dryRun)
        {
            this.dryRun = dryRun;
            this.ruleset = available_rulesets.Single(r => r.RulesetInfo.ID == ruleset);
        }

        public void ProcessScore(int score)
        {
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
