namespace TaskManagement.Infrastructure.Authentication;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "TaskManagement";
    public string Audience { get; set; } = "TaskManagementUsers";
    public string SecretKey { get; set; } = "THIS_IS_A_DEVELOPMENT_SECRET_KEY_CHANGE_ME_123456";
    public int ExpiryMinutes { get; set; } = 120;
}

