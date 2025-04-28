using System.Security.Cryptography;

namespace TotpDemo
{
    public class TotpService
    {
        private readonly byte[] _secretKey;
        private readonly int _timeStepSeconds;
        private readonly int _codeDigits;

        public TotpService(string base32Secret, int timeStepSeconds = 30, int codeDigits = 6)
        {
            _secretKey = Base32Decode(base32Secret);
            _timeStepSeconds = timeStepSeconds;
            _codeDigits = codeDigits;
        }
        
        private static byte[] Base32Decode(string base32)
        {
            const string base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            
            base32 = base32.ToUpper().Replace("=", "");
            
            var bytes = new byte[base32.Length * 5 / 8];
            var byteIndex = 0;
            var bits = 0;
            var bitCount = 0;

            foreach (var c in base32)
            {
                var value = base32Chars.IndexOf(c);

                if (value < 0)
                {
                    throw new ArgumentException("Invalid Base32 character.");
                }
                
                bits = bits << 5 | value;
                bitCount += 5;
                
                while (bitCount >= 8)
                {
                    bytes[byteIndex++] = (byte)(bits >> (bitCount - 8));
                    bitCount -= 8;
                }
            }
            return bytes;
        }

        private long GetTimeCounter(DateTime utcTime)
        {
            return (long)(utcTime - DateTime.UnixEpoch).TotalSeconds / _timeStepSeconds;
        }
        
        public int GetSecondsRemaining(DateTime? utcNow = null)
        {
            utcNow ??= DateTime.UtcNow;
            var secondsSinceEpoch = (long)(utcNow.Value - DateTime.UnixEpoch).TotalSeconds;
            return _timeStepSeconds - (int)(secondsSinceEpoch % _timeStepSeconds);
        }
        
        public string GenerateCode(DateTime? utcNow = null)
        {
            utcNow ??= DateTime.UtcNow;
            
            var timeCounter = GetTimeCounter(utcNow.Value);
            var timeBytes = BitConverter.GetBytes(timeCounter);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(timeBytes);
            }

            using var hmac = new HMACSHA1(_secretKey);
            
            var hash = hmac.ComputeHash(timeBytes);
            var offset = hash[^1] & 0x0F; // Last byte determines offset
            var binary = (hash[offset] & 0x7F) << 24 |
                                hash[offset + 1] << 16 |
                                hash[offset + 2] << 8 |
                                hash[offset + 3];
            
            var code = binary % (int)Math.Pow(10, _codeDigits);
            
            return code.ToString().PadLeft(_codeDigits, '0');
        }

        public bool ValidateCode(string code, DateTime? utcNow = null)
        {
            utcNow ??= DateTime.UtcNow;
            
            // To handle time drift, we check the current time and the previous and next time steps
            // This is a common practice to ensure that the user has a bit of leeway in case their clock is slightly off
            // So, a code generated at time T will be valid for T, T-1, and T+1
            for (var i = -1; i <= 1; i++)
            {
                var timeCounter = GetTimeCounter(utcNow.Value.AddSeconds(i * _timeStepSeconds)); 
                var timeBytes = BitConverter.GetBytes(timeCounter);

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(timeBytes);
                }

                using var hmac = new HMACSHA1(_secretKey);
                
                var hash = hmac.ComputeHash(timeBytes);
                var offset = hash[^1] & 0x0F;
                var binary = (hash[offset] & 0x7F) << 24 |
                                    hash[offset + 1] << 16 |
                                    hash[offset + 2] << 8 |
                                    hash[offset + 3];
                
                var expectedCode = binary % (int)Math.Pow(10, _codeDigits);

                if (expectedCode.ToString().PadLeft(_codeDigits, '0') == code)
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}