using Globals.Enums;

namespace Globals;

public class Company(int id, string name, int companyManager)
{
  public int Id { get; } = id;
  public string Name { get; set; } = name;
  public int CompanyManager { get; set; } = companyManager;

  public override string ToString()
  {
    return "Company: " + Id + ", " + Name + ", " + CompanyManager;
  }
}
