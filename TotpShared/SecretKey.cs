namespace TotpDemo
{
    // This class contains the hardcoded Base32 secret key used for TOTP generation and validation.
    // In production, this key should be securely stored and managed.
    public static class Secrets
    {
        public const string SecretKey = "JBSWY3DPEHPK3PXPJBSWY3DPEHPK3PXP";
    }
}
