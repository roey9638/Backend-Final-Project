namespace Reactivities.DTOs
{
    public class UserDto
    {
        public string DisplayName { get; set; }
        public string Token { get; set; } //This is what the [users] [gonna use] to [authenticate] to the [API]
        public string Username { get; set; }
        public string Image { get; set; }
    }
}
