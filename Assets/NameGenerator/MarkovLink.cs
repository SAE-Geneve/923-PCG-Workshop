namespace NameGenerator
{
    public class MarkovLink
    {
        public MarkovLink(string text, int weight = 1)
        {
            _text = text;
            _weight = weight;
        }

        private string _text;
        private string _difficulty;
        private int _weight;
    
        public string Text => _text;
        public int Weight => _weight;
    }
}
