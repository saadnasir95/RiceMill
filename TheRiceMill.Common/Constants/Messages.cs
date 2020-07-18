using System;

namespace TheRiceMill.Common.Constants
{
    public class Messages
    {
        public const string PhoneNumber = "Phone Number";
        public const string NoGateKeeperRoleFound = "No GateKeeper Role Found";
        public const string NoAdminRoleFound = "No Admin Role Found";
        public const string PasswordEmpty = "is Empty";
        public const string EmptyError = "is Empty";
        public static string OtpIncorrectOrExpired = "Otp is Incorrect or Expired";
        
        public static string MaxLengthError(int num)
        {
            return "cannot have more than " + num + " characters";
        }

        public const string PasswordLength = "must be greaters than 7 characters";
        public const string PasswordUppercaseLetter = "requires at least one upper case letter";
        public const string PasswordLowercaseLetter = "requires at least one lower case letter";
        public const string PasswordDigit = "requires at least one digit";
        public const string PasswordSpecialCharacter = "requires at least one special charater";
        public const string IncorrectPassword = "Password is Incorrect";
        public const string IncorrectValue = "has incorrect value";
        public const string InvalidFormat = "has invalid format";
        public const string InsufficientBalance = "You balance is insufficient";
        public const string IncorrectPin = "Your pin is incorrect";
        public static string GreaterThan(int i)
        {
            return "cannot be greater than " + i;
        }
        
        public static string LessThan(int i)
        {
            return "cannot be less than " + i;
        }
        public static string LessThan(DateTime date)
        {
            return "cannot be less than " + date.ToShortDateString();
        }

        public static string OtMessage(string phoneCode)
        {
            return "You PwPay Otp: " + phoneCode;
        }
    }
}