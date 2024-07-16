namespace Simple_CRUD_Project.Abstractions
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyHashPassword(string password, string inputPassword);
    }
}
