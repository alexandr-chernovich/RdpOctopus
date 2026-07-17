using RdpOctopus.Extensions;
using RdpOctopus.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace RdpOctopus.Services
{
    public class TextTyper : IKeyboardSimulator
    {
        private readonly InputSimulator _simulator = new();

        public TypeSettings Settings { get; set; }

        protected TimeSpan IntervalDelay =>
            TimeSpan.FromMilliseconds(Settings.IntervalDelay);


        public TextTyper(TypeSettings typeSettings)
        {
            Settings = typeSettings;
        }


        public async Task Type(
            string text,
            IProgress<int> progress,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(text))
                return;

            var lines = text.Replace("\r", string.Empty).Split('\n');
            var charNumber = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(lines[i]))
                {
                    foreach (var c in lines[i])
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        _simulator.Keyboard.TextEntry(c);
                        progress.Report(charNumber++);
                        await Task.Delay(IntervalDelay, cancellationToken);
                    }
                }
                if (i != lines.Length - 1)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await _simulator.Keyboard.PressWithDelay(VirtualKeyCode.RETURN, IntervalDelay, cancellationToken);
                    progress.Report(charNumber++);
                }
            }
        }
        protected TimeSpan EnterDelay =>
            TimeSpan.FromMilliseconds(Math.Max(Settings.IntervalDelay * 3, 40));
    }
}