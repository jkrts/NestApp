namespace NestApp
{
    class Part
    {
        public float _length;
        public string _mark;
        public bool _placed;

        public Part(string mark, float length)
        {
            _mark = mark;
            _length = length;
            _placed = false;
        }

        public override string ToString()
        {
            return $"Mark: {_mark}, Length: {_length}\" ----------- placed: {_placed}";
        }
    }
}