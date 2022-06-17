namespace NestApp
{
    class Beam
    {
        public int id;
        public float _length;
        public float _remainingLength;
        private static int count = 1;
        public List<Part> sourceParts;

        public Beam()
        {
            id = count;
            count++;
            _length = 480.0f;
            _remainingLength = _length;
            sourceParts = new List<Part>();
        }

        public float getLengthUsed() => _length - _remainingLength;

        public float getDrop() => _remainingLength;
      
        public float getDropPercent() => _remainingLength / _length;

        public override string ToString()
        {
            string output = String.Empty;

            output = $"Beam{id} - Length: {_length}\" => Used: {getLengthUsed()}\" ";
            output += $" Remaining: {_remainingLength}\" ";
            output += String.Format("Drop%: {0:P2}", getDropPercent());

            for(int i = 0; i < sourceParts.Count; i++)
            {
                output += $"\n{sourceParts[i]._mark}, Length: {sourceParts[i]._length} --------- placed:{sourceParts[i]._placed}";
            }

            return output;
        }

    }
}