using TotpDemo;

namespace TotpGenerator
{
    static class Program
    {
        private static void Main()
        {
            Console.Title = "===== TOTP Generator =====";
            Console.WriteLine("===== TOTP Generator =====");
            Console.WriteLine("TOTP Generator started. Press Ctrl+C to exit.\n\n");
            
            var totp = new TotpService(Secrets.SecretKey);
            
            // Default TOTP time step in seconds. This might go into a configuration file in a real application.
            const int timeStepSeconds = 30; 
            
            while (true)
            {
                var code = totp.GenerateCode();
                var secondsRemaining = totp.GetSecondsRemaining();
                
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.WriteLine($"Current Code: {code} (Valid for {secondsRemaining} seconds)");
                
                Thread.Sleep(1000); // Update every second
                
                if (secondsRemaining == timeStepSeconds)
                {
                    // New time step; clear the previous line and show new code
                    Console.SetCursorPosition(0, Console.CursorTop );
                }
            }
        }
    }
}
