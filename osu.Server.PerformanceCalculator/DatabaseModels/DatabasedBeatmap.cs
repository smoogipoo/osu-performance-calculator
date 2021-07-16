// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

// ReSharper disable InconsistentNaming

namespace osu.Server.PerformanceCalculator.DatabaseModels
{
    public class DatabasedBeatmap
    {
        public int beatmap_id;
        public short countTotal;
        public short countNormal;
        public short countSlider;
        public short countSpinner;
        public float difficultyrating;
    }
}
