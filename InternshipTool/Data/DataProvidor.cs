using System;
using System.IO;
using Globals.Interfaces;

namespace Data;

public class DataProvidor : IData
{
  private const string UsersFilePath = "users.txt";

  public bool UserExists(string username)
  {
    if (!File.Exists(UsersFilePath))
      return false;

    string[] users = File.ReadAllLines(UsersFilePath);
    foreach (var user in users)
    {
      var parts = user.Split(';');
      if (parts.Length > 0 && parts[0] == username)
      {
        return true;
      }
    }
    return false;
  }

  public bool AddUser(string username, string hashedPassword)
  {
    try
    {
      string newUser = $"{username};{hashedPassword}";
      File.AppendAllText(UsersFilePath, newUser + Environment.NewLine);
      return true;
    }
    catch
    {
      return false;
    }
  }

  public string? GetUserHashedPassword(string username)
  {
    if (!File.Exists(UsersFilePath))
      return null;

    string[] users = File.ReadAllLines(UsersFilePath);
    foreach (var user in users)
    {
      var parts = user.Split(';');
      if (parts.Length == 2 && parts[0] == username)
      {
        return parts[1];
      }
    }
    return null;
  }
}

