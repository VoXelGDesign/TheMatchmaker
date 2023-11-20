using System.Text.RegularExpressions;
using Domain.Users.Properties;
using Microsoft.AspNet.Identity;


namespace Domain.Users;

public record UserHashedPassword
{
    public string HashedPassword { get; private set; } = null!;

    private UserHashedPassword(string hashedPassword)
    {
        HashedPassword = hashedPassword;
    }

    public static UserHashedPassword Create(string password, IPasswordHasher passwordHasher)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Password cannot be null or empty.");
        }

        
        if (!UserValidationProperties.PasswordRegex.IsMatch(password))
        {
            throw new ArgumentException("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
        }

       
        string hashedPassword = passwordHasher.HashPassword(password);

        return new UserHashedPassword(hashedPassword);
    }

    public static UserHashedPassword CreateAlreadyHashed(string hashedPassword)
    {
        if (string.IsNullOrEmpty(hashedPassword))
        {
            throw new ArgumentException("Password cannot be null or empty.");
        }
        return new UserHashedPassword(hashedPassword);
    }
}
