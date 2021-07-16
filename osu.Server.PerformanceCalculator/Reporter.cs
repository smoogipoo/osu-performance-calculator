// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.IO;
using McMaster.Extensions.CommandLineUtils;

namespace osu.Server.PerformanceCalculator
{
    public class Reporter : IReporter
    {
        /// <summary>
        /// Whether verbose output should be displayed.
        /// </summary>
        public bool IsVerbose { get; set; }

        /// <summary>
        /// Whether quiet output should be displayed.
        /// </summary>
        public bool IsQuiet { get; set; }

        private readonly object writeLock = new object();
        private readonly IConsole console;
        private readonly StreamWriter? fileWriter;
        private readonly StreamWriter errorFileWriter;

        public Reporter(IConsole console, string? file = null)
        {
            this.console = console;

            if (file != null)
            {
                try
                {
                    fileWriter = new StreamWriter(file);
                }
                catch (Exception e)
                {
                    Warn($"Failed to initialise log file ({file}): {e}");
                    Warn("Continuing without log file.");
                }
            }

            errorFileWriter = new StreamWriter("error.log", true) { AutoFlush = true };
        }

        private void writeLine(TextWriter consoleWriter, string message, ConsoleColor? foregroundColour = null)
        {
            lock (writeLock)
            {
                if (foregroundColour.HasValue)
                    Console.ForegroundColor = foregroundColour.Value;

                string line = $"[{DateTime.UtcNow}]: {message}";

                if (!IsQuiet)
                    consoleWriter.WriteLine(line);
                fileWriter?.WriteLine(line);

                if (consoleWriter == console.Error)
                    errorFileWriter.WriteLine(line);

                if (foregroundColour.HasValue)
                    Console.ResetColor();
            }
        }

        /// <summary>
        /// Writes a message in <see cref="ConsoleColor.DarkGray"/> to the console/file outputs.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void Verbose(string message)
        {
            if (!IsVerbose)
                return;

            writeLine(console.Out, message, ConsoleColor.DarkGray);
        }

        public void Output(string message) => writeLine(console.Out, message);

        public void Warn(string message) => writeLine(console.Out, message, ConsoleColor.Yellow);

        public void Error(string message) => writeLine(console.Error, message, ConsoleColor.Red);
    }
}
