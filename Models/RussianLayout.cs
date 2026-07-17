using System.Collections.Generic;
using WindowsInput.Native;

namespace RdpOctopus.Models
{
    public class RussianLayout : IKeyboardLayout
    {
        public string Name => "Русская (Ru)";
        /// <summary>
        /// Маппинг русских букв на английские виртуальные клавиши (раскладка ЙЦУКЕН) 
        /// </summary>
        private static readonly Dictionary<char, (VirtualKeyCode KeyCode, bool NeedShift)> _map =
            new Dictionary<char, (VirtualKeyCode, bool)>();

        static RussianLayout()
        {
            // Русские буквы и соответствующие им английские клавиши
            var mapping = new (char RusLower, char RusUpper, VirtualKeyCode KeyCode)[]
            {
                ('й', 'Й', VirtualKeyCode.VK_Q),
                ('ц', 'Ц', VirtualKeyCode.VK_W),
                ('у', 'У', VirtualKeyCode.VK_E),
                ('к', 'К', VirtualKeyCode.VK_R),
                ('е', 'Е', VirtualKeyCode.VK_T),
                ('н', 'Н', VirtualKeyCode.VK_Y),
                ('г', 'Г', VirtualKeyCode.VK_U),
                ('ш', 'Ш', VirtualKeyCode.VK_I),
                ('щ', 'Щ', VirtualKeyCode.VK_O),
                ('з', 'З', VirtualKeyCode.VK_P),
                ('х', 'Х', VirtualKeyCode.OEM_4),     // [
                ('ъ', 'Ъ', VirtualKeyCode.OEM_6),     // ]
                ('ф', 'Ф', VirtualKeyCode.VK_A),
                ('ы', 'Ы', VirtualKeyCode.VK_S),
                ('в', 'В', VirtualKeyCode.VK_D),
                ('а', 'А', VirtualKeyCode.VK_F),
                ('п', 'П', VirtualKeyCode.VK_G),
                ('р', 'Р', VirtualKeyCode.VK_H),
                ('о', 'О', VirtualKeyCode.VK_J),
                ('л', 'Л', VirtualKeyCode.VK_K),
                ('д', 'Д', VirtualKeyCode.VK_L),
                ('ж', 'Ж', VirtualKeyCode.OEM_1),     // ;
                ('э', 'Э', VirtualKeyCode.OEM_7),     // '
                ('я', 'Я', VirtualKeyCode.VK_Z),
                ('ч', 'Ч', VirtualKeyCode.VK_X),
                ('с', 'С', VirtualKeyCode.VK_C),
                ('м', 'М', VirtualKeyCode.VK_V),
                ('и', 'И', VirtualKeyCode.VK_B),
                ('т', 'Т', VirtualKeyCode.VK_N),
                ('ь', 'Ь', VirtualKeyCode.VK_M),
                ('ё', 'Ё', VirtualKeyCode.OEM_3),
                ('б', 'Б', VirtualKeyCode.OEM_COMMA), // ,
                ('ю', 'Ю', VirtualKeyCode.OEM_PERIOD) // .
            };

            // Добавляем символы в словарь
            foreach (var (rusLower, rusUpper, keyCode) in mapping)
            {
                _map[rusLower] = (keyCode, false); 
                _map[rusUpper] = (keyCode, true);  
            }

            // Цифры (те же, что и в английской раскладке)
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

            // Цифры с Shift для символов
            _map['!'] = (VirtualKeyCode.VK_1, true);
            _map['"'] = (VirtualKeyCode.VK_2, true);
            _map['№'] = (VirtualKeyCode.VK_3, true);
            _map[';'] = (VirtualKeyCode.VK_4, true);
            _map['%'] = (VirtualKeyCode.VK_5, true);
            _map[':'] = (VirtualKeyCode.VK_6, true);
            _map['?'] = (VirtualKeyCode.VK_7, true);
            _map['*'] = (VirtualKeyCode.VK_8, true);
            _map['('] = (VirtualKeyCode.VK_9, true);
            _map[')'] = (VirtualKeyCode.VK_0, true);

            // Специальные символы
            _map['-'] = (VirtualKeyCode.OEM_MINUS, false);
            _map['_'] = (VirtualKeyCode.OEM_MINUS, true);
            _map['='] = (VirtualKeyCode.OEM_PLUS, false);
            _map['+'] = (VirtualKeyCode.OEM_PLUS, true);
            _map[' '] = (VirtualKeyCode.SPACE, false);
            _map['\t'] = (VirtualKeyCode.TAB, false);
            _map['\n'] = (VirtualKeyCode.RETURN, false);
            _map['\r'] = (VirtualKeyCode.RETURN, false);
            _map['\b'] = (VirtualKeyCode.BACK, false);
        }

        public bool Contains(char c) => _map.ContainsKey(c);

        public bool IsNeedShift(char c) => _map.TryGetValue(c, out var info) && info.NeedShift;

        public VirtualKeyCode GetKeyCode(char c) =>
            _map.TryGetValue(c, out var info) ? info.KeyCode : VirtualKeyCode.SPACE;
    }
}