using RdpOctopus.Models;
using System.Linq;

namespace RdpOctopus.Services
{
    public class KeyboardLayoutService
    {
        public IKeyboardLayout[] KeyboardLayouts { get; set; }

        public KeyboardLayoutService(params IKeyboardLayout[] keyboardLayouts)
        {
            KeyboardLayouts = keyboardLayouts;
        }

        public char[] GetInvalidCharacters(string text)
        {
            return text
                .Where(IsInvalidCharacter)
                .Distinct()
                .ToArray();
        }

        public bool IsInvalidCharacter(char c)
        {
            return KeyboardLayouts.All(l => !l.Contains(c));
        }
    }
}
