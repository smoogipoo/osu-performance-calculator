// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

// ReSharper disable InconsistentNaming

namespace osu.Server.PerformanceCalculator.DatabaseModels
{
    public class DatabasedBeatmapDifficultyAttrib
    {
        public int beatmap_id;
        public short mode;
        public int mods;
        public short attrib_id;
        public float value;
    }
}
