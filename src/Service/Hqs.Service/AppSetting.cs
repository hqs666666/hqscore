namespace Hqs.Service
{
    public class AppSetting
    {
        public Logging Logging { get; set; }
        public string AllowedHosts { get; set; }
        public AuthConfig AuthConfig { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
    }

    #region Logging

    public class Logging
    {
        public LogLevel LogLevel;
    }

    public class LogLevel
    {
        public string Default { get; set; }
        public string System { get; set; }
        public string Microsoft { get; set; }
    }

    #endregion

    #region ConnectionStrings

    public class ConnectionStrings
    {
        public string SqlServer { get; set; }
        public string Redis { get; set; }
    }

    #endregion

    #region AuthConfig

    public class AuthConfig
    {
        public string AuthServer { get; set; }
        public string AuthAddress { get; set; }
        public string ApiName { get; set; }
    }

    #endregion
}
