namespace TheBlogg.Services
{
    public class GenerateRandomPassword : IGenerateRandomPassword
    {
        private readonly IMyRandom _random;
        public GenerateRandomPassword(IMyRandom random)
        {
            _random = random;
        }

        public string RandomPasswordGenerator()
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!-+/";
            const int length = 12;

            char[] password = new char[length];

            for (int i = 0; i < length; i++)
            {
                password[i] = validChars[_random.GetRandom(validChars.Length)];
            }

            return new string(password);
        }
    }
}
