using System;
using System.IO;
using Globals.Interfaces;

namespace Logic;
public class Logic : ILogic
{
  private const string UsersFilePath = "users.txt"; // Path to user data file

  public bool ValidateLogin(string username, string password)
  {
    if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
      return false;

    string hashedPassword = HashPassword(password);

    if (File.Exists(UsersFilePath))
    {
      string[] users = File.ReadAllLines(UsersFilePath);
      foreach (var user in users)
      {
        var parts = user.Split(';');
        if (parts.Length == 2 && parts[0] == username && parts[1] == hashedPassword)
        {
          return true;
        }
      }
    }

    return false;
  }

  public bool RegisterUser(string username, string password)
  {
    if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
      return false;

    if (File.Exists(UsersFilePath))
    {
      string[] users = File.ReadAllLines(UsersFilePath);
      foreach (var user in users)
      {
        var parts = user.Split(';');
        if (parts.Length > 0 && parts[0] == username)
        {
          return false; // Username already exists
        }
      }
    }

    string hashedPassword = HashPassword(password);
    File.AppendAllText(UsersFilePath, $"{username};{hashedPassword}{Environment.NewLine}");
    return true;
  }

  private string HashPassword(string password)
  {
    // Simplified hashing example using SHA256 (replace with Argon2-ID for production)
    using (var sha = System.Security.Cryptography.SHA256.Create())
    {
      var bytes = System.Text.Encoding.UTF8.GetBytes(password);
      var hash = sha.ComputeHash(bytes);
      return Convert.ToBase64String(hash);
    }
  }
}

