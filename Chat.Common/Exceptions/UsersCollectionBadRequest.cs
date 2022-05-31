namespace Chat.Common.Exceptions
{
    public sealed class UsersCollectionBadRequest : BadRequestException
    {
        public UsersCollectionBadRequest() : base("Users collection is null.")
        {
        }
    }
}