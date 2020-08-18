namespace DatingApp.API.DTOs
{

    /// <summary>
    /// Accepts a Username & Password for Controller parameters passed in a Post body
    /// For the purpose of Logging in existing users of the DB
    /// </summary>
    public class UserFromLoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}