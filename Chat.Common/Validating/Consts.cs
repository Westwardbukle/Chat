namespace Chat.Core.Validating
{
    public class Consts
    {
        public const string EmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                           + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                           + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        public const string PasswordPattern = "^ ( ?=.* [ 0 - 9 ] ) ( ?=.* [ a - z ] )"
                                              + " ( ?=.* [ A - Z ] ) ( ?=.* [ *.!@ $% ^& ( )"
                                              + " { } [ ] :;<>,.?/~ _ +-=| \\ ] ) . { 8 , 32 } $ ";
    }
}