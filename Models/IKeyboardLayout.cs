using WindowsInput.Native;

namespace RdpOctopus.Models
{
    public interface IKeyboardLayout
    {
        string Name { get; }
        bool Contains(char c);
        bool IsNeedShift(char c);
        VirtualKeyCode GetKeyCode(char c);
    }
}
