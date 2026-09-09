using System.Security.Cryptography;
using System.Text;

namespace StudentManagement.Application.Security;

public static class PasswordHasher
{
    public static string Hash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password is required.", nameof(password));

        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(Convert.ToBase64String(salt) + password));
        return $"SHA256${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }

    public static bool Verify(string password, string storedHash)
    {
        if (string.IsNullOrWhiteSpace(storedHash) || string.IsNullOrWhiteSpace(password))
            return false;

        var parts = storedHash.Split('$');
        if (parts.Length != 3 || parts[0] != "SHA256")
            return false;

        var salt = parts[1];
        var expected = parts[2];
        var actual = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(salt + password)));
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(actual),
            Encoding.UTF8.GetBytes(expected));
    }
}
