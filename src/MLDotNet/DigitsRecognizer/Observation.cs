
namespace DigitsRecognizer
{
    public class Observation
    {
        public readonly string Label;
        public readonly int[] Pixels;

        public Observation(string label, int[] pixels)
        {
            Label = label;
            Pixels = pixels;
        }
    }
}
