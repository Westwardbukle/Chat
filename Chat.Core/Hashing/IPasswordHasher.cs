namespace Chat.Core.Hashing
{
    public interface IPasswordHasher
    {
        public string HashPassword(string password);
        public bool VerifyHashedPassword(string password, string passwordHash);
    }
}