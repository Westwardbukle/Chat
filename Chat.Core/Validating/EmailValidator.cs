using System.Text.RegularExpressions;
using Chat.Common.Result;

namespace Chat.Core.Validating
{
    public class EmailValidator
    {
        private const string ValidEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                                 + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                                 + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        public static bool IsEmailValid(string email)
        {
            return email != null && Regex.IsMatch(email, ValidEmailPattern);
        }
    }
}