using System;
using System.Linq;
using FluentValidation;
using TheRiceMill.Common.Constants;

namespace TheRiceMill.Application.Extensions
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder, int minimumLength = 8)
        {
            var options = ruleBuilder
                .Required()
                .MinimumLength(minimumLength).WithMessage(Messages.PasswordLength)
                .Matches("[A-Z]").WithMessage(Messages.PasswordUppercaseLetter)
                .Matches("[a-b]").WithMessage(Messages.PasswordLowercaseLetter)
                .Matches("[0-9]").WithMessage(Messages.PasswordDigit)
                .Matches("[^a-zA-Z0-9]").WithMessage(Messages.PasswordSpecialCharacter);
            return options;
        }
        
        public static IRuleBuilder<T, string[]> EqualLength<T>(this IRuleBuilder<T, string[]> ruleBuilder, int length)
        {
            var options = ruleBuilder.Must(p => p.Length == length).WithMessage(Messages.IncorrectValue);
            return options;
        }
        
        
        public static IRuleBuilder<T, string> Pin<T>(this IRuleBuilder<T, string> ruleBuilder, int minimumLength = 4)
        {
            var options = ruleBuilder
                .Required()
                .MinimumLength(minimumLength).WithMessage(Messages.LessThan(minimumLength))
                .MaximumLength(minimumLength).WithMessage(Messages.GreaterThan(minimumLength));
            return options;
        }
        public static IRuleBuilder<T, string> Phone<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            int minimumLength = 12;
            int maximumLength = 13;
            var options = ruleBuilder
                .Required()
                .Must(p => p != null && p.StartsWith("+")).WithMessage(Messages.InvalidFormat)
                .MinimumLength(minimumLength).WithMessage(Messages.LessThan(minimumLength))
                .MaximumLength(maximumLength).WithMessage(Messages.GreaterThan(maximumLength));
            return options;
        }
        public static IRuleBuilder<T, string> Time<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            int minimumLength = 8;
            var options = ruleBuilder
                .Required()
                .MinimumLength(minimumLength).WithMessage(Messages.LessThan(minimumLength))
                .MaximumLength(minimumLength).WithMessage(Messages.GreaterThan(minimumLength));
            return options;
        }

        public static IRuleBuilder<T, string> MustBeOneOf<T>(this IRuleBuilder<T, string> ruleBuilder, string[] list)
        {
            var options = ruleBuilder
                .Required()
                .Must(list.Contains).WithMessage(Messages.IncorrectValue);
            return options;
        }
        public static IRuleBuilder<T, int> Max<T>(this IRuleBuilder<T, int> ruleBuilder, int max)
        {
            var options = ruleBuilder
                .Must(p => p <= max).WithMessage(Messages.GreaterThan(max));
            return options;
        }        
        public static IRuleBuilder<T, int> Min<T>(this IRuleBuilder<T, int> ruleBuilder, int min)
        {
            var options = ruleBuilder
                .Must(p => p >= min).WithMessage(Messages.LessThan(min));
            return options;
        }        
        public static IRuleBuilder<T, double> Min<T>(this IRuleBuilder<T, double> ruleBuilder, int min)
        {
            var options = ruleBuilder
                .Must(p => p >= min).WithMessage(Messages.LessThan(min));
            return options;
        }        
        public static IRuleBuilder<T, string> Max<T>(this IRuleBuilder<T,  string> ruleBuilder, int max)
        {
            var options = ruleBuilder
                .Required()
                .Must(p => p.Length <= max).WithMessage(Messages.MaxLengthError(max));
            return options;
        }        

        public static IRuleBuilder<T, string> Required<T>(this IRuleBuilder<T,  string> ruleBuilder)
        {
            var options = ruleBuilder
                .Must(p => !string.IsNullOrEmpty(p)).WithMessage(Messages.EmptyError);
            return options;
        }
        public static IRuleBuilder<T, string[]> Required<T>(this IRuleBuilder<T,  string[]> ruleBuilder)
        {
            var options = ruleBuilder
                .Must(p => p != null && p.Length > 0).WithMessage(Messages.EmptyError);
            return options;
        }
        public static IRuleBuilder<T, string> Identification<T>(this IRuleBuilder<T,  string> ruleBuilder)
        {
            var options = ruleBuilder
                .Required()
                .Must(p => p.Length == 13 || p.Length == 17).WithMessage(Messages.InvalidFormat);
            return options;
        }
        public static IRuleBuilder<T, int> Required<T>(this IRuleBuilder<T,  int> ruleBuilder)
        {
            var options = ruleBuilder
                .Min(0);
            return options;
        }
        public static IRuleBuilder<T, DateTime> Required<T>(this IRuleBuilder<T,  DateTime> ruleBuilder)
        {
            var options = ruleBuilder
                .Must( p => p > DateTime.MinValue).WithMessage(Messages.IncorrectValue);
            return options;
        }
        public static IRuleBuilder<T, double> Required<T>(this IRuleBuilder<T,  double> ruleBuilder)
        {
            var options = ruleBuilder
                .Min(0);
            return options;
        }

    }
}