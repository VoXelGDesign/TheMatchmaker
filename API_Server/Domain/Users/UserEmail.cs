using Domain.Users.Properties;
using System.Text.RegularExpressions;

namespace Domain.Users;

public record UserEmail
{
    

    public string Email { get; private set; } = null!;

    public UserEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("Email cannot be null or empty.");
        }

        if (!UserValidationProperties.EmailRegex.IsMatch(email))
        {
            throw new ArgumentException("Email must be a valid email address.");
        }

        Email = email;
    }
}
