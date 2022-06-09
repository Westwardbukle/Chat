namespace Chat.Common.Exceptions
{
    public sealed class ActivationСodeТotFoundException : NotFoundException
    {
        public ActivationСodeТotFoundException() : base("Activation code not found")
        {
        }
    }
}