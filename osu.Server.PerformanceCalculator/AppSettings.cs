// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Server.PerformanceCalculator
{
    public static class AppSettings
    {
        /// <summary>
        /// Whether the difficulty command should wait for docker to be ready and perform automatic operations.
        /// </summary>
        public static readonly bool RUN_AS_SANDBOX_DOCKER;

        static AppSettings()
        {
            RUN_AS_SANDBOX_DOCKER = Environment.GetEnvironmentVariable("DOCKER") == "1";
        }
    }
}
