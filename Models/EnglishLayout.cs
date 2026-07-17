using System.Collections.Generic;
using WindowsInput.Native;

namespace RdpOctopus.Models
{
    public class EnglishLayout : IKeyboardLayout
    {
        public string Name => "Английская (En)";

        private static readonly Dictionary<char, (VirtualKeyCode KeyCode, bool NeedShift)> _map =
            new Dictionary<char, (VirtualKeyCode, bool)>();

        static EnglishLayout()
        {
            // Буквы (нижний регистр)
            for (char c = 'a'; c <= 'z'; c++)
            {
                var vk = (VirtualKeyCode)((int)VirtualKeyCode.VK_A + (c - 'a'));
                _map[c] = (vk, false);
            }

            // Буквы (верхний регистр)
            for (char c = 'A'; c <= 'Z'; c++)
            {
                var vk = (VirtualKeyCode)((int)VirtualKeyCode.VK_A + (c - 'A'));
                _map[c] = (vk, true);
            }

            // Цифры
            _map['0'] = (VirtualKeyCode.VK_0, false);
            _map['1'] = (VirtualKeyCode.VK_1, false);
            _map['2'] = (VirtualKeyCode.VK_2, false);
            _map['3'] = (VirtualKeyCode.VK_3, false);
            _map['4'] = (VirtualKeyCode.VK_4, false);
            _map['5'] = (VirtualKeyCode.VK_5, false);
            _map['6'] = (VirtualKeyCode.VK_6, false);
            _map['7'] = (VirtualKeyCode.VK_7, false);
            _map['8'] = (VirtualKeyCode.VK_8, false);
            _map['9'] = (VirtualKeyCode.VK_9, false);

            // Символы верхнего ряда с Shift
            _map['!'] = (VirtualKeyCode.VK_1, true);
            _map['@'] = (VirtualKeyCode.VK_2, true);
            _map['#'] = (VirtualKeyCode.VK_3, true);
            _map['$'] = (VirtualKeyCode.VK_4, true);
            _map['%'] = (VirtualKeyCode.VK_5, true);
            _map['^'] = (VirtualKeyCode.VK_6, true);
            _map['&'] = (VirtualKeyCode.VK_7, true);
            _map['*'] = (VirtualKeyCode.VK_8, true);
            _map['('] = (VirtualKeyCode.VK_9, true);
            _map[')'] = (VirtualKeyCode.VK_0, true);

            // Специальные символы
            _map['-'] = (VirtualKeyCode.OEM_MINUS, false);
            _map['_'] = (VirtualKeyCode.OEM_MINUS, true);
            _map['='] = (VirtualKeyCode.OEM_PLUS, false);
            _map['+'] = (VirtualKeyCode.OEM_PLUS, true);
            _map['['] = (VirtualKeyCode.OEM_4, false);
            _map['{'] = (VirtualKeyCode.OEM_4, true);
            _map[']'] = (VirtualKeyCode.OEM_6, false);
            _map['}'] = (VirtualKeyCode.OEM_6, true);
            _map[';'] = (VirtualKeyCode.OEM_1, false);
            _map[':'] = (VirtualKeyCode.OEM_1, true);
            _map['\''] = (VirtualKeyCode.OEM_7, false);
            _map['"'] = (VirtualKeyCode.OEM_7, true);
            _map['\\'] = (VirtualKeyCode.OEM_5, false);
            _map['|'] = (VirtualKeyCode.OEM_5, true);
            _map[','] = (VirtualKeyCode.OEM_COMMA, false);
            _map['<'] = (VirtualKeyCode.OEM_COMMA, true);
            _map['.'] = (VirtualKeyCode.OEM_PERIOD, false);
            _map['>'] = (VirtualKeyCode.OEM_PERIOD, true);
            _map['/'] = (VirtualKeyCode.OEM_2, false);
            _map['?'] = (VirtualKeyCode.OEM_2, true);
            _map['`'] = (VirtualKeyCode.OEM_3, false);
            _map['~'] = (VirtualKeyCode.OEM_3, true);
            _map[' '] = (VirtualKeyCode.SPACE, false);

            // Специальные клавиши
            _map['\t'] = (VirtualKeyCode.TAB, false);
            _map['\n'] = (VirtualKeyCode.RETURN, false);
            _map['\r'] = (VirtualKeyCode.RETURN, false);
            _map['\b'] = (VirtualKeyCode.BACK, false);
        }

        public bool Contains(char c) => _map.ContainsKey(c);

        public bool IsNeedShift(char c) => _map.TryGetValue(c, out var info) && info.NeedShift;

        public VirtualKeyCode GetKeyCode(char c) =>
            _map.TryGetValue(c, out var info) ? info.KeyCode : VirtualKeyCode.OEM_2;
    }
}
