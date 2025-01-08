namespace Globals.Interfaces;

public interface ILogic
{
  bool ValidateLogin(string username, string password);
  bool RegisterUser(string username, string password);
}
