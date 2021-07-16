// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

// ReSharper disable InconsistentNaming

namespace osu.Server.PerformanceCalculator.DatabaseModels
{
    public class DatabasedBeatmapDifficultyAttrib
    {
        public uint beatmap_id;
        public ushort mode;
        public uint mods;
        public ushort attrib_id;
        public float value;
    }
}
