// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

// ReSharper disable InconsistentNaming

namespace osu.Server.PerformanceCalculator.DatabaseModels
{
    public class DatabasedScore
    {
        public long score_id;
        public int beatmap_id;
        public int user_id;
        public int score;
        public short maxcombo;
        public string? rank;
        public short count50;
        public short count100;
        public short count300;
        public short countmiss;
        public short countgeki;
        public short countkatu;
        public bool perfect;
        public short enabled_mods;
        public float pp;
        public bool hidden;
    }
}
