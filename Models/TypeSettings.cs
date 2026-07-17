using Newtonsoft.Json;
using System.Linq;

namespace RdpOctopus.Models
{
    public class TypeSettings
    {
        /// <summary>
        /// Задержка между нажатиями клавиш в милисекундах
        /// </summary>
        public float IntervalDelay { get; set; } = 10f;

        /// <summary>
        /// Задержка после удерживания шифта в милисекундах
        /// </summary>
        public float ShiftDelay { get; set; } = 5f;

        /// <summary>
        /// Задержка после смены языка в милисекундах
        /// </summary> 
        public float SwitchDelay { get; set; } = 100f;

        /// <summary>
        /// Наименование раскладки
        /// </summary>
        public string InitLayoutName { get; set; } = "Английская (En)";

        [JsonIgnore]
        public IKeyboardLayout InitLayout
        {
            get => GetLayoutByName(InitLayoutName);
            set => InitLayoutName = value?.Name ?? "English";
        }

        private IKeyboardLayout GetLayoutByName(string name)
        {
            return KeyboardLayouts.FirstOrDefault(l => l.Name == name) ?? new EnglishLayout();
        }

        /// <summary>
        /// Доступные раскладки клавиатуры
        /// </summary>
        [JsonIgnore]
        public IKeyboardLayout[] KeyboardLayouts => new IKeyboardLayout[] { new EnglishLayout(), new RussianLayout() };

        /// <summary>
        /// Комбинация клавиш для смены языка
        /// </summary>
        public SwitchMethod SwitchMethod { get; set; } = SwitchMethod.AltShift;
    }
}
