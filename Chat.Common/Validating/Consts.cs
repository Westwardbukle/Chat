namespace Chat.Common.Validating
{
    public class Consts
    {
        public const string EmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                           + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                           + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        public const string PasswordPattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$";

        public const string ErrorPassword = "Хотя бы одна цифра [0-9] "
                                            + "Хотя бы один символ нижнего регистра [a-z] "
                                            + "Хотя бы один символ верхнего регистра [A-Z] "
                                            + "Длина не менее 8 символов";

        public const string Subject = "Restore Code";
    }
}