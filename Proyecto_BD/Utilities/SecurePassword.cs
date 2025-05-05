namespace Proyecto_BD.Utilities
{
    public class SecurePassword
    {
        public static string GenerateSecurePassword(int length)
        {            
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+";
            char[] passwordChars = new char[length];
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                passwordChars[i] = validChars[random.Next(validChars.Length)];
            }
            return new string(passwordChars);
        }
    }
}
