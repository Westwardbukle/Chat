namespace Chat.Common.Exceptions
{
    public sealed class MaxDateRangeBadRequestException : BadRequestException
    {
        public MaxDateRangeBadRequestException() : base("Max date can't be less than min date")
        {
        }
    }
}