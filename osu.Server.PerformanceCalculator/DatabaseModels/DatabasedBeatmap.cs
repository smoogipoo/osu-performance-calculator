// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

// ReSharper disable InconsistentNaming

namespace osu.Server.PerformanceCalculator.DatabaseModels
{
    public class DatabasedBeatmap
    {
        public uint beatmap_id;
        public ushort countTotal;
        public ushort countNormal;
        public ushort countSlider;
        public ushort countSpinner;
        public float difficultyrating;
    }
}
