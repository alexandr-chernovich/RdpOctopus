namespace RdpOctopus.Models
{
    public class MainSettings
    {
        public TypeSettings TypeSettings { get; set; }
        public float StartDelay { get; set; } = 4.5f;
        public string InputMethodName { get; set; }
    }
}
