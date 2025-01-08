namespace Globals.Interfaces;

public interface IData
{
  bool UserExists(string username);
  bool AddUser(string username, string hashedPassword);
  string? GetUserHashedPassword(string username);
}
