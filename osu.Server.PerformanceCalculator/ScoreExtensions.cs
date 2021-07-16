// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Game.Beatmaps.Legacy;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mods;
using osu.Game.Scoring;
using osu.Game.Scoring.Legacy;
using osu.Server.PerformanceCalculator.DatabaseModels;

namespace osu.Server.PerformanceCalculator
{
    public static class ScoreExtensions
    {
        public static ScoreInfo ToScore(this DatabasedScore databasedScore, Ruleset ruleset)
        {
            var scoreInfo = new ScoreInfo
            {
                Ruleset = ruleset.RulesetInfo,
                RulesetID = ruleset.RulesetInfo.ID!.Value,
                Mods = ruleset.ConvertFromLegacyMods((LegacyMods)databasedScore.enabled_mods).ToArray(),
                TotalScore = databasedScore.score,
                MaxCombo = databasedScore.maxcombo
            };

            scoreInfo.SetCount50(databasedScore.count50);
            scoreInfo.SetCount100(databasedScore.count100);
            scoreInfo.SetCount300(databasedScore.count300);
            scoreInfo.SetCountMiss(databasedScore.countmiss);
            scoreInfo.SetCountGeki(databasedScore.countgeki);
            scoreInfo.SetCountKatu(databasedScore.countkatu);

            calculateAccuracy(scoreInfo);

            return scoreInfo;
        }

        private static void calculateAccuracy(ScoreInfo score)
        {
            int countMiss = score.GetCountMiss() ?? 0;
            int count50 = score.GetCount50() ?? 0;
            int count100 = score.GetCount100() ?? 0;
            int count300 = score.GetCount300() ?? 0;
            int countGeki = score.GetCountGeki() ?? 0;
            int countKatu = score.GetCountKatu() ?? 0;

            switch (score.Ruleset.ID)
            {
                case 0:
                {
                    int totalHits = count50 + count100 + count300 + countMiss;
                    score.Accuracy = totalHits > 0 ? (double)(count50 * 50 + count100 * 100 + count300 * 300) / (totalHits * 300) : 1;

                    float ratio300 = (float)count300 / totalHits;
                    float ratio50 = (float)count50 / totalHits;

                    if (ratio300 == 1)
                        score.Rank = score.Mods.Any(m => m is ModHidden || m is ModFlashlight) ? ScoreRank.XH : ScoreRank.X;
                    else if (ratio300 > 0.9 && ratio50 <= 0.01 && countMiss == 0)
                        score.Rank = score.Mods.Any(m => m is ModHidden || m is ModFlashlight) ? ScoreRank.SH : ScoreRank.S;
                    else if ((ratio300 > 0.8 && countMiss == 0) || ratio300 > 0.9)
                        score.Rank = ScoreRank.A;
                    else if ((ratio300 > 0.7 && countMiss == 0) || ratio300 > 0.8)
                        score.Rank = ScoreRank.B;
                    else if (ratio300 > 0.6)
                        score.Rank = ScoreRank.C;
                    else
                        score.Rank = ScoreRank.D;
                    break;
                }

                case 1:
                {
                    int totalHits = count50 + count100 + count300 + countMiss;
                    score.Accuracy = totalHits > 0 ? (double)(count100 * 150 + count300 * 300) / (totalHits * 300) : 1;

                    float ratio300 = (float)count300 / totalHits;
                    float ratio50 = (float)count50 / totalHits;

                    if (ratio300 == 1)
                        score.Rank = score.Mods.Any(m => m is ModHidden || m is ModFlashlight) ? ScoreRank.XH : ScoreRank.X;
                    else if (ratio300 > 0.9 && ratio50 <= 0.01 && countMiss == 0)
                        score.Rank = score.Mods.Any(m => m is ModHidden || m is ModFlashlight) ? ScoreRank.SH : ScoreRank.S;
                    else if ((ratio300 > 0.8 && countMiss == 0) || ratio300 > 0.9)
                        score.Rank = ScoreRank.A;
                    else if ((ratio300 > 0.7 && countMiss == 0) || ratio300 > 0.8)
                        score.Rank = ScoreRank.B;
                    else if (ratio300 > 0.6)
                        score.Rank = ScoreRank.C;
                    else
                        score.Rank = ScoreRank.D;
                    break;
                }

                case 2:
                {
                    int totalHits = count50 + count100 + count300 + countMiss + countKatu;
                    score.Accuracy = totalHits > 0 ? (double)(count50 + count100 + count300) / totalHits : 1;

                    if (score.Accuracy == 1)
                        score.Rank = score.Mods.Any(m => m is ModHidden || m is ModFlashlight) ? ScoreRank.XH : ScoreRank.X;
                    else if (score.Accuracy > 0.98)
                        score.Rank = score.Mods.Any(m => m is ModHidden || m is ModFlashlight) ? ScoreRank.SH : ScoreRank.S;
                    else if (score.Accuracy > 0.94)
                        score.Rank = ScoreRank.A;
                    else if (score.Accuracy > 0.9)
                        score.Rank = ScoreRank.B;
                    else if (score.Accuracy > 0.85)
                        score.Rank = ScoreRank.C;
                    else
                        score.Rank = ScoreRank.D;
                    break;
                }

                case 3:
                {
                    int totalHits = count50 + count100 + count300 + countMiss + countGeki + countKatu;
                    score.Accuracy = totalHits > 0 ? (double)(count50 * 50 + count100 * 100 + countKatu * 200 + (count300 + countGeki) * 300) / (totalHits * 300) : 1;

                    if (score.Accuracy == 1)
                        score.Rank = score.Mods.Any(m => m is ModHidden || m is ModFlashlight) ? ScoreRank.XH : ScoreRank.X;
                    else if (score.Accuracy > 0.95)
                        score.Rank = score.Mods.Any(m => m is ModHidden || m is ModFlashlight) ? ScoreRank.SH : ScoreRank.S;
                    else if (score.Accuracy > 0.9)
                        score.Rank = ScoreRank.A;
                    else if (score.Accuracy > 0.8)
                        score.Rank = ScoreRank.B;
                    else if (score.Accuracy > 0.7)
                        score.Rank = ScoreRank.C;
                    else
                        score.Rank = ScoreRank.D;
                    break;
                }
            }
        }
    }
}
