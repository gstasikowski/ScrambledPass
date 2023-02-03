namespace ScrambledPass.Logic
{
    public static class Helpers
    {
        public static double CalculateEntropy(string password, ref Models.DataBank dataBank)
        {
            int possibleUniqueCharacters = 0;

            if (password.Any(char.IsLower))
            {
                possibleUniqueCharacters += 26;
            }

            if (password.Any(char.IsUpper))
            {
                possibleUniqueCharacters += 26;
            }

            if (password.Any(char.IsDigit))
            {
                possibleUniqueCharacters += 10;
            }

            if (password.Any(char.IsSymbol) || password.Any(char.IsPunctuation))
            {
                possibleUniqueCharacters += dataBank.SpecialCharsCount;
            }

            double entropy = Math.Log(Math.Pow(possibleUniqueCharacters, password.Length));
            return Math.Round(entropy, 2);
        }
    }
}