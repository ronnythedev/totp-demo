using TotpDemo;

namespace TotpClient
{
    static class Program
    {
        static void Main()
        {
            var totp = new TotpService(Secrets.SecretKey);
            
            Console.Title = "TOTP Client";
            Console.WriteLine("===== TOTP Client =====");
            Console.WriteLine("TOTP Client started. Enter a code to validate (or 'exit' to quit).\n");

            while (true)
            {
                Console.Write("Enter TOTP code: ");
                
                var input = Console.ReadLine()?.Trim()!;

                if (string.IsNullOrEmpty(input) || input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                if (input.Length != 6 || !int.TryParse(input, out _))
                {
                    Console.WriteLine("Error: Please enter a 6-digit code.");
                    
                    continue;
                }

                var isValid = totp.ValidateCode(input);
                
                Console.WriteLine(isValid ? "Code is valid!" : "Code is invalid or expired.");
                Console.WriteLine();
            }
        }
    }
}
