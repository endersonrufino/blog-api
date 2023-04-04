namespace blog;

public static class Configuration
{
    public static string JwtKey = "twtsgge56AWE7yhsfgHLkewhejh2762=";
    public static string ApiKeyName = "api_key";
    public static string ApiKey = "teste_api_Ilthhyt/lkiyt/oiubnMgtyRRl=";
    public static SmtpConfiguration Smtp = new();

    public class SmtpConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
