namespace WebApi.ViewModels
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public bool IsAuthorized(string user, string pass)
        {
            return Email == user && Password == pass;
        }
    }
}
