// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

// ReSharper disable InconsistentNaming

namespace osu.Server.PerformanceCalculator.DatabaseModels
{
    public class DatabasedScore
    {
        public ulong score_id;
        public uint beatmap_id;
        public uint user_id;
        public int score;
        public ushort maxcombo;
        public string? rank;
        public ushort count50;
        public ushort count100;
        public ushort count300;
        public ushort countmiss;
        public ushort countgeki;
        public ushort countkatu;
        public bool perfect;
        public ushort enabled_mods;
        public float? pp;
        public bool hidden;
    }
}
