using Chat.Common.Error;

namespace Chat.Common.Result
{
    public class ResultContainer<T>
    {
        public T Data { get; set; }

        public ErrorType? ErrorType { get; set; }
    }
}