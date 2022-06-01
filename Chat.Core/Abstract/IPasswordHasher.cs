namespace Chat.Core.Abstract
{
    public interface IPasswordHasher
    {
        public string HashPassword(string password);
        public bool VerifyHashedPassword(string password, string passwordHash);
    }
}