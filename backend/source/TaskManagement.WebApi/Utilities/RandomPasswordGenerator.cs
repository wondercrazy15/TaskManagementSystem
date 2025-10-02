namespace TaskManagementWebApi.Utilities
{
    /// <summary>
    /// Random Password Generator
    /// </summary>
    public class RandomPasswordGenerator
    {
        const string LOWER_CASE = "abcdefghijklmnopqursuvwxyz";
        const string UPPER_CAES = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string NUMBERS = "123456789";
        const string SPECIALS = @"!@£$%^&*()#€";

        /// <summary>
        /// Generate Password
        /// </summary>
        /// <param name="passwordSize"></param>
        /// <returns></returns>
        public string GeneratePassword(int passwordSize=12)
        {
            char[] _password = new char[passwordSize];
            string charSet = "";
            System.Random _random = new Random();
            int counter;
            charSet += LOWER_CASE;

            charSet += UPPER_CAES;

            charSet += NUMBERS;

            charSet += SPECIALS;

            for (counter = 0; counter < passwordSize; counter++)
            {
                _password[counter] = charSet[_random.Next(charSet.Length - 1)];
            }

            return String.Join(null, _password);
        }

    }
}







