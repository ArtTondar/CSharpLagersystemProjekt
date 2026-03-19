namespace API.Controllers
{
    public class CurrentUserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool IsAdmin { get; set; }
    }
}