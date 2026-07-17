using RdpOctopus.Extensions;
using RdpOctopus.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace RdpOctopus.Services
{
    public class LowLeverSimulator : IKeyboardSimulator
    {
        private InputSimulator _simulator = new InputSimulator();

        public TypeSettings Settings { get; private set; } = new TypeSettings();

        protected TimeSpan IntervalDelay => TimeSpan.FromMilliseconds(Settings.IntervalDelay);
        protected TimeSpan ShiftDelay => TimeSpan.FromMilliseconds(Settings.ShiftDelay);
        protected TimeSpan SwitchDelay => TimeSpan.FromMilliseconds(Settings.SwitchDelay);

        protected bool IsShiftPress { get; set; }
        protected IKeyboardLayout CurrentLayout { get; set; }

        public LowLeverSimulator(TypeSettings settings)
        {
            Settings = settings;
        }

        public async Task Type(string text, IProgress<int> progress, CancellationToken cancellationToken = default)
        {
            IsShiftPress = false;
            CurrentLayout = Settings.InitLayout;

            var chars = text.Replace("\r\n", "\n").ToCharArray();

            var currentChar = 0;
            foreach (char c in chars)
            {
                await TypeChar(c, cancellationToken);
                progress.Report(currentChar++);
            }
        }

        private async Task TypeChar(char c, CancellationToken cancellationToken = default)
        {
            if (!CurrentLayout.Contains(c))
            {
                var otherLayouts = Settings.KeyboardLayouts.Where(l => l.Name != CurrentLayout.Name);

                foreach (var otherLayout in otherLayouts)
                {
                    await SwitchLayout(otherLayout, cancellationToken);

                    if (otherLayout.Contains(c))
                        break;
                }
            }

            var key = CurrentLayout.GetKeyCode(c);

            if (CurrentLayout.IsNeedShift(c))
                await ShiftDown(cancellationToken);
            else
                await ShiftUp(cancellationToken);

            await _simulator.Keyboard.PressWithDelay(key, IntervalDelay, cancellationToken);
        }

        private async Task ShiftDown(CancellationToken cancellationToken = default)
        {
            if (!IsShiftPress)
            {
                _simulator.Keyboard.KeyDown(VirtualKeyCode.SHIFT);
                await Task.Delay(ShiftDelay, cancellationToken);
                IsShiftPress = true;
            }
        }

        private async Task ShiftUp(CancellationToken cancellationToken = default)
        {
            if (IsShiftPress)
            {
                _simulator.Keyboard.KeyUp(VirtualKeyCode.SHIFT);
                await Task.Delay(ShiftDelay, cancellationToken);
                IsShiftPress = false;
            }
        }

        public async Task SwitchLayout(IKeyboardLayout layout, CancellationToken cancellationToken = default)
        {
            if (IsShiftPress && Settings.SwitchMethod == SwitchMethod.AltShift)
                _simulator.Keyboard.KeyUp(VirtualKeyCode.SHIFT);

            if (CurrentLayout.Name == layout.Name)
            {
                return;
            }

            CurrentLayout = layout;

            var keys = new VirtualKeyCode[] { VirtualKeyCode.LMENU, VirtualKeyCode.LSHIFT };

            if (SwitchMethod.WinSpace == Settings.SwitchMethod)
            {
                keys = new VirtualKeyCode[] { VirtualKeyCode.LWIN, VirtualKeyCode.SPACE };
            }

            await _simulator.Keyboard.PressWithDelay(keys, SwitchDelay, cancellationToken);

            if (IsShiftPress && Settings.SwitchMethod == SwitchMethod.AltShift)
                _simulator.Keyboard.KeyDown(VirtualKeyCode.SHIFT);
        }
    }
}
