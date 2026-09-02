namespace Application.Users
{
    public sealed class UserProfile
    {
        public UserProfile(string displayName, string role, string email)
        {
            DisplayName = displayName;
            Role = role;
            Email = email;
        }

        public string DisplayName { get; }
        public string Role { get; }
        public string Email { get; }
    }
}
