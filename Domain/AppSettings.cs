namespace Domain
{
    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public Logging Logging { get; set; }
        public string AllowedHosts { get; set; }
        public string OllamaAI { get; set; }
        public string SecretToken { get; set; }
        public SMTP SMTP { get; set; }
    }
    public class SMTP
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
        public string LocalDockerConnection { get; set; }
    }

    public class Logging
    {
        public LogLevel LogLevel { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }
        public string MicrosoftAspNetCore { get; set; }
    }
}
