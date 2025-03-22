namespace WebApplication_Flight.Services.UserServicies
{
    public interface IUserServicie
    {
        User? GetUserByUsername(string username);
        void RegisterUser(User user);
        void SavedChanges();
    }
}
