namespace Core
{
    public interface IUserService
    {
        string GetCurrentUserId();
        public string GetCurrentUserName();
        public bool IsLoggedIn();
    }
}