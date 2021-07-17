// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Server.PerformanceCalculator
{
    public static class TableUtils
    {
        public static string LastUserIdKey(int ruleset) => $"`pp_last_user_id{RulesetSuffix(ruleset)}`";

        public static string ScoresTable(int ruleset) => $"`osu_scores{RulesetSuffix(ruleset)}_high`";

        public static string RulesetSuffix(int ruleset)
        {
            switch (ruleset)
            {
                case 0:
                    return string.Empty;

                case 1:
                    return "_taiko";

                case 2:
                    return "_fruits";

                case 3:
                    return "_mania";

                default:
                    throw new ArgumentException($"Invalid ruleset ID ({ruleset}).", nameof(ruleset));
            }
        }
    }
}
