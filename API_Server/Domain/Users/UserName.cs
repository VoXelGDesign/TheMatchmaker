using Domain.Users.Properties;

namespace Domain.Users;

public record UserName
{
    public string Name { get; private set; }

    public UserName(string name)
    {
        if (UserValidationProperties.MaxNameLength < name.Length)
        {
            throw new ArgumentException($"Name cannot be longer than {UserValidationProperties.MaxNameLength} characters.");
        }

        if (UserValidationProperties.MinNameLength > name.Length)
        {
            throw new ArgumentException($"Name cannot be shorter than {UserValidationProperties.MinNameLength} characters.");
        }

        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Name cannot be null or empty.");
        }

        if (!UserValidationProperties.NameRegex.IsMatch(name))
        {
            throw new ArgumentException("Name must contain only letters, numbers, and spaces.");
        }
    }
}
