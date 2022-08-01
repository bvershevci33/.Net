namespace AdminPandel.Settings
{
    public class MailSettings
    {
        public static string SectionName { get; set; } = "MailSettings";
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
