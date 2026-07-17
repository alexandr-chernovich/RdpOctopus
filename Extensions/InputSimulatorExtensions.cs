using System;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace RdpOctopus.Extensions
{
    public static class IKeyboardSimulatorExtensions
    {
        public static async Task<IKeyboardSimulator> PressWithDelay(this IKeyboardSimulator simulator, VirtualKeyCode[] keys, TimeSpan delay, CancellationToken cancellationToken = default)
        {
            foreach (var key in keys)
            {
                simulator.KeyDown(key);
            }

            await Task.Delay(delay, cancellationToken);

            foreach (var key in keys)
            {
                simulator.KeyUp(key);
            }

            return simulator;
        }

        public static async Task<IKeyboardSimulator> PressWithDelay(this IKeyboardSimulator simulator, VirtualKeyCode key, TimeSpan delay, CancellationToken cancellationToken = default)
        {
            simulator.KeyDown(key);
            await Task.Delay(delay, cancellationToken);
            simulator.KeyUp(key);
            return simulator;
        }
    }
}
