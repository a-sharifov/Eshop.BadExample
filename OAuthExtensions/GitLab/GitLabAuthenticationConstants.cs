namespace eShop.Services.Identity.OAuthExtensions.GitLab;

public static class GitLabAuthenticationConstants
{
    public static class Claims
    {
        public const string Name = "urn:gitlab:name";
        public const string Avatar = "urn:gitlab:avatar";
        public const string Url = "urn:gitlab:url";
    }
}
