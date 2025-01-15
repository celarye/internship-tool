using Globals;
using Globals.Enums;

namespace LogicInterface;

public interface ILogicProvidor
{
  // User methods
  void LoginUser(string email, string password);
  void RegisterUser(string email, string password, string firstName, string secondName, UserTypes userType);
  void UpdateUser(string? email, string? password, string? firstName, string? secondName, UserTypes? userType);

  // Company methods
  void RegisterCompany(string companyName);
}
