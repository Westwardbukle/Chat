using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Server.HttpSys;

namespace Chat.Core.Validating
{
    
    public static class Consts
    {
        public const string EmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                                 + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                                 + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
    }
    
    [AttributeUsage(AttributeTargets.Property)]
    public class RegExAttribute : ValidationAttribute 
    {
        private string _pattern;
        
        public RegExAttribute(string pattern)
        {
            _pattern = pattern;
        }
        
        public override bool IsValid(object? value)
        {
            var propertyValue = value as string;

            if (propertyValue is null)
            {
                return false;
            }

            return Regex.IsMatch(propertyValue, _pattern);
        }
        
    }

    public class EmailAttribute : RegularExpressionAttribute
    {
        public EmailAttribute() : base(Consts.EmailPattern)
        {
            
        }
        
    }
}