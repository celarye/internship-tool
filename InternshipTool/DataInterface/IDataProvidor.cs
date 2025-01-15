using Globals;

namespace DataInterface;

public interface IDataProvidor
{
  // User methods
  User? GetUser(int id);
  User? GetUserByEmail(string email);
  int GetNextUserId();
  void AddUser(User user);
  void UpdateUser(User user);

  // Company methods
  int GetNextCompanyId();
  void AddCompany(Company company);

  // Internship methods
  Internship? GetInternship(int id);
  void AddInternship(Internship internship);
}

